// ========================================
// Project Name : WodiLib
// File Name    : ClearItemHandlerList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection.ClearItemイベントハンドラリスト
    /// </summary>
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