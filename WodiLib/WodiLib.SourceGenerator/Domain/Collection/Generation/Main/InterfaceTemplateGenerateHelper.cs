// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : InterfaceTemplateGenerateHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main
{
    internal static class InterfaceTemplateGenerateHelper
    {
        public static string[] GenerateRestrictedCapacityListInterfaceSourceText(
            string interfaceName,
            string description,
            string accessibility,
            string itemType
        )
        {
            return new[]
            {
                $"/// <summary>",
                $"///{__} {description}",
                $"/// </summary>",
                $"{accessibility} partial interface {interfaceName} :",
                $"{__}Sys.Collections.IRestrictedCapacityList<{itemType}>,",
                $"{__}Sys.IDeepCloneable<{interfaceName}>",
                $"{{",
                $"}}",
            };
        }

        public static string[] GenerateFixedLengthListInterfaceSourceText(
            string interfaceName,
            string description,
            string accessibility,
            string itemType
        )
        {
            return new[]
            {
                $"/// <summary>",
                $"///{__} 【容量固定】{description}",
                $"/// </summary>",
                $"{accessibility} partial interface {interfaceName} :",
                $"{__}Sys.Collections.IFixedLengthList<{itemType}>,",
                $"{__}Sys.IDeepCloneable<{interfaceName}>",
                $"{{",
                $"}}",
            };
        }

        public static string[] GenerateReadOnlyListInterfaceSourceText(
            string interfaceName,
            string description,
            string accessibility,
            string itemType
        )
        {
            return new[]
            {
                $"/// <summary>",
                $"///{__} 【読取専用】{description}",
                $"/// </summary>",
                $"{accessibility} partial interface {interfaceName} :",
                $"{__}Sys.Collections.IReadOnlyExtendedList<{itemType}>,",
                $"{__}Sys.IDeepCloneable<{interfaceName}>",
                $"{{",
                $"}}",
            };
        }
    }
}
