// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ExecuteInfoResolver.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using static WodiLib.SourceGenerator.Core.Templates.FromAttribute.SyntaxWorker;

namespace WodiLib.SourceGenerator.Operation
{
    public partial class Generator
    {
        /// <summary>
        ///     ソースコード生成処理解決処理
        /// </summary>
        private class ExecuteInfoResolver : IExecuteInfoResolver
        {
            public bool IsRootAttribute(INamedTypeSymbol targetAttribute)
            {
                return ExecuteInfoRegister.MakeExecuteGenerateInfoList().Any(info
                    => targetAttribute.IsSame(info.TargetAttribute.TypeFullName));
            }

            public IMainSourceAddable? Resolve(INamedTypeSymbol targetAttribute)
            {
                return ExecuteInfoRegister.MakeExecuteGenerateInfoList().FirstOrDefault(info
                    => targetAttribute.IsSameOrExtended(info.TargetAttribute.TypeFullName));
            }
        }
    }
}
