// ========================================
// Project Name : WodiLib
// File Name    : IDeepCloneableList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     ディープクローン可能なリストであることを表すインタフェース
    /// </summary>
    /// <typeparam name="T">出力型</typeparam>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    public interface IDeepCloneableList<out T, in TIn> :
        IDeepCloneable<T>
    {
        /// <summary>
        ///     自身の要素をコピーした新たなインスタンスを返却する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         自身の内包する要素が <see cref="IDeepCloneable{T}"/>を実装している場合 または 構造体の場合、
        ///         返却するインスタンスの要素はすべてディープコピーされた状態で格納される。
        ///         それ以外の場合はシャローコピーされた要素が格納される。
        ///     </para>
        /// </remarks>
        /// <param name="param">ディープコピー後の編集情報</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="param"/> が <see langword="null"/> の場合。
        /// </exception>
        public T DeepCloneWith<TItem>(ListDeepCloneParam<TItem> param)
            where TItem : TIn;
    }
}
