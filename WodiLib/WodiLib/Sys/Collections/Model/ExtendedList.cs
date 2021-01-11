// ========================================
// Project Name : WodiLib
// File Name    : ExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// WodiLib 独自リスト
    /// </summary>
    /// <remarks>
    /// 機能概要は <seealso cref="IExtendedList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Serializable]
    public partial class ExtendedList<T> : ModelBase<ExtendedList<T>>, IExtendedList<T>
    {
        /*
         * WodiLib 内部で使用する独自汎用リスト。
         * リストとしての機能は 内部クラス Impl に実装。
         * ExtendedList はコレクション変更通知を行うためのラッパークラスとする。
         *
         * 引数の検証も当クラスでは行わない。
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event NotifyCollectionChangedEventHandler? _collectionChanging;

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanging
        {
            add
            {
                if (_collectionChanging != null
                    && _collectionChanging.GetInvocationList().Contains(value)) return;
                _collectionChanging += value;
            }
            remove => _collectionChanging -= value;
        }

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event NotifyCollectionChangedEventHandler? _collectionChanged;

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (_collectionChanged != null
                    && _collectionChanged.GetInvocationList().Contains(value)) return;
                _collectionChanged += value;
            }
            remove => _collectionChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IExtendedList{T}.this" />
        public T this[int index]
        {
            get => Get_Impl(index, 1).First();
            set => Set_Impl(index, value);
        }

        T IReadOnlyList<T>.this[int index] => Get_Impl(index, 1).First();

        /// <inheritdoc cref="IExtendedList{T}.Count" />
        public int Count => Items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>
        /// 要素初期化関数
        /// </summary>
        /// <remarks>
        /// 第一引数：開始インデックス<br/>
        /// 第二引数：必要要素数<br/>
        /// 呼び出し元で設定必須。
        /// </remarks>
        public Func<int, int, IEnumerable<T>> FuncMakeItems { get; set; } = default!;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト本体</summary>
        private Impl Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        internal ExtendedList(IEnumerable<T>? initItems = null)
        {
            var initItemArray = initItems?.ToArray() ?? Array.Empty<T>();

            Items = new Impl(initItemArray);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc cref="IExtendedList{T}.IndexOf" />
        public int IndexOf([AllowNull] T item)
            => Items.IndexOf(item);

        /// <inheritdoc cref="IExtendedList{T}.Contains" />
        public bool Contains([AllowNull] T item)
            => Items.Contains(item);

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count)
            => Get_Impl(index, count);

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<T> items)
            => Set_Impl(index, items.ToArray());

        /// <inheritdoc/>
        public void Add(T item)
            => Insert_Impl(Count, item);

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> items)
            => Insert_Impl(Count, items.ToArray());

        /// <inheritdoc/>
        public void Insert(int index, T item)
            => Insert_Impl(index, item);

        /// <inheritdoc/>
        public void InsertRange(int index, IEnumerable<T> items)
            => Insert_Impl(index, items.ToArray());

        /// <inheritdoc/>
        public void Overwrite(int index, IEnumerable<T> items)
            => Overwrite_Impl(index, items.ToArray());

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex)
            => Move_Impl(oldIndex, newIndex, 1);

        /// <inheritdoc/>
        public void MoveRange(int oldIndex, int newIndex, int count)
            => Move_Impl(oldIndex, newIndex, count);

        /// <inheritdoc/>
        public bool Remove(T item)
            => Remove_Impl(item);

        /// <inheritdoc/>
        public void RemoveAt(int index)
            => Remove_Impl(index, 1);

        /// <inheritdoc/>
        public void RemoveRange(int index, int count)
            => Remove_Impl(index, count);

        /// <inheritdoc/>
        public void AdjustLength(int length)
            => AdjustLength_Impl(length);

        /// <inheritdoc/>
        public void AdjustLengthIfShort(int length)
            => AdjustLengthIfShort_Impl(length);

        /// <inheritdoc/>
        public void AdjustLengthIfLong(int length)
            => AdjustLengthIfLong_Impl(length);

        /// <inheritdoc/>
        public void Reset(IEnumerable<T> initItems)
            => Reset_Impl(initItems.ToArray());

        /// <inheritdoc/>
        public void Clear()
            => Clear_Impl();

        /// <inheritdoc cref="IExtendedList{T}.IndexOf" />
        public void CopyTo(T[] array, int arrayIndex)
            => Items.CopyTo(array, arrayIndex);

        #region Equals

        /// <inheritdoc />
        public override bool Equals(ExtendedList<T>? other)
            => Equals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool Equals(IExtendedList<T>? other)
            => Equals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool Equals(IReadOnlyExtendedList<T>? other)
            => Equals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool Equals(IReadOnlyList<T>? other)
            => Equals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool Equals(IEnumerable<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Action Implements

        /// <summary>
        /// インデクサによる要素取得、GetRange メソッドの実装処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        /// <returns></returns>
        protected IEnumerable<T> Get_Impl(int index, int count)
            => Items.Get(index, count);

        /// <summary>
        /// インデクサによる要素更新、SetRange メソッドの実装処理
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        protected void Set_Impl(int index, params T[] items)
        {
            var oldItems = Items.Get(index, items.Length);
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Set(items, oldItems.ToList(), index);

            CallCollectionChanging(eventArgs);

            Items.Set(index, items);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// Add, AddRange, Insert, InsertRange メソッドの実装処理
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        protected void Insert_Impl(int index, params T[] items)
        {
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Insert(items, index);

            CallCollectionChanging(eventArgs);

            Items.Insert(index, items);

            NotifyPropertyChanged(nameof(IList.Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// Overwrite メソッドの実装処理
        /// </summary>
        /// <param name="index">上書き開始インデックス</param>
        /// <param name="items">上書き要素</param>
        protected void Overwrite_Impl(int index, params T[] items)
        {
            /*
             * 要素を上書きする場合に Set、追加する場合に Insert の処理をそれぞれ実行する。
             * PropertyChanged は "Item[]", "Count" ともに通知するが、
             * CollectionChange は実行するものだけを通知。
             * そのため実行する必要がある処理を事前に作成しておき、最後にまとめて実行する。
             */

            // 変更前通知アクションリスト
            var actionCallCollectionChangingList = new List<Action>();
            // 処理本体リスト
            var actionCoreList = new List<Action>();
            // 変更後通知アクションリスト
            var actionCallCollectionChangedList = new List<Action>();

            var updateCnt = index + items.Length > Count
                ? Count - index
                : items.Length;

            // 上書き要素
            {
                var replaceOldItems = Items.Get(index, updateCnt);
                var replaceItems = items.Take(updateCnt).ToArray();

                var eventArgs = NotifyCollectionChangedEventArgsHelper.Set(
                    replaceItems, replaceOldItems.ToList(), index);

                actionCallCollectionChangingList.Add(
                    () => CallCollectionChanging(eventArgs)
                );
                actionCoreList.Add(
                    () => Items.Set(index, replaceItems)
                );
                actionCallCollectionChangedList.Add(
                    () => CallCollectionChanged(eventArgs)
                );
            }

            // 追加要素
            {
                var insertStartIndex = index + updateCnt;
                var insertItems = items.Skip(updateCnt).ToArray();

                var eventArgs = NotifyCollectionChangedEventArgsHelper.Insert(
                    insertItems, insertStartIndex);

                actionCallCollectionChangingList.Add(
                    () => CallCollectionChanging(eventArgs)
                );
                actionCoreList.Add(
                    () => Items.Insert(insertStartIndex, insertItems)
                );
                actionCallCollectionChangedList.Add(
                    () => CallCollectionChanged(eventArgs)
                );
            }

            // 処理本体
            actionCallCollectionChangingList.ForEach(action => action());

            actionCoreList.ForEach(action => action());

            NotifyPropertyChanged(nameof(IList.Count));
            NotifyPropertyChanged(ListConstant.IndexerName);

            actionCallCollectionChangedList.ForEach(action => action());
        }

        /// <summary>
        /// Move, MoveRange メソッドの実装処理
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        protected void Move_Impl(int oldIndex, int newIndex, int count)
        {
            var movedItems = Items.Get(oldIndex, count);
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Move(
                movedItems.ToArray(), newIndex, oldIndex);

            CallCollectionChanging(eventArgs);

            Items.Move(oldIndex, newIndex, count);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// Remove メソッドの実装処理
        /// </summary>
        /// <param name="item">除去対象</param>
        /// <returns>削除成否</returns>
        protected bool Remove_Impl([AllowNull] T item)
        {
            if (item is null) return false;

            var index = this.FindIndex(x => EquatableCompareHelper.Equals(x, item));
            if (index < 0) return false;

            var eventArgs = NotifyCollectionChangedEventArgsHelper.Remove(new[] {item}, index);

            CallCollectionChanging(eventArgs);

            Items.Remove(index, 1);

            NotifyPropertyChanged(nameof(IList.Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);

            return true;
        }

        /// <summary>
        /// Remove, RemoveRange メソッドの実装処理
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        protected void Remove_Impl(int index, int count)
        {
            var removeItems = Get_Impl(index, count)
                .ToArray();
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Remove(removeItems, index);

            CallCollectionChanging(eventArgs);

            Items.Remove(index, count);

            NotifyPropertyChanged(nameof(IList.Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// AdjustLength メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustLength_Impl(int length)
        {
            if (length == Count)
            {
                NotifyPropertyChanged(nameof(Count));
                NotifyPropertyChanged(ListConstant.IndexerName);

                return;
            }

            if (Count > length)
            {
                AdjustLengthIfLong_Main(length);
            }
            else // Count < length
            {
                AdjustLengthIfShort_Main(length);
            }
        }

        /// <summary>
        /// AdjustLengthIfShort メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustLengthIfShort_Impl(int length)
        {
            AdjustLengthIfShort_Main(length);
        }

        /// <summary>
        /// AdjustLengthIfLong メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustLengthIfLong_Impl(int length)
        {
            AdjustLengthIfLong_Main(length);
        }

        /// <summary>
        /// AdjustLengthIfShort メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        private void AdjustLengthIfShort_Main(int length)
        {
            if (length <= Count)
            {
                NotifyPropertyChanged(nameof(Count));
                NotifyPropertyChanged(ListConstant.IndexerName);
                return;
            }

            var startIndex = Count;
            var items = FuncMakeItems(startIndex, length - startIndex)
                .ToArray();

            var eventArgs = NotifyCollectionChangedEventArgsHelper.Insert(items, startIndex);

            CallCollectionChanging(eventArgs);

            Items.Insert(startIndex, items);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// AdjustLengthIfLong メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        private void AdjustLengthIfLong_Main(int length)
        {
            if (length >= Count)
            {
                NotifyPropertyChanged(nameof(Count));
                NotifyPropertyChanged(ListConstant.IndexerName);
                return;
            }

            var index = length;
            var count = Count - length;
            var removeItems = Get_Impl(index, count)
                .ToArray();
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Remove(removeItems, index);

            CallCollectionChanging(eventArgs);

            Items.Remove(index, count);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// Clear メソッドの実装処理
        /// </summary>
        protected void Clear_Impl()
        {
            Reset_Impl(Array.Empty<T>());
        }

        /// <summary>
        /// Reset メソッドの実装処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        protected void Reset_Impl(params T[] items)
        {
            // Replace, Add, Remove のうち必要な処理を先にAction化 → 最後にまとめて実行
            var coreActions = new List<Action>();

            var replaceLength = Math.Min(items.Length, Count);
            var insertLength = items.Length - Count;

            if (replaceLength != 0)
            {
                var newItems = items.Take(replaceLength).ToArray();

                coreActions.Add(() => Items.Set(0, newItems));
            }

            if (insertLength > 0)
            {
                var index = Count;
                var insertItems = items.Skip(replaceLength).ToArray();

                coreActions.Add(() => Items.Insert(index, insertItems));
            }

            if (insertLength < 0)
            {
                var removeLength = Math.Abs(insertLength);
                var index = Count - removeLength;

                coreActions.Add(() => Items.Remove(index, removeLength));
            }

            var eventArgs = NotifyCollectionChangedEventArgsHelper.Clear();
            CallCollectionChanging(eventArgs);

            // 必要な処理を順次実行
            coreActions.ForEach(action => action.Invoke());

            NotifyPropertyChanged(nameof(IList.Count));
            NotifyPropertyChanged(ListConstant.IndexerName);

            CallCollectionChanged(eventArgs);
        }

        #endregion

        /// <summary>
        /// PreCollectionChanged イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanging(NotifyCollectionChangedEventArgs args)
            => _collectionChanging?.Invoke(this, args);

        /// <summary>
        /// CollectionChanged イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanged(NotifyCollectionChangedEventArgs args)
            => _collectionChanged?.Invoke(this, args);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ExtendedList(SerializationInfo info, StreamingContext context)
        {
            Items = info.GetValue<Impl>(nameof(Items));
        }

        /// <inheritdoc/>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Items), Items);
        }
    }
}
