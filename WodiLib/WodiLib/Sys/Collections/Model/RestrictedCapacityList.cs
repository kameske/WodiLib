// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限のあるListクラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IRestrictedCapacityList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    public abstract class RestrictedCapacityList<T, TImpl> : ModelBase<TImpl>,
        IRestrictedCapacityList<T>
        where TImpl : RestrictedCapacityList<T, TImpl>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanging
        {
            add => Items.CollectionChanging += value;
            remove => Items.CollectionChanging -= value;
        }

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => Items.CollectionChanged += value;
            remove => Items.CollectionChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IRestrictedCapacityList{T}.Count"/>
        public int Count => Items.Count;

        /// <inheritdoc cref="IRestrictedCapacityList{T}.this"/>
        public T this[int index]
        {
            get
            {
                Validator?.Get(index, 1);
                return Items[index];
            }
            set
            {
                Validator?.Set(index, value);
                Items[index] = value;
            }
        }

        /// <inheritdoc cref="INotifyCollectionChange.NotifyCollectionChangingEventType"/>
        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType
        {
            get => Items.NotifyCollectionChangingEventType;
            set => Items.NotifyCollectionChangingEventType = value;
        }

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.NotifyCollectionChangedEventType"/>
        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType
        {
            get => Items.NotifyCollectionChangedEventType;
            set => Items.NotifyCollectionChangedEventType = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>引数検証処理</summary>
        private IWodiLibListValidator<T>? Validator { get; }

        /// <summary>リスト</summary>
        private ExtendedList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        protected RestrictedCapacityList()
        {
            // MakeClearItems() 内で自身にアクセスする可能性を考慮して Items を空リストで初期化
            Items = new ExtendedList<T>();

            var items = MakeClearItems();

            Validator = MakeValidator();
            Validator?.Constructor(items);

            Items = new ExtendedList<T>(items)
            {
                FuncMakeItems = MakeItems
            };

            PropagatePropertyChangeEvent();
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> の要素数が <see cref="GetMinCapacity"/> 未満
        ///     または <see cref="GetMaxCapacity"/> を超える場合。
        /// </exception>
        protected RestrictedCapacityList(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var items = initItems.ToList();

            Validator = MakeValidator();
            Validator?.Constructor(items);
            Items = new ExtendedList<T>(items)
            {
                FuncMakeItems = MakeItems
            };

            PropagatePropertyChangeEvent();
        }

        /// <summary>
        ///     各プロパティのプロパティ変更通知を自身に伝播させる。
        /// </summary>
        private void PropagatePropertyChangeEvent()
        {
            PropagatePropertyChangeEvent(Items, GetNotifyPropertyNameMapper());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public abstract int GetMaxCapacity();

        /// <inheritdoc/>
        public abstract int GetMinCapacity();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count)
        {
            Validator?.Get(index, count);
            return Items.GetRange(index, count);
        }

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemList = items.ToList();
            Validator?.Set(index, itemList);
            Items.SetRange(index, itemList);
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            Validator?.Insert(Count, item);
            Items.Add(item);
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemList = items.ToList();
            Validator?.Insert(Count, itemList);
            Items.AddRange(itemList);
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            Validator?.Insert(index, item);
            Items.Insert(index, item);
        }

        /// <inheritdoc/>
        public void InsertRange(int index, IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemList = items.ToList();
            Validator?.Insert(index, itemList);
            Items.InsertRange(index, itemList);
        }

        /// <inheritdoc/>
        public void Overwrite(int index, IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemList = items.ToList();
            Validator?.Overwrite(index, itemList);
            Items.Overwrite(index, itemList);
        }

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex)
        {
            Validator?.Move(oldIndex, newIndex, 1);
            Items.Move(oldIndex, newIndex);
        }

        /// <inheritdoc/>
        public void MoveRange(int oldIndex, int newIndex, int count)
        {
            Validator?.Move(oldIndex, newIndex, count);
            Items.MoveRange(oldIndex, newIndex, count);
        }

        /// <inheritdoc/>
        public bool Remove([AllowNull] T item)
        {
            Validator?.Remove(item);

            if (item is null) return false;

            return Items.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            Validator?.Remove(index, 1);
            Items.RemoveAt(index);
        }

        /// <inheritdoc/>
        public void RemoveRange(int index, int count)
        {
            Validator?.Remove(index, count);
            Items.RemoveRange(index, count);
        }

        /// <inheritdoc/>
        public void AdjustLength(int length)
        {
            Validator?.AdjustLength(length);
            Items.AdjustLength(length);
        }

        /// <inheritdoc/>
        public void AdjustLengthIfShort(int length)
        {
            Validator?.AdjustLengthIfShort(length);
            Items.AdjustLengthIfShort(length);
        }

        /// <inheritdoc/>
        public void AdjustLengthIfLong(int length)
        {
            Validator?.AdjustLengthIfLong(length);
            Items.AdjustLengthIfLong(length);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            var initItems = MakeClearItems();
            Items.Reset(initItems);
        }

        /// <inheritdoc/>
        public void Reset(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var itemList = initItems.ToList();

            Validator?.Reset(itemList);
            Items.Reset(itemList);
        }

        /// <inheritdoc/>
        public bool Contains([AllowNull] T item)
        {
            if (item is null) return false;
            return Items.Contains(item);
        }

        /// <inheritdoc/>
        public int IndexOf([AllowNull] T item)
        {
            if (item is null) return -1;
            return Items.IndexOf(item);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int index)
            => Items.CopyTo(array, index);

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
            => Items.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) Items).GetEnumerator();

        /// <inheritdoc/>
        public override bool ItemEquals(TImpl? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc/>
        public bool ItemEquals(IRestrictedCapacityList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyRestrictedCapacityList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyExtendedList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc/>
        public bool ItemEquals(IEnumerable<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Items.ItemEquals(other);
        }

        /// <inheritdoc/>
        IRestrictedCapacityList<T> IDeepCloneable<IRestrictedCapacityList<T>>.DeepClone()
            => DeepClone();

        IReadOnlyRestrictedCapacityList<T> IDeepCloneable<IReadOnlyRestrictedCapacityList<T>>.DeepClone()
            => DeepClone();

        /// <inheritdoc/>
        IReadOnlyExtendedList<T> IDeepCloneable<IReadOnlyExtendedList<T>>.DeepClone()
            => DeepClone();

        /// <inheritdoc cref="IDeepCloneableRestrictedCapacityList{T,TIn}.DeepCloneWith"/>
        public TImpl DeepCloneWith(int? length = null,
            IReadOnlyDictionary<int, T>? values = null)
        {
            if (length is not null)
            {
                ThrowHelper.ValidateArgumentValueRange(
                    length.Value < GetMinCapacity() || GetMaxCapacity() < length.Value, nameof(length), length.Value,
                    GetMinCapacity(), GetMaxCapacity());
            }

            values?.ForEach(pair =>
            {
                ThrowHelper.ValidateArgumentNotNull(pair.Value is null, $"{nameof(values)} の要素 (Key: {pair.Key})");
            });

            var result = DeepClone();

            if (length is not null)
            {
                result.AdjustLength(length.Value);
            }

            values?.ForEach(pair =>
            {
                if (-1 < pair.Key && pair.Key < result.Count) result[pair.Key] = pair.Value;
            });

            return result;
        }

        /// <inheritdoc/>
        IRestrictedCapacityList<T> IDeepCloneableRestrictedCapacityList<IRestrictedCapacityList<T>, T>.DeepCloneWith(
            int? length, IReadOnlyDictionary<int, T>? values)
            => DeepCloneWith(length, values);

        IReadOnlyRestrictedCapacityList<T> IDeepCloneableRestrictedCapacityList<IReadOnlyRestrictedCapacityList<T>, T>.
            DeepCloneWith(int? length, IReadOnlyDictionary<int, T>? values)
            => DeepCloneWith(length, values);

        /// <inheritdoc/>
        IReadOnlyExtendedList<T> IDeepCloneableExtendedList<IReadOnlyExtendedList<T>, T>.DeepCloneWith(int? length,
            IReadOnlyDictionary<int, T>? values)
            => new ExtendedList<T>(this, length, values, MakeDefaultItem);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     初期化された <typeparamref name="T"/> のインスタンスを生成する。
        /// </summary>
        /// <remarks>
        ///     このメソッドは <see langward="null"/> を返却してはならない。
        ///     <see langward="null"/> が返却された場合、呼び出し元で <see cref="NullReferenceException"/> が発生する。
        /// </remarks>
        /// <param name="index">インデックス</param>
        /// <returns>要素のデフォルト値</returns>
        [return: NotNull]
        protected abstract T MakeDefaultItem(int index);

        /// <summary>
        ///     自身の検証処理を実行する <see cref="IWodiLibListValidator{T}"/> インスタンスを生成する。
        /// </summary>
        /// <returns>検証処理実行クラスのインスタンス。検証処理を行わない場合 <see langward="null"/></returns>
        protected virtual IWodiLibListValidator<T>? MakeValidator()
        {
            return new RestrictedCapacityListValidator<T>(this);
        }

        /// <summary>
        ///     自身の内部リストが通知するプロパティ変更通知のプロパティ名を変換するためのMapperを取得する。
        /// </summary>
        /// <remarks>
        ///     対象のプロパティ名は <see cref="Count"/> および <see cref="ListConstant.IndexerName"/>。<br/>
        ///     デフォルトでは <see langword="null"/> を返却する。
        /// </remarks>
        /// <returns>
        ///     プロパティ名Mapper。
        ///     <see langword="null"/> の場合変換を行わず通知する。
        /// </returns>
        protected virtual PropertyChangeNotificationHelper.MapNotifyPropertyName?
            GetNotifyPropertyNameMapper()
            => null;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     引数なしコンストラクタによる要素初期化、
        ///     および <see cref="Clear"/> メソッドで初期化し直す際の要素を生成する。
        /// </summary>
        /// <returns>初期化用要素</returns>
        /// <exception cref="NullReferenceException">
        ///     <see cref="MakeDefaultItem"/> が <see langword="null"/> を返却した場合。
        /// </exception>
        private List<T> MakeClearItems()
            => MakeItems(0, GetMinCapacity()).ToList();

        /// <summary>
        ///     自身に設定するための要素を生成する。
        /// </summary>
        /// <param name="index">挿入または更新開始インデックス</param>
        /// <param name="count">挿入または更新要素数</param>
        /// <returns>挿入または更新要素</returns>
        /// <exception cref="NullReferenceException">
        ///     <see cref="MakeDefaultItem"/> が <see langword="null"/> を返却した場合。
        /// </exception>
        private IEnumerable<T> MakeItems(int index, int count)
        {
            return Enumerable.Range(0, count)
                .Select(i =>
                {
                    var result = MakeDefaultItem(i + index);
                    if (result is null)
                    {
                        throw new NullReferenceException(
                            ErrorMessage.NotNull($"{nameof(MakeDefaultItem)}(index: {i}) の結果"));
                    }

                    return result;
                });
        }
    }
}
