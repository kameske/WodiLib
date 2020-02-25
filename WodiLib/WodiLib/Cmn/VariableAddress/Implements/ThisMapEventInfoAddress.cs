// ========================================
// Project Name : WodiLib
// File Name    : ThisMapEventInfoAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(9190000, 9199999)] このマップイベント情報アドレス値
    /// </summary>
    [Serializable]
    public class ThisMapEventInfoAddress : VariableAddress, IEquatable<ThisMapEventInfoAddress>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public new static int MinValue => 9190000;

        /// <summary>最大値</summary>
        public new static int MaxValue => 9199999;

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

        /// <summary>取得情報</summary>
        public InfoAddressInfoType InfoType { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(9190000, 9199999)] 変数アドレス値</param>
        /// <exception cref="ArgumentOutOfRangeException">valueがこのマップイベント情報アドレス値として不適切な場合</exception>
        public ThisMapEventInfoAddress(int value) : base(value)
        {
            InfoType = InfoAddressInfoType.FromCode(Value.SubInt(0, 1));

            VersionCheck(value);
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
            => InfoType.MakeEventCommandSentenceForThisMapEvent();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バージョンによる定義チェックを行い、警告メッセージを出力する
        /// </summary>
        /// <param name="value">変数アドレス値</param>
        private static void VersionCheck(int value)
        {
            var infoCode = value % 10;

            if (infoCode == 7 || infoCode == 8)
            {
                WodiLibLogger.GetInstance().Warning(VersionWarningMessage.NotUsingVariableAddress(value));
            }
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
        public bool Equals(ThisMapEventInfoAddress other)
        {
            return !(other is null) && Value == other.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> ThisMapEventInfoAddress への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator ThisMapEventInfoAddress(int src)
        {
            var result = new ThisMapEventInfoAddress(src);
            return result;
        }

        /// <summary>
        /// ThisMapEventInfoAddress -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合
        /// </exception>
        public static implicit operator int(ThisMapEventInfoAddress src)
        {
            if (src is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(ThisMapEventInfoAddress)));

            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// このマップイベント情報アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     加算後の値がこのマップイベント情報アドレス値として不適切な場合
        /// </exception>
        public static ThisMapEventInfoAddress operator +(ThisMapEventInfoAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new ThisMapEventInfoAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"このマップイベント情報アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// このマップイベント情報アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     減算後の値がこのマップイベント情報アドレス値として不適切な場合
        /// </exception>
        public static ThisMapEventInfoAddress operator -(ThisMapEventInfoAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return new ThisMapEventInfoAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"このマップイベント情報アドレス値として不適切な値です。(value = {src.Value - value})", ex);
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
        public static int operator -(ThisMapEventInfoAddress left, VariableAddress right)
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
        public static bool operator ==(ThisMapEventInfoAddress left, VariableAddress right)
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
        public static bool operator !=(ThisMapEventInfoAddress left, VariableAddress right)
        {
            return !(left == right);
        }

        #endregion

        #region ThisMapEventInfoAddress

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺==右辺の場合true</returns>
        public static bool operator ==(ThisMapEventInfoAddress left, ThisMapEventInfoAddress right)
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
        public static bool operator !=(ThisMapEventInfoAddress left, ThisMapEventInfoAddress right)
        {
            return !(left == right);
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(InfoType), InfoType.Code);
            info.AddValue(nameof(Value), Value);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ThisMapEventInfoAddress(SerializationInfo info, StreamingContext context) : base(
            ((Func<int>) (() => info.GetInt32(nameof(Value))))())
        {
            InfoType = InfoAddressInfoType.FromCode(info.GetInt32(nameof(InfoType)));
        }
    }
}