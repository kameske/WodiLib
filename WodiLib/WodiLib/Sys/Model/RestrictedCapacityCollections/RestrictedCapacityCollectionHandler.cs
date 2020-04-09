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
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
    public abstract class RestrictedCapacityCollectionHandler<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベントタグ</summary>
        public string Tag { get; }

        /// <summary>削除可否フラグ</summary>
        public bool CanDelete { get; }

        private bool enabled;

        /// <summary>ハンドラ有効フラグ</summary>
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (!CanChangeEnabled)
                    throw new PropertyException(
                        $"{nameof(CanChangeEnabled)}がfalseのため、{nameof(Enabled)}を変更することはできません。");
                enabled = value;
            }
        }

        /// <summary>ハンドラ有効フラグ変更可否フラグ</summary>
        public bool CanChangeEnabled { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 実行アクション
        /// </summary>
        internal AnyAction<T> AnyAction { get; private protected set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tag">[NotNull] イベントハンドラタグ</param>
        /// <param name="canDelete">削除可否フラグ</param>
        /// <param name="enabled">ハンドラ有効フラグ</param>
        /// <param name="canChangeEnabled">ハンドラ有効フラグ変更可否フラグ</param>
        /// <exception cref="ArgumentNullException">tag にnullが設定された場合</exception>
        public RestrictedCapacityCollectionHandler(string tag, bool canDelete, bool enabled, bool canChangeEnabled)
        {
            if (tag is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tag)));

            Tag = tag;
            CanDelete = canDelete;
            this.enabled = enabled;
            CanChangeEnabled = canChangeEnabled;
        }
    }
}