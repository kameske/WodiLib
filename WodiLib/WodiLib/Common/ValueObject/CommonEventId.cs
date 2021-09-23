// ========================================
// Project Name : WodiLib
// File Name    : CommonEventId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    ///     [Range(0, 9999)][Range(500000, 509999)] コモンイベントID
    /// </summary>
    [CommonIntValueObject]
    public partial class CommonEventId
    {
        /// <summary>許容最大値</summary>
        public static int MaxValue => Const_MaxValue;

        /// <summary>許容最大値（定数）</summary>
        public const int Const_MaxValue = 9999;

        /// <summary>許容最小値</summary>
        public static int MinValue => Const_MinValue;

        /// <summary>許容最小値（定数）</summary>
        public const int Const_MinValue = 0;

        /// <summary>コモンイベント指定時のオフセット</summary>
        public static int CommonEventOffset = 500000;

        /// <summary>コモンイベントを相対指定したときのオフセット（基準値）</summary>
        public static int CommonEventRelativeOffset = 600100;

        /// <summary>コモンイベントを相対指定したときのオフセット（最小値）</summary>
        public static int CommonEventRelativeOffset_Min = 600050;

        /// <summary>コモンイベントを相対指定したときのオフセット（最大値）</summary>
        public static int CommonEventRelativeOffset_Max = 600150;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        partial void DoConstructorExpansion(int value)
        {
            if ((value < MinValue || MaxValue < value)
                && (value < MinValue + CommonEventOffset || MaxValue + CommonEventOffset < value))
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(value), MinValue, MaxValue, value));
            RawValue = value >= CommonEventOffset
                ? value - CommonEventOffset
                : value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     byte配列に変換する。
        /// </summary>
        /// <param name="endian">エンディアン</param>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes(Endian endian) => RawValue.ToBytes(endian);
    }
}
