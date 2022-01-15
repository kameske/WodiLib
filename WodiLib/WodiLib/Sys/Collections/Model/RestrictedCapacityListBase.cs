// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityListBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限のあるListクラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IRestrictedCapacityList{T, T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract partial class RestrictedCapacityListBase<T, TImpl> : ModelBase<TImpl>,
        IRestrictedCapacityList<T>
        where TImpl : RestrictedCapacityListBase<T, TImpl>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public virtual event EventHandler<NotifyCollectionChangedEventArgsEx<T>> CollectionChanging
        {
            add => Items.CollectionChanging += value;
            remove => Items.CollectionChanging -= value;
        }

        /// <inheritdoc/>
        public virtual event EventHandler<NotifyCollectionChangedEventArgsEx<T>> CollectionChanged
        {
            add => Items.CollectionChanged += value;
            remove => Items.CollectionChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IFixedLengthList{T, T}.this"/>
        public T this[int index]
        {
            get
            {
                Validator?.Get((nameof(index), index), ("1", 1));
                return Items[index];
            }
            set
            {
                Validator?.Set((nameof(index), index), (nameof(value), value));
                Items[index] = value;
            }
        }

        /// <inheritdoc/>
        // ReSharper disable ConstantConditionalAccessQualifier
        public int Count => Items?.Count ?? -1; // コンストラクタ中のみ、 Items == null の可能性がある
        // ReSharper restore ConstantConditionalAccessQualifier

        /// <inheritdoc/>
        public override NotifyPropertyChangeEventType NotifyPropertyChangingEventType
        {
            get => Items.NotifyPropertyChangingEventType;
            set => Items.NotifyPropertyChangingEventType = value;
        }

        /// <inheritdoc/>
        public override NotifyPropertyChangeEventType NotifyPropertyChangedEventType
        {
            get => Items.NotifyPropertyChangedEventType;
            set => Items.NotifyPropertyChangedEventType = value;
        }

        /// <inheritdoc/>
        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType
        {
            get => Items.NotifyCollectionChangingEventType;
            set => Items.NotifyCollectionChangingEventType = value;
        }

        /// <inheritdoc/>
        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType
        {
            get => Items.NotifyCollectionChangedEventType;
            set => Items.NotifyCollectionChangedEventType = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>引数検証処理</summary>
        private IWodiLibListValidator<T>? Validator => validators.ForMe;

        /// <summary>引数検証処理（IFixedLengthList キャスト時用）</summary>
        private IWodiLibListValidator<T>? ValidatorForWritableList => validators.ForWritableList;

        /// <summary>リスト</summary>
        protected virtual IExtendedList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Fields
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private readonly Validators validators;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        protected RestrictedCapacityListBase()
        {
            // Validator からアクセスされても問題ないよう Items を空リストで初期化
            Items = new ExtendedList<T>();

            var items = MakeClearItems();

            validators = MakeValidator();
            Validator?.Constructor(("InitItems of RestrictedCapacityListBase()", items));

            Items = new ExtendedList<T>(items)
            {
                FuncMakeItems = MakeItems
            };

            PropagatePropertyChangeEvent();
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="length">要素数</param>
        protected RestrictedCapacityListBase(int length)
        {
            var items = MakeItems(0, length)
                .ToArray();

            validators = MakeValidator();
            Validator?.Constructor(("InitItems of RestrictedCapacityListBase(int)", items));

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
        public RestrictedCapacityListBase(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var items = initItems.ToList();

            validators = MakeValidator();
            Validator?.Constructor((nameof(initItems), items));
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
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public abstract int GetMaxCapacity();

        /// <inheritdoc/>
        public abstract int GetMinCapacity();

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
            => Items.GetEnumerator();

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count)
        {
            Validator?.Get((nameof(index), index), (nameof(count), count));
            return Items.GetRange(index, count);
        }

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemList = items.ToList();
            Validator?.Set((nameof(index), index), (nameof(items), itemList));
            Items.SetRange(index, itemList);
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            Validator?.Insert((nameof(Count), Count), (nameof(item), item));
            Items.Add(item);
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemList = items.ToList();
            Validator?.Insert((nameof(Count), Count), (nameof(itemList), itemList));
            Items.AddRange(itemList);
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            Validator?.Insert((nameof(index), index), (nameof(item), item));
            Items.Insert(index, item);
        }

        /// <inheritdoc/>
        public void InsertRange(int index, IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemList = items.ToList();
            Validator?.Insert((nameof(index), index), (nameof(items), itemList));
            Items.InsertRange(index, itemList);
        }

        /// <inheritdoc/>
        public void Overwrite(int index, IEnumerable<T> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            var itemArray = items.ToArray();
            ThrowHelper.ValidateArgumentItemsHasNotNull(itemArray.HasNullItem(), nameof(items));
            Validator?.Overwrite((nameof(index), index), (nameof(items), itemArray));

            if (itemArray.Length == 0)
            {
                return;
            }

            Items.Overwrite(index, itemArray);
        }

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex)
        {
            Validator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex), ("", 1));
            Items.Move(oldIndex, newIndex);
        }

        /// <inheritdoc/>
        public void MoveRange(int oldIndex, int newIndex, int count)
        {
            Validator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex), (nameof(count), count));
            Items.MoveRange(oldIndex, newIndex, count);
        }

        /// <inheritdoc/>
        public bool Remove(T? item)
        {
            Validator?.Remove((nameof(item), item));

            if (item is null) return false;

            return Items.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            Validator?.Remove((nameof(index), index), ("", 1));
            Items.RemoveAt(index);
        }

        /// <inheritdoc/>
        public void RemoveRange(int index, int count)
        {
            Validator?.Remove((nameof(index), index), (nameof(count), count));
            Items.RemoveRange(index, count);
        }

        /// <inheritdoc/>
        public void AdjustLength(int length)
        {
            Validator?.AdjustLength((nameof(length), length));
            Items.AdjustLength(length);
        }

        /// <inheritdoc/>
        public void AdjustLengthIfShort(int length)
        {
            Validator?.AdjustLengthIfShort((nameof(length), length));
            Items.AdjustLengthIfShort(length);
        }

        /// <inheritdoc/>
        public void AdjustLengthIfLong(int length)
        {
            Validator?.AdjustLengthIfLong((nameof(length), length));
            Items.AdjustLengthIfLong(length);
        }

        /// <inheritdoc/>
        public void Reset()
            => Items.Reset();

        /// <inheritdoc cref="ISizeChangeableList{TIn,TOut}.Reset"/>
        public void Reset(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var itemList = initItems.ToList();

            Validator?.Reset((nameof(initItems), itemList));
            Items.Reset(itemList);
        }

        void IWritableList<T, T>.Reset(IEnumerable<T> initItems)
            => ResetAsWritableList(initItems);

        /// <inheritdoc/>
        public void Clear()
        {
            var initItems = MakeClearItems();
            Items.Reset(initItems);
        }

        /// <inheritdoc/>
        public override bool ItemEquals(TImpl? other)
            => Items.ItemEquals(other);

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyExtendedList<T, T>? other)
            => Items.ItemEquals(other);

        /// <inheritdoc/>
        public bool ItemEquals<TOther>(IEnumerable<TOther>? other, IEqualityComparer<TOther>? itemComparer)
            => Items.ItemEquals(other, itemComparer);

        /// <inheritdoc/>
        public override TImpl DeepClone()
        {
            return MakeInstance(Items);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region CollectionChanging

        event EventHandler<NotifyCollectionChangedEventArgs>? INotifiableCollectionChange.CollectionChanging
        {
            add => ((INotifiableCollectionChange)Items).CollectionChanging += value;
            remove => ((INotifiableCollectionChange)Items).CollectionChanging -= value;
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => ((INotifyCollectionChanged)Items).CollectionChanged += value;
            remove => ((INotifyCollectionChanged)Items).CollectionChanged -= value;
        }

        #endregion

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Items).GetEnumerator();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     自身を <see cref="IWritableList{TIn,TOut}"/> とみなして
        ///     <see cref="IWritableList{TIn,TOut}.Reset(IEnumerable{TIn})"/> メソッドを実行する。
        /// </summary>
        /// <param name="initItems">初期化要素</param>
        protected void ResetAsWritableList(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var itemList = initItems.ToList();

            ValidatorForWritableList?.Reset((nameof(itemList), itemList));
            Items.Reset(itemList);
        }

        /// <summary>
        ///     初期化された <typeparamref name="T"/> のインスタンスを生成する。
        /// </summary>
        /// <remarks>
        ///     このメソッドは <see langward="null"/> を返却してはならない。
        ///     <see langward="null"/> が返却された場合、呼び出し元で <see cref="NullReferenceException"/> が発生する。
        /// </remarks>
        /// <param name="index">インデックス</param>
        /// <returns>要素のデフォルト値</returns>
        protected abstract T MakeDefaultItem(int index);

        /// <summary>
        ///     新規インスタンスを作成する。
        /// </summary>
        /// <param name="items">新規インスタンスの要素</param>
        /// <returns>新規作成したインスタンス</returns>
        // TODO: 一時的にVirtual宣言
        protected virtual TImpl MakeInstance(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     自身の検証処理を実行する <see cref="IWodiLibListValidator{T}"/> インスタンスを生成する。
        /// </summary>
        /// <returns>検証処理実行クラスのインスタンス。検証処理を行わない場合 <see langward="null"/></returns>
        protected virtual Validators MakeValidator()
        {
            return new Validators(
                new RestrictedCapacityListValidator<T>(this),
                new FixedLengthListValidator<T>(this, () => Count)
            );
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
        //      Private Methods
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
                .Select(
                    i =>
                    {
                        var result = MakeDefaultItem(i + index);
                        if (result is null)
                        {
                            throw new NullReferenceException(
                                ErrorMessage.NotNull($"{nameof(MakeDefaultItem)}(index: {i}) の結果")
                            );
                        }

                        return result;
                    }
                );
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Classes
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     Validator保持クラス
        /// </summary>
        [CommonMultiValueObject]
        protected partial record Validators
        {
            /// <summary>通常使用するValidator</summary>
            public IWodiLibListValidator<T>? ForMe { get; }

            /// <summary>IFixedLengthListキャスト時に使用するValidator</summary>
            public IWodiLibListValidator<T>? ForWritableList { get; }
        }
    }
}
