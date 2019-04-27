// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityCollectionHandler.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    /// RestrictedCapacityCollection イベントハンドラ基底クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class RestrictedCapacityCollectionHandler<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベントタグ</summary>
        public string Tag { get; }

        /// <summary>削除可否フラグ</summary>
        public bool CanDelete { get; }

        /// <summary>ハンドラ有効フラグ</summary>
        public bool Enabled { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 実行アクション
        /// </summary>
        internal AnyAction<T> AnyAction { get; protected private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tag">[NotNull] イベントハンドラタグ</param>
        /// <param name="canDelete">削除可否フラグ</param>
        /// <param name="enabled">ハンドラ有効フラグ</param>
        /// <exception cref="ArgumentNullException">tag にnullが設定された場合</exception>
        public RestrictedCapacityCollectionHandler(string tag, bool canDelete, bool enabled)
        {
            if (tag == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tag)));

            Tag = tag;
            CanDelete = canDelete;
            Enabled = enabled;
        }
    }
}