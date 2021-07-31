// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.ObjectModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     長さが固定されたListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量を固定した機能。
    ///     <see cref="ObservableCollection{T}"/> の Read, Update 各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。
    ///     固定しているのは容量のみで、要素の入れ替えや更新は可能。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IFixedLengthList<T> : IWritableList<T, IFixedLengthList<T>, IReadOnlyExtendedList<T>>
    {
    }
}
