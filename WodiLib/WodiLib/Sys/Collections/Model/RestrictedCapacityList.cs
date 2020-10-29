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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量制限のあるList基底クラス
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class RestrictedCapacityList<T> : ExtendedListBase<T>,
        IRestrictedCapacityList<T>,
        IEquatable<IFixedLengthList<T>>
    {
        /*
         * 変更通知イベントについて
         *
         * PropertyChanged, CollectionChanged については ObservableCollection 同様。
         * 加えて、要素変更直前に CollectionChanging イベントが発生する。
         * 具体的な処理順序は以下のとおり。
         *     - 引数の検証
         *     - CollectionChanging 発火
         *     - 要素に対する操作実施
         *     - （要素数が変化している場合）NotifyPropertyChange("Count") 発火
         *     - NotifyPropertyChanged("Index") 発火
         *     - CollectionChanged 発火
         * CollectionChanging は CollectionChanged と
         * 同一の NotifyCollectionChangedEventArgs インスタンスを送出する。
         *
         * 範囲操作を行った場合、各種イベントは一度だけ発火する。
         * CollectionChanging, CollectionChanged については
         * oldItems や newItems に複数の要素が設定された状態となる。
         *
         * なお、要素の変更が起こらなかった場合、各種イベントは発火しない。
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanging
        {
            add => CollectionChanging_Impl += value;
            remove => CollectionChanging_Impl -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /*
         * 継承先のクラスで内部的に独自リストを使用したい場合、
         * Countプロパティを継承して独自リストの要素数を返すこと。
         */
        /// <summary>要素数</summary>
        public override int Count => Items.Count;

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException">nullをセットしようとした場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        public new T this[int index]
        {
            get => Get_Impl(index, 1).First();
            set => Set_Impl(index, value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト</summary>
        protected virtual List<T> Items { get; private set; } = new List<T>();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        protected RestrictedCapacityList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">initItemsの要素数が不適切な場合</exception>
        protected RestrictedCapacityList(IEnumerable<T> initItems) : base(initItems)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public abstract int GetMaxCapacity();

        /// <inheritdoc />
        public abstract int GetMinCapacity();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// すべての列挙子を取得する。
        /// </summary>
        /// <returns>すべての列挙子</returns>
        [Obsolete("不適切なメソッドのため Ver 2.6 で削除します。")]
        public IEnumerable<T> All() => this;

        /// <inheritdoc />
        public void SetRange(int index, IEnumerable<T> items)
            => Set_Impl(index, items.ToArray());

        /// <inheritdoc />
        public void Add(T item)
            => Insert_Impl(Count, item);

        /// <inheritdoc />
        public void AddRange(IEnumerable<T> items)
            => Insert_Impl(Count, items.ToArray());

        /// <inheritdoc />
        public void Insert(int index, T item)
            => Insert_Impl(index, item);

        /// <inheritdoc />
        public void InsertRange(int index, IEnumerable<T> items)
            => Insert_Impl(index, items.ToArray());

        /// <inheritdoc />
        public void Overwrite(int index, IEnumerable<T> items)
            => Overwrite_Impl(index, items.ToArray());

        /// <inheritdoc />
        public void Move(int oldIndex, int newIndex)
            => Move_Impl(oldIndex, newIndex, 1);

        /// <inheritdoc />
        public void MoveRange(int oldIndex, int newIndex, int count)
            => Move_Impl(oldIndex, newIndex, count);

        /// <inheritdoc />
        public bool Remove([AllowNull] T item)
            => Remove_Impl(item);

        /// <inheritdoc />
        public void RemoveAt(int index)
            => Remove_Impl(index, 1);

        /// <inheritdoc />
        public void RemoveRange(int index, int count)
            => Remove_Impl(index, count);

        /// <inheritdoc />
        public void AdjustLength(int length)
            => AdjustLength_Impl(length);

        /// <inheritdoc />
        public void AdjustLengthIfShort(int length)
            => AdjustLengthIfShort_Impl(length);

        /// <inheritdoc />
        public void AdjustLengthIfLong(int length)
            => AdjustLengthIfLong_Impl(length);

        /// <inheritdoc />
        public void Clear()
            => Clear_Impl();

        /// <inheritdoc />
        public void Reset(IEnumerable<T> initItems)
            => Reset_Impl(initItems.ToArray());

        /// <summary>
        /// 指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>指定の要素が含まれる場合はtrue</returns>
        public bool Contains([AllowNull] T item)
        {
            if (item is null) return false;
            return Items.Contains(item);
        }

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf([AllowNull] T item)
        {
            if (item is null) return -1;
            return Items.IndexOf(item);
        }

        /// <summary>
        /// すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">[Range(0, Count - 1)] コピー開始インデックス</param>
        /// <exception cref="ArgumentNullException">arrayがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが0未満の場合</exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        public void CopyTo(T[] array, int index)
            => Items.CopyTo(array, index);

        /// <inheritdoc />
        public override IEnumerator<T> GetEnumerator()
            => Items.GetEnumerator();

        /// <inheritdoc />
        public bool Equals(IRestrictedCapacityList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public bool Equals(IReadOnlyRestrictedCapacityList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public bool Equals(IFixedLengthList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public override bool Equals(ExtendedListBase<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Implements Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        bool ICollection<T>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) Items).GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override IExtendedListValidator<T> MakeValidator()
        {
            return new RestrictedCapacityListValidator<T>(this);
        }

        #region Action Core

        /// <inheritdoc />
        protected override void Constructor_Core(params T[] initItems)
        {
            Items = initItems.ToList();
        }

        /// <inheritdoc />
        protected override T[] Get_Core(int index, int count)
        {
            return Items.GetRange(index, count)
                .ToArray();
        }

        /// <inheritdoc />
        protected override void Set_Core(int index, params T[] items)
        {
            items.ForEach((item, i) => Items[index + i] = item);
        }

        /// <inheritdoc />
        protected override void Insert_Core(int index, params T[] items)
        {
            Items.InsertRange(index, items);
        }

        /// <inheritdoc />
        protected override void Move_Core(int oldIndex, int newIndex, int count)
        {
            var movedItems = Items.GetRange(oldIndex, count);
            Items.RemoveRange(oldIndex, count);
            Items.InsertRange(newIndex, movedItems);
        }

        /// <inheritdoc />
        protected override void Remove_Core(int index, int count)
        {
            Items.RemoveRange(index, count);
        }

        #endregion

        /// <inheritdoc />
        protected override IEnumerable<T> MakeInitItems()
        {
            return Enumerable.Range(0, GetMinCapacity())
                .Select(MakeDefaultItem);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Items), Items);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected RestrictedCapacityList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Items = info.GetValue<List<T>>(nameof(Items));
        }
    }
}
