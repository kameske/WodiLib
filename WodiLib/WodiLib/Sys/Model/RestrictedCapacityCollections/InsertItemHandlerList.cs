// ========================================
// Project Name : WodiLib
// File Name    : InsertItemHandlerList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection.InsertItemイベントハンドラリスト
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
    public class InsertItemHandlerList<T> : RestrictedCapacityCollectionHandlerList<T, OnInsertItemHandler<T>>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 有効なイベントハンドラをすべて実行する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">対象インスタンス</param>
        internal void Execute(int index, T item)
        {
            var @params = new object[] {index, item};
            Execute(@params);
        }
    }
}