// ========================================
// Project Name : WodiLib
// File Name    : ICastableFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// 容量固定リストにキャスト可能であることを表すインタフェース。
    /// </summary>
    /// <typeparam name="T">キャストした容量固定リストの要素型</typeparam>
    public interface ICastableFixedLengthList<T>
    {
        /// <summary>
        /// 容量固定リストにキャストする。
        /// </summary>
        /// <returns><see cref="IFixedLengthList{T}"/> を実装した、自分自身を参照するインスタンス。</returns>
        public IFixedLengthList<T> AsFixedLengthList();
    }
}
