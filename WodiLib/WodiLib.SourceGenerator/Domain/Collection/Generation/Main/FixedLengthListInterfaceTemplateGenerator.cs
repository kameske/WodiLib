// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : FixedLengthListInterfaceTemplateGenerator.cs
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
    internal class FixedLengthListInterfaceTemplateGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            FixedLengthListInterfaceTemplateAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var interfaceName = workState.Name;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var description = propertyValues[FixedLengthListInterfaceTemplateAttribute.Description.Name]!;
            var interfaceItemType =
                propertyValues[FixedLengthListInterfaceTemplateAttribute.InterfaceItemType.Name]!;

            var readOnlyInterfaceName = $"IReadOnly{interfaceName.Substring(1)}";

            return SourceTextFormatter.Format(
                "",
                InterfaceTemplateGenerateHelper.GenerateFixedLengthListInterfaceSourceText(
                    interfaceName,
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

        private FixedLengthListInterfaceTemplateGenerator()
        {
        }

        public static FixedLengthListInterfaceTemplateGenerator Instance { get; } = new();
    }
}
