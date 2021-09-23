// ========================================
// Project Name : WodiLib
// File Name    : CalledEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Project;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    ///     [Range(1100000, 1100009)]
    ///     [Range(1600000, 1600009)]
    ///     このマップイベントセルフ変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 1100000, MaxValue = 1600009)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(CalledEventVariableAddress), typeof(VariableAddress) }
    )]
    public partial class CalledEventVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>"このマップイベント変数"かどうか</summary>
        public bool IsThisMapEventVariableAddress { get; private set; }

        /// <summary>"このコモンイベント変数"かどうか</summary>
        public bool IsThisCommonEventVariableAddress { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        partial void DoConstructorExpansion(int value)
        {
            // このコモンイベントセルフ変数（限定範囲）なら許可
            if (ThisCommonEventVariableAddress.MinValue <= value
                && value <= MaxValue)
            {
                IsThisCommonEventVariableAddress = true;
                return;
            }

            // このマップイベントセルフ変数なら許可
            if (ThisMapEventVariableAddress.MinValue <= value
                && value <= ThisMapEventVariableAddress.MaxValue)
            {
                IsThisMapEventVariableAddress = true;
                return;
            }

            // それ以外なら許可しない
            throw new ArgumentOutOfRangeException(
                $"{nameof(value)}は" +
                $"{ThisMapEventVariableAddress.MinValue}～{ThisMapEventVariableAddress.MaxValue}または" +
                $"{ThisCommonEventVariableAddress.MinValue}～{MaxValue}のみ設定できます。" +
                $"(設定値：{value})");
        }

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
            if (IsThisCommonEventVariableAddress)
            {
                var common = (ThisCommonEventVariableAddress)RawValue;
                return common.MakeEventCommandString(resolver, type,
                    VariableAddressValueType.Numeric, desc);
            }

            var map = (ThisMapEventVariableAddress)RawValue;
            return map.MakeEventCommandString(resolver, type,
                VariableAddressValueType.Numeric, desc);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     値が CalledEventVariableAddress の値として適切かどうかを返す。
        /// </summary>
        /// <param name="src">判定対象値</param>
        /// <returns>判定結果</returns>
        public static bool CanCast(int src)
        {
            // このコモンイベントセルフ変数（限定範囲）なら許可
            if (ThisCommonEventVariableAddress.MinValue <= src
                && src <= MaxValue)
            {
                return true;
            }

            // このマップイベントセルフ変数なら許可
            if (ThisMapEventVariableAddress.MinValue <= src
                && src <= ThisMapEventVariableAddress.MaxValue)
            {
                return true;
            }

            return false;
        }
    }
}
