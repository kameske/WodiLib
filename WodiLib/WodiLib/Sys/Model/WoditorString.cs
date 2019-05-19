// ========================================
// Project Name : WodiLib
// File Name    : WoditorString.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WodiLib.Sys
{
    /// <summary>
    /// ウディタ仕様の文字列
    /// </summary>
    internal class WoditorString
    {
        /// <summary>ウディタ内部のエンコーディング</summary>
        public static Encoding WoditorEncoding
        {
            get
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                return Encoding.GetEncoding("shift-jis");
            }
        }

        /// <summary>
        /// 特殊変換文字群
        /// </summary>
        private static readonly List<Tuple<string, string>> SpecialConversionStringList =
            new List<Tuple<string, string>>
            {
                new Tuple<string, string>("\n", "<\\n>"),
                new Tuple<string, string>("\r\n", "<\\n>"),
                new Tuple<string, string>("\"", "<dqrt>")
            };

        /// <summary>文字列</summary>
        public string String { get; }

        /// <summary>文字列（byte[]）</summary>
        public IEnumerable<byte> StringByte { get; }

        /// <summary>文字列をbyte配列化したときの長さ</summary>
        public int ByteLength { get; }

        /// <summary>
        /// バイト配列からインスタンスを生成する。
        /// 配列末尾に不要なデータがあっても無視する。
        /// </summary>
        /// <param name="src">変換対象byte配列</param>
        /// <param name="offset">読み込み開始オフセット</param>
        public WoditorString(byte[] src, long offset = 0) : this(ref src, offset)
        {
        }

        /// <summary>
        /// バイト配列からインスタンスを生成する。
        /// 配列末尾に不要なデータがあっても無視する。
        /// </summary>
        /// <param name="src">変換対象byte配列</param>
        /// <param name="offset">読み込み開始オフセット</param>
        public WoditorString(ref byte[] src, long offset = 0)
        {
            var innerOffset = offset;

            var lengthByte = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                lengthByte[i] = src[innerOffset];
                innerOffset++;
            }

            var length = lengthByte.ToInt32(Endian.Woditor);
            // 文字列の末尾に '0' が付与されているため、一部の処理では無視する
            var removeZeroLength = length - 1;

            var chars = new byte[removeZeroLength];
            Array.Copy(src, innerOffset, chars, 0, removeZeroLength);

            String = WoditorEncoding.GetString(chars);

            // 長さ
            ByteLength = (int) (innerOffset - offset) + length;

            // byte配列
            var copy = new byte[ByteLength];
            Array.Copy(src, offset, copy, 0, ByteLength);
            StringByte = copy;
        }

        /// <summary>stringからインスタンスを生成する</summary>
        public WoditorString(string src)
        {
            // 文字列
            String = src;

            foreach (var (originStr, commandStr) in SpecialConversionStringList)
            {
                String = String.Replace(commandStr, originStr);
            }

            // byte配列
            var unityNewLineString = String;
            if (unityNewLineString.IsEmpty() || unityNewLineString.Last() != 0x00)
            {
                // 末尾 '0' が欠けているため補正
                unityNewLineString += '\0';
            }

            var stringByte = WoditorEncoding.GetBytes(unityNewLineString);
            var lengthByte = stringByte.Length.ToBytes(Endian.Woditor);

            var strBytes = new List<byte>();
            strBytes.AddRange(lengthByte);
            strBytes.AddRange(stringByte);
            StringByte = strBytes;

            // 長さ
            ByteLength = strBytes.Count;
        }

        /// <inheritdoc />
        public override string ToString() => String;
    }
}