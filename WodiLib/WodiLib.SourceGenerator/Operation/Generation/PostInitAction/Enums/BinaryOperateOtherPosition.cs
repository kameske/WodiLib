// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BinaryOperateOtherPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums
{
    /// <summary>
    ///     二項演算子・他方要素のポジション
    /// </summary>
    internal class BinaryOperateOtherPosition : InitializeEnumSourceAddable
    {
        public static readonly int Code_Left = 0;
        public static readonly int Code_Right = 1;

        public override string NameSpace => GenerationConst.NameSpaces.Enums;
        public override string EnumName => nameof(BinaryOperateOtherPosition);
        public override string Summary => "二項演算子・他方要素のポジション";

        public static EnumMember Left = (nameof(Left), "左項", Code_Left);
        public static EnumMember Right = (nameof(Right), "右項", Code_Right);

        public override IEnumerable<EnumMember> Members()
            => new[]
            {
                Left,
                Right
            };

        private BinaryOperateOtherPosition()
        {
        }

        public static BinaryOperateOtherPosition Instance { get; } = new();
    }
}
