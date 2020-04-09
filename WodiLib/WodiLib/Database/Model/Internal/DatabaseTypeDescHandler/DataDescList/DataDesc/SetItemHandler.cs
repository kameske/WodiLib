// ========================================
// Project Name : WodiLib
// File Name    : SetItemHandler.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database.DatabaseTypeDescHandler.DataDescList.DataDesc
{
    /// <summary>
    /// DatabaseDataDescList.SetItemのイベントハンドラ
    /// </summary>
    [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
    internal class SetItemHandler : OnSetItemHandler<DatabaseDataDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// リストイベントハンドラにつけるタグ
        /// </summary>
        public static readonly string HandlerTag = "__SYSTEM__";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public SetItemHandler(DatabaseTypeDesc outer)
            : base(MakeHandler(outer), HandlerTag, false, canChangeEnabled: false)
        {
        }

        /// <summary>
        /// DatabaseItemDataList.SetItemのイベントを生成する。
        /// </summary>
        /// <param name="outer">連係外部クラスインスタンス</param>
        /// <returns>InsertItemイベント</returns>
        private static Action<int, DatabaseDataDesc> MakeHandler(DatabaseTypeDesc outer)
        {
            return (i, set) =>
            {
                // Outerを解除したItemValueListを渡す、かつ元のItemValueListのOuterは解除しない
                outer.WritableItemValuesList[i] = new DBItemValueList(set.ItemValueList);

                outer.WritableDataNameList[i] = set.DataName;
            };
        }
    }
}