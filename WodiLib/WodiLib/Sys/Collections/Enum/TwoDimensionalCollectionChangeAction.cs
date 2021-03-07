// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeAction.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Specialized;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リストのアクション
    /// </summary>
    /// <remarks>
    ///     内容は <see cref="NotifyCollectionChangedAction"/> と同様。
    /// </remarks>
    public enum TwoDimensionalCollectionChangeAction
    {
        /// <summary>
        ///     置換
        /// </summary>
        Replace,

        /// <summary>
        ///     追加・挿入
        /// </summary>
        Add,

        /// <summary>
        ///     移動
        /// </summary>
        Move,

        /// <summary>
        ///     除去
        /// </summary>
        Remove,

        /// <summary>
        ///     初期化
        /// </summary>
        Reset
    }
}
