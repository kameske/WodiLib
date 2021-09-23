// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TypeDefinitionInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Enums;

namespace WodiLib.SourceGenerator.Core.Dtos
{
    /// <summary>
    ///     型定義情報
    /// </summary>
    internal class TypeDefinitionInfo
    {
        /// <summary>型名（フル）</summary>
        public string TypeFullName { get; init; } = default!;

        /// <summary>型タイプ</summary>
        public ObjectType ObjectType { get; init; }

        /// <summary>型のアクセシビリティ</summary>
        public Accessibility Accessibility { get; init; }

        /// <summary>Abstractフラグ</summary>
        public bool IsAbstract { get; init; }

        /// <summary>Staticフラグ</summary>
        public bool IsStatic { get; init; }

        /// <summary>Sealedフラグ</summary>
        public bool IsSealed { get; init; }

        /// <summary>クラスフラグ</summary>
        public bool IsClass => ObjectType.Equals(ObjectType.Class);

        /// <summary>構造体フラグ</summary>
        public bool IsStruct => ObjectType.Equals(ObjectType.Struct);

        /// <summary>レコードフラグ</summary>
        public bool IsRecord => ObjectType.Equals(ObjectType.Record);

        /// <inheritdoc/>
        public override string ToString()
            => $"{{ TypeFullName = {TypeFullName}, ObjectType = {ObjectType}, " +
               $"Accessibility = {Accessibility}, " +
               $"IsAbstract = {IsAbstract}, IsStatic = {IsStatic}, " +
               $"IsSealed = {IsSealed} }}";
    }
}
