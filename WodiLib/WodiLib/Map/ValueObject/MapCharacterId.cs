// ========================================
// Project Name : WodiLib
// File Name    : MapCharacterId.cs
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
    /// [Range(int.MinValue, int.MaxValue)] マップキャラクタID
    /// </summary>
    [Serializable]
    public readonly struct MapCharacterId : IConvertibleInt, IEquatable<MapCharacterId>, IComparable<MapCharacterId>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大値</summary>
        public static readonly int MaxValue = int.MaxValue;

        /// <summary>最小値</summary>
        public static readonly int MinValue = int.MinValue;

        /// <summary>"このマップイベントID"を示すインスタンス</summary>
        public static MapCharacterId ThisMapEvent = new MapCharacterId(-1);

        /// <summary>"主人公"を示すインスタンス</summary>
        public static MapCharacterId Hero = new MapCharacterId(-2);

        /// <summary>"仲間1"を示すインスタンス</summary>
        public static MapCharacterId Member1 = new MapCharacterId(-3);

        /// <summary>"仲間2"を示すインスタンス</summary>
        public static MapCharacterId Member2 = new MapCharacterId(-4);

        /// <summary>"仲間3"を示すインスタンス</summary>
        public static MapCharacterId Member3 = new MapCharacterId(-5);

        /// <summary>"仲間4"を示すインスタンス</summary>
        public static MapCharacterId Member4 = new MapCharacterId(-6);

        /// <summary>"仲間5"を示すインスタンス</summary>
        public static MapCharacterId Member5 = new MapCharacterId(-7);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>マップキャラクタID</summary>
        private int Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number">[Range(int.MinValue, int.MaxValue)] マップキャラクタID</param>
        /// <exception cref="ArgumentOutOfRangeException">numberがマップキャラクタIDとして不適切な場合</exception>
        public MapCharacterId(int number)
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
            return obj is MapCharacterId other && Equals(other);
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
        public int ToInt() => Value;

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
        public bool Equals(MapCharacterId other)
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
        public int CompareTo(MapCharacterId other)
        {
            return Value - other.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> MapCharacterId への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator MapCharacterId(int src)
        {
            var result = new MapCharacterId(src);
            return result;
        }

        /// <summary>
        /// MapCharacterId -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator int(MapCharacterId src)
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
        public static bool operator ==(MapCharacterId left, MapCharacterId right)
        {
            return left.Value == right.Value;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(MapCharacterId left, MapCharacterId right)
        {
            return !(left == right);
        }

        /// <summary>
        /// &lt;
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&lt;右辺の場合true</returns>
        public static bool operator <(MapCharacterId left, MapCharacterId right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// &lt;=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&lt;=右辺の場合true</returns>
        public static bool operator <=(MapCharacterId left, MapCharacterId right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// &gt;=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&gt;=右辺の場合true</returns>
        public static bool operator >=(MapCharacterId left, MapCharacterId right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// &gt;
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺&gt;右辺の場合true</returns>
        public static bool operator >(MapCharacterId left, MapCharacterId right)
        {
            return left.CompareTo(right) > 0;
        }
    }
}