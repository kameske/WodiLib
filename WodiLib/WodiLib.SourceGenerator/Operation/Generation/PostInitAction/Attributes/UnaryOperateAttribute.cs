// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : UnaryOperateAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums;

namespace WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     単項演算子オーバーロード情報
    /// </summary>
    internal class UnaryOperateAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(UnaryOperateAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "単項演算子オーバーロード情報";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Operation = new()
        {
            Name = nameof(Operation),
            Type = UnaryOperationType.Instance.TypeFullName,
            Summary = "オーバーロードする単項演算子",
            DefaultValue = $"({UnaryOperationType.Instance.TypeFullName}) 0",
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo InnerCastType = new()
        {
            Name = nameof(InnerCastType),
            Type = typeof(Type).FullName,
            Summary = $"計算時に内部でキャストする型"
        };

        public static readonly PropertyInfo ReturnCodeType = new()
        {
            Name = nameof(ReturnCodeType),
            Type = OperationResultReturnCodeType.Instance.TypeFullName,
            Summary = $"返却インスタンスの生成方法",
            DefaultValue =
                $"{OperationResultReturnCodeType.Instance.TypeFullName}.{OperationResultReturnCodeType.New.MemberName}",
            DefaultValueAsSourceCode = true
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class | AttributeTargets.Struct;

        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Operation,
                InnerCastType,
                ReturnCodeType
            };

        private UnaryOperateAttribute()
        {
        }

        public static UnaryOperateAttribute Instance { get; } = new();
    }
}
