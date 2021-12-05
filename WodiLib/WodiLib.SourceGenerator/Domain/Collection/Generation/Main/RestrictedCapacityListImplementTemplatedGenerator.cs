// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListImplementTemplatedGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main
{
    /// <summary>
    /// テンプレートを用いたリスト実装クラス生成
    /// </summary>
    internal class RestrictedCapacityListImplementTemplatedGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            RestrictedCapacityListImplementTemplatedAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var className = workState.Name;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var interfaceTypeName =
                propertyValues[RestrictedCapacityListImplementTemplatedAttribute.InterfaceType.Name]!;
            var maxCapacity = propertyValues[RestrictedCapacityListImplementTemplatedAttribute.MaxCapacity.Name]!;
            var minCapacity = propertyValues[RestrictedCapacityListImplementTemplatedAttribute.MinCapacity.Name]!;
            var interfaceInternalItemType =
                propertyValues[RestrictedCapacityListImplementTemplatedAttribute.InterfaceInternalItemType.Name]!;
            var interfaceInItemType =
                propertyValues[RestrictedCapacityListImplementTemplatedAttribute.InterfaceInItemType.Name]!;
            var interfaceOutItemType =
                propertyValues[RestrictedCapacityListImplementTemplatedAttribute.InterfaceOutItemType.Name]!;
            var isOverrideMakeDefaultItem =
                bool.Parse(propertyValues[
                    RestrictedCapacityListImplementTemplatedAttribute.IsOverrideMakeDefaultItem.Name]!);
            var isOverrideMakeInstance =
                bool.Parse(propertyValues[
                    RestrictedCapacityListImplementTemplatedAttribute.IsOverrideMakeInstance.Name]!);
            var isOverrideCloneToInternal =
                bool.Parse(propertyValues[
                    RestrictedCapacityListImplementTemplatedAttribute.IsOverrideCloneToInternal.Name]!);
            var isImplementDeepCloneableList =
                bool.Parse(propertyValues[
                    RestrictedCapacityListInterfaceTemplatedAttribute.IsImplementDeepCloneableList.Name]!);

            var isSameTypeInAndOut = interfaceInItemType.Equals(interfaceOutItemType);
            var interfaceTypeNameSplit = interfaceTypeName.Split('.');
            var interfaceName = interfaceTypeNameSplit[interfaceTypeNameSplit.Length - 1];
            var fixedLengthInterfaceName = $"IFixedLength{interfaceName.Substring(1)}";
            var readOnlyInterfaceName = $"IReadOnly{interfaceName.Substring(1)}";

            return SourceTextFormatter.Format("",
                new[]
                {
                    isSameTypeInAndOut
                        ? $"{accessibility} partial class {className} : Sys.Collections.RestrictedCapacityList<{interfaceInItemType}, {className}>,"
                        : $"{accessibility} partial class {className} : Sys.Collections.RestrictedCapacityList<{interfaceInItemType}, {interfaceOutItemType}, {interfaceInternalItemType}, {className}>,",
                    $"{__}{interfaceName},",
                    $"{__}Sys.Collections.IDeepCloneableList<{className}, {interfaceInItemType}>",
                    $"{{",
                    $"{__}/// <summary>容量最大値</summary>",
                    $"{__}public static int MaxCapacity => {maxCapacity};",
                    $"",
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
                    $"{__}public {className}(System.Collections.Generic.IEnumerable<{interfaceInItemType}> initItems) : base(initItems) {{ }}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"initParam\">初期化パラメータ</param>",
                    $"{__}/// <exception cref=\"System.ArgumentNullException\">",
                    $"{__}///{__} <paramref name=\"initParam\"/> が <see langword=\"null\"/> の場合、",
                    $"{__}///{__} または <paramref name=\"initParam\"/> 中に <see langword=\"null\"/> が含まれる場合。",
                    $"{__}/// </exception>",
                    $"{__}/// <exception cref=\"System.ArgumentException\">",
                    $"{__}///{__} <paramref name=\"initParam\"/> の要素数が <see cref=\"MinCapacity\"/> 未満",
                    $"{__}///{__} または <see cref=\"MaxCapacity\"/> を超える場合。",
                    $"{__}/// </exception>",
                    $"{__}public {className}(Sys.Collections.ListInitParam<{interfaceInItemType}> initParam) : base(initParam) {{ }}",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override int GetMaxCapacity() => MaxCapacity;",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override int GetMinCapacity() => MinCapacity;",
                    $"",
                },
                SourceTextFormatter.If(isOverrideMakeDefaultItem, new[]
                {
                    $"{__}/// <inheritdoc />",
                    isSameTypeInAndOut
                        ? $"{__}protected override {interfaceInItemType} MakeDefaultItem(int index) => new();"
                        : $"{__}protected override {interfaceInternalItemType} MakeDefaultItem(int index) => new();",
                    $"",
                }),
                SourceTextFormatter.If(isOverrideMakeInstance, new[]
                {
                    $"{__}/// <inheritdoc />",
                    isSameTypeInAndOut
                        ? $"{__}protected override {className} MakeInstance(System.Collections.Generic.IEnumerable<{interfaceInItemType}> items) => new(items);"
                        : $"{__}protected override {className} MakeInstance(System.Collections.Generic.IEnumerable<{interfaceInternalItemType}> items) => new(items);",
                    $"",
                }),
                SourceTextFormatter.If(!isSameTypeInAndOut && isOverrideCloneToInternal, new[]
                {
                    $"{__}/// <inheritdoc/>",
                    $"{__}protected override {interfaceInternalItemType} CloneToInternal({interfaceInItemType} src) => new(src);",
                    $"",
                }),
                SourceTextFormatter.If(isImplementDeepCloneableList, new[]
                {
                    $"{__}{interfaceName} {interfaceName}.DeepClone() => DeepClone();",
                    $"{__}{interfaceName} Sys.IDeepCloneable<{interfaceName}>.DeepClone() => DeepClone();",
                    $"{__}{fixedLengthInterfaceName} {fixedLengthInterfaceName}.DeepClone() => DeepClone();",
                    $"{__}{fixedLengthInterfaceName} Sys.IDeepCloneable<{fixedLengthInterfaceName}>.DeepClone() => DeepClone();",
                    $"{__}{readOnlyInterfaceName} Sys.IDeepCloneable<{readOnlyInterfaceName}>.DeepClone() => DeepClone();",
                    $"",
                    $"{__}{interfaceName} {interfaceName}.DeepCloneWith<TItem>(Sys.Collections.ListDeepCloneParam<TItem> param) => DeepCloneWith(param);",
                    $"{__}{interfaceName} Sys.Collections.IDeepCloneableList<{interfaceName}, {interfaceInItemType}>.DeepCloneWith<TItem>(Sys.Collections.ListDeepCloneParam<TItem> param) => DeepCloneWith(param);",
                    $"{__}{fixedLengthInterfaceName} {fixedLengthInterfaceName}.DeepCloneWith<TItem>(Sys.Collections.ListDeepCloneParam<TItem> param) => DeepCloneWith(param);",
                    $"{__}{fixedLengthInterfaceName} Sys.Collections.IDeepCloneableList<{fixedLengthInterfaceName}, {interfaceInItemType}>.DeepCloneWith<TItem>(Sys.Collections.ListDeepCloneParam<TItem> param) => DeepCloneWith(param);",
                    $"{__}{readOnlyInterfaceName} Sys.Collections.IDeepCloneableList<{readOnlyInterfaceName}, {interfaceInItemType}>.DeepCloneWith<TItem>(Sys.Collections.ListDeepCloneParam<TItem> param) => DeepCloneWith(param);",
                }),
                new[]
                {
                    $"}}",
                }
            );
        }

        private RestrictedCapacityListImplementTemplatedGenerator()
        {
        }

        public static RestrictedCapacityListImplementTemplatedGenerator Instance { get; } = new();
    }
}
