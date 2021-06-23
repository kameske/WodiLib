// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalCollectionChangeEventArgs.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// 二次元リストの要素が変更された際に発火するイベントの引数
    /// </summary>
    /// <remarks>
    /// <seealso cref="NotifyCollectionChangedEventArgsEx{T}"/> と同様の使い方ができる、
    /// 2次元リストの変更通知引数であることを示すインタフェース。
    /// </remarks>
    /// <typeparam name="T">イベント発火元二次元リストの内包型</typeparam>
    public interface ITwoDimensionalCollectionChangeEventArgs<T>
    {
    }
}
