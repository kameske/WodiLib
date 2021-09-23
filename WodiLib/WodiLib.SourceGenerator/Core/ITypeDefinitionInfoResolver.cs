// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ITypeDefinitionInfoResolver.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     タイプ定義情報受け渡しインタフェース
    /// </summary>
    internal interface ITypeDefinitionInfoResolver
    {
        /// <summary>
        ///     タイプ情報取得可能かどうかを返す。
        /// </summary>
        /// <param name="typeFullName">対象タイプ名</param>
        /// <returns>取得可能な場合 <see langword="true"/></returns>
        bool CanResolve(string typeFullName);

        /// <summary>
        ///     タイプ情報を取得する。
        /// </summary>
        /// <param name="typeFullName">対象タイプ名</param>
        /// <returns>タイプ情報</returns>
        TypeDefinitionInfo Resolve(string typeFullName);
    }
}
