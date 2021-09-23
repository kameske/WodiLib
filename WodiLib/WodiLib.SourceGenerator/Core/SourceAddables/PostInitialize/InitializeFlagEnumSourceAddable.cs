// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : InitializeEnumSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize
{
    /// <summary>
    ///     <see cref="ISourceGenerator.Initialize"/> で
    ///     フラグ列挙型を追加する処理を行うためのテンプレートクラス
    /// </summary>
    internal abstract class InitializeFlagEnumSourceAddable : InitializeEnumSourceAddable
    {
        /// <inheritDoc/>
        public sealed override bool IsFlags => true;

        /// <summary>
        ///     フラグ値文字列が指定のフラグ値を持っているかどうかを確認する。
        /// </summary>
        /// <param name="flagStr">フラグ値文字列</param>
        /// <param name="flagValue">判定対象フラグ値</param>
        /// <returns>フラグが立っている場合 <see langword="true"/></returns>
        protected static bool HasFlag(string? flagStr, int flagValue)
        {
            if (!int.TryParse(flagStr, out var code)) return false;
            return (code & flagValue) != 0;
        }
    }
}
