// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BinaryOperationType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums
{
    /// <summary>
    ///     単項演算子
    /// </summary>
    internal class UnaryOperationType : InitializeFlagEnumSourceAddable
    {
        private static readonly int FlagValue_Increase = (int)Math.Pow(2, 0);
        private static readonly int FlagValue_Decrease = (int)Math.Pow(2, 1);
        private static readonly int FlagValue_Complement = (int)Math.Pow(2, 2);

        public override string NameSpace => GenerationConst.NameSpaces.Enums;
        public override string EnumName => nameof(UnaryOperationType);
        public override string Summary => "単項演算子";

        public static EnumMember Increase = ("Increase", "インクリメント", FlagValue_Increase);
        public static EnumMember Decrease = ("Decrease", "デクリメント", FlagValue_Decrease);
        public static EnumMember Xcrease = ("Xecrease", "インクリメント & デクリメント", FlagValue_Increase + FlagValue_Decrease);
        public static EnumMember Complement = ("Complement", "補完", FlagValue_Complement);

        public override IEnumerable<EnumMember> Members()
            => new[]
            {
                Increase,
                Decrease,
                Xcrease,
                Complement
            };

        public static bool CanIncrease(string? flagStr)
            => HasFlag(flagStr, FlagValue_Increase);

        public static bool CanDecrease(string? flagStr)
            => HasFlag(flagStr, FlagValue_Decrease);

        public static bool CanComplement(string? flagStr)
            => HasFlag(flagStr, FlagValue_Complement);

        private UnaryOperationType()
        {
        }

        public static UnaryOperationType Instance { get; } = new();
    }
}
