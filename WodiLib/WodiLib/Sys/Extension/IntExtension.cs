// ========================================
// Project Name : WodiLib
// File Name    : IntExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    ///     int拡張クラス
    /// </summary>
    internal static class IntExtension
    {
        /// <summary>
        ///     エンディアンを指定して byte[] の指定したオフセットから始まる4バイトデータ -> int32 変換を行う
        /// </summary>
        /// <param name="value">変換対象バイト配列</param>
        /// <param name="endian">エンディアン</param>
        /// <param name="offset">読み込み開始オフセット</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="ArgumentException">
        ///     変換対象が0バイトの場合。
        /// </exception>
        public static int ToInt32(this byte[] value, Endian endian, long offset = 0)
        {
            var byteList = new List<byte>();
            var innerOffset = offset;
            while (true)
            {
                if (innerOffset >= value.Length) break;
                byteList.Add(value[innerOffset]);
                innerOffset++;
                if (byteList.Count == 4) break;
            }

            if (byteList.Count == 0) throw new ArgumentException("変換対象が存在しません");

            if (byteList.Count >= 4)
            {
                if (endian == Endian.Big)
                {
                    byteList.Reverse();
                }

                return BitConverter.ToInt32(byteList.ToArray(), 0);
            }

            var range = 4 - byteList.Count;
            if (endian == Endian.Big)
            {
                var tmpByteList = new List<byte>();
                for (var i = 0; i < range; i++)
                {
                    tmpByteList.Add(0);
                }

                tmpByteList.AddRange(byteList);
                byteList = tmpByteList;
            }
            else
            {
                for (var i = 0; i < range; i++)
                {
                    byteList.Add(0);
                }
            }

            return BitConverter.ToInt32(byteList.ToArray(), 0);
        }

        /// <summary>
        ///     1byte -> int32 変換を行う
        /// </summary>
        /// <param name="value">対象バイト</param>
        /// <returns>変換したint値</returns>
        public static int ToInt32(this byte value)
        {
            // リトルエンディアンとして扱う
            var bytes = new byte[] {value, 0x00, 0x00, 0x00};
            bytes = GetEndianBytes(bytes, Endian.Little);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        ///     リトル/ビッグエンディアンを指定し、intをバイト列に変換する。
        /// </summary>
        /// <param name="value">変換対象のint値</param>
        /// <param name="endian">エンディアン</param>
        /// <returns>変換後のバイト列</returns>
        public static byte[] ToBytes(this int value, Endian endian)
        {
            var bytes = BitConverter.GetBytes(value);
            return GetEndianBytes(bytes, endian);
        }

        /// <summary>
        ///     リトル/ビッグエンディアンを判定し、（必要ならば反転した）バイト列を返す。
        /// </summary>
        /// <param name="bytes">変換対象のint値</param>
        /// <param name="endian">エンディアン</param>
        /// <returns>変換後のバイト列</returns>
        private static byte[] GetEndianBytes(byte[] bytes, Endian endian)
        {
            if (BitConverter.IsLittleEndian ^ (endian == Endian.Little)) return bytes.Reverse().ToArray();

            return bytes;
        }

        /// <summary>
        ///     intをウディタ内部で使用するbyte配列に変換する。
        /// </summary>
        /// <param name="value">変換対象のint値</param>
        /// <returns>変換後のバイト列</returns>
        public static byte[] ToWoditorIntBytes(this int value)
        {
            return value.ToBytes(Endian.Woditor);
        }

        /// <summary>
        ///     指定した桁から指定桁数のIntを取り出す。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="beginColumn">
        ///     [Range(0, targetの桁数-1)] 取り出し開始桁（1の位＝0とする）
        /// </param>
        /// <param name="length">
        ///     [Range(1, 10)] 取り出す桁数
        /// </param>
        /// <returns>取り出した数値</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="beginColumn"/>, <paramref name="length"/> が指定範囲以外の場合。
        /// </exception>
        public static int SubInt(this int target, int beginColumn, int length)
        {
            const int lengthMin = 1;
            const int lengthMax = 10;
            if (length < lengthMin || lengthMax < length)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(length), lengthMin, lengthMax, length));

            var maxColumn = target == 0 ? 0 : (int) Math.Log10(target);

            if (beginColumn < 0 || maxColumn < beginColumn)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(beginColumn), 0, maxColumn, beginColumn));

            // 指定より下の桁を除去
            var result = target / (int) Math.Pow(10, beginColumn);
            // 指定より上の桁を除去
            result %= (int) Math.Pow(10, length);

            return result;
        }
    }
}
