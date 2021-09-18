// ========================================
// Project Name : WodiLib
// File Name    : NotifyTwoDimensionalListChangeEventType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Specialized;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト変更通知種別
    /// </summary>
    public class NotifyTwoDimensionalListChangeEventType : TypeSafeEnum<NotifyTwoDimensionalListChangeEventType>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Types
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     通知なし
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知は行われない。
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType None;

        /// <summary>
        ///     一度
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知は操作の内容によらず一度だけ呼ばれる。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作したすべての要素が、<see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に 操作方向に応じて
        ///         <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に
        ///         実行された操作種別が格納された <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作したすべての要素が、 <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に 操作方向に応じて
        ///         <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に
        ///         <see cref="NotifyCollectionChangedAction.Reset"/> が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度だけ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Once;

        /// <summary>
        ///     単純・行
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知はひとつのアクションだけが複数回呼ばれる。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作した要素が一つずつ、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に <see cref="Direction.Row"/> が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した行数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作したすべての要素が、<see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に
        ///         <see cref="Direction.Row"/> が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に
        ///         <see cref="NotifyCollectionChangedAction.Reset"/> が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度だけ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Simple_Row;

        /// <summary>
        ///     単純・列
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知はひとつのアクションだけが複数回呼ばれる。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作した要素が一つずつ、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に <see cref="Direction.Column"/>
        ///         が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した列数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作したすべての要素が、 <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に
        ///         <see cref="Direction.Column"/> が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に
        ///         <see cref="NotifyCollectionChangedAction.Reset"/> が格納された
        ///         実行された操作種別が格納された <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度だけ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Simple_Column;

        /// <summary>
        ///     単純・操作方向依存
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知はひとつのアクションだけが行または列が変更されるごとに呼び出される。
        ///     通知されるイベントの <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> は
        ///     操作方向に応じて <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が格納される。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作した要素が一つずつ、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に 操作方向に応じて
        ///         <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した行または列数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作したすべての要素が、<see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に 操作方向に応じて
        ///         <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に
        ///         <see cref="NotifyCollectionChangedAction.Reset"/> が格納された
        ///         実行された操作種別が格納された <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度だけ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Simple_Line;

        /// <summary>
        ///     単純・個別
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知はひとつのアクションだけが複数回呼ばれる。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作した要素が一つずつ、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した要素数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作したすべての要素が、<see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に 操作方向に応じて
        ///         <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に
        ///         <see cref="NotifyCollectionChangedAction.Reset"/> が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度だけ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Simple_All;

        /// <summary>
        ///     単一
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知はアクションごとに一度だけ呼ばれる。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作したすべての要素が、 <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に 操作方向に応じて
        ///         <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に
        ///         実行された操作種別が格納された <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> ごとに
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が一度ずつ通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Single;

        /// <summary>
        ///     複数・行
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知は行が変更されるごとに呼び出される。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に <see cref="Direction.Row"/> が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した行数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         操作した行・アクションごとに <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Multi_Row;

        /// <summary>
        ///     複数・列
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知は列が変更されるごとに呼び出される。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> に <see cref="Direction.Column"/>
        ///         が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した列数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         操作した列・アクションごとに <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Multi_Column;

        /// <summary>
        ///     複数・不定
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知は行または列が変更されるごとに呼び出される。
        ///     通知されるイベントの <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Direction"/> は
        ///     操作方向に応じて <see cref="Direction.Row"/> または <see cref="Direction.Column"/> が格納される。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した行または列列数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         操作した行または列・アクションごとに <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Multi_Line;

        /// <summary>
        ///     複数・個別
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、二次元リスト変更前/後の通知は一要素が変更されるごとに呼び出される。
        ///     <para>
        ///         AddRange のような同一操作を複数回行うタイプの操作の場合、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.NewItems"/> や
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.OldItems"/> に
        ///         操作した要素が一つずつ、
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> に実行された操作種別が格納された
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が操作した要素数と同じ数だけ通知される。
        ///     </para>
        ///     <para>
        ///         <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}.Action"/> が複数にまたがる操作(Overwrite)の場合、
        ///         操作した要素・アクションごとに <see cref="TwoDimensionalCollectionChangeEventInternalArgs{T}"/> が通知される。
        ///     </para>
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventType Multi_All;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Static Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static NotifyTwoDimensionalListChangeEventType()
        {
            None = new NotifyTwoDimensionalListChangeEventType("None", false, false,
                NotifyTwoDimensionalListChangeEventGroupingType.None);
            Once = new NotifyTwoDimensionalListChangeEventType("Once", true, false,
                NotifyTwoDimensionalListChangeEventGroupingType.All);
            Simple_Row = new NotifyTwoDimensionalListChangeEventType("Simple_Row", true, false,
                NotifyTwoDimensionalListChangeEventGroupingType.Row);
            Simple_Column = new NotifyTwoDimensionalListChangeEventType("Simple_Column", true, false,
                NotifyTwoDimensionalListChangeEventGroupingType.Column);
            Simple_Line = new NotifyTwoDimensionalListChangeEventType("Simple_Line", true, false,
                NotifyTwoDimensionalListChangeEventGroupingType.Direct);
            Simple_All = new NotifyTwoDimensionalListChangeEventType("Simple_All", true, false,
                NotifyTwoDimensionalListChangeEventGroupingType.None);
            Single = new NotifyTwoDimensionalListChangeEventType("Single", true, true,
                NotifyTwoDimensionalListChangeEventGroupingType.All);
            Multi_Row = new NotifyTwoDimensionalListChangeEventType("Multi_Row", true, true,
                NotifyTwoDimensionalListChangeEventGroupingType.Row);
            Multi_Column = new NotifyTwoDimensionalListChangeEventType("Multi_Column", true, true,
                NotifyTwoDimensionalListChangeEventGroupingType.Column);
            Multi_Line = new NotifyTwoDimensionalListChangeEventType("Multi_Line", true, true,
                NotifyTwoDimensionalListChangeEventGroupingType.Direct);
            Multi_All = new NotifyTwoDimensionalListChangeEventType("Multi_All", true, true,
                NotifyTwoDimensionalListChangeEventGroupingType.None);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // ______________________ From ______________________

        /// <summary>
        ///     IDからイベント種別を取得する。
        /// </summary>
        /// <param name="id">対象ID</param>
        /// <returns>IDから取得したインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="id"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     指定した <paramref name="id"/> に該当するイベント種別が存在しない場合。
        /// </exception>
        public static NotifyTwoDimensionalListChangeEventType FromId(string id)
        {
            ThrowHelper.ValidateArgumentNotNull(id is null, nameof(id));
            return _FindFirst(x => x.Id.Equals(id));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     通知フラグ
        /// </summary>
        public bool IsNotify { get; }

        /// <summary>
        ///     複数アクション通知フラグ
        /// </summary>
        public bool IsMultiAction { get; }

        /// <summary>
        ///     同一アクション複数通知要素グルーピング種別
        /// </summary>
        public NotifyTwoDimensionalListChangeEventGroupingType GroupingType { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private NotifyTwoDimensionalListChangeEventType(string id, bool isNotify, bool isMultiAction,
            NotifyTwoDimensionalListChangeEventGroupingType groupingType) :
            base(id)
        {
            IsNotify = isNotify;
            IsMultiAction = isMultiAction;
            GroupingType = groupingType;
        }
    }
}
