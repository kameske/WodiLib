// ========================================
// Project Name : WodiLib
// File Name    : CommonEventReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Event;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// コモンイベント読み込みクラス
    /// </summary>
    internal class CommonEventReader
    {
        /// <summary>読み込み経過状態</summary>
        private FileReadStatus Status { get; }

        /// <summary>コモンイベント数</summary>
        private int Length { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">コモンイベント数</param>
        public CommonEventReader(FileReadStatus status, int length)
        {
            Status = status;
            Length = length;
        }

        /// <summary>
        /// コモンイベントを読み込み、返す。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        public List<CommonEvent> Read()
        {
            Logger.Debug(FileIOMessage.StartCommonRead(GetType(), ""));

            var list = new List<CommonEvent>();
            for (var i = 0; i < Length; i++)
            {
                ReadOneCommonEvent(Status, list);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(GetType(), ""));

            return list;
        }

        /// <summary>
        /// コモンイベント一つ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="result">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">バイナリデータがファイル仕様と異なる場合</exception>
        private void ReadOneCommonEvent(FileReadStatus status, ICollection<CommonEvent> result)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(CommonEventReader), "コモンイベント"));

            var commonEvent = new CommonEvent();

            // コモンイベントヘッダ
            ReadHeader(status);

            // コモンイベントID
            ReadCommonEventId(status, commonEvent);

            // 起動条件
            ReadBootCondition(status, commonEvent);

            // 数値引数の数
            ReadNumberArgLength(status, commonEvent);

            // 文字列引数の数
            ReadStringArgLength(status, commonEvent);

            // コモンイベント名
            ReadCommonEventName(status, commonEvent);

            // イベントコマンド
            ReadEventCommand(status, commonEvent);

            // メモ前の文字列
            ReadBeforeMemoString(status, commonEvent);

            // メモ
            ReadMemo(status, commonEvent);

            // 引数特殊指定
            ReadSpecialArgDesc(status, commonEvent);

            // 引数初期値後のチェックディジット
            ReadAfterInitValueBytes(status);

            // ラベル色
            ReadLabelColor(status, commonEvent);

            // 変数名
            ReadSelfVarName(status, commonEvent);

            // セルフ変数名の後のチェックディジット
            ReadAfterMemoBytesSelfVariableNamesBytes(status);

            // フッタ文字列
            ReadFooterString(status, commonEvent);

            // コモンイベント末尾A
            var hasNext = ReadFooterA(status);
            if (hasNext == HasNext.No)
            {
                result.Add(commonEvent);
                return;
            }

            // 返戻値
            ReadReturnValue(status, commonEvent);

            // コモンイベント末尾B
            ReadFooterB(status);

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(CommonEventReader), "コモンイベント"));

            result.Add(commonEvent);
        }

        /// <summary>
        /// コモンイベントヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ヘッダが仕様と異なる場合</exception>
        private void ReadHeader(FileReadStatus status)
        {
            foreach (var b in CommonEvent.HeaderBytes)
            {
                if (status.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルヘッダが仕様と異なります。（offset:{status.Offset}）");
                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(CommonEventReader), "コモンイベントヘッダ"));
        }

        /// <summary>
        /// コモンイベントID
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadCommonEventId(FileReadStatus status, CommonEvent commonEvent)
        {
            commonEvent.Id = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "コモンイベントID", commonEvent.Id));
        }

        /// <summary>
        /// 起動条件
        /// </summary>
        /// <param name="status"></param>
        /// <param name="commonEvent"></param>
        private void ReadBootCondition(FileReadStatus status, CommonEvent commonEvent)
        {
            // 起動条件比較演算子 & 起動条件
            ReadBootConditionOperationAndType(status, commonEvent.BootCondition);

            // 起動条件左辺
            ReadBootConditionLeftSide(status, commonEvent.BootCondition);

            // 起動条件右辺
            ReadBootConditionRightSide(status, commonEvent.BootCondition);
        }

        /// <summary>
        /// 起動条件比較演算子 &amp; 起動条件
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="condition">結果格納インスタンス</param>
        private void ReadBootConditionOperationAndType(FileReadStatus status,
            CommonEventBootCondition condition)
        {
            var b = status.ReadByte();
            status.IncreaseByteOffset();
            condition.Operation = CriteriaOperator.FromByte((byte) (b & 0xF0));
            condition.CommonEventBootType = CommonEventBootType.FromByte((byte) (b & 0x0F));

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "起動条件比較演算子", condition.Operation));
            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "起動条件", condition.CommonEventBootType));
        }

        /// <summary>
        /// 起動条件左辺
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="condition">結果格納インスタンス</param>
        private void ReadBootConditionLeftSide(FileReadStatus status,
            CommonEventBootCondition condition)
        {
            condition.LeftSide = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "起動条件左辺", condition.LeftSide));
        }

        /// <summary>
        /// 起動条件右辺
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="condition">結果格納インスタンス</param>
        private void ReadBootConditionRightSide(FileReadStatus status,
            CommonEventBootCondition condition)
        {
            condition.RightSide = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "起動条件右辺", condition.RightSide));
        }

        /// <summary>
        /// 数値引数の数
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadNumberArgLength(FileReadStatus status, CommonEvent commonEvent)
        {
            commonEvent.NumberArgsLength = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "数値引数の数", commonEvent.NumberArgsLength));
        }

        /// <summary>
        /// 文字列引数の数
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadStringArgLength(FileReadStatus status, CommonEvent commonEvent)
        {
            commonEvent.StrArgsLength = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "文字列引数の数", commonEvent.StrArgsLength));
        }

        /// <summary>
        /// コモンイベント名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadCommonEventName(FileReadStatus status, CommonEvent commonEvent)
        {
            var commonEventName = status.ReadString();
            commonEvent.Name = commonEventName.String;
            status.AddOffset(commonEventName.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "コモンイベント名", commonEvent.Name));
        }

        /// <summary>
        /// イベントコマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadEventCommand(FileReadStatus status, CommonEvent commonEvent)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "イベントコマンド数", length));

            var reader = new EventCommandListReader(status, length);
            commonEvent.EventCommands = reader.Read();
        }

        /// <summary>
        /// メモ前の文字列
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadBeforeMemoString(FileReadStatus status, CommonEvent commonEvent)
        {
            var str = status.ReadString();
            commonEvent.Description = str.String;
            status.AddOffset(str.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "メモ前の文字列", commonEvent.Description));
        }

        /// <summary>
        /// メモ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadMemo(FileReadStatus status, CommonEvent commonEvent)
        {
            var str = status.ReadString();
            commonEvent.Memo = str.String;
            status.AddOffset(str.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "メモ", commonEvent.Memo));
        }

        /// <summary>
        /// 引数特殊指定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadSpecialArgDesc(FileReadStatus status, CommonEvent commonEvent)
        {
            try
            {
                var specialArgDescReader = new SpecialArgDescReader(status);
                specialArgDescReader.Read();

                UpdateSpecialNumberArgDesc(
                    specialArgDescReader.NumberArgNameList,
                    specialArgDescReader.NumberArgTypeList,
                    specialArgDescReader.NumberArgStringParamsList,
                    specialArgDescReader.NumberArgNumberParamsList,
                    specialArgDescReader.NumberArgInitValueList,
                    commonEvent
                );

                UpdateSpecialStringArgDesc(
                    specialArgDescReader.StringArgNameList,
                    commonEvent
                );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"引数特殊指定データが仕様と異なります。（offset:{status.Offset}）", ex);
            }
        }

        /// <summary>
        /// 数値引数特殊指定
        /// </summary>
        /// <param name="argNameList">引数名リスト</param>
        /// <param name="argTypeList">引数特殊指定リスト</param>
        /// <param name="stringArgLists">数値特殊指定文字列パラメータリスト</param>
        /// <param name="numberArgLists">数値特殊指定数値パラメータリスト</param>
        /// <param name="numberArgInitValueList">数値特殊指定数値初期値リスト</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void UpdateSpecialNumberArgDesc(IReadOnlyList<string> argNameList,
            IReadOnlyList<CommonEventArgType> argTypeList, IReadOnlyList<List<string>> stringArgLists,
            IReadOnlyList<List<int>> numberArgLists, IReadOnlyList<int> numberArgInitValueList,
            CommonEvent commonEvent)
        {
            var argNameListCount = argNameList.Count;
            if (argNameListCount != argTypeList.Count || argNameListCount != stringArgLists.Count ||
                argNameListCount != numberArgLists.Count || argNameListCount != numberArgInitValueList.Count)
                throw new ArgumentException("引数リストの長さが異なります。");

            for (var i = 0; i < argNameListCount; i++)
            {
                var stringArgList = stringArgLists[i];
                var numberArgList = numberArgLists[i];

                var desc = MakeSpecialNumberArgDesc(argTypeList[i], argNameList[i],
                    numberArgInitValueList[i], numberArgList, stringArgList);

                commonEvent.UpdateSpecialNumberArgDesc(i, desc);
            }
        }

        private CommonEventSpecialNumberArgDesc MakeSpecialNumberArgDesc(CommonEventArgType type,
            string argName, int initValue, List<int> numberArgList, List<string> stringArgList)
        {
            return type == CommonEventArgType.ReferDatabase
                ? UpdateSpecialNumberArgDesc_MakeDescForReferDatabase(type, argName,
                    initValue, numberArgList, stringArgList)
                : UpdateSpecialNumberArgDesc_MakeDescForElse(type, argName,
                    initValue, numberArgList, stringArgList);
        }

        private CommonEventSpecialNumberArgDesc UpdateSpecialNumberArgDesc_MakeDescForReferDatabase(
            CommonEventArgType type,
            string argName, int initValue, List<int> numberArgList, List<string> stringArgList)
        {
            var caseList = new List<CommonEventSpecialArgCase>();
            for (var j = 0; j < stringArgList.Count; j++)
            {
                var argCase = new CommonEventSpecialArgCase(-1 * (j + 1), stringArgList[j]);
                caseList.Add(argCase);
            }

            var desc = new CommonEventSpecialNumberArgDesc
            {
                ArgName = argName,
                InitValue = initValue
            };

            desc.ChangeArgType(type, caseList);
            desc.SetDatabaseRefer(DBKind.FromSpecialArgCode(numberArgList[0]), numberArgList[1]);
            desc.SetDatabaseUseAdditionalItemsFlag(numberArgList[2] == 1);

            return desc;
        }

        private CommonEventSpecialNumberArgDesc UpdateSpecialNumberArgDesc_MakeDescForElse(
            CommonEventArgType type,
            string argName, int initValue, List<int> numberArgList, List<string> stringArgList)
        {
            var stringArgListCount = stringArgList.Count;
            var numberArgListCount = numberArgList.Count;

            // 旧バージョンで作られたデータ限定？で文字列と数値の数が一致しないことがある。
            //   基本システムVer2のコモンイベント14などで確認。
            if (stringArgListCount != numberArgListCount)
            {
                WodiLibLogger.GetInstance().Warning(
                    $"[Warning] 文字列引数リストと数値引数リストの長さが一致しません。（文字列数：{stringArgListCount}, 数値数：{numberArgListCount}）");
            }

            var loopTimes = stringArgListCount <= numberArgListCount
                ? stringArgListCount
                : numberArgListCount;

            var caseList = new List<CommonEventSpecialArgCase>();
            for (var j = 0; j < loopTimes; j++)
            {
                var argCase = new CommonEventSpecialArgCase(numberArgList[j], stringArgList[j]);
                caseList.Add(argCase);
            }

            var desc = new CommonEventSpecialNumberArgDesc
            {
                ArgName = argName,
                InitValue = initValue
            };

            desc.ChangeArgType(type, caseList);

            return desc;
        }

        /// <summary>
        /// 文字列引数特殊指定
        /// </summary>
        /// <param name="argNameList">引数名リスト</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void UpdateSpecialStringArgDesc(IReadOnlyList<string> argNameList,
            CommonEvent commonEvent)
        {
            var argNameListCount = argNameList.Count;

            for (var i = 0; i < argNameListCount; i++)
            {
                var desc = new CommonEventSpecialStringArgDesc
                {
                    ArgName = argNameList[i]
                };

                commonEvent.UpdateSpecialStringArgDesc(i, desc);
            }
        }

        /// <summary>
        /// 引数初期値後のチェックディジット
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadAfterInitValueBytes(FileReadStatus status)
        {
            foreach (var b in CommonEvent.AfterInitValueBytes)
            {
                if (status.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルデータが仕様と異なります。（offset:{status.Offset}）");
                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(CommonEventReader),
                "引数初期値後のチェックディジット"));
        }

        /// <summary>
        /// ラベル色
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadLabelColor(FileReadStatus status, CommonEvent commonEvent)
        {
            var colorNumber = status.ReadInt();
            status.IncreaseIntOffset();

            commonEvent.LabelColor = CommonEventLabelColor.FromInt(colorNumber);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "ラベル色", commonEvent.LabelColor));
        }

        /// <summary>
        /// 変数名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadSelfVarName(FileReadStatus status, CommonEvent commonEvent)
        {
            const int varNameLength = 100;

            var varNameList = new List<CommonEventSelfVariableName>();

            for (var i = 0; i < varNameLength; i++)
            {
                var varName = status.ReadString();
                status.AddOffset(varName.ByteLength);

                varNameList.Add(varName.String);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                    $"セルフ変数{i}名", varName.String));
            }

            commonEvent.SelfVariableNameList = new CommonEventSelfVariableNameList(varNameList);
        }

        /// <summary>
        /// セルフ変数名の後のチェックディジット
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadAfterMemoBytesSelfVariableNamesBytes(FileReadStatus status)
        {
            foreach (var b in CommonEvent.AfterMemoBytesSelfVariableNamesBytes)
            {
                if (status.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルデータが仕様と異なります。（offset:{status.Offset}）");
                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(CommonEventReader),
                "セルフ変数後のチェックディジット", "（返戻値あり）"));
        }

        /// <summary>
        /// フッタ文字列
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadFooterString(FileReadStatus status, CommonEvent commonEvent)
        {
            var footerString = status.ReadString();
            status.AddOffset(footerString.ByteLength);

            commonEvent.FooterString = footerString.String;

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "フッタ文字列", commonEvent.FooterString));
        }

        /// <summary>
        /// コモンイベント末尾A
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>あとに返戻値が続く場合true</returns>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private HasNext ReadFooterA(FileReadStatus status)
        {
            var b1 = status.ReadByte();

            if (b1 == CommonEvent.BeforeReturnValueSummaryBytesBefore[0])
            {
                foreach (var b in CommonEvent.BeforeReturnValueSummaryBytesBefore)
                {
                    if (status.ReadByte() != b)
                        throw new InvalidOperationException(
                            $"ファイルデータが仕様と異なります。（offset:{status.Offset}）");
                    status.IncreaseByteOffset();
                }


                Logger.Debug(FileIOMessage.CheckOk(typeof(CommonEventReader),
                    "コモンイベント末尾", "（返戻値あり）"));

                return HasNext.Yes;
            }

            if (b1 == CommonEvent.FooterBytesBeforeVer2_00[0])
            {
                status.IncreaseByteOffset();
                return HasNext.No;
            }

            throw new InvalidOperationException(
                $"ファイルデータが仕様と異なります。（offset:{status.Offset}）");
        }

        /// <summary>
        /// 返戻値
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadReturnValue(FileReadStatus status, CommonEvent commonEvent)
        {
            // 返戻値の意味
            ReadReturnValueDescription(status, commonEvent);

            // 返戻セルフ変数インデックス
            ReadReturnVariableIndex(status, commonEvent);
        }

        /// <summary>
        /// 返戻値の意味
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadReturnValueDescription(FileReadStatus status, CommonEvent commonEvent)
        {
            var description = status.ReadString();
            status.AddOffset(description.ByteLength);

            commonEvent.ReturnValueDescription = description.String;

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "返戻値の意味", commonEvent.ReturnValueDescription));
        }

        /// <summary>
        /// 返戻セルフ変数インデックス
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadReturnVariableIndex(FileReadStatus status, CommonEvent commonEvent)
        {
            var index = status.ReadInt();
            status.IncreaseIntOffset();

            commonEvent.SetReturnVariableIndex(index);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(CommonEventReader),
                "返戻セルフ変数インデックス", index));
        }

        /// <summary>
        /// コモンイベント末尾B
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadFooterB(FileReadStatus status)
        {
            foreach (var b in CommonEvent.FooterBytesAfterVer2_00)
            {
                if (status.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルデータが仕様と異なります。（offset:{status.Offset}）");
                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(CommonEventReader),
                "コモンイベント末尾B"));
        }

        /// <summary>
        /// 次へ続くフラグ
        /// </summary>
        private enum HasNext
        {
            Yes,
            No
        }
    }
}