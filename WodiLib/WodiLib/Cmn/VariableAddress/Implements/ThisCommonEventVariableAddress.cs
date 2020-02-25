// ========================================
// Project Name : WodiLib
// File Name    : ThisCommonEventVariableAddress.cs
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
    /// [Range(1600000, 1600099)] このコモンイベントセルフ変数アドレス値
    /// </summary>
    [Serializable]
    public class ThisCommonEventVariableAddress : VariableAddress, IEquatable<ThisCommonEventVariableAddress>
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

        /// <summary>最小値</summary>
        public new static int MinValue => 1600000;

        /// <summary>最大値</summary>
        public new static int MaxValue => 1600099;

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(1600000, 1600099)] 変数アドレス値</param>
        /// <exception cref="ArgumentOutOfRangeException">valueがこのコモンイベントセルフ変数アドレス値として不適切な場合</exception>
        public ThisCommonEventVariableAddress(int value) : base(value)
        {
            Index = value.SubInt(0, 2);
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
        public bool Equals(ThisCommonEventVariableAddress other)
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
            if (type == EventCommandSentenceType.Map) return EventCommandSentenceMapEventFormat;

            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));
            if (desc.CommonEventId is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc.CommonEventId)));

            var variableName = resolver.GetCommonEventSelfVariableName(
                (CommonEventId) desc.CommonEventId, Index);

            var varNameStr = ((string) variableName).Equals(string.Empty)
                ? string.Empty
                : string.Format(EventCommandSentenceCommonEventName, variableName);

            return string.Format(EventCommandSentenceCommonEventFormat,
                Index, varNameStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> ThisCommonEventVariableAddress への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator ThisCommonEventVariableAddress(int src)
        {
            var result = new ThisCommonEventVariableAddress(src);
            return result;
        }

        /// <summary>
        /// ThisCommonEventVariableAddress -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合
        /// </exception>
        public static implicit operator int(ThisCommonEventVariableAddress src)
        {
            if (src is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(ThisCommonEventVariableAddress)));

            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// このコモンイベントセルフ変数アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     加算後の値がこのコモンイベントセルフ変数アドレス値として不適切な場合
        /// </exception>
        public static ThisCommonEventVariableAddress operator +(ThisCommonEventVariableAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new ThisCommonEventVariableAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"このコモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// このコモンイベントセルフ変数アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     減算後の値がこのコモンイベントセルフ変数アドレス値として不適切な場合
        /// </exception>
        public static ThisCommonEventVariableAddress operator -(ThisCommonEventVariableAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new ThisCommonEventVariableAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"このコモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value - value})", ex);
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
        public static int operator -(ThisCommonEventVariableAddress left, VariableAddress right)
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
        public static bool operator ==(ThisCommonEventVariableAddress left, VariableAddress right)
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
        public static bool operator !=(ThisCommonEventVariableAddress left, VariableAddress right)
        {
            return !(left == right);
        }

        #endregion

        #region ThisCommonEventVariableAddress

        /// <summary>
        /// このコモンイベントセルフ変数アドレス値 - このコモンイベントセルフ変数アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">このコモンイベントセルフ変数アドレス左辺</param>
        /// <param name="right">このコモンイベントセルフ変数アドレス右辺</param>
        /// <returns>このコモンイベントセルフ変数アドレス値の差</returns>
        public static int operator -(ThisCommonEventVariableAddress left, ThisCommonEventVariableAddress right)
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
        public static bool operator ==(ThisCommonEventVariableAddress left, ThisCommonEventVariableAddress right)
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
        public static bool operator !=(ThisCommonEventVariableAddress left, ThisCommonEventVariableAddress right)
        {
            return !(left == right);
        }

        #endregion
    }
}