// ========================================
// Project Name : WodiLib
// File Name    : ExtendedListBase.cs
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
    /// 各種リストの基底クラス
    /// </summary>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ExtendedListBase<T> : ModelBase<ExtendedListBase<T>>, IReadOnlyExtendedList<T>
    {
        /*
         * このクラスではすべてのリストで必要な共通処理についてのみ定義。
         * リストの実体や各リストごとの個別の処理は継承先でそれぞれ定義する。
         * また、このリスト自身は各種操作を外部に公開しない。必要に応じて継承先で処理を公開する。
         * 各種リストインタフェースも継承していないため、継承先のクラスでインタフェースも合わせて継承が必要。
         *
         * 各種メソッドについて
         * ・XXX_Impl
         *     各種操作の本体。
         *     外部に公開したい操作はこのメソッドに処理を転送する。
         * ・ValidateXXX
         *     リスト共通の引数検証処理後に呼び出される。
         *     継承先のクラスで独自の引数検証が必要ならオーバーライド。
         *     デフォルトでは何もしない。
         * ・XXX_Core
         *     各種操作の実装。
         *     継承先のクラスで該当する処理を行うならオーバーライド。
         *     デフォルトでは NotSupportedException を発生させる。
         *
         * XXX_Implメソッドの処理の流れ
         *     １．引数の NotNull 検証
         *     ２．リスト共通の引数検証
         *     ３．ValidateXXX メソッド実行
         *     ４．CollectionChanging イベント発火
         *     ５．XXX_Core メソッド実行
         *     ６．NotifyPropertyChanged イベント発火
         *     ７．CollectionChangedイベント発火
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Virtual Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 要素変更誤通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => CollectionChanged_Impl += value;
            remove => CollectionChanged_Impl -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event NotifyCollectionChangedEventHandler? _collectionChanging;

        /// <summary>
        /// 要素変更前通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        protected event NotifyCollectionChangedEventHandler CollectionChanging_Impl
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

        /// <summary>
        /// 要素変更誤通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        protected event NotifyCollectionChangedEventHandler CollectionChanged_Impl
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
        //     Abstract Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 要素数
        /// </summary>
        public abstract int Count { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">index が指定範囲外の場合</exception>
        public T this[int index]
            => Get_Impl(index, 1).First();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 各種操作時の検証処理実装
        /// </summary>
        private IExtendedListValidator<T>? Validator { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected ExtendedListBase()
        {
            Validator = MakeValidator();

            var initItems = MakeInitItems()
                .ToArray();
            Constructor_Impl(initItems);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        /// <exception cref="ArgumentNullException">initItems が null の場合</exception>
        protected ExtendedListBase(IEnumerable<T> initItems)
        {
            if (initItems is null)
            {
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(initItems)));
            }

            Validator = MakeValidator();

            var itemArray = initItems.ToArray();
            Constructor_Impl(itemArray);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public abstract IEnumerator<T> GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count)
            => Get_Impl(index, count);

        #region Equals

        /// <inheritdoc />
        public bool Equals(IReadOnlyExtendedList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public bool Equals(IReadOnlyList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public bool Equals(IEnumerable<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Interface Implements
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 要素のデフォルト値を生成する。
        /// </summary>
        /// <remarks>
        /// AdjustLength で要素を追加する必要がある際に
        /// このメソッドで作成された値を追加する。
        /// </remarks>
        /// <param name="index">インデックス</param>
        /// <returns>追加要素</returns>
        protected abstract T MakeDefaultItem(int index);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// IExtendedListValidator&gt;T&lt; インスタンスを作成する。
        /// </summary>
        /// <remarks>
        /// このメソッドはコンストラクタの冒頭で呼ばれる。
        /// </remarks>
        /// <returns></returns>
        protected virtual IExtendedListValidator<T> MakeValidator()
        {
            return new CommonListValidator<T>(this);
        }

        #region Action Core

        /// <summary>
        /// コンストラクタ処理本体
        /// </summary>
        /// <param name="initItems">初期化要素</param>
        protected virtual void Constructor_Core(params T[] initItems)
            => throw new NotSupportedException();

        /// <summary>
        /// インデクサによる要素取得、GetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        protected virtual T[] Get_Core(int index, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// インデクサによる要素更新、SetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        protected virtual void Set_Core(int index, params T[] items)
            => throw new NotSupportedException();

        /// <summary>
        /// Add, AddRange, Insert, InsertRange メソッドの処理本体
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        protected virtual void Insert_Core(int index, params T[] items)
            => throw new NotSupportedException();

        /// <summary>
        /// Move, MoveRange メソッドの処理本体
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        protected virtual void Move_Core(int oldIndex, int newIndex, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// RemoveAt, Remove, RemoveRange メソッドの処理本体
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        protected virtual void Remove_Core(int index, int count)
            => throw new NotSupportedException();

        #endregion

        /// <summary>
        /// 引数なしコンストラクタ、Clear メソッド実行時にListを初期化するための要素を作成する。
        /// </summary>
        /// <returns>初期化要素。デフォルトでは空要素。</returns>
        protected virtual IEnumerable<T> MakeInitItems()
        {
            return Array.Empty<T>();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Action Implements

        /// <summary>
        /// コンストラクタ処理実装
        /// </summary>
        /// <param name="initItems">初期化要素</param>
        protected void Constructor_Impl(params T[] initItems)
        {
            Validator?.Constructor(initItems);

            Constructor_Core(initItems);
        }

        /// <summary>
        /// インデクサによる要素取得、GetRange メソッドの実装処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        /// <returns></returns>
        protected IEnumerable<T> Get_Impl(int index, int count)
        {
            Validator?.Get(index, count);

            return Get_Core(index, count);
        }

        /// <summary>
        /// インデクサによる要素更新、SetRange メソッドの実装処理
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        protected void Set_Impl(int index, params T[] items)
        {
            Validator?.Set(index, items);

            var oldItems = Get_Core(index, items.Length);
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Set(items, oldItems, index);

            CallCollectionChanging(eventArgs);

            Set_Core(index, items);

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
            Validator?.Insert(index, items);

            var eventArgs = NotifyCollectionChangedEventArgsHelper.Insert(items, index);

            CallCollectionChanging(eventArgs);

            Insert_Core(index, items);

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
            Validator?.Overwrite(index, items);

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
                var replaceOldItems = Get_Core(index, updateCnt);
                var replaceItems = items.Take(updateCnt).ToArray();

                var eventArgs = NotifyCollectionChangedEventArgsHelper.Set(
                    replaceItems, replaceOldItems, index);

                actionCallCollectionChangingList.Add(
                    () => CallCollectionChanging(eventArgs)
                );
                actionCoreList.Add(
                    () => Set_Core(index, replaceItems)
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
                    () => Insert_Core(insertStartIndex, insertItems)
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
            Validator?.Move(oldIndex, newIndex, count);

            var movedItems = Get_Core(oldIndex, count);
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Move(movedItems, newIndex, oldIndex);

            CallCollectionChanging(eventArgs);

            Move_Core(oldIndex, newIndex, count);

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
            Validator?.Remove(item);

            if (item is null) return false;

            var index = this.FindIndex(x => EquatableCompareHelper.Equals(x, item));
            if (index < 0) return false;

            Validator?.Remove(index, 1);

            var eventArgs = NotifyCollectionChangedEventArgsHelper.Remove(new[] {item}, index);

            CallCollectionChanging(eventArgs);

            Remove_Core(index, 1);

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
            Validator?.Remove(index, count);

            var removeItems = Get_Impl(index, count)
                .ToArray();
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Remove(removeItems, index);

            CallCollectionChanging(eventArgs);

            Remove_Core(index, count);

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
            Validator?.AdjustLength(length);

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
            Validator?.AdjustLengthIfShort(length);

            AdjustLengthIfShort_Main(length);
        }

        /// <summary>
        /// AdjustLengthIfLong メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustLengthIfLong_Impl(int length)
        {
            Validator?.AdjustLengthIfLong(length);

            AdjustLengthIfLong_Main(length);
        }

        /// <summary>
        /// Clear メソッドの実装処理
        /// </summary>
        protected void Clear_Impl()
        {
            var items = MakeInitItems()
                .ToArray();
            Reset_Impl(items);
        }

        /// <summary>
        /// Reset メソッドの実装処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        protected void Reset_Impl(params T[] items)
        {
            Validator?.Reset(items);

            // Replace, Add, Remove のうち必要な処理を先にAction化 → 最後にまとめて実行
            var coreActions = new List<Action>();

            var replaceLength = Math.Min(items.Length, Count);
            var insertLength = items.Length - Count;

            if (replaceLength != 0)
            {
                var newItems = items.Take(replaceLength).ToArray();

                coreActions.Add(() => Set_Core(0, newItems));
            }

            if (insertLength > 0)
            {
                var index = Count;
                var insertItems = items.Skip(replaceLength).ToArray();

                coreActions.Add(() => Insert_Core(index, insertItems));
            }

            if (insertLength < 0)
            {
                var removeLength = Math.Abs(insertLength);
                var index = Count - removeLength;

                coreActions.Add(() => Remove_Core(index, removeLength));
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
            var items = MakeDefaultItemArray(startIndex, length - startIndex);

            var eventArgs = NotifyCollectionChangedEventArgsHelper.Insert(items, startIndex);

            CallCollectionChanging(eventArgs);

            Insert_Core(startIndex, items);

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

            Remove_Core(index, count);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// デフォルト要素配列を作成する。
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private T[] MakeDefaultItemArray(int startIndex, int count)
        {
            return Enumerable.Range(startIndex, count)
                .Select(MakeDefaultItem)
                .ToArray();
        }

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
        protected ExtendedListBase(SerializationInfo info, StreamingContext context)
        {
            Validator = MakeValidator();
        }

        /// <inheritdoc />
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
    }
}
