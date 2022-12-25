// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ImplementTemplateGenerateHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main
{
    internal static class ImplementTemplateGenerateHelper
    {
        public static SourceFormatTargetBlock GenerateRestrictedCapacityListSourceText(
            string className,
            string description,
            string fixedLengthClassName,
            string readOnlyClassName,
            string interfaceName,
            string fixedLengthInterfaceName,
            string readOnlyInterfaceName,
            string accessibility,
            string interfaceItemType,
            string maxCapacity,
            string minCapacity,
            bool isOverrideMakeDefaultItem
        )
        {
            return SourceTextFormatter.Format(
                "",
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}{description}",
                    $"/// </summary>",
                    $"{accessibility} partial class {className} : Sys.Collections.RestrictedCapacityList<{interfaceItemType}, {className}>,",
                    $"{__}{interfaceName}",
                    $"{{",
                    $"{__}/// <summary>容量最大値</summary>",
                    $"{__}public static int MaxCapacity => {maxCapacity};",
                    $"{__}/// <summary>容量最小値</summary>",
                    $"{__}public static int MinCapacity => {minCapacity};",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}public {className}() {{ }}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"length\">要素数</param>",
                    $"{__}/// <exception cref=\"System.ArgumentOutOfRangeException\">",
                    $"{__}///{__} <paramref name=\"length\"/> が <see cref=\"MinCapacity\"/> 未満または <see cref=\"MaxCapacity\"/> を超える場合。",
                    $"{__}/// </exception>",
                    $"{__}public {className}(int length) : base(length) {{ }}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"initItems\">初期要素</param>",
                    $"{__}/// <exception cref=\"System.ArgumentNullException\">",
                    $"{__}///{__} <paramref name=\"initItems\"/> が <see langword=\"null\"/> の場合、",
                    $"{__}///{__} または <paramref name=\"initItems\"/> 中に <see langword=\"null\"/> が含まれる場合。",
                    $"{__}/// </exception>",
                    $"{__}/// <exception cref=\"System.ArgumentException\">",
                    $"{__}///{__} <paramref name=\"initItems\"/> の要素数が <see cref=\"MinCapacity\"/> 未満",
                    $"{__}///{__} または <see cref=\"MaxCapacity\"/> を超える場合。",
                    $"{__}/// </exception>",
                    $"{__}public {className}(System.Collections.Generic.IEnumerable<{interfaceItemType}> initItems) : base(initItems) {{ }}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"itemsImpl\">リスト実装インスタンス</param>",
                    $"{__}internal {className}(WodiLib.Sys.Collections.IExtendedList<{interfaceItemType}> itemsImpl) : base(itemsImpl) {{ }}",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override int GetMaxCapacity() => MaxCapacity;",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override int GetMinCapacity() => MinCapacity;",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override bool ItemEquals({className}? other) => ItemEquals(other);",
                    $"",
                },
                SourceTextFormatter.If(
                    isOverrideMakeDefaultItem,
                    new[]
                    {
                        $"{__}/// <inheritdoc/>",
                        $"{__}protected override {interfaceItemType} MakeDefaultItem(int index) => new();",
                        $"",
                    }
                ),
                new[]
                {
                    $"{__}/// <summary>",
                    $"{__}/// 容量固定リストにキャストする。",
                    $"{__}/// </summary>",
                    $"{__}/// <returns><see cref=\"WodiLib.Sys.Collections.IFixedLengthList{{T}}\"/> を実装した、自分自身を参照するインスタンス。</returns>",
                    $"{__}public {fixedLengthInterfaceName} AsFixedLengthList() => new {fixedLengthClassName}(Items);",
                    $"{__}/// <summary>",
                    $"{__}/// 読取専用リストにキャストする。",
                    $"{__}/// </summary>",
                    $"{__}/// <returns><see cref=\"WodiLib.Sys.Collections.IReadOnlyExtendedList{{T}}\"/> を実装した、自分自身を参照するインスタンス。</returns>",
                    $"{__}public {readOnlyInterfaceName} AsReadOnlyList() => new {readOnlyClassName}(Items);",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override {className} DeepClone() => new(this);",
                    $"{__}{interfaceName} WodiLib.Sys.IDeepCloneable<{interfaceName}>.DeepClone() => DeepClone();",
                    $"}}",
                }
            );
        }

        public static SourceFormatTargetBlock GenerateFixedLengthListSourceText(
            string className,
            string description,
            string readOnlyClassName,
            string interfaceName,
            string readOnlyInterfaceName,
            string accessibility,
            string interfaceItemType,
            string maxCapacity,
            string minCapacity,
            bool isOverrideMakeDefaultItem
        )
        {
            return SourceTextFormatter.Format(
                "",
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}【容量固定】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial class {className} : Sys.Collections.FixedLengthList<{interfaceItemType}, {className}>,",
                    $"{__}{interfaceName}",
                    $"{{",
                    $"{__}/// <summary>容量最大値</summary>",
                    $"{__}public static int MaxCapacity => {maxCapacity};",
                    $"{__}/// <summary>容量最小値</summary>",
                    $"{__}public static int MinCapacity => {minCapacity};",
                    $"",
                },
                SourceTextFormatter.If(
                    maxCapacity == minCapacity,
                    new[]
                    {
                        $"{__}/// <summary>",
                        $"{__}///{__} コンストラクタ",
                        $"{__}/// </summary>",
                        $"{__}public {className}() : base(MinCapacity) {{ }}",
                        $"",
                    }
                ),
                new[]
                {
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"initItems\">初期要素</param>",
                    $"{__}/// <exception cref=\"System.ArgumentNullException\">",
                    $"{__}///{__} <paramref name=\"initItems\"/> が <see langword=\"null\"/> の場合、",
                    $"{__}///{__} または <paramref name=\"initItems\"/> 中に <see langword=\"null\"/> が含まれる場合。",
                    $"{__}/// </exception>",
                    $"{__}/// <exception cref=\"System.ArgumentException\">",
                    $"{__}///{__} <paramref name=\"initItems\"/> の要素数が <see cref=\"MinCapacity\"/> 未満",
                    $"{__}///{__} または <see cref=\"MaxCapacity\"/> を超える場合。",
                    $"{__}/// </exception>",
                    $"{__}public {className}(System.Collections.Generic.IEnumerable<{interfaceItemType}> initItems) : base(initItems, System.Linq.Enumerable.ToList(initItems).Count) {{ }}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"itemsImpl\">リスト実装インスタンス</param>",
                    $"{__}internal {className}(WodiLib.Sys.Collections.IExtendedList<{interfaceItemType}> itemsImpl) : base(itemsImpl) {{ }}",
                    $"",
                },
                SourceTextFormatter.If(
                    isOverrideMakeDefaultItem,
                    new[]
                    {
                        $"{__}/// <inheritdoc/>",
                        $"{__}protected override {interfaceItemType} MakeDefaultItem(int index) => new();",
                        $"",
                    }
                ),
                new[]
                {
                    $"{__}/// <summary>",
                    $"{__}/// 読取専用リストにキャストする。",
                    $"{__}/// </summary>",
                    $"{__}/// <returns><see cref=\"WodiLib.Sys.Collections.IReadOnlyExtendedList{{T}}\"/> を実装した、自分自身を参照するインスタンス。</returns>",
                    $"{__}public {readOnlyInterfaceName} AsReadOnlyList() => new {readOnlyClassName}(Items);",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override {className} DeepClone() => new(this);",
                    $"{__}{interfaceName} WodiLib.Sys.IDeepCloneable<{interfaceName}>.DeepClone() => DeepClone();",
                    $"}}",
                }
            );
        }

        public static SourceFormatTargetBlock GenerateReadOnlyListSourceText(
            string className,
            string description,
            string interfaceName,
            string accessibility,
            string interfaceItemType
        )
        {
            return SourceTextFormatter.Format(
                "",
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}【読取専用】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial class {className} : Sys.Collections.ReadOnlyExtendedList<{interfaceItemType}, {className}>,",
                    $"{__}{interfaceName}",
                    $"{{",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"initItems\">初期要素</param>",
                    $"{__}/// <exception cref=\"System.ArgumentNullException\">",
                    $"{__}///{__} <paramref name=\"initItems\"/> が <see langword=\"null\"/> の場合、",
                    $"{__}///{__} または <paramref name=\"initItems\"/> 中に <see langword=\"null\"/> が含まれる場合。",
                    $"{__}/// </exception>",
                    $"{__}public {className}(System.Collections.Generic.IEnumerable<{interfaceItemType}> initItems) : base(initItems) {{ }}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"itemsImpl\">リスト実装インスタンス</param>",
                    $"{__}internal {className}(WodiLib.Sys.Collections.IExtendedList<{interfaceItemType}> itemsImpl) : base(itemsImpl) {{ }}",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override bool ItemEquals({className}? other) => ItemEquals(other);",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override {className} DeepClone() => new(this);",
                    $"{__}{interfaceName} WodiLib.Sys.IDeepCloneable<{interfaceName}>.DeepClone() => DeepClone();",
                    $"}}",
                }
            );
        }
    }
}
