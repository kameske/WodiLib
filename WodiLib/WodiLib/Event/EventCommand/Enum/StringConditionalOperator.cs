// ========================================
// Project Name : WodiLib
// File Name    : StringConditionalOperator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 条件（文字列）条件演算子
    /// </summary>
    public class StringConditionalOperator : TypeSafeEnum<StringConditionalOperator>
    {
        /// <summary>同じ</summary>
        public static readonly StringConditionalOperator Equal;

        /// <summary>以外</summary>
        public static readonly StringConditionalOperator Not;

        /// <summary>を含む</summary>
        public static readonly StringConditionalOperator Exists;

        /// <summary>が先頭にある</summary>
        public static readonly StringConditionalOperator StartWith;

        /// <summary>値</summary>
        public byte Code { get; }

        static StringConditionalOperator()
        {
            Equal = new StringConditionalOperator(nameof(Equal), 0x00);
            Not = new StringConditionalOperator(nameof(Not), 0x10);
            Exists = new StringConditionalOperator(nameof(Exists), 0x20);
            StartWith = new StringConditionalOperator(nameof(StartWith), 0x30);
        }

        private StringConditionalOperator(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="src">バイト値</param>
        /// <returns>インスタンス</returns>
        public static StringConditionalOperator ForByte(byte src)
        {
            return _FindFirst(x => x.Code == src);
        }
    }
}