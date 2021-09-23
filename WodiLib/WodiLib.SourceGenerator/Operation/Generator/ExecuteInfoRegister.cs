// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ExecuteInfoRegister.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Operation.Generation.Main.Binary;
using WodiLib.SourceGenerator.Operation.Generation.Main.Shift;
using WodiLib.SourceGenerator.Operation.Generation.Main.Unary;

namespace WodiLib.SourceGenerator.Operation
{
    public partial class Generator
    {
        /// <summary>
        ///     メインソースコード生成情報登録処理
        /// </summary>
        private static class ExecuteInfoRegister
        {
            public static IEnumerable<MainSourceAddableTemplate> MakeExecuteGenerateInfoList()
            {
                yield return BinaryOperatorGenerator.Instance;
                yield return UnaryOperatorGenerator.Instance;
                yield return ShiftOperatorGenerator.Instance;
            }
        }
    }
}
