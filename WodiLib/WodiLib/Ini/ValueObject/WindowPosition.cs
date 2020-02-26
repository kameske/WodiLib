// ========================================
// Project Name : WodiLib
// File Name    : WindowPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// ウィンドウ座標
    /// </summary>
    [Serializable]
    public readonly struct WindowPosition : IEquatable<WindowPosition>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>X座標最大値</summary>
        public static readonly int MaxValue_X = int.MaxValue;

        /// <summary>X座標最小値</summary>
        public static readonly int MinValue_X = 0;

        /// <summary>Y座標最大値</summary>
        public static readonly int MaxValue_Y = int.MaxValue;

        /// <summary>Y座標最小値</summary>
        public static readonly int MinValue_Y = 0;

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
        public WindowPosition(int x, int y)
        {
            if (x < MinValue_X || MaxValue_X < x)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(x), MinValue_X, MaxValue_X, x));
            if (y < MinValue_Y || MaxValue_Y < y)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(y), MinValue_Y, MaxValue_Y, y));

            X = x;
            Y = y;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;

            return obj is WindowPosition other && Equals(other);
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
        public bool Equals(WindowPosition other)
        {
            return X == other.X
                   && Y == other.Y;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Tuple&lt;int, int> -> WindowPosition 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        /// <exception cref="InvalidCastException">
        ///     tuple が null の場合
        /// </exception>
        public static implicit operator WindowPosition(Tuple<int, int> tuple)
        {
            if (tuple is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(tuple), nameof(WindowPosition)));

            return new WindowPosition(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// (int, int) -> WindowPosition 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator WindowPosition(ValueTuple<int, int> tuple)
        {
            return new WindowPosition(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// WindowPosition -> Tuple&lt;int, int> 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator Tuple<int, int>(WindowPosition src)
        {
            return new Tuple<int, int>(src.X, src.Y);
        }

        /// <summary>
        /// WindowPosition -> (int, int) 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator ValueTuple<int, int>(WindowPosition src)
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
        public static bool operator ==(WindowPosition left, WindowPosition right)
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
        public static bool operator !=(WindowPosition left, WindowPosition right)
        {
            return !(left == right);
        }
    }
}