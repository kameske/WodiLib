// ========================================
// Project Name : WodiLib
// File Name    : IntOrStrType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    ///     所有タイプフラグ
    /// </summary>
    public class IntOrStrType : TypeSafeEnum<IntOrStrType>
    {
        /// <summary>数値</summary>
        public static readonly IntOrStrType Int;

        /// <summary>文字列</summary>
        public static readonly IntOrStrType Str;

        /// <summary>数値＆文字列</summary>
        public static readonly IntOrStrType IntAndStr;

        /// <summary>なし</summary>
        public static readonly IntOrStrType None;

        static IntOrStrType()
        {
            Int = new IntOrStrType(nameof(Int));
            Str = new IntOrStrType(nameof(Str));
            IntAndStr = new IntOrStrType(nameof(IntAndStr));
            None = new IntOrStrType(nameof(None));
        }

        private IntOrStrType(string id) : base(id)
        {
        }

        internal static IntOrStrType FromId(string id)
        {
            return AllItems.First(x => x.Id.Equals(id));
        }
    }
}
