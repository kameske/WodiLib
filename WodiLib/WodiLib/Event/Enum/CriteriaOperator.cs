// ========================================
// Project Name : WodiLib
// File Name    : CriteriaOperator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    ///     条件演算子
    /// </summary>
    public record CriteriaOperator : TypeSafeEnum<CriteriaOperator>
    {
        /// <summary>より大きい</summary>
        public static readonly CriteriaOperator Above;

        /// <summary>以上</summary>
        public static readonly CriteriaOperator Greater;

        /// <summary>同じ</summary>
        public static readonly CriteriaOperator Equal;

        /// <summary>以下</summary>
        public static readonly CriteriaOperator Less;

        /// <summary>未満</summary>
        public static readonly CriteriaOperator Below;

        /// <summary>以外</summary>
        public static readonly CriteriaOperator Not;

        /// <summary>ビットを満たす</summary>
        public static readonly CriteriaOperator BitAnd;

        /// <summary>デフォルト値</summary>
        public static readonly CriteriaOperator Default;

        /// <summary>値</summary>
        public byte Code { get; }

        static CriteriaOperator()
        {
            Above = new CriteriaOperator("Above", 0x00);
            Greater = new CriteriaOperator("Greater", 0x10);
            Equal = new CriteriaOperator("Equal", 0x20);
            Less = new CriteriaOperator("Less", 0x30);
            Below = new CriteriaOperator("Below", 0x40);
            Not = new CriteriaOperator("Not", 0x50);
            BitAnd = new CriteriaOperator("BitAnd", 0x60);
            Default = Equal;
        }

        private CriteriaOperator(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static CriteriaOperator FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
