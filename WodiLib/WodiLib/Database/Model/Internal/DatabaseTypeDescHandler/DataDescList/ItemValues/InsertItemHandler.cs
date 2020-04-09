// ========================================
// Project Name : WodiLib
// File Name    : InsertItemHandler.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database.DatabaseTypeDescHandler.DataDescList.ItemValues
{
    /// <summary>
    /// DatabaseItemDescList.InsertItemのイベントハンドラ
    /// </summary>
    [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
    internal class InsertItemHandler : OnInsertItemHandler<IFixedLengthDBItemValueList>
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

        public InsertItemHandler(DatabaseTypeDesc outer)
            : base(MakeHandler(outer), HandlerTag, false, canChangeEnabled: false)
        {
        }

        /// <summary>
        /// DatabaseItemDescList.InsertItemのイベントを生成する。
        /// </summary>
        /// <param name="outer">連係外部クラスインスタンス</param>
        /// <returns>InsertItemイベント</returns>
        private static Action<int, IFixedLengthDBItemValueList> MakeHandler(DatabaseTypeDesc outer)
        {
            return (i, values) =>
            {
                var name = (Database.DataName) "";
                outer.DataDescList.Insert(i, new DatabaseDataDesc(name, (DBItemValueList) values));
                outer.WritableDataNameList.Insert(i, name);
            };
        }
    }
}