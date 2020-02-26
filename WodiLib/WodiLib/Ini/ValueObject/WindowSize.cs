// ========================================
// Project Name : WodiLib
// File Name    : WindowSize.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// ウィンドウサイズ
    /// </summary>
    [Serializable]
    public readonly struct WindowSize : IEquatable<WindowSize>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>X座標最大値</summary>
        public static readonly int MaxValue_Width = int.MaxValue;

        /// <summary>X座標最小値</summary>
        public static readonly int MinValue_Width = 0;

        /// <summary>Y座標最大値</summary>
        public static readonly int MaxValue_Height = int.MaxValue;

        /// <summary>Y座標最小値</summary>
        public static readonly int MinValue_Height = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[Range(0, int.MaxValue)] X座標</summary>
        public int X { get; }

        /// <summary>[Range(0, int.MaxValue)] Y座標</summary>
        public int Y { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">[Range(0, int.MaxValue)] X座標</param>
        /// <param name="y">[Range(0, int.MaxValue)] Y座標</param>
        /// <exception cref="ArgumentOutOfRangeException">x、yが指定範囲外の場合</exception>
        public WindowSize(int x, int y)
        {
            if (x < MinValue_Width || MaxValue_Width < x)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(x), MinValue_Width, MaxValue_Width, x));
            if (y < MinValue_Height || MaxValue_Height < y)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(y), MinValue_Height, MaxValue_Height, y));

            X = x;
            Y = y;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            return obj is WindowSize other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>比較結果</returns>
        public bool Equals(WindowSize other)
        {
            return X == other.X
                   && Y == other.Y;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Tuple&lt;int, string> -> WindowSize 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator WindowSize(Tuple<int, int> tuple)
        {
            if (tuple is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(tuple), nameof(WindowSize)));

            return new WindowSize(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// (int, string) -> WindowSize 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator WindowSize(ValueTuple<int, int> tuple)
        {
            return new WindowSize(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// WindowSize -> Tuple&lt;int, string> 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator Tuple<int, int>(WindowSize src)
        {
            return new Tuple<int, int>(src.X, src.Y);
        }

        /// <summary>
        /// WindowSize -> (int, string) 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator ValueTuple<int, int>(WindowSize src)
        {
            return (src.X, src.Y);
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
        public static bool operator ==(WindowSize left, WindowSize right)
        {
            // X座標
            if (left.X != right.X) return false;

            // Y座標
            if (left.Y != right.Y) return false;

            return true;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(WindowSize left, WindowSize right)
        {
            return !(left == right);
        }
    }
}