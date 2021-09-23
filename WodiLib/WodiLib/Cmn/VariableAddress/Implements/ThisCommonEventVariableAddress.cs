// ========================================
// Project Name : WodiLib
// File Name    : ThisCommonEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1600000, 1600099)] このコモンイベントセルフ変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 1600000, MaxValue = 1600099)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(ThisCommonEventVariableAddress), typeof(VariableAddress) }
    )]
    public partial class ThisCommonEventVariableAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceCommonEventFormat = "CSelf{0}{1}";
        private const string EventCommandSentenceCommonEventName = "[{0}]";
        private const string EventCommandSentenceMapEventFormat = "★エラー！マップEvでは「ｺﾓﾝｾﾙﾌ」は動作しません！！";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     セルフ変数インデックス
        /// </summary>
        public CommonEventVariableIndex Index => RawValue.SubInt(0, 2);

        /// <summary>
        ///     文字列変数フラグ
        /// </summary>
        public bool IsStringVariable => Index.IsStringIndex;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string ResolveEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
        {
            if (type == EventCommandSentenceType.Map) return EventCommandSentenceMapEventFormat;

            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));
            if (desc.CommonEventId is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc.CommonEventId)));

            var variableName = resolver.GetCommonEventSelfVariableName(
                desc.CommonEventId, Index);

            var varNameStr = ((string)variableName).Equals(string.Empty)
                ? string.Empty
                : string.Format(EventCommandSentenceCommonEventName, variableName);

            return string.Format(EventCommandSentenceCommonEventFormat,
                Index, varNameStr);
        }
    }
}
