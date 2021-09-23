// ========================================
// Project Name : WodiLib
// File Name    : CommonEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Common;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(15000000, 15999999)] コモンイベントセルフ変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 15000000, MaxValue = 15999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(CommonEventVariableAddress), typeof(VariableAddress) }
    )]
    public partial class CommonEventVariableAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ｺﾓﾝEv{0}ｾﾙﾌ{1}{2}";

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

        /// <summary>コモンイベントID</summary>
        public CommonEventId CommonEventId => RawValue.SubInt(2, 3);

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
            var commonVariableName = resolver.GetCommonEventSelfVariableName(CommonEventId, Index);
            if (!commonVariableName.Equals(string.Empty))
            {
                commonVariableName = $"[{commonVariableName}]";
            }

            return string.Format(EventCommandSentenceFormat, CommonEventId, Index, commonVariableName);
        }
    }
}
