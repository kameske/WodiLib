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
    ///     二項演算子
    /// </summary>
    internal class BinaryOperationType : InitializeFlagEnumSourceAddable
    {
        private static readonly int FlagValue_Add = (int)Math.Pow(2, 0);
        private static readonly int FlagValue_Subtract = (int)Math.Pow(2, 1);
        private static readonly int FlagValue_Multiple = (int)Math.Pow(2, 2);
        private static readonly int FlagValue_Divide = (int)Math.Pow(2, 3);
        private static readonly int FlagValue_Modulo = (int)Math.Pow(2, 4);
        private static readonly int FlagValue_And = (int)Math.Pow(2, 5);
        private static readonly int FlagValue_Or = (int)Math.Pow(2, 6);
        private static readonly int FlagValue_Xor = (int)Math.Pow(2, 7);

        public override string NameSpace => GenerationConst.NameSpaces.Enums;
        public override string EnumName => nameof(BinaryOperationType);
        public override string Summary => "二項演算子";

        public static EnumMember Add = ("Add", "加算", FlagValue_Add);
        public static EnumMember Subtract = ("Subtract", "減算", FlagValue_Subtract);
        public static EnumMember AddAndSubtract = ("AddAndSubtract", "加減算", FlagValue_Add + FlagValue_Subtract);
        public static EnumMember Multiple = ("Multiple", "乗算", FlagValue_Multiple);
        public static EnumMember Divide = ("Divide", "除算", FlagValue_Divide);

        public static EnumMember MultipleAndDivide =
            ("MultipleAndDivide", "乗除算", FlagValue_Multiple + FlagValue_Divide);

        public static EnumMember FourArithmeticOperations = ("FourArithmeticOperations", "四則演算",
            FlagValue_Add + FlagValue_Subtract + FlagValue_Multiple + FlagValue_Divide);

        public static EnumMember Modulo = ("Modulo", "剰余", FlagValue_Modulo);
        public static EnumMember And = ("And", "And", FlagValue_And);
        public static EnumMember Or = ("Or", "Or", FlagValue_Or);
        public static EnumMember Xor = ("Xor", "Xor", FlagValue_Xor);

        public override IEnumerable<EnumMember> Members()
            => new[]
            {
                Add,
                Subtract,
                AddAndSubtract,
                Multiple,
                Divide,
                MultipleAndDivide,
                FourArithmeticOperations,
                Modulo,
                And,
                Or,
                Xor
            };

        public static bool CanAdd(string? flagStr)
            => HasFlag(flagStr, FlagValue_Add);

        public static bool CanSubtract(string? flagStr)
            => HasFlag(flagStr, FlagValue_Subtract);

        public static bool CanMultiple(string? flagStr)
            => HasFlag(flagStr, FlagValue_Multiple);

        public static bool CanDivide(string? flagStr)
            => HasFlag(flagStr, FlagValue_Divide);

        public static bool CanModulo(string? flagStr)
            => HasFlag(flagStr, FlagValue_Modulo);

        public static bool CanAnd(string? flagStr)
            => HasFlag(flagStr, FlagValue_And);

        public static bool CanOr(string? flagStr)
            => HasFlag(flagStr, FlagValue_Or);

        public static bool CanXor(string? flagStr)
            => HasFlag(flagStr, FlagValue_Xor);

        private BinaryOperationType()
        {
        }

        public static BinaryOperationType Instance { get; } = new();
    }
}
