// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolver.CommonEvent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Cmn;
using WodiLib.Common;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文文字列解決クラス・コモンイベント
    /// </summary>
    internal class EventCommandSentenceResolver_CommonEvent
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string CommonEventIdNotFound = "該当無し";

        private const string CommonEventNameNotFound = "コモンイベントがありません";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public EventCommandSentenceResolver Master { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="master">呼び出し元</param>
        public EventCommandSentenceResolver_CommonEvent(
            EventCommandSentenceResolver master)
        {
            Master = master;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベントIDを返す。
        /// </summary>
        /// <param name="name">コモンイベント名</param>
        /// <returns>コモンイベントID（存在しない場合null）</returns>
        public CommonEventId? GetCommonEventId(CommonEventName name)
        {
            return Master.CommonEventList.FirstOrDefault(x => x.Name.Equals(name))?
                .Id;
        }

        /// <summary>
        /// コモンイベントID（文字列）を返す。
        /// </summary>
        /// <param name="name">コモンイベント名</param>
        /// <returns>コモンイベントIDの文字列（存在しない場合専用の文字列）</returns>
        public string GetCommonEventIdString(CommonEventName name)
        {
            var target = Master.CommonEventList.FirstOrDefault(x => x.Name.Equals(name));

            return target?.Id.ToString() ?? CommonEventIdNotFound;
        }

        /// <summary>
        /// コモンイベント名を返す。
        /// </summary>
        /// <param name="id">コモンイベントID</param>
        /// <returns>コモンイベント名（存在しない場合専用の文字列）</returns>
        public (bool, CommonEventName) GetCommonEventName(CommonEventId id)
        {
            if (Master.CommonEventList.Count <= id) return (false, CommonEventNameNotFound);

            return (true, Master.CommonEventList[id].Name);
        }

        /// <summary>
        /// コモンイベントIDとインデックスからコモンイベントセルフ変数名を取得する。
        /// </summary>
        /// <param name="commonEventId">コモンイベントID</param>
        /// <param name="index">インデックス</param>
        /// <returns>コモンイベントセルフ変数名（存在しないコモンイベントの場合空文字）</returns>
        public CommonEventSelfVariableName GetCommonEventSelfVariableName(
            CommonEventId commonEventId, CommonEventVariableIndex index)
        {
            if (Master.CommonEventList.Count <= commonEventId) return string.Empty;
            var commonEvent = Master.CommonEventList[commonEventId];

            return commonEvent.SelfVariableNameList[index];
        }

        /// <summary>
        /// コモンイベント数値引数の文字列を取得する。
        /// </summary>
        /// <param name="id">コモンイベントID</param>
        /// <param name="intArgIndex">[Range(0, 0～4)]引数インデックス</param>
        /// <param name="value">引数設定値</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>コモンイベント数値引数の文字列</returns>
        public string GetCommonEventIntArgSentence(CommonEventId id,
            CommonEventNumberArgIndex intArgIndex, int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            // 値が変数アドレス値の場合は引数の設定によらず変数アドレスの文字列だけを表示
            if (value.IsVariableAddressSimpleCheck())
            {
                return Master.GetNumericVariableAddressString(value, type, desc);
            }

            // 該当コモンイベントが存在しないときは値をそのまま表示
            var target = Master.CommonEventList.FirstOrDefault(x => x.Id == id);
            if (target is null) return value.ToString();

            // 該当引数が設定されていないときは値をそのまま表示
            if (intArgIndex >= target.NumberArgsLength) return value.ToString();
            var argDesc = target.NumberArgDescList[intArgIndex];

            // 引数の値種別によって表示内容を決定
            if (argDesc.ArgType == CommonEventArgType.Normal) return value.ToString();
            if (argDesc.ArgType == CommonEventArgType.ReferDatabase)
            {
                // -1以下の場合は手動指定した文字列
                if (value < 0)
                {
                    var targetCase = argDesc.GetAllSpecialCase()
                        .FirstOrDefault(x => x.CaseNumber == value);

                    return targetCase is null
                        ? value.ToString()
                        : $"{value.ToString()}:{targetCase.Description}";
                }

                // 0以上の場合はデータ名
                var (isFind, dataName) = Master.GetDatabaseDataName(argDesc.DatabaseUseDbKind,
                    argDesc.DatabaseDbTypeId, value);
                if (!isFind)
                {
                    return value.ToString();
                }

                return $"{value.ToString()}:{dataName}";
            }

            if (argDesc.ArgType == CommonEventArgType.Manual)
            {
                var targetCase = argDesc.GetAllSpecialCase()
                    .LastOrDefault(x => x.CaseNumber == value);

                return targetCase is null
                    ? value.ToString()
                    : $"{value.ToString()}:{targetCase.Description}";
            }

            // 通常ここへは来ない
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 相対指定のコモンイベントIDから絶対指定のコモンイベントIDを取得する。
        /// </summary>
        /// <param name="targetCommonEventId">相対指定のコモンイベントID</param>
        /// <param name="thisCommonEventId">走査対象のコモンイベントID</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <returns>絶対指定したコモンイベントID</returns>
        /// <exception cref="InvalidOperationException">イベントコマンド種別がCommon、かつthisCommonEventIdがnullの場合</exception>
        public CommonEventId GetCorrectEventIdByRelativeId(int targetCommonEventId,
            CommonEventId? thisCommonEventId, EventCommandSentenceType type)
        {
            if (targetCommonEventId < CommonEventId.CommonEventRelativeOffset_Min ||
                CommonEventId.CommonEventRelativeOffset_Max < targetCommonEventId)
            {
                return targetCommonEventId;
            }

            var difference = targetCommonEventId - CommonEventId.CommonEventRelativeOffset;

            int targetId;
            switch (type)
            {
                case EventCommandSentenceType t when t == EventCommandSentenceType.Common:
                    if (thisCommonEventId is null) throw new InvalidOperationException();

                    targetId = thisCommonEventId.Value + difference;
                    break;

                case EventCommandSentenceType t when t == EventCommandSentenceType.Map:
                    // マップイベントの場合、自身のコモンイベントID＝0として表示判定を行う
                    targetId = 0 + difference;
                    break;

                default:
                    throw new InvalidOperationException();
            }

            if (targetId < 0) targetId = 0;

            var maxId = Master.CommonEventList.Count - 1;
            if (targetId > maxId) targetId = maxId;

            return targetId;
        }
    }
}