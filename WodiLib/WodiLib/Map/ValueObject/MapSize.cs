// ========================================
// Project Name : WodiLib
// File Name    : MapSize.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Map
{
    /// <summary>
    /// マップサイズ
    /// </summary>
    internal struct MapSize
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>マップサイズ横</summary>
        public MapSizeWidth Width { get; }

        /// <summary>マップサイズ縦</summary>
        public MapSizeHeight Height { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="width">マップサイズ横</param>
        /// <param name="height">マップサイズ縦</param>
        public MapSize(MapSizeWidth width, MapSizeHeight height)
        {
            Width = width;
            Height = height;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Width)} = {Width}, {nameof(Height)} = {Height}";
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is MapSize other &&
                   (Width == other.Width
                    && Height == other.Height);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Tuple&lt;MapSizeWidth, MapSizeHeight> -> MapSize 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator MapSize(Tuple<MapSizeWidth, MapSizeHeight> tuple)
        {
            return new MapSize(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// (MapSizeWidth, MapSizeHeight) -> MapSize 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator MapSize(ValueTuple<MapSizeWidth, MapSizeHeight> tuple)
        {
            return new MapSize(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// MapSize -> Tuple&lt;MapSizeWidth, MapSizeHeight> 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator Tuple<MapSizeWidth, MapSizeHeight>(MapSize src)
        {
            return new Tuple<MapSizeWidth, MapSizeHeight>(src.Width, src.Height);
        }

        /// <summary>
        /// MapSize -> (MapSizeWidth, MapSizeHeight) 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator ValueTuple<MapSizeWidth, MapSizeHeight>(MapSize src)
        {
            return (src.Width, src.Height);
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
        public static bool operator ==(MapSize left, MapSize right)
        {
            // 幅
            if (left.Width != right.Width) return false;

            // 高さ
            if (left.Height != right.Height) return false;

            return true;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(MapSize left, MapSize right)
        {
            return !(left == right);
        }
    }
}