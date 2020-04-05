// ========================================
// Project Name : WodiLib
// File Name    : RemoveItemHandlerList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection.RemoveItemイベントハンドラリスト
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
    public class RemoveItemHandlerList<T> : RestrictedCapacityCollectionHandlerList<T, OnRemoveItemHandler<T>>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 有効なイベントハンドラをすべて実行する。
        /// </summary>
        /// <param name="index">インデックス</param>
        internal void Execute(int index)
        {
            var @params = new object[] {index};
            Execute(@params);
        }
    }
}