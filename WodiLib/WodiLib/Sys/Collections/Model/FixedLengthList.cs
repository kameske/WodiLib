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

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    /// 機能概要は <seealso cref="IFixedLengthList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<T, TImpl> : ModelBase<TImpl>, IFixedLengthList<T>
        where TImpl : FixedLengthList<T, TImpl>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanging
        {
            add => Items.CollectionChanging += value;
            remove => Items.CollectionChanging -= value;
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => Items.CollectionChanged += value;
            remove => Items.CollectionChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public int Count => Items.Count;

        /// <inheritdoc cref="IFixedLengthList{T}.this" />
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

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.IsNotifyBeforeCollectionChange" />
        public bool IsNotifyBeforeCollectionChange
        {
            get => Items.IsNotifyBeforeCollectionChange;
            set => Items.IsNotifyBeforeCollectionChange = value;
        }

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.IsNotifyAfterCollectionChange" />
        public bool IsNotifyAfterCollectionChange
        {
            get => Items.IsNotifyAfterCollectionChange;
            set => Items.IsNotifyAfterCollectionChange = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>引数検証処理</summary>
        private IWodiLibListValidator<T>? Validator { get; }

        /// <summary>リスト</summary>
        private ExtendedList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        protected FixedLengthList()
        {
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
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
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
        /// 各プロパティのプロパティ変更通知を自身に伝播させる。
        /// </summary>
        private void PropagatePropertyChangeEvent()
        {
            PropagatePropertyChangeEvent(Items,
                (_, propName) =>
                {
                    if (propName.Equals(nameof(Count))) return null;
                    if (propName.Equals(ListConstant.IndexerName)) return new[] {GetNotifyPropertyIndexerName()};
                    return null;
                });
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public abstract int GetCapacity();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public IEnumerable<T> GetRange(int index, int count)
        {
            Validator?.Get(index, count);
            return Items.GetRange(index, count);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Move(int oldIndex, int newIndex)
        {
            Validator?.Move(oldIndex, newIndex, 1);
            Items.Move(oldIndex, newIndex);
        }

        /// <inheritdoc />
        public void MoveRange(int oldIndex, int newIndex, int count)
        {
            Validator?.Move(oldIndex, newIndex, count);
            Items.MoveRange(oldIndex, newIndex, count);
        }

        /// <inheritdoc />
        public void Clear()
        {
            var initItems = MakeClearItems();
            Items.Reset(initItems);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool Contains([AllowNull] T item)
        {
            if (item is null) return false;
            return Items.Contains(item);
        }

        /// <inheritdoc />
        public int IndexOf([AllowNull] T item)
        {
            if (item is null) return -1;
            return Items.IndexOf(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int index)
            => Items.CopyTo(array, index);

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
            => Items.AsEnumerable().GetEnumerator();

        /// <inheritdoc/>
        public override bool ItemEquals(TImpl? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool ItemEquals(IFixedLengthList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool ItemEquals(IReadOnlyFixedLengthList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool ItemEquals(IReadOnlyExtendedList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc />
        public bool ItemEquals(IEnumerable<T>? other)
            => Items.ItemEquals(other);

        IFixedLengthList<T> IDeepCloneable<IFixedLengthList<T>>.DeepClone()
            => DeepClone();

        IReadOnlyFixedLengthList<T> IDeepCloneable<IReadOnlyFixedLengthList<T>>.DeepClone()
            => DeepClone();

        IReadOnlyExtendedList<T> IDeepCloneable<IReadOnlyExtendedList<T>>.DeepClone()
            => DeepClone();

        /// <inheritdoc cref="IFixedLengthList{T}.DeepCloneWith(IEnumerable{System.Collections.Generic.KeyValuePair{int,T}}?)"/>
        public TImpl DeepCloneWith(IEnumerable<KeyValuePair<int, T>>? values = null)
        {
            var result = DeepClone();

            values?.ForEach(pair =>
            {
                ThrowHelper.ValidateArgumentNotNull(pair.Value is null, $"{nameof(values)} の要素 (Key: {pair.Key})");
                if (-1 < pair.Key && pair.Key < result.Count) result[pair.Key] = pair.Value;
            });

            return result;
        }

        /// <inheritdoc/>
        IFixedLengthList<T> IFixedLengthList<T>.DeepCloneWith(IEnumerable<KeyValuePair<int, T>>? values)
            => DeepCloneWith(values);

        /// <inheritdoc/>
        IReadOnlyFixedLengthList<T> IReadOnlyFixedLengthList<T>.DeepCloneWith(IEnumerable<KeyValuePair<int, T>>? values)
            => DeepCloneWith(values);

        /// <inheritdoc/>
        IReadOnlyExtendedList<T> IReadOnlyExtendedList<T>.DeepCloneWith(int? length,
            IEnumerable<KeyValuePair<int, T>>? values)
            => DeepCloneWith(values);

        /// <inheritdoc/>
        IFixedLengthList<T> IFixedLengthList<T>.DeepCloneWith(int? length, IEnumerable<KeyValuePair<int, T>>? values)
            => DeepCloneWith(values);

        /// <inheritdoc/>
        IReadOnlyFixedLengthList<T> IReadOnlyFixedLengthList<T>.DeepCloneWith(int? length,
            IEnumerable<KeyValuePair<int, T>>? values)
            => DeepCloneWith(values);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Implements Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 初期化された <typeparamref name="T"/> のインスタンスを生成する。
        /// </summary>
        /// <remarks>
        /// このメソッドは <see langward="null"/> を返却してはならない。
        /// <see langward="null"/> が返却された場合、呼び出し元で <see cref="NullReferenceException"/> が発生する。
        /// </remarks>
        /// <param name="index">インデックス</param>
        /// <returns>要素のデフォルト値</returns>
        protected abstract T MakeDefaultItem(int index);

        /// <summary>
        /// 自身の検証処理を実行する <see cref="IWodiLibListValidator{T}"/> インスタンスを生成する。
        /// </summary>
        /// <returns>検証処理実行クラスのインスタンス。検証処理を行わない場合 <see langward="null"/></returns>
        protected virtual IWodiLibListValidator<T> MakeValidator()
        {
            return new FixedLengthListValidator<T>(this);
        }

        /// <summary>
        /// 自身の内部リストが通知する IndexerName プロパティ変更通知のプロパティ名を変換する。
        /// </summary>
        /// <remarks>
        /// デフォルトでは <see cref="ListConstant.IndexerName"/> を返却する。
        /// </remarks>
        /// <returns>
        ///     IndexerName プロパティ変換後の文字列。
        ///     <see langword="null"/> の場合通知しない。
        /// </returns>
        protected virtual string
            GetNotifyPropertyIndexerName()
            => ListConstant.IndexerName;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 引数なしコンストラクタによる要素初期化、
        /// および <see cref="Clear"/> メソッドで初期化し直す際の要素を生成する。
        /// </summary>
        /// <returns>初期化用要素</returns>
        /// <exception cref="NullReferenceException">
        ///     <see cref="MakeDefaultItem"/> が <see langword="null"/> を返却した場合
        /// </exception>
        private List<T> MakeClearItems()
            => MakeItems(0, GetCapacity()).ToList();

        /// <summary>
        /// 自身に設定するための要素を生成する。
        /// </summary>
        /// <param name="index">挿入または更新開始インデックス</param>
        /// <param name="count">挿入または更新要素数</param>
        /// <returns>挿入または更新要素</returns>
        /// <exception cref="NullReferenceException">
        /// <see cref="MakeDefaultItem"/> が <see langword="null"/> を返却した場合
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
