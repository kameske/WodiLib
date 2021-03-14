// ========================================
// Project Name : WodiLib
// File Name    : NotifyCollectionChangeEventType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Specialized;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     コレクション変更通知種別
    /// </summary>
    public record NotifyCollectionChangeEventType : TypeSafeEnum<NotifyCollectionChangeEventType>
    {
        /// <summary>
        ///     通知なし
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、コレクション変更前/後の通知は行われない。
        /// </remarks>
        public static readonly NotifyCollectionChangeEventType None;

        /// <summary>
        ///     一度
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、コレクション変更前/後の通知は操作の内容によらず一度だけ呼ばれる。
        ///     <para>
        ///     <see cref="IExtendedList{T}.AddRange"/> のような同一操作を複数回行うタイプの操作の場合、
        ///     <see cref="NotifyCollectionChangedEventArgs.NewItems"/> や <see cref="NotifyCollectionChangedEventArgs.OldItems"/> に
        ///     操作したすべての要素が格納され、 <see cref="NotifyCollectionChangedEventArgs.Action"/> に
        ///     実行された操作種別が格納された <see cref="NotifyCollectionChangedEventArgs"/> が一度だけ通知される。
        ///     </para>
        ///     <para>
        ///     <see cref="NotifyCollectionChangedEventArgs.Action"/> が複数にまたがる操作(<see cref="IExtendedList{T}.Overwrite"/>)の場合、
        ///     <see cref="NotifyCollectionChangedEventArgs.NewItems"/> や <see cref="NotifyCollectionChangedEventArgs.OldItems"/> に
        ///     操作したすべての要素が格納され、 <see cref="NotifyCollectionChangedEventArgs.Action"/> に
        ///     <see cref="NotifyCollectionChangedAction.Reset"/> が格納された
        ///     実行された操作種別が格納された <see cref="NotifyCollectionChangedEventArgs"/> が一度だけ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyCollectionChangeEventType Once;

        /// <summary>
        ///     単純
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、コレクション変更前/後の通知はひとつのアクションだけが複数回呼ばれる。
        ///     <para>
        ///     <see cref="IExtendedList{T}.AddRange"/> のような同一操作を複数回行うタイプの操作の場合、
        ///     <see cref="NotifyCollectionChangedEventArgs.Action"/> に実行された操作種別が格納された
        ///     <see cref="NotifyCollectionChangedEventArgs"/> が操作した要素数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///     <see cref="NotifyCollectionChangedEventArgs.Action"/> が複数にまたがる操作(<see cref="IExtendedList{T}.Overwrite"/>)の場合、
        ///     <see cref="NotifyCollectionChangedEventArgs.NewItems"/> や <see cref="NotifyCollectionChangedEventArgs.OldItems"/> に
        ///     操作したすべての要素が格納され、 <see cref="NotifyCollectionChangedEventArgs.Action"/> に
        ///     <see cref="NotifyCollectionChangedAction.Reset"/> が格納された
        ///     実行された操作種別が格納された <see cref="NotifyCollectionChangedEventArgs"/> が一度だけ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyCollectionChangeEventType Simple;

        /// <summary>
        ///     単一
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、コレクション変更前/後の通知はアクションごとに一度だけ呼ばれる。
        ///     <para>
        ///     <see cref="IExtendedList{T}.AddRange"/> のような同一操作を複数回行うタイプの操作の場合、
        ///     <see cref="NotifyCollectionChangedEventArgs.NewItems"/> や <see cref="NotifyCollectionChangedEventArgs.OldItems"/> に
        ///     操作したすべての要素が格納され、 <see cref="NotifyCollectionChangedEventArgs.Action"/> に
        ///     実行された操作種別が格納された <see cref="NotifyCollectionChangedEventArgs"/> が一度だけ通知される。
        ///     </para>
        ///     <para>
        ///     <see cref="NotifyCollectionChangedEventArgs.Action"/> が複数にまたがる操作(<see cref="IExtendedList{T}.Overwrite"/>)の場合、
        ///     <see cref="NotifyCollectionChangedEventArgs.Action"/> ごとに
        ///     <see cref="NotifyCollectionChangedEventArgs"/> が一度ずつ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyCollectionChangeEventType Single;

        /// <summary>
        ///     複数
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、コレクション変更前/後の通知は一要素が変更されるごとに呼び出される。
        ///     <para>
        ///     <see cref="IExtendedList{T}.AddRange"/> のような同一操作を複数回行うタイプの操作の場合、
        ///     <see cref="NotifyCollectionChangedEventArgs.Action"/> に実行された操作種別が格納された
        ///     <see cref="NotifyCollectionChangedEventArgs"/> が操作した要素数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///     <see cref="NotifyCollectionChangedEventArgs.Action"/> が複数にまたがる操作(<see cref="IExtendedList{T}.Overwrite"/>)の場合、
        ///     操作した要素数と同じ数だけ <see cref="NotifyCollectionChangedEventArgs"/> が通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyCollectionChangeEventType Multi;

        /// <summary>
        /// IDからイベント種別を取得する。
        /// </summary>
        /// <param name="id">対象ID</param>
        /// <returns>IDから取得したインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="id"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 指定した <paramref name="id"/> に該当するイベント種別が存在しない場合。
        /// </exception>
        public static NotifyCollectionChangeEventType FromId(string id)
        {
            ThrowHelper.ValidateArgumentNotNull(id is null, nameof(id));
            return _FindFirst(x => x.Id.Equals(id));
        }

        /// <summary>
        /// 通知フラグ
        /// </summary>
        public bool IsNotify { get; }

        /// <summary>
        /// 複数アクション通知フラグ
        /// </summary>
        public bool IsMultiAction { get; }

        /// <summary>
        /// 同一アクション複数通知フラグ
        /// </summary>
        public bool IsMultipart { get; }

        static NotifyCollectionChangeEventType()
        {
            None = new NotifyCollectionChangeEventType("None", false, false, false);
            Once = new NotifyCollectionChangeEventType("Once", true, false, false);
            Simple = new NotifyCollectionChangeEventType("Simple", true, false, true);
            Single = new NotifyCollectionChangeEventType("Single", true, true, false);
            Multi = new NotifyCollectionChangeEventType("Multi", true, true, true);
        }

        private NotifyCollectionChangeEventType(string id, bool isNotify, bool isMultiAction, bool isMultipart) :
            base(id)
        {
            IsNotify = isNotify;
            IsMultiAction = isMultiAction;
            IsMultipart = isMultipart;
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
