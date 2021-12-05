// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListInterfaceTemplatedGenerator.cs
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
    /// テンプレートを用いたリストインタフェース生成
    /// </summary>
    internal class RestrictedCapacityListInterfaceTemplatedGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            RestrictedCapacityListInterfaceTemplatedAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var interfaceName = workState.Name;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var description = propertyValues[RestrictedCapacityListInterfaceTemplatedAttribute.Description.Name]!;
            var interfaceInItemType =
                propertyValues[RestrictedCapacityListInterfaceTemplatedAttribute.InterfaceInItemType.Name]!;
            var interfaceOutItemType =
                propertyValues[RestrictedCapacityListInterfaceTemplatedAttribute.InterfaceOutItemType.Name]!;
            var isImplementDeepCloneableList =
                bool.Parse(propertyValues[
                    RestrictedCapacityListInterfaceTemplatedAttribute.IsImplementDeepCloneableList.Name]!);

            var isSameTypeInAndOut = interfaceInItemType.Equals(interfaceOutItemType);
            var fixedLengthInterfaceName = $"IFixedLength{interfaceName.Substring(1)}";
            var readOnlyInterfaceName = $"IReadOnly{interfaceName.Substring(1)}";

            return SourceTextFormatter.Format("",
                new[]
                {
                    $"/// <summary>",
                    $"///{__} {description}",
                    $"/// </summary>",
                    $"{accessibility} partial interface {interfaceName} :",
                    $"{__}{fixedLengthInterfaceName},",
                    isSameTypeInAndOut
                        ? $"{__}Sys.Collections.IRestrictedCapacityList<{interfaceInItemType}>"
                        : $"{__}Sys.Collections.IRestrictedCapacityList<{interfaceInItemType}, {interfaceOutItemType}>",
                    isImplementDeepCloneableList
                        ? $"{__}, Sys.Collections.IDeepCloneableList<{interfaceName}, {interfaceInItemType}>"
                        : "",
                    $"{{",
                },
                SourceTextFormatter.If(isImplementDeepCloneableList, new[]
                {
                    $"{__}/// <inheritdoc cref=\"Sys.IDeepCloneable{{T}}.DeepClone\"/>",
                    $"{__}public new {interfaceName} DeepClone();",
                    $"",
                    $"{__}/// <inheritdoc cref=\"Sys.Collections.IDeepCloneableList{{T,TIn}}.DeepCloneWith{{TItem}}\"/>",
                    $"{__}public new {interfaceName} DeepCloneWith<TItem>(Sys.Collections.ListDeepCloneParam<TItem> param) where TItem : {interfaceInItemType};",
                }),
                new[]
                {
                    $"}}",
                    $"",
                    $"/// <summary>",
                    $"///{__} 【容量固定】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial interface {fixedLengthInterfaceName} :",
                    $"{__}{readOnlyInterfaceName},",
                    isSameTypeInAndOut
                        ? $"{__}Sys.Collections.IFixedLengthList<{interfaceInItemType}>"
                        : $"{__}Sys.Collections.IFixedLengthList<{interfaceInItemType}, {interfaceOutItemType}>",
                    isImplementDeepCloneableList
                        ? $"{__}, Sys.Collections.IDeepCloneableList<{fixedLengthInterfaceName}, {interfaceInItemType}>"
                        : "",
                    $"{{",
                },
                SourceTextFormatter.If(isImplementDeepCloneableList, new[]
                {
                    $"{__}/// <inheritdoc cref=\"Sys.IDeepCloneable{{T}}.DeepClone\"/>",
                    $"{__}public new {fixedLengthInterfaceName} DeepClone();",
                    $"",
                    $"{__}/// <inheritdoc cref=\"Sys.Collections.IDeepCloneableList{{T,TIn}}.DeepCloneWith{{TItem}}\"/>",
                    $"{__}public new {fixedLengthInterfaceName} DeepCloneWith<TItem>(Sys.Collections.ListDeepCloneParam<TItem> param) where TItem : {interfaceInItemType};",
                }),
                new[]
                {
                    $"}}",
                    $"",
                    $"/// <summary>",
                    $"///{__} 【読取専用】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial interface {readOnlyInterfaceName} :",
                    isSameTypeInAndOut
                        ? $"{__}Sys.Collections.IReadOnlyExtendedList<{interfaceInItemType}>"
                        : $"{__}Sys.Collections.IReadOnlyExtendedList<{interfaceInItemType}, {interfaceOutItemType}>",
                    isImplementDeepCloneableList
                        ? $"{__}, Sys.Collections.IDeepCloneableList<{readOnlyInterfaceName}, {interfaceInItemType}>"
                        : "",
                    $"{{",
                    $"}}",
                }
            );
        }

        private RestrictedCapacityListInterfaceTemplatedGenerator()
        {
        }

        public static RestrictedCapacityListInterfaceTemplatedGenerator Instance { get; } = new();
    }
}
