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

namespace WodiLib.Cmn
{
    /// <summary>
    ///     変数呼び出し値基底クラス
    /// </summary>
    [VariableAddressBaseAttribute(MinValue = 1000000, MaxValue = 2000000000)]
    [VariableAddressGapCalculatableAttribute]
    [VariableAddressAddAndSubtractableInt]
    public abstract partial class VariableAddress
    {
        /*
         * 演算子をオーバーロードしたいため、インタフェースは使用しない
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>変数種別</summary>
        public abstract VariableAddressValueType ValueType { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public virtual int ToInt() => RawValue;

        /// <summary>
        ///     byte配列に変換する。
        /// </summary>
        /// <param name="endian">エンディアン</param>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes(Endian endian) => RawValue.ToBytes(endian);

        /// <summary>
        ///     イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="sentenceType">イベントコマンド種別</param>
        /// <param name="valueType">変数種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver, type が null の場合、
        ///     または必要なときに desc または desc のプロパティが null の場合
        /// </exception>
        public string MakeEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType sentenceType, VariableAddressValueType valueType,
            EventCommandSentenceResolveDesc? desc)
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
                return valueType.MakeEventCommandErrorSentence(RawValue);
            }

            return ResolveEventCommandString(resolver, sentenceType,
                desc);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string ResolveEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     int -> VariableAddress への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator VariableAddress(int src)
        {
            return VariableAddressFactory.Create(src);
        }

        /// <summary>
        ///     VariableAddress -> int への暗黙的な型変換
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

            return src.RawValue;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>++ 演算子</summary>
        /// <param name="src"/>
        /// <returns>演算結果</returns>
        public static VariableAddress operator ++(VariableAddress src) => src.RawValue++;

        /// <summary>-- 演算子</summary>
        /// <param name="src"/>
        /// <returns>演算結果</returns>
        public static VariableAddress operator --(VariableAddress src) => src.RawValue--;
    }
}
