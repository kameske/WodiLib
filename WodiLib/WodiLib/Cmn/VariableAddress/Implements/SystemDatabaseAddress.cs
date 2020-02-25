// ========================================
// Project Name : WodiLib
// File Name    : SystemDatabaseAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(1300000000, 1399999999)]
    /// [SafetyRange(1300000000, 1399999920)]
    /// システムDB変アドレス値
    /// </summary>
    [Serializable]
    public class SystemDatabaseAddress : VariableAddress, IEquatable<SystemDatabaseAddress>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public new static int MinValue => 1300000000;

        /// <summary>安全に使用できる最小値</summary>
        public static int SafetyMinValue => MinValue;

        /// <summary>安全に使用できる最大値</summary>
        public static int SafetyMaxValue => 1399999920;

        /// <summary>最大値</summary>
        public new static int MaxValue => 1399999999;

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
        protected override int _SafetyMinValue => SafetyMinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最大値</summary>
        protected override int _SafetyMaxValue => SafetyMaxValue;

        /// <inheritdoc />
        /// <summary>最大値</summary>
        protected override int _MaxValue => MaxValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ｼｽﾃﾑDB({0},{1},{2})[{3} {4} ]";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>タイプID</summary>
        public TypeId TypeId { get; }

        /// <summary>データID</summary>
        public DataId DataId { get; }

        /// <summary>項目ID</summary>
        public ItemId ItemId { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">
        ///     [Range(1300000000, 1399999999)]
        ///     [SafetyRange(1300000000, 1399999920)]
        ///     変数アドレス値
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">valueがシステムDB変アドレス値として不適切な場合</exception>
        public SystemDatabaseAddress(int value) : base(value)
        {
            TypeId = value.SubInt(6, 2);
            DataId = value.SubInt(2, 4);
            ItemId = value.SubInt(0, 2);
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
        public bool Equals(SystemDatabaseAddress other)
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
            var dataName = resolver.GetDatabaseDataName(DBKind.System, TypeId, DataId).Item2;
            var itemName = resolver.GetDatabaseItemName(DBKind.System, TypeId, ItemId).Item2;

            return string.Format(EventCommandSentenceFormat,
                TypeId, DataId, ItemId, dataName, itemName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> SystemDatabaseAddress への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator SystemDatabaseAddress(int src)
        {
            var result = new SystemDatabaseAddress(src);
            return result;
        }

        /// <summary>
        /// SystemDatabaseAddress -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合
        /// </exception>
        public static implicit operator int(SystemDatabaseAddress src)
        {
            if (src is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(SystemDatabaseAddress)));

            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// システムDB変アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     加算後の値がシステムDB変アドレス値として不適切な場合
        /// </exception>
        public static SystemDatabaseAddress operator +(SystemDatabaseAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new SystemDatabaseAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"システムDB変アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// システムDB変アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     減算後の値がシステムDB変アドレス値として不適切な場合
        /// </exception>
        public static SystemDatabaseAddress operator -(SystemDatabaseAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new SystemDatabaseAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"システムDB変アドレス値として不適切な値です。(value = {src.Value - value})", ex);
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
        public static int operator -(SystemDatabaseAddress left, VariableAddress right)
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
        public static bool operator ==(SystemDatabaseAddress left, VariableAddress right)
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
        public static bool operator !=(SystemDatabaseAddress left, VariableAddress right)
        {
            return !(left == right);
        }

        #endregion

        #region SystemDatabaseAddress

        /// <summary>
        /// システムDB変アドレス値 - システムDB変アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">システムDB変数アドレス左辺</param>
        /// <param name="right">システムDB変数アドレス右辺</param>
        /// <returns>システムDB変アドレス値の差</returns>
        public static int operator -(SystemDatabaseAddress left, SystemDatabaseAddress right)
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
        public static bool operator ==(SystemDatabaseAddress left, SystemDatabaseAddress right)
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
        public static bool operator !=(SystemDatabaseAddress left, SystemDatabaseAddress right)
        {
            return !(left == right);
        }

        #endregion
    }
}