// ========================================
// Project Name : WodiLib
// File Name    : SetItemHandlerList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection.SetItemイベントハンドラリスト
    /// </summary>
    public class SetItemHandlerList<T> : RestrictedCapacityCollectionHandlerList<T, OnSetItemHandler<T>>
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