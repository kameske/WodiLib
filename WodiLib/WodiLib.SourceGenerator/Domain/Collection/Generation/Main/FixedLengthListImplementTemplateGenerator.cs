// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : FixedLengthListImplementTemplateGenerator.cs
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
    /// テンプレートを用いたリスト実装クラス生成
    /// </summary>
    internal class FixedLengthListImplementTemplateGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            FixedLengthListImplementTemplateAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var description = propertyValues[FixedLengthListImplementTemplateAttribute.Description.Name]!;
            var interfaceTypeName =
                propertyValues[FixedLengthListImplementTemplateAttribute.InterfaceType.Name]!;
            var maxCapacity = propertyValues[FixedLengthListImplementTemplateAttribute.MaxCapacity.Name]!;
            var minCapacity = propertyValues[FixedLengthListImplementTemplateAttribute.MinCapacity.Name]!;
            var interfaceItemType =
                propertyValues[FixedLengthListImplementTemplateAttribute.InterfaceItemType.Name]!;
            var isOverrideMakeDefaultItem =
                bool.Parse(
                    propertyValues[
                        FixedLengthListImplementTemplateAttribute.IsAutoOverrideMakeDefaultItem.Name]!
                );
            var isOverrideGenerateValidatorForItemsInFixedLengthList = 
                bool.Parse(
                    propertyValues[
                        FixedLengthListImplementTemplateAttribute.IsAutoOverrideGenerateValidatorForItemsInFixedLengthList.Name]!
                );
            var interfaceTypeNameSplit = interfaceTypeName.Split('.');
            var interfaceName = interfaceTypeNameSplit[interfaceTypeNameSplit.Length - 1];
            var fixedLengthInterfaceName = $"IFixedLength{interfaceName.Substring(1)}";
            var readOnlyInterfaceName = $"IReadOnly{interfaceName.Substring(1)}";
            var fixedLengthClassName = $"FixedLength{interfaceName.Substring(1)}";
            var readOnlyClassName = $"ReadOnly{interfaceName.Substring(1)}";

            return SourceTextFormatter.Format(
                "",
                ImplementTemplateGenerateHelper.GenerateFixedLengthListSourceText(
                    fixedLengthClassName,
                    description,
                    readOnlyClassName,
                    fixedLengthInterfaceName,
                    readOnlyInterfaceName,
                    accessibility,
                    interfaceItemType,
                    maxCapacity,
                    minCapacity,
                    isOverrideMakeDefaultItem,
                    isOverrideGenerateValidatorForItemsInFixedLengthList
                ),
                ImplementTemplateGenerateHelper.GenerateReadOnlyListSourceText(
                    readOnlyClassName,
                    description,
                    readOnlyInterfaceName,
                    accessibility,
                    interfaceItemType
                )
            );
        }

        private FixedLengthListImplementTemplateGenerator()
        {
        }

        public static FixedLengthListImplementTemplateGenerator Instance { get; } = new();
    }
}
