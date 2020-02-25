// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityCollectionHandlerList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollectionイベントハンドラリスト基底クラス
    /// </summary>
    /// <typeparam name="TItem">リスト内包クラス</typeparam>
    /// <typeparam name="THandler">ハンドラクラス</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class
        RestrictedCapacityCollectionHandlerList<TItem, THandler> : Collection<THandler>
        where THandler : RestrictedCapacityCollectionHandler<TItem>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>エラーメッセージ</summary>
        private static readonly string ErrorMsg = "削除不可能なイベントハンドラのため{0}できません。";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RestrictedCapacityCollectionHandlerList()
        {
        }

        /// <summary>
        /// コンストラクタ（初期値指定）
        /// </summary>
        /// <param name="list">[NotNull] 初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        public RestrictedCapacityCollectionHandlerList(IReadOnlyCollection<THandler> list)
        {
            if (list is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(list)));

            if (list.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(list)));

            var insertIndex = 0;
            foreach (var item in list)
            {
                InsertItem(insertIndex, item);
                insertIndex++;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override void SetItem(int index, THandler item)
        {
            if (item is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));

            if (!Items[index].CanDelete)
                throw new InvalidOperationException(
                    string.Format(ErrorMsg, "上書き"));

            base.SetItem(index, item);
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, THandler item)
        {
            if (item is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));

            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            if (!Items[index].CanDelete)
                throw new InvalidOperationException(
                    string.Format(ErrorMsg, "削除"));

            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void ClearItems()
        {
            // 削除不可のイベントハンドラは削除しない
            ((List<THandler>) Items).RemoveAll(x => x.CanDelete);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 有効なイベントハンドラをすべて実行する。
        /// </summary>
        /// <param name="params">イベントハンドラに渡すパラメータ</param>
        protected void Execute(params object[] @params)
        {
            Items.Where(x => x.Enabled).ToList()
                .ForEach(x => x.AnyAction.Execute(@params));
        }
    }
}