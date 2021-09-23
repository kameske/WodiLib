// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BinaryOperateAttribute.cs
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
    ///     二項演算子オーバーロード情報
    /// </summary>
    internal class BinaryOperateAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(BinaryOperateAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "二項演算子オーバーロード情報";

        public override bool AllowMultiple => true;

        public static readonly PropertyInfo Operation = new()
        {
            Name = nameof(Operation),
            Type = BinaryOperationType.Instance.TypeFullName,
            Summary = "オーバーロードする二項演算子",
            DefaultValue = $"({BinaryOperationType.Instance.TypeFullName}) 0",
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo OtherTypes = new()
        {
            Name = nameof(OtherTypes),
            Type = $"{typeof(Type).FullName}[]",
            Summary = $"他項の型",
            DefaultValue = null
        };

        public static readonly PropertyInfo InnerCastType = new()
        {
            Name = nameof(InnerCastType),
            Type = typeof(Type).FullName,
            Summary = $"計算時に内部でキャストする型",
            DefaultValue = null
        };

        public static readonly PropertyInfo OtherPosition = new()
        {
            Name = nameof(OtherPosition),
            Type = BinaryOperateOtherPosition.Instance.TypeFullName,
            Summary = $"他項の位置",
            DefaultValue =
                $"{BinaryOperateOtherPosition.Instance.TypeFullName}.{BinaryOperateOtherPosition.Left.MemberName}",
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo ReturnType = new()
        {
            Name = nameof(ReturnType),
            Type = typeof(Type).FullName,
            Summary = $"返却型",
            Remarks = "null の場合左項の型を返す",
            DefaultValue = null
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

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Operation,
                OtherTypes,
                InnerCastType,
                OtherPosition,
                ReturnType,
                ReturnCodeType
            };

        private BinaryOperateAttribute()
        {
        }

        public static BinaryOperateAttribute Instance { get; } = new();
    }
}
