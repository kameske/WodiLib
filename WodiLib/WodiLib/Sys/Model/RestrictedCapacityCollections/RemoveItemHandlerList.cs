// ========================================
// Project Name : WodiLib
// File Name    : RemoveItemHandlerList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection.RemoveItemイベントハンドラリスト
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
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