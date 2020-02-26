// ========================================
// Project Name : WodiLib
// File Name    : MapEventId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// [Range(-1, 9999)] マップイベントID
    /// </summary>
    [Serializable]
    public readonly struct MapEventId : IConvertibleInt, IEquatable<MapEventId>, IComparable<MapEventId>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大値</summary>
        public static readonly int MaxValue = 9999;

        /// <summary>最小値</summary>
        public static readonly int MinValue = -1;

        /// <summary>"このマップイベントID"を示すインスタンス</summary>
        public static MapEventId ThisMapEvent = new MapEventId(-1);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>マップイベントID</summary>
        private int Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number">[Range(-1, 9999)] マップイベントID</param>
        /// <exception cref="ArgumentOutOfRangeException">numberがマップイベントIDとして不適切な場合</exception>
        public MapEventId(int number)
        {
            if (number < MinValue || MaxValue < number)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(number), MinValue, MaxValue, number));
            Value = number;
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
            return obj is MapEventId other && Equals(other);
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
        public int ToInt() => this;

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
        public bool Equals(MapEventId other)
        {
            return Value == other.Value;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>
        ///     このインスタンスが<paramref name="other"/>より小さい場合：0より小さい値
        ///     このインスタンスが<paramref name="other"/>と同等の場合：0
        ///     このインスタンスが<paramref name="other"/>より大きい場合：0より大きい値
        /// </returns>
        public int CompareTo(MapEventId other)
        {
            return Value - other.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> MapEventId への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator MapEventId(int src)
        {
            var result = new MapEventId(src);
            return result;
        }

        /// <summary>
        /// MapEventId -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator int(MapEventId src)
        {
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
        /// <returns>左辺と右辺の</returns>
        public static bool operator ==(MapEventId left, MapEventId right)
        {
            return left.Value == right.Value;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(MapEventId left, MapEventId right)
        {
            return !(left == right);
        }

        /// <summary>
        /// &lt;
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&lt;右辺の場合true</returns>
        public static bool operator <(MapEventId left, MapEventId right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// &lt;=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&lt;=右辺の場合true</returns>
        public static bool operator <=(MapEventId left, MapEventId right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// &gt;=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&gt;=右辺の場合true</returns>
        public static bool operator >=(MapEventId left, MapEventId right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// &gt;
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&gt;右辺の場合true</returns>
        public static bool operator >(MapEventId left, MapEventId right)
        {
            return left.CompareTo(right) > 0;
        }
    }
}