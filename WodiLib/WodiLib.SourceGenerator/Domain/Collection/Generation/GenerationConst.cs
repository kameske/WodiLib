// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : GenerationConst.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Domain.Collection.Generation
{
    /// <summary>
    ///     リストSourceGenerator用定数クラス
    /// </summary>
    internal static class GenerationConst
    {
        /// <summary>ルート名前空間</summary>
        private static string RootNameSpace => $"{Core.Generation.GenerationConst.RootNameSpace}.Domain.Collection";

        public static class NameSpaces
        {
            /// <summary>Attributes名前空間</summary>
            public static string Attributes => $"{RootNameSpace}.Attributes";
        }
    }
}
