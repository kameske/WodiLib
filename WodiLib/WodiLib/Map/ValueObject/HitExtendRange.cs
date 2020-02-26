// ========================================
// Project Name : WodiLib
// File Name    : HitExtendRange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(0, 255)]
    ///     [SafetyRange(0, 250)]
    ///     接触範囲拡張
    /// </summary>
    [Serializable]
    public readonly struct HitExtendRange : IEquatable<HitExtendRange>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>幅最大値</summary>
        public static readonly int MaxValue_Width = 255;

        /// <summary>安全に使用できる幅最大値</summary>
        public static readonly int SafetyMaxValue_Width = 250;

        /// <summary>安全に使用できる幅最小値</summary>
        public static readonly int SafetyMinValue_Width = 0;

        /// <summary>幅最小値</summary>
        public static readonly int MinValue_Width = 0;

        /// <summary>高さ最大値</summary>
        public static readonly int MaxValue_Height = 255;

        /// <summary>安全に使用できる高さ最大値</summary>
        public static readonly int SafetyMaxValue_Height = 250;

        /// <summary>安全に使用できる高さ最小値</summary>
        public static readonly int SafetyMinValue_Height = 0;

        /// <summary>高さ最小値</summary>
        public static readonly int MinValue_Height = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     [Range(0, 255)]
        ///     [SafetyRange(0, 250)]
        ///     幅
        /// </summary>
        public byte Width { get; }

        /// <summary>
        ///     [Range(0, 255)]
        ///     [SafetyRange(0, 250)]
        ///     高さ
        /// </summary>
        public byte Height { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="width">
        ///     [Range(0, 255)]
        ///     [SafetyRange(0, 250)]
        ///     幅
        /// </param>
        /// <param name="height">
        ///     [Range(0, 255)]
        ///     [SafetyRange(0, 250)]
        ///     高さ
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">width、heightが指定範囲外の場合</exception>
        public HitExtendRange(byte width, byte height)
        {
            if (width < MinValue_Width || MaxValue_Width < width)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(width), MinValue_Width, MaxValue_Width, width));
            if (height < MinValue_Height || MaxValue_Height < height)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(height), MinValue_Height, MaxValue_Height, height));

            if (width < SafetyMinValue_Width || SafetyMaxValue_Width < width)
                WodiLibLogger.GetInstance().Warning(
                    WarningMessage.OutOfRange(nameof(width), SafetyMinValue_Width,
                        SafetyMaxValue_Width, width));
            if (height < SafetyMinValue_Height || SafetyMaxValue_Height < height)
                WodiLibLogger.GetInstance().Warning(
                    WarningMessage.OutOfRange(nameof(height), SafetyMinValue_Height,
                        SafetyMaxValue_Height, height));

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

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is HitExtendRange other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(HitExtendRange other)
        {
            return Width == other.Width
                   && Height == other.Height;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Tuple&lt;int, int> -> HitExtendRange 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        /// <exception cref="InvalidCastException">
        ///     tuple が null の場合
        /// </exception>
        public static implicit operator HitExtendRange(Tuple<byte, byte> tuple)
        {
            if (tuple is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(tuple), nameof(HitExtendRange)));

            return new HitExtendRange(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// (int, int) -> HitExtendRange 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator HitExtendRange(ValueTuple<byte, byte> tuple)
        {
            return new HitExtendRange(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// HitExtendRange -> Tuple&lt;int, int> 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator Tuple<byte, byte>(HitExtendRange src)
        {
            return new Tuple<byte, byte>(src.Width, src.Height);
        }

        /// <summary>
        /// HitExtendRange -> (int, int) 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator ValueTuple<byte, byte>(HitExtendRange src)
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
        public static bool operator ==(HitExtendRange left, HitExtendRange right)
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
        public static bool operator !=(HitExtendRange left, HitExtendRange right)
        {
            return !(left == right);
        }
    }
}