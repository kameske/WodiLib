// ========================================
// Project Name : WodiLib
// File Name    : ClearItemHandlerList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection.ClearItemイベントハンドラリスト
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
    public class ClearItemHandlerList<T> : RestrictedCapacityCollectionHandlerList<T, OnClearItemHandler<T>>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 有効なイベントハンドラをすべて実行する。
        /// </summary>
        internal void Execute()
        {
            Execute(null);
        }
    }
}