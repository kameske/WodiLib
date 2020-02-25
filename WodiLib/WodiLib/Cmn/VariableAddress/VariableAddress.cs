// ========================================
// Project Name : WodiLib
// File Name    : VariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Cmn
{
    /// <summary>
    /// 変数アドレス値基底クラス
    /// </summary>
    [Serializable]
    public abstract class VariableAddress : IConvertibleInt, IEquatable<VariableAddress>
    {
        /*
         * 演算子をオーバーロードしたいため、インタフェースは使用しない
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>変数アドレスとして認識される最大の数値</summary>
        public static int MaxValue => 2000000000;

        /// <summary>変数アドレスとして認識される最小の数値</summary>
        public static int MinValue => 1000000;

        /// <summary>変数種別</summary>
        public abstract VariableAddressValueType ValueType { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        protected abstract int _MinValue { get; }

        /// <summary>安全に使用できる最小値</summary>
        protected abstract int _SafetyMinValue { get; }

        /// <summary>安全に使用できる最大値</summary>
        protected abstract int _SafetyMaxValue { get; }

        /// <summary>最大値</summary>
        protected abstract int _MaxValue { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>アドレス値</summary>
        protected int Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">
        ///     [Range(_MinValue, _MaxValue)]
        ///     [SafetyRange(_SafetyMinValue, _SafetyMaxValue)]
        ///     変数アドレス値
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">value変数アドレス値として不適切な場合</exception>
        protected VariableAddress(int value)
        {
            if (value < _MinValue || _MaxValue < value)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(value), _MinValue, _MaxValue, value));

            if (value < _SafetyMinValue || _SafetyMaxValue < value)
                WodiLibLogger.GetInstance().Warning(
                    WarningMessage.OutOfRange(nameof(value), _SafetyMinValue, _SafetyMaxValue, value));

            Value = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((VariableAddress) obj);
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
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public virtual int ToInt() => this;

        /// <summary>
        /// byte配列に変換する。
        /// </summary>
        /// <param name="endian">エンディアン</param>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes(Endian endian) => Value.ToBytes(endian);

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(VariableAddress other)
        {
            return !(other is null) && Value == other.Value;
        }

        /// <summary>
        /// イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="sentenceType">[NotNull] イベントコマンド種別</param>
        /// <param name="valueType">[NotNull] 変数種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver, type が null の場合、
        ///     または必要なときに desc または desc のプロパティが null の場合
        /// </exception>
        public string MakeEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType sentenceType, VariableAddressValueType valueType,
            EventCommandSentenceResolveDesc desc)
        {
            if (resolver is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(resolver)));
            if (sentenceType is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(sentenceType)));
            if (valueType is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(valueType)));

            if (!ValueType.CheckTypeInclude(valueType))
            {
                return valueType.MakeEventCommandErrorSentence(Value);
            }

            return ResolveEventCommandString(resolver, sentenceType,
                desc);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string ResolveEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> VariableAddress への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator VariableAddress(int src)
        {
            return VariableAddressFactory.Create(src);
        }

        /// <summary>
        /// VariableAddress -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合
        /// </exception>
        public static implicit operator int(VariableAddress src)
        {
            if (src is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(ChangeableDatabaseAddress)));

            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// アドレス値 - アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">右辺</param>
        /// <param name="right">左辺</param>
        /// <returns>アドレス値の差</returns>
        /// <exception cref="InvalidOperationException">減算後の値がアドレス値値として不適切な場合</exception>
        /// <exception cref="InvalidOperationException">
        ///    left, right が null の場合
        /// </exception>
        public static int operator -(VariableAddress left, VariableAddress right)
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
        /// アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     加算後の値がアドレス値として不適切な場合
        /// </exception>
        public static VariableAddress operator +(VariableAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return src.Value + value;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">
        ///     left が null の場合、または
        ///     減算後の値がアドレス値値として不適切な場合
        /// </exception>
        public static VariableAddress operator -(VariableAddress src, int value)
        {
            if (src is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull("左オペランド"));

            try
            {
                return src.Value - value;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }
    }
}