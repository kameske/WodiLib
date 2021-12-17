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
                        ? $"{__}Sys.Collections.IRestrictedCapacityList<{interfaceInItemType}>,"
                        : $"{__}Sys.Collections.IRestrictedCapacityList<{interfaceInItemType}, {interfaceOutItemType}>,",
                    $"{__}Sys.IDeepCloneable<{interfaceName}>",
                    $"{{",
                    $"{__}/// <inheritdoc cref=\"Sys.IDeepCloneable{{T}}.DeepClone\"/>",
                    $"{__}public new {interfaceName} DeepClone();",
                },
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
                        ? $"{__}Sys.Collections.IFixedLengthList<{interfaceInItemType}>,"
                        : $"{__}Sys.Collections.IFixedLengthList<{interfaceInItemType}, {interfaceOutItemType}>,",
                    $"{__}Sys.IDeepCloneable<{fixedLengthInterfaceName}>",
                    $"{{",
                },
                new[]
                {
                    $"{__}/// <inheritdoc cref=\"Sys.IDeepCloneable{{T}}.DeepClone\"/>",
                    $"{__}public new {fixedLengthInterfaceName} DeepClone();",
                    $"}}",
                    $"",
                    $"/// <summary>",
                    $"///{__} 【読取専用】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial interface {readOnlyInterfaceName} :",
                    isSameTypeInAndOut
                        ? $"{__}Sys.Collections.IReadOnlyExtendedList<{interfaceInItemType}>,"
                        : $"{__}Sys.Collections.IReadOnlyExtendedList<{interfaceInItemType}, {interfaceOutItemType}>,",
                    $"{__}Sys.IDeepCloneable<{readOnlyInterfaceName}>",
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
