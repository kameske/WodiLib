// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : _Main.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using IMainSourceAddable = WodiLib.SourceGenerator.Core.IMainSourceAddable;

namespace WodiLib.SourceGenerator.Domain.Collection
{
    /// <summary>
    ///     二項演算子SourceGenerator
    /// </summary>
    [Generator]
    public partial class Generator : ISourceGenerator
    {
        /// <summary>Generator Core</summary>
        private readonly GeneratorCore<SyntaxWorker> core = new(new MyDelegate());

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
            => core.Initialize(context);

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
            => core.Execute(context);

        private class MyDelegate : GeneratorCore<SyntaxWorker>.Delegate
        {
            public ILogger CreateLogger()
                => new DefaultLogger();

            public SyntaxWorker CreateSyntaxWorker()
                => new(new ExecuteInfoResolver(), CreateLogger());

            public IEnumerable<IInitializeSourceAddable> GetPostInitializationRegisterInfoList(ILogger? logger)
                => PostInitializationInfoRegister.MakePostInitializationRegisterInfoList();

            public IEnumerable<IMainSourceAddable> GetExecuteGenerateInfoList(ILogger? logger)
                => ExecuteInfoRegister.MakeExecuteGenerateInfoList();
        }
    }
}
