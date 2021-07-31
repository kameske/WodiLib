// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自読み取り専用リストインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="IReadOnlyList{T}"/> のメソッドと <see cref="ObservableCollection{T}"/> の機能を融合した機能。
    ///         <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///         それ以外にもいくつかメソッドを追加している。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyExtendedList<T> : IReadableList<T, IReadOnlyExtendedList<T>>
    {
    }
}
