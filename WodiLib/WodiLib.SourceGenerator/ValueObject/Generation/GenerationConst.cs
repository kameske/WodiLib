// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : GenerationConst.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.ValueObject.Generation
{
    /// <summary>
    ///     値オブジェクトSourceGenerator用定数クラス
    /// </summary>
    internal static class GenerationConst
    {
        /// <summary>ルート名前空間</summary>
        private static string RootNameSpace => $"{Core.Generation.GenerationConst.RootNameSpace}.ValueObject";

        public static class NameSpaces
        {
            /// <summary>Enums名前空間</summary>
            public static string Enums => $"{RootNameSpace}.Enums";

            /// <summary>Attributes名前空間</summary>
            public static string Attributes => $"{RootNameSpace}.Attributes";
        }
    }
}