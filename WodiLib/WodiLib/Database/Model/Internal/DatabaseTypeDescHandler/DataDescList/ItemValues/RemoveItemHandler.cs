// ========================================
// Project Name : WodiLib
// File Name    : RemoveItemHandler.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database.DatabaseTypeDescHandler.DataDescList.ItemValues
{
    /// <summary>
    /// DatabaseItemDescList.RemoveItemのイベントハンドラ
    /// </summary>
    [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
    internal class RemoveItemHandler : OnRemoveItemHandler<IFixedLengthDBItemValueList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// リストイベントハンドラにつけるタグ
        /// </summary>
        public static readonly string HandlerTag = ItemDescList.ItemDesc.SetItemHandler.HandlerTag;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public RemoveItemHandler(DatabaseTypeDesc outer)
            : base(MakeHandler(outer), HandlerTag, false, canChangeEnabled: false)
        {
        }

        /// <summary>
        /// DatabaseItemDescList.RemoveItemのイベントを生成する。
        /// </summary>
        /// <param name="outer">連係外部クラスインスタンス</param>
        /// <returns>RemoveItemイベント</returns>
        private static Action<int> MakeHandler(DatabaseTypeDesc outer)
        {
            return i =>
            {
                outer.DataDescList.RemoveAt(i);
                outer.WritableDataNameList.RemoveAt(i);
            };
        }
    }
}