// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ShiftOperationType.cs
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
    ///     シフト演算子
    /// </summary>
    internal class ShiftOperationType : InitializeFlagEnumSourceAddable
    {
        private static readonly int FlagValue_LeftShift = (int)Math.Pow(2, 0);
        private static readonly int FlagValue_RightShift = (int)Math.Pow(2, 1);

        public override string NameSpace => GenerationConst.NameSpaces.Enums;
        public override string EnumName => nameof(ShiftOperationType);
        public override string Summary => "シフト演算子";

        public static EnumMember Left = (nameof(Left), "左シフト", FlagValue_LeftShift);
        public static EnumMember Right = (nameof(Right), "右シフト", FlagValue_RightShift);
        public static EnumMember Both = (nameof(Both), "左右シフト", FlagValue_LeftShift + FlagValue_RightShift);

        public override IEnumerable<EnumMember> Members()
            => new[]
            {
                Left,
                Right,
                Both
            };

        public static bool CanLeftShift(string? flagStr)
            => HasFlag(flagStr, FlagValue_LeftShift);

        public static bool CanRightShift(string? flagStr)
            => HasFlag(flagStr, FlagValue_RightShift);

        private ShiftOperationType()
        {
        }

        public static ShiftOperationType Instance { get; } = new();
    }
}
