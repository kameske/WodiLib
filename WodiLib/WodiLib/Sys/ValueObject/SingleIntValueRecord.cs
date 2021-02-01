// ========================================
// Project Name : WodiLib
// File Name    : SingleIntValueRecord.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// 単一数値レコード
    /// </summary>
    public abstract record SingleIntValueRecord<T> : IConvertibleInt, IComparable<T>
        where T : SingleIntValueRecord<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値上限値
        /// </summary>
        protected abstract int _MaxValue { get; }

        /// <summary>
        /// 値下限値
        /// </summary>
        protected abstract int _MinValue { get; }

        /// <summary>
        /// 値
        /// </summary>
        protected int Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(<see cref="_MaxValue"/>, <see cref="_MinValue"/>)] 値</param>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="value"/> が 指定範囲外の場合</exception>
        protected SingleIntValueRecord(int value)
        {
            ThrowHelper.ValidateArgumentValueRange(
                value < _MinValue || _MaxValue < value,
                nameof(value), value, _MinValue, _MaxValue);
            Value = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
            => Value.ToString();

        /// <summary>
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public int ToInt() => Value;

        /// <inheritdoc/>
        public int CompareTo(T? other)
        {
            if (other is null)
            {
                /*
                 * string.CompareTo() に null を渡すと0より大きな値が返ることから、
                 * 比較対象が null であれば 0より大きな値を返す。
                 */
                return 1;
            }

            return Value.CompareTo(other.Value);
        }
    }
}
