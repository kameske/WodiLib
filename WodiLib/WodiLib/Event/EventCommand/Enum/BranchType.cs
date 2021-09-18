// ========================================
// Project Name : WodiLib
// File Name    : BranchType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     分岐種別
    /// </summary>
    public class BranchType : TypeSafeEnum<BranchType>
    {
        /// <summary>選択肢</summary>
        public static readonly BranchType Choice;

        /// <summary>条件分岐（数値）</summary>
        public static readonly BranchType ConditionNumber;

        /// <summary>条件分岐（文字列）</summary>
        public static readonly BranchType ConditionString;

        static BranchType()
        {
            Choice = new BranchType(nameof(Choice));
            ConditionNumber = new BranchType(nameof(ConditionNumber));
            ConditionString = new BranchType(nameof(ConditionString));
        }

        private BranchType(string id) : base(id)
        {
        }
    }
}
