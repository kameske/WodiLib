// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListImplementTemplateGenerator.cs
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
    internal class RestrictedCapacityListImplementTemplateGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            RestrictedCapacityListImplementTemplateAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var className = workState.Name;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var description = propertyValues[RestrictedCapacityListImplementTemplateAttribute.Description.Name]!;
            var interfaceTypeName =
                propertyValues[RestrictedCapacityListImplementTemplateAttribute.InterfaceType.Name]!;
            var maxCapacity = propertyValues[RestrictedCapacityListImplementTemplateAttribute.MaxCapacity.Name]!;
            var minCapacity = propertyValues[RestrictedCapacityListImplementTemplateAttribute.MinCapacity.Name]!;
            var interfaceItemType =
                propertyValues[RestrictedCapacityListImplementTemplateAttribute.InterfaceItemType.Name]!;
            var isOverrideMakeDefaultItem =
                bool.Parse(
                    propertyValues[
                        RestrictedCapacityListImplementTemplateAttribute.IsAutoOverrideMakeDefaultItem.Name]!
                );
            var isOverrideGenerateValidatorForItemsInFixedLengthList =
                bool.Parse(
                    propertyValues[
                        RestrictedCapacityListImplementTemplateAttribute
                            .IsAutoOverrideGenerateValidatorForItemsInFixedLengthList.Name]!
                );
            var interfaceTypeNameSplit = interfaceTypeName.Split('.');
            var interfaceName = interfaceTypeNameSplit[interfaceTypeNameSplit.Length - 1];
            var fixedLengthInterfaceName = $"IFixedLength{interfaceName.Substring(1)}";
            var readOnlyInterfaceName = $"IReadOnly{interfaceName.Substring(1)}";
            var fixedLengthClassName = $"FixedLength{interfaceName.Substring(1)}";
            var readOnlyClassName = $"ReadOnly{interfaceName.Substring(1)}";

            return SourceTextFormatter.Format(
                "",
                ImplementTemplateGenerateHelper.GenerateRestrictedCapacityListSourceText(
                    className,
                    description,
                    fixedLengthClassName,
                    readOnlyClassName,
                    interfaceName,
                    fixedLengthInterfaceName,
                    readOnlyInterfaceName,
                    accessibility,
                    interfaceItemType,
                    maxCapacity,
                    minCapacity,
                    isOverrideMakeDefaultItem
                ),
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

        private RestrictedCapacityListImplementTemplateGenerator()
        {
        }

        public static RestrictedCapacityListImplementTemplateGenerator Instance { get; } = new();
    }
}
