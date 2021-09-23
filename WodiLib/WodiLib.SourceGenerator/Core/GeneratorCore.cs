// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : GeneratorCore.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     ソースコード自動生成処理中核
    /// </summary>
    /// <typeparam name="TWorker">構文処理</typeparam>
    internal class GeneratorCore<TWorker> : ISourceGenerator
        where TWorker : ISyntaxWorker
    {
        /// <summary>デリゲート</summary>
        private readonly Delegate @delegate;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="delegate">デリゲート</param>
        public GeneratorCore(Delegate @delegate)
        {
            this.@delegate = @delegate;
        }

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
            var cnt = 0;
            context.RegisterForPostInitialization(i =>
            {
                var logger = @delegate.CreateLogger();
                foreach (var info in @delegate.GetPostInitializationRegisterInfoList(logger))
                {
                    cnt++;
                    try
                    {
                        logger?.AppendLine(info.ToString());
                        info.AddSource(i);
                    }
                    catch (Exception ex)
                    {
                        logger?.AppendLine(
                            $"        {cnt}{Environment.NewLine}{info}{Environment.NewLine}{ex}{Environment.NewLine}");
                    }
                }

                if (logger?.HasLog ?? false)
                {
                    i.AddSource("RegisterForPostInitialization.log", logger.ToOutputText());
                }
            });

            try
            {
                context.RegisterForSyntaxNotifications(() => @delegate.CreateSyntaxWorker());
            }
            catch (Exception ex)
            {
                context.RegisterForPostInitialization(i =>
                    i.AddSource("CreateSyntaxWorkerError.log", $"{ex.Message}{Environment.NewLine}{ex.Message}"));
                Console.WriteLine(ex.Message);
            }
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            var errorLogs = new List<string>();
            var logger = @delegate.CreateLogger();
            try
            {
                if (context.SyntaxContextReceiver is not TWorker worker)
                {
                    return;
                }

                OutputSyntaxWorkerLog(worker, context);

                var generateInfoList = @delegate.GetExecuteGenerateInfoList(logger);

                foreach (var info in generateInfoList)
                {
                    try
                    {
                        logger?.AppendLine($"start addSource. info: {info.GetType().FullName}");
                        info.AddSource(context, worker);
                        logger?.AppendLine(info.Logger);
                    }
                    catch (Exception ex)
                    {
                        errorLogs.Add("// Error AddSource.");
                        errorLogs.AddRange(ex.ToString().Lines(false).Select(s => $"// {s}"));
                        if (info.Logger is not null)
                        {
                            errorLogs.Add("// --- SourceAddable Logs. ---");
                            errorLogs.Add(info.Logger.ToOutputText());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogs.Add("Error occured before generate.");
                errorLogs.AddRange(ex.ToString().Lines(false).Select(s => $"// {s}"));
            }

            if (logger is not null)
            {
                context.AddSource("Execute.log", logger.ToOutputText());
            }

            if (errorLogs.Count > 0)
            {
                context.AddSource("ExecuteError.log", errorLogs.Join(Environment.NewLine));
            }
        }

        /// <summary>
        ///     <see cref="ISyntaxWorker"/> のログをファイルに出力する。
        /// </summary>
        /// <param name="worker">構文処理</param>
        /// <param name="context">コンテキスト</param>
        private void OutputSyntaxWorkerLog(TWorker worker, GeneratorExecutionContext context)
        {
            var logger = worker.Logger;
            if (logger is null) return;

            context.AddSource("SyntaxWorkResult.log", logger.ToOutputText());
        }

        /// <summary>
        ///     <see cref="GeneratorCore{TWorker}"/> のデリゲートインタフェース
        /// </summary>
        public interface Delegate
        {
            /// <summary>
            ///     ソース自動生成に必要な情報等を登録するための情報一覧を返却する。
            /// </summary>
            /// <param name="logger">ロガー</param>
            /// <returns>情報一覧</returns>
            public IEnumerable<IInitializeSourceAddable> GetPostInitializationRegisterInfoList(ILogger? logger);

            /// <summary>
            ///     ソース自動生成に必要なクラス情報一覧を返却する。
            /// </summary>
            /// <param name="logger">ロガー</param>
            /// <returns>情報一覧</returns>
            public IEnumerable<IMainSourceAddable> GetExecuteGenerateInfoList(ILogger? logger);

            /// <summary>
            ///     <see cref="GeneratorInitializationContext"/> に登録する <see cref="ISyntaxWorker"/> を生成する。
            /// </summary>
            /// <returns><see cref="ISyntaxWorker"/> インスタンス</returns>
            public TWorker CreateSyntaxWorker();

            /// <summary>
            ///     <see cref="ILogger"/> インスタンスを生成する。
            /// </summary>
            /// <returns><see cref="ILogger"/> インスタンス</returns>
            public ILogger? CreateLogger();
        }
    }
}
