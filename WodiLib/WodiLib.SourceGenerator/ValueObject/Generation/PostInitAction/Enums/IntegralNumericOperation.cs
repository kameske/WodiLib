// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IntegralNumericOperation.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums
{
    /// <summary>
    ///     整数数値型演算子
    /// </summary>
    internal class IntegralNumericOperation : InitializeEnumSourceAddable
    {
        private static readonly int FlagValue_IncreaseAndDecreasable = (int)Math.Pow(2, 0);
        private static readonly int FlagValue_AddAndSubtractable = (int)Math.Pow(2, 1);
        private static readonly int FlagValue_MultipleAndDivide = (int)Math.Pow(2, 2);
        private static readonly int FlagValue_Modulo = (int)Math.Pow(2, 3);
        private static readonly int FlagValue_Complement = (int)Math.Pow(2, 4);
        private static readonly int FlagValue_Shift = (int)Math.Pow(2, 5);
        private static readonly int FlagValue_And = (int)Math.Pow(2, 6);
        private static readonly int FlagValue_Or = (int)Math.Pow(2, 7);
        private static readonly int FlagValue_Xor = (int)Math.Pow(2, 8);
        private static readonly int FlagValue_Compare = (int)Math.Pow(2, 9);

        public override string NameSpace => GenerationConst.NameSpaces.Enums;
        public override string EnumName => nameof(IntegralNumericOperation);
        public override string Summary => "整数数値型演算子";
        public override bool IsFlags => true;

        public override IEnumerable<EnumMember> Members()
            => new EnumMember[]
            {
                ("IncreaseAndDecreasable", "インクリメント・デクリメント", FlagValue_IncreaseAndDecreasable),
                ("AddAndSubtractable", "加減算", FlagValue_AddAndSubtractable),
                ("MultipleAndDivide", "乗除算", FlagValue_MultipleAndDivide),
                ("Modulo", "剰余", FlagValue_Modulo),
                ("Complement", "反転", FlagValue_Complement),
                ("Shift", "シフト", FlagValue_Shift),
                ("And", "BitAnd", FlagValue_And),
                ("Or", "BitOr", FlagValue_Or),
                ("Xor", "BitXOr", FlagValue_Xor),
                ("Compare", "比較", FlagValue_Compare),
                ("All", "全項目", FlagValue_IncreaseAndDecreasable
                               + FlagValue_AddAndSubtractable
                               + FlagValue_MultipleAndDivide
                               + FlagValue_Modulo
                               + FlagValue_Complement
                               + FlagValue_Shift
                               + FlagValue_And
                               + FlagValue_Or
                               + FlagValue_Xor
                               + FlagValue_Compare)
            };

        public static bool CanIncreaseAndDecrease(string? flagStr)
            => HasFlag(flagStr, FlagValue_IncreaseAndDecreasable);

        public static bool CanAddAndSubtract(string? flagStr)
            => HasFlag(flagStr, FlagValue_AddAndSubtractable);

        public static bool CanMultipleAndDivide(string? flagStr)
            => HasFlag(flagStr, FlagValue_MultipleAndDivide);

        public static bool CanModulo(string? flagStr)
            => HasFlag(flagStr, FlagValue_Modulo);

        public static bool CanComplement(string? flagStr)
            => HasFlag(flagStr, FlagValue_Complement);

        public static bool CanShift(string? flagStr)
            => HasFlag(flagStr, FlagValue_Shift);

        public static bool CanAnd(string? flagStr)
            => HasFlag(flagStr, FlagValue_And);

        public static bool CanOr(string? flagStr)
            => HasFlag(flagStr, FlagValue_Or);

        public static bool CanXor(string? flagStr)
            => HasFlag(flagStr, FlagValue_Xor);

        public static bool CanCompare(string? flagStr)
            => HasFlag(flagStr, FlagValue_Compare);

        private static bool HasFlag(string? flagStr, int flagValue)
        {
            if (!int.TryParse(flagStr, out var code)) return false;
            return (code & flagValue) != 0;
        }

        private IntegralNumericOperation()
        {
        }

        public static IntegralNumericOperation Instance { get; } = new();
    }
}
