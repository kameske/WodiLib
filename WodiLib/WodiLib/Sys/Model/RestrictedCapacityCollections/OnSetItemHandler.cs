// ========================================
// Project Name : WodiLib
// File Name    : OnSetItemHandler.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection.SetItemイベントハンドラ
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public class OnSetItemHandler<T> : RestrictedCapacityCollectionHandler<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベントハンドラ</summary>
        public Action<int, T> Handler { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="handler">[NotNull] イベントハンドラ</param>
        /// <param name="tag">[NotNull] イベントハンドラタグ</param>
        /// <param name="canDelete">削除可否フラグ</param>
        /// <param name="enabled">ハンドラ有効フラグ</param>
        /// <param name="canChangeEnabled">ハンドラ有効フラグ変更可否フラグ</param>
        /// <exception cref="ArgumentNullException">handler, tag にnullが設定された場合</exception>
        public OnSetItemHandler(Action<int, T> handler, string tag = "",
            bool canDelete = true, bool enabled = true,
            bool canChangeEnabled = true) : base(tag, canDelete, enabled, canChangeEnabled)
        {
            if (handler is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(handler)));

            Handler = handler;
            AnyAction = new AnyAction<T>(handler);
        }
    }
}