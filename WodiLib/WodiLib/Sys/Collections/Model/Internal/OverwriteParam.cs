// ========================================
// Project Name : WodiLib
// File Name    : OverwriteParam.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// <see cref="ExtendedList{T}.Overwrite_Impl"/> の実行パラメータ
    /// </summary>
    internal record OverwriteParam<T>
    {
        public T[] ReplaceOldItems { get; }
        public T[] ReplaceNewItems { get; }

        public int InsertStartIndex { get; }
        public T[] InsertItems { get; }

        public string[] NotifyProperties { get; }

        private OverwriteParam(T[] replaceOldItems, T[] replaceNewItems, int insertStartIndex, T[] insertItems,
            string[] notifyProperties)
        {
            ReplaceOldItems = replaceOldItems;
            ReplaceNewItems = replaceNewItems;
            InsertStartIndex = insertStartIndex;
            InsertItems = insertItems;
            NotifyProperties = notifyProperties;
        }

        /// <summary>
        /// <see cref="ExtendedList{T}.Overwrite_Impl"/> の実行パラメータFactoryクラス
        /// </summary>
        public static class Factory
        {
            public static OverwriteParam<T> Create(IReadOnlyExtendedList<T> target, int index, params T[] items)
            {
                var updateCnt = index + items.Length > target.Count
                    ? target.Count - index
                    : items.Length;

                // 上書き要素
                var replaceItems = items.Take(updateCnt).ToArray();
                var replaceOldItems = target.GetRange(index, updateCnt).ToArray();

                // 追加要素
                var insertStartIndex = index + updateCnt;
                var insertItems = items.Skip(updateCnt).ToArray();

                var notifyProperties = new List<string>();

                if (insertItems.Length > 0)
                {
                    notifyProperties.Add(nameof(IList.Count));
                }

                notifyProperties.Add(ListConstant.IndexerName);

                return new OverwriteParam<T>(replaceOldItems, replaceItems, insertStartIndex, insertItems,
                    notifyProperties.ToArray());
            }
        }
    }
}
