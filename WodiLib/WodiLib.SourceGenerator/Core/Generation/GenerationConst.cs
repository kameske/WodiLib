// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : GenerationConst.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Reflection;

namespace WodiLib.SourceGenerator.Core.Generation
{
    /// <summary>
    ///     ソース自動生成用定数
    /// </summary>
    internal static class GenerationConst
    {
        public static string RootNameSpace => Assembly.GetExecutingAssembly().GetName().Name;
    }
}
