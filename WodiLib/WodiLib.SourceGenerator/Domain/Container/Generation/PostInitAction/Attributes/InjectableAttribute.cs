// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : InjectableAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.Domain.Container.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     WodiLibContainer で Injection するための情報
    /// </summary>
    internal class InjectableAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(InjectableAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "WodiLibContainer で Injection するための情報";

        /// <inheritdoc/>
        public override string Remarks =>
            $"{Tag.See.Cref(nameof(OutputTypes))} × {Tag.See.Cref(nameof(ParamTypes))} の情報が{Tag.See.Cref("Sys.WodiLibContainer")}に登録される。";

        public override bool AllowMultiple => true;

        public static readonly PropertyInfo OutputType = new()
        {
            Name = nameof(OutputType),
            Type = typeof(Type).FullName,
            Summary = "出力型",
            DefaultValue = null,
        };

        public static readonly PropertyInfo OutputTypes = new()
        {
            Name = nameof(OutputTypes),
            Type = $"{typeof(Type).FullName}[]",
            Summary = "出力型（複数指定）",
            Remarks = $"{Tag.See.Cref(nameof(OutputType))} と同時に指定した場合は統合される。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo ParamType = new()
        {
            Name = nameof(ParamType),
            Type = $"{typeof(Type).FullName}?",
            Summary = "コンストラクタパラメータ型",
            Remarks = "初期化パラメータが必要な場合のみ設定する。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo ParamTypes = new()
        {
            Name = nameof(ParamTypes),
            Type = $"{typeof(Type).FullName}?[]?",
            Summary = "コンストラクタパラメータ型（複数指定）",
            Remarks = $"{Tag.See.Cref(nameof(ParamType))} と同時に指定した場合は統合される。"
                      + "初期化パラメータが必要な場合のみ設定する。",
            DefaultValue = null,
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class | AttributeTargets.Struct;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                OutputType,
                OutputTypes,
                ParamType,
                ParamTypes,
            };

        private InjectableAttribute()
        {
        }

        public static InjectableAttribute Instance { get; } = new();
    }
}
