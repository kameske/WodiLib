// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : OperationResultReturnCodeType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums
{
    /// <summary>
    ///     演算結果返却方法種別
    /// </summary>
    internal class OperationResultReturnCodeType : InitializeEnumSourceAddable
    {
        /// <summary>コンストラクタによる生成</summary>
        public const int Code_New = 0;

        /// <summary>明示的型変換による生成</summary>
        public const int Code_ImplicitCast = 1;

        /// <summary>暗黙的型変換による生成</summary>
        public const int Code_ExplicitCast = 2;

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Enums;

        /// <inheritdoc/>
        public override string EnumName => nameof(OperationResultReturnCodeType);

        /// <inheritdoc/>
        public override string Summary => "演算結果返却方法種別";

        public static EnumMember New = (nameof(New), "コンストラクタによる生成", Code_New);
        public static EnumMember ImplicitCast = (nameof(ImplicitCast), "明示的型変換による生成", Code_ImplicitCast);
        public static EnumMember ExplicitCast = (nameof(ExplicitCast), "暗黙的型変換による生成", Code_ExplicitCast);

        public override IEnumerable<EnumMember> Members()
            => new[]
            {
                New,
                ImplicitCast,
                ExplicitCast
            };

        private OperationResultReturnCodeType()
        {
        }

        public static OperationResultReturnCodeType Instance { get; } = new();
    }
}
