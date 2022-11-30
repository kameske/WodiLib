// ========================================
// Project Name : WodiLib
// File Name    : ICastableReadOnlyExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// 読取専用リストにキャスト可能であることを表すインタフェース。
    /// </summary>
    /// <typeparam name="T">キャストした容量固定リストの要素型</typeparam>
    public interface ICastableReadOnlyExtendedList<T>
    {
        /// <summary>
        /// 読取専用リストにキャストする。
        /// </summary>
        /// <returns><see cref="IReadOnlyExtendedList{T}"/> を実装した、自分自身を参照するインスタンス。</returns>
        public IReadOnlyExtendedList<T> AsReadOnlyList();
    }
}
