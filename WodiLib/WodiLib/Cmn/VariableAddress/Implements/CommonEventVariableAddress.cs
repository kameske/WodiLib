// ========================================
// Project Name : WodiLib
// File Name    : CommonEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Common;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(15000000, 15999999)] コモンイベントセルフ変数アドレス値
    /// </summary>
    [Serializable]
    public class CommonEventVariableAddress : VariableAddress, IEquatable<CommonEventVariableAddress>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ｺﾓﾝEv{0}ｾﾙﾌ{1}{2}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public new static int MinValue => 15000000;

        /// <summary>最大値</summary>
        public new static int MaxValue => 15999999;

        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>最小値</summary>
        protected override int _MinValue => MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最小値</summary>
        protected override int _SafetyMinValue => MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最大値</summary>
        protected override int _SafetyMaxValue => MaxValue;

        /// <inheritdoc />
        /// <summary>最大値</summary>
        protected override int _MaxValue => MaxValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// セルフ変数インデックス
        /// </summary>
        public CommonEventVariableIndex Index { get; }

        /// <summary>
        /// 文字列変数フラグ
        /// </summary>
        public bool IsStringVariable => Index.IsStringIndex;

        /// <summary>コモンイベントID</summary>
        public CommonEventId CommonEventId { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(15000000, 15999999)] 変数アドレス値</param>
        /// <exception cref="ArgumentOutOfRangeException">valueがコモンイベントセルフ変数アドレス値として不適切な場合</exception>
        public CommonEventVariableAddress(int value) : base(value)
        {
            Index = value.SubInt(0, 2);
            CommonEventId = value.SubInt(2, 3);
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
        public bool Equals(CommonEventVariableAddress other)
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
            var commonVariableName = resolver.GetCommonEventSelfVariableName(CommonEventId, Index);
            if (!commonVariableName.Equals(string.Empty))
            {
                commonVariableName = $"[{commonVariableName}]";
            }

            return string.Format(EventCommandSentenceFormat, CommonEventId, Index, commonVariableName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> CommonEventVariableAddress への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator CommonEventVariableAddress(int src)
        {
            var result = new CommonEventVariableAddress(src);
            return result;
        }

        /// <summary>
        /// CommonEventVariableAddress -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合
        /// </exception>
        public static implicit operator int(CommonEventVariableAddress src)
        {
            if (src is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(CommonEventVariableAddress)));

            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// コモンイベントセルフ変数アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     加算後の値がコモンイベントセルフ変数アドレス値として不適切な場合
        /// </exception>
        public static CommonEventVariableAddress operator +(CommonEventVariableAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new CommonEventVariableAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"コモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// コモンイベントセルフ変数アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     減算後の値がコモンイベントセルフ変数アドレス値値として不適切な場合
        /// </exception>
        public static CommonEventVariableAddress operator -(CommonEventVariableAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new CommonEventVariableAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"コモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }

        #endregion

        #region VariableAddress

        /// <summary>
        /// アドレス値 - アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">アドレス左辺</param>
        /// <param name="right">アドレス右辺</param>
        /// <returns>アドレス値の差</returns>
        /// <exception cref="InvalidOperationException">
        ///    left, right が null の場合
        /// </exception>
        public static int operator -(CommonEventVariableAddress left, VariableAddress right)
        {
            if (left is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));
            if (right is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("右オペランド"));

            return left.Value - right;
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺==右辺の場合true</returns>
        public static bool operator ==(CommonEventVariableAddress left, VariableAddress right)
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
        public static bool operator !=(CommonEventVariableAddress left, VariableAddress right)
        {
            return !(left == right);
        }

        #endregion

        #region CommonEventVariableAddress

        /// <summary>
        /// コモンイベントセルフ変数アドレス値 - コモンイベントセルフ変数アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">コモンイベントセルフ変数アドレス左辺</param>
        /// <param name="right">コモンイベントセルフ変数アドレス右辺</param>
        /// <returns>コモンイベントセルフ変数アドレス値の差</returns>
        /// <exception cref="InvalidOperationException">
        ///    left, right が null の場合
        /// </exception>
        public static int operator -(CommonEventVariableAddress left, CommonEventVariableAddress right)
        {
            if (left is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));
            if (right is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("右オペランド"));

            return left.Value - right.Value;
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺==右辺の場合true</returns>
        public static bool operator ==(CommonEventVariableAddress left, CommonEventVariableAddress right)
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
        public static bool operator !=(CommonEventVariableAddress left, CommonEventVariableAddress right)
        {
            return !(left == right);
        }

        #endregion
    }
}