// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListInterfaceTemplateGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main
{
    /// <summary>
    /// テンプレートを用いたリストインタフェース生成
    /// </summary>
    internal class RestrictedCapacityListInterfaceTemplateGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            RestrictedCapacityListInterfaceTemplateAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var interfaceName = workState.Name;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var description = propertyValues[RestrictedCapacityListInterfaceTemplateAttribute.Description.Name]!;
            var interfaceItemType =
                propertyValues[RestrictedCapacityListInterfaceTemplateAttribute.InterfaceItemType.Name]!;

            var fixedLengthInterfaceName = $"IFixedLength{interfaceName.Substring(1)}";
            var readOnlyInterfaceName = $"IReadOnly{interfaceName.Substring(1)}";

            return SourceTextFormatter.Format(
                "",
                InterfaceTemplateGenerateHelper.GenerateRestrictedCapacityListInterfaceSourceText(
                    interfaceName,
                    fixedLengthInterfaceName,
                    readOnlyInterfaceName,
                    description,
                    accessibility,
                    interfaceItemType
                ),
                "",
                InterfaceTemplateGenerateHelper.GenerateFixedLengthListInterfaceSourceText(
                    fixedLengthInterfaceName,
                    readOnlyInterfaceName,
                    description,
                    accessibility,
                    interfaceItemType
                ),
                "",
                InterfaceTemplateGenerateHelper.GenerateReadOnlyListInterfaceSourceText(
                    readOnlyInterfaceName,
                    description,
                    accessibility,
                    interfaceItemType
                )
            );
        }

        private RestrictedCapacityListInterfaceTemplateGenerator()
        {
        }

        public static RestrictedCapacityListInterfaceTemplateGenerator Instance { get; } = new();
    }
}
