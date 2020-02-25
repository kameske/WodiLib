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
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    ///     [Range(1100000, 1100009)]
    ///     [Range(1600000, 1600009)]
    ///     このマップイベントセルフ変数アドレス値
    /// </summary>
    [Serializable]
    public class CalledEventVariableAddress : VariableAddress, IEquatable<CalledEventVariableAddress>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>最小値</summary>
        protected override int _MinValue => ThisMapEventVariableAddress.MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最小値</summary>
        protected override int _SafetyMinValue => ThisMapEventVariableAddress.MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最大値</summary>
        protected override int _SafetyMaxValue => MaxValue_Common;

        /// <inheritdoc />
        /// <summary>最大値</summary>
        protected override int _MaxValue => MaxValue_Common;

        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>コモンイベントセルフ変数アドレス値としての最大値</summary>
        private static int MaxValue_Common => 1600009;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>"このマップイベント変数"かどうか</summary>
        public bool IsThisMapEventVariableAddress { get; }

        /// <summary>"このコモンイベント変数"かどうか</summary>
        public bool IsThisCommonEventVariableAddress { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">
        ///     [Range(1100000, 1100009)]
        ///     [Range(1600000, 1600009)]
        ///     変数アドレス値
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">valueが指定範囲外の場合</exception>
        public CalledEventVariableAddress(int value) : base(value)
        {
            // このコモンイベントセルフ変数（限定範囲）なら許可
            if (ThisCommonEventVariableAddress.MinValue <= value
                && value <= MaxValue_Common)
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
                $"{ThisCommonEventVariableAddress.MinValue}～{MaxValue_Common}のみ設定できます。" +
                $"(設定値：{value})");
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is VariableAddress other) return Equals(other);
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(CalledEventVariableAddress other)
        {
            return !(other is null) && Value == other.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string ResolveEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (IsThisCommonEventVariableAddress)
            {
                var common = (ThisCommonEventVariableAddress) Value;
                return common.MakeEventCommandString(resolver, type,
                    VariableAddressValueType.Numeric, desc);
            }

            var map = (ThisMapEventVariableAddress) Value;
            return map.MakeEventCommandString(resolver, type,
                VariableAddressValueType.Numeric, desc);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> CalledEventVariableAddress への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator CalledEventVariableAddress(int src)
        {
            var result = new CalledEventVariableAddress(src);
            return result;
        }

        /// <summary>
        /// CalledEventVariableAddress -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合
        /// </exception>
        public static implicit operator int(CalledEventVariableAddress src)
        {
            if (src is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(StringVariableAddress)));

            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺==右辺の場合true</returns>
        public static bool operator ==(CalledEventVariableAddress left, CalledEventVariableAddress right)
        {
            if (ReferenceEquals(left, right)) return true;

            if (left is null || right is null) return false;

            return left.Equals(right);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(CalledEventVariableAddress left, CalledEventVariableAddress right)
        {
            return !(left == right);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値が CalledEventVariableAddress の値として適切かどうかを返す。
        /// </summary>
        /// <param name="src">判定対象値</param>
        /// <returns>判定結果</returns>
        public static bool CanCast(int src)
        {
            // このコモンイベントセルフ変数（限定範囲）なら許可
            if (ThisCommonEventVariableAddress.MinValue <= src
                && src <= MaxValue_Common)
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