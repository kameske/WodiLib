// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthList.cs
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

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IFixedLengthList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<T, TImpl> : ModelBase<TImpl>,
        IFixedLengthList<T>, IReadOnlyExtendedList<T>
        where TImpl : FixedLengthList<T, TImpl>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
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
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IFixedLengthList{T}.this"/>
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

        /// <inheritdoc/>
        public int Count => Items.Count;

        /// <inheritdoc/>
        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType
        {
            get => Items.NotifyCollectionChangingEventType;
            set
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(value));
                Items.NotifyCollectionChangingEventType = value;
            }
        }

        /// <inheritdoc/>
        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType
        {
            get => Items.NotifyCollectionChangedEventType;
            set
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(value));
                Items.NotifyCollectionChangedEventType = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>引数検証処理</summary>
        private IWodiLibListValidator<T>? Validator { get; }

        /// <summary>リスト</summary>
        private ExtendedList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="length">要素数</param>
        protected FixedLengthList(int length)
        {
            var items = MakeItems(0, length)
                .ToArray();

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
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        protected FixedLengthList(IEnumerable<T> initItems)
        {
            if (initItems is null)
            {
                throw new ArgumentNullException(ErrorMessage.NotNull(nameof(initItems)));
            }

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
            PropagatePropertyChangeEvent(Items,
                (_, propName) =>
                {
                    if (propName.Equals(nameof(Count))) return null;
                    if (propName.Equals(ListConstant.IndexerName)) return new[] { GetNotifyPropertyIndexerName() };
                    return null;
                });
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.Contains(TItem)"/>
        public bool Contains(T? item)
            => Contains(item, null);

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.Contains(TItem, IEqualityComparer{TItem}?)"/>
        public bool Contains(T? item, IEqualityComparer<T>? itemComparer)
        {
            if (item is null) return false;
            return Items.Contains(item, itemComparer);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
            => Items.AsEnumerable().GetEnumerator();

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.GetRange"/>
        public IEnumerable<T> GetRange(int index, int count)
        {
            Validator?.Get(index, count);
            return Items.GetRange(index, count);
        }

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.IndexOf(TItem)"/>
        public int IndexOf(T? item)
            => IndexOf(item, null);

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.IndexOf(TItem, IEqualityComparer{TItem}?)"/>
        public int IndexOf(T? item, IEqualityComparer<T>? itemComparer)
        {
            if (item is null) return -1;
            return Items.IndexOf(item, itemComparer);
        }

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.CopyTo"/>
        public void CopyTo(T[] array, int index)
            => Items.CopyTo(array, index);

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<T> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));
            }

            var itemList = items.ToList();
            Validator?.Set(index, itemList);
            Items.SetRange(index, itemList);
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
        public void Reset()
            => Items.Reset();

        /// <inheritdoc/>
        public void Reset(IEnumerable<T> initItems)
        {
            if (initItems is null)
            {
                throw new ArgumentNullException(ErrorMessage.NotNull(nameof(initItems)));
            }

            var itemList = initItems.ToList();

            Validator?.Reset(itemList);
            Items.Reset(itemList);
        }


        /// <inheritdoc/>
        public override bool ItemEquals(TImpl? other)
            => ItemEquals(other, null);

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.ItemEquals(IEnumerable{TItem}?)"/>
        public bool ItemEquals(IEnumerable<T>? other)
            => ItemEquals(other, null);

        /// <inheritdoc cref="IWritableList{TItem,TImpl,TReadable}.ItemEquals(IEnumerable{TItem}?, IEqualityComparer{TItem}?)"/>
        public bool ItemEquals(IEnumerable<T>? other, IEqualityComparer<T>? itemComparer)
            => Items.ItemEquals(other, itemComparer);


        /// <inheritdoc/>
        public IReadOnlyExtendedList<T> AsReadableList()
            => this;

        /// <inheritdoc/>
        public override TImpl DeepClone()
            => DeepCloneWith();

        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepCloneWith"/>
        public TImpl DeepCloneWith(int? length = null, IReadOnlyDictionary<int, T>? values = null)
        {
            values?.ForEach(pair =>
            {
                ThrowHelper.ValidateArgumentNotNull(pair.Value is null, $"{nameof(values)} の要素 (Key: {pair.Key})");
            });

            ExtendedList<T> newItems = new(this)
            {
                FuncMakeItems = MakeItems
            };

            if (length is not null)
            {
                newItems.AdjustLength(length.Value);
            }

            values?.ForEach(pair =>
            {
                if (-1 < pair.Key && pair.Key < newItems.Count) newItems[pair.Key] = pair.Value;
            });

            return MakeInstance(newItems);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region ItemEquals

        bool IEqualityComparable<IFixedLengthList<T>>.ItemEquals(IFixedLengthList<T>? other)
            => ItemEquals(other, null);

        bool IEqualityComparable<IReadOnlyExtendedList<T>>.ItemEquals(IReadOnlyExtendedList<T>? other)
            => ItemEquals(other, null);

        #endregion

        #region DeepClone

        IFixedLengthList<T> IDeepCloneable<IFixedLengthList<T>>.DeepClone()
            => DeepClone();

        IReadOnlyExtendedList<T> IDeepCloneable<IReadOnlyExtendedList<T>>.DeepClone()
            => DeepClone();

        #endregion

        #region DeepCloneWith

        IFixedLengthList<T> IDeepCloneableList<IFixedLengthList<T>, T>.DeepCloneWith(int? length,
            IReadOnlyDictionary<int, T>? values)
            => DeepCloneWith(length, values);

        IReadOnlyExtendedList<T> IDeepCloneableList<IReadOnlyExtendedList<T>, T>.DeepCloneWith(int? length,
            IReadOnlyDictionary<int, T>? values)
            => DeepCloneWith(length, values);

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     自身の内部リストが通知する IndexerName プロパティ変更通知のプロパティ名を変換する。
        /// </summary>
        /// <remarks>
        ///     デフォルトでは <see cref="ListConstant.IndexerName"/> を返却する。
        /// </remarks>
        /// <returns>
        ///     IndexerName プロパティ変換後の文字列。
        ///     <see langword="null"/> の場合通知しない。
        /// </returns>
        protected virtual string
            GetNotifyPropertyIndexerName()
            => ListConstant.IndexerName;

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
        ///     新規インスタンスを作成する。
        /// </summary>
        /// <param name="items">新規インスタンスの要素</param>
        /// <returns>新規作成したインスタンス</returns>
        protected abstract TImpl MakeInstance(IEnumerable<T> items);

        /// <summary>
        ///     自身の検証処理を実行する <see cref="IWodiLibListValidator{T}"/> インスタンスを生成する。
        /// </summary>
        /// <returns>検証処理実行クラスのインスタンス。検証処理を行わない場合 <see langward="null"/></returns>
        protected virtual IWodiLibListValidator<T> MakeValidator()
        {
            return new FixedLengthListValidator<T>(this);
        }

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Obsolete
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /*
         * 一時的に定義。
         * Ver 3.0 正式公開時には削除する。
         */

        [Obsolete]
        public virtual int GetCapacity() => 0;

        [Obsolete]
        protected FixedLengthList()
        {
            Items = new ExtendedList<T>(MakeItems(0, GetCapacity()));
        }
    }
}
