// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityCollection.cs
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

// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedParameter.Global

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量制限のあるList基底クラス
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class RestrictedCapacityCollection<T> : ModelBase<RestrictedCapacityCollection<T>>,
        IRestrictedCapacityCollection<T>,
        IEquatable<IFixedLengthCollection<T>>
    {
        /*
         * 継承先用のイベントについて
         *
         * CollectionChanging, CollectionChanged を購読することで
         * 要素操作の前後に処理を挟むことができる。
         * 具体的な処理順序は以下のとおり。
         *     - 引数の検証
         *     - CollectionChanging 発火
         *     - 要素に対する操作実施
         *     - （要素数が変化している可能性がある場合）NotifyPropertyChange("Count") 発火
         *     - NotifyPropertyChanged("Index") 発火
         *     - CollectionChanged 発火
         * CollectionChanging は CollectionChanged と
         * 同一の NotifyCollectionChangedEventArgs インスタンスを送出する。
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event NotifyCollectionChangedEventHandler? _collectionChanging;

        /// <summary>
        /// 要素変更前通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
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

        /// <summary>
        /// 要素変更誤通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /*
         * 継承先のクラスで内部的に独自リストを使用したい場合、
         * Countプロパティを継承して独自リストの要素数を返すこと。
         */
        /// <summary>要素数</summary>
        public virtual int Count => Items.Count;

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException">nullをセットしようとした場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        public T this[int index]
        {
            get => Items[index];
            set
            {
                if (value is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(value)));

                ListValidationHelper.SelectIndex(index, Count);

                Set_Impl(index, value);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト</summary>
        protected virtual List<T> Items { get; } = new List<T>();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        protected RestrictedCapacityCollection()
        {
#if DEBUG
            try
            {
                ListValidationHelper.CapacityConfig(GetMinCapacity(), GetMaxCapacity());
                ValidateDefaultItem();
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(RestrictedCapacityCollection<T>), ex);
            }
#endif

            FillMinCapacity();
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
        protected RestrictedCapacityCollection(IEnumerable<T> initItems)
        {
#if DEBUG
            try
            {
                ListValidationHelper.CapacityConfig(GetMinCapacity(), GetMaxCapacity());
                ValidateDefaultItem();
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(RestrictedCapacityCollection<T>), ex);
            }
#endif

            if (initItems is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(initItems)));

            var initList = initItems.ToArray();

            ListValidationHelper.ItemsHasNotNull(initList, nameof(initItems));
            RestrictedListValidationHelper.ItemCount(initList.Length, GetMinCapacity(), GetMaxCapacity());

            Insert_Core(0, initList);
        }

        /// <summary>
        /// デフォルト値チェック
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="MakeDefaultItem"/>がnullを返却する場合</exception>
        private void ValidateDefaultItem()
        {
            var value = MakeDefaultItem(0);
            if (value is null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull($"{nameof(MakeDefaultItem)}メソッドの返戻値"));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /*
         * GetXXXCapacity
         *      許容サイズ上下限値をメンバにさせないために。
         * */

        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public abstract int GetMaxCapacity();

        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public abstract int GetMinCapacity();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, countが0未満の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<T> GetRange(int index, int count)
        {
            ListValidationHelper.SelectIndex(index, Count);
            ListValidationHelper.Count(count, Count);
            ListValidationHelper.Range(index, count, Count);

            return GetRange_Impl(index, count);
        }

        /// <summary>
        /// すべての列挙子を取得する。
        /// </summary>
        /// <returns>すべての列挙子</returns>
        public IEnumerable<T> All() => this;

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void Add(T item)
        {
            if (item is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(item)));
            RestrictedListValidationHelper.ItemMaxCount(Count + 1, GetMaxCapacity());

            Add_Impl(item);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void AddRange(IEnumerable<T> items)
        {
            if (items is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));

            var list = items.ToArray();

            ListValidationHelper.ItemsHasNotNull(list);
            RestrictedListValidationHelper.ItemMaxCount(Count + list.Length, GetMaxCapacity());

            AddRange_Impl(list);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="item">挿入する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void Insert(int index, T item)
        {
            ListValidationHelper.InsertIndex(index, Count);
            if (item is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));
            RestrictedListValidationHelper.ItemMaxCount(Count + 1, GetMaxCapacity());

            Insert_Impl(index, item);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void InsertRange(int index, IEnumerable<T> items)
        {
            ListValidationHelper.InsertIndex(index, Count);
            if (items is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));

            var list = items.ToArray();

            ListValidationHelper.ItemsHasNotNull(list);
            RestrictedListValidationHelper.ItemMaxCount(Count + list.Length, GetMaxCapacity());

            InsertRange_Impl(index, list);
        }

        /// <summary>
        /// 指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemsがnullの場合</exception>
        /// <exception cref="ArgumentException">items中にnull要素が含まれる場合</exception>
        /// <exception cref="InvalidOperationException">追加操作によって要素数がMaxCapacityを超える場合</exception>
        /// <example>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10, 11, 12 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 11, 12 }
        ///     </code>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 3 }
        ///     </code>
        /// </example>
        public void Overwrite(int index, IEnumerable<T> items)
        {
            ListValidationHelper.InsertIndex(index, Count);
            if (items is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(items)));

            var list = items.ToArray();

            ListValidationHelper.ItemsHasNotNull(list);
            RestrictedListValidationHelper.OverwriteLength(index, Count, list.Length, GetMaxCapacity());

            Overwrite_Impl(index, list);
        }

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, Count - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex が指定範囲外の場合
        /// </exception>
        public void Move(int oldIndex, int newIndex)
        {
            if (Count == 0)
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute("リストの要素が0個のため"));

            ListValidationHelper.SelectIndex(oldIndex, Count, nameof(oldIndex));
            ListValidationHelper.SelectIndex(newIndex, Count, nameof(newIndex));

            Move_Impl(oldIndex, newIndex);
        }

        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, Count - count)] 移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, Count - oldIndex)] 移動させる要素数</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合</exception>
        public void MoveRange(int oldIndex, int newIndex, int count)
        {
            ListValidationHelper.ItemCountNotZero(Count);
            ListValidationHelper.SelectIndex(oldIndex, Count, nameof(oldIndex));
            ListValidationHelper.InsertIndex(newIndex, Count, nameof(newIndex));
            ListValidationHelper.Count(count, Count);
            ListValidationHelper.Range(oldIndex, count, Count, nameof(oldIndex));
            ListValidationHelper.Range(count, newIndex, Count, nameof(count), nameof(newIndex));

            MoveRange_Impl(oldIndex, newIndex, count);
        }

        /// <summary>
        /// 特定のオブジェクトを要素として持つとき、最初に出現したものを削除する。
        /// </summary>
        /// <param name="item">削除する要素</param>
        /// <returns>削除成否</returns>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinCapacity未満になる場合</exception>
        public bool Remove([AllowNull] T item)
        {
            if (item is null) return false;

            var index = Items.IndexOf(item);
            if (index < 0) return false;

            RestrictedListValidationHelper.ItemMinCount(Count - 1, GetMinCapacity());

            Remove_Impl(index);

            return true;
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinCapacity未満になる場合</exception>
        public void RemoveAt(int index)
        {
            ListValidationHelper.SelectIndex(index, Count);
            RestrictedListValidationHelper.ItemMinCount(Count - 1, GetMinCapacity());

            Remove_Impl(index);
        }

        /// <summary>
        /// 要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinCapacity未満になる場合</exception>
        public void RemoveRange(int index, int count)
        {
            ListValidationHelper.SelectIndex(index, Count);
            ListValidationHelper.Count(count, Count);
            ListValidationHelper.Range(index, count, Count);
            RestrictedListValidationHelper.ItemMinCount(Count - count, GetMinCapacity());

            RemoveRange_Impl(index, count);
        }

        /// <summary>
        /// 要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(GetMinCapacity(), GetMaxCapacity())] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">要素を追加した際にnullがセットされた場合</exception>
        public void AdjustLength(int length)
        {
            if (length < GetMinCapacity())
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));
            if (length > GetMaxCapacity())
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));


            AdjustLength_Impl(length);
        }

        /// <summary>
        /// 要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(GetMinCapacity(), GetMaxCapacity())] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">要素を追加した際にnullがセットされた場合</exception>
        public void AdjustLengthIfShort(int length)
        {
            var min = GetMinCapacity();
            var max = GetMaxCapacity();
            if (length < min || max < length)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(length), min, max, length));

            if (Count >= length)
            {
                // 変更しない場合でも変更通知だけは行う
                NotifyPropertyChanged(nameof(Count));
                NotifyPropertyChanged(ListConstant.IndexerName);
                return;
            }

            AdjustLengthIfShort_Impl(length);
        }

        /// <summary>
        /// 要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(GetMinCapacity(), GetMaxCapacity())] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        public void AdjustLengthIfLong(int length)
        {
            var min = GetMinCapacity();
            var max = GetMaxCapacity();
            if (length < min || max < length)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(length), min, max, length));

            if (Count <= length)
            {
                // 変更しない場合でも変更通知だけは行う
                NotifyPropertyChanged(nameof(Count));
                NotifyPropertyChanged(ListConstant.IndexerName);
                return;
            }

            AdjustLengthIfLong_Impl(length);
        }

        /// <summary>
        /// すべての要素を削除し、最小の要素数だけ初期化する。
        /// </summary>
        public void Clear()
        {
            Clear_Impl();
        }

        /// <summary>
        /// 指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>指定の要素が含まれる場合はtrue</returns>
        public bool Contains([AllowNull] T item)
        {
            if (item == null) return false;
            return Items.Contains(item);
        }

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf([AllowNull] T item)
        {
            if (item == null) return -1;
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
        public void CopyTo(T[] array, int index) => Items.CopyTo(array, index);

        /// <summary>
        /// 反復処理する列挙子を返す。
        /// </summary>
        /// <returns>反復処理列挙子</returns>
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IRestrictedCapacityCollection<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            // TODO: Ver 2.6 までキャスト記述
            return All().SequenceEqual(((IExtendedReadOnlyList<T>) other).All());
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
#pragma warning disable 618 // TODO: Ver 2.6 まで
        public bool Equals(IReadOnlyRestrictedCapacityCollection<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.SequenceEqual(other);
        }
#pragma warning restore 618

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(RestrictedCapacityCollection<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.SequenceEqual(other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IReadOnlyList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return All().SequenceEqual(other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IFixedLengthCollection<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IExtendedReadOnlyList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IEnumerable<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return All().SequenceEqual(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Implements Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        bool ICollection<T>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) Items).GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected abstract T MakeDefaultItem(int index);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region List Operation Impl

        /// <summary>
        /// 指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 要素数</param>
        private IEnumerable<T> GetRange_Impl(int index, int count)
        {
            return Items.GetRange(index, count);
        }

        /// <summary>
        /// 指定したインデックス位置にある要素を置き換える。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        private void Set_Impl(int index, T item)
        {
            var oldItem = Items[index];
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Set(item!, oldItem!, index);

            CallCollectionChanging(eventArgs);

            Set_Core(index, item);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">追加する要素</param>
        private void Add_Impl(T item)
        {
            var index = Count;

            var eventArgs = NotifyCollectionChangedEventArgsHelper.Insert(item!, index);

            CallCollectionChanging(eventArgs);

            Insert_Core(index, item);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        private void AddRange_Impl(T[] items)
        {
            var index = Count;

            var eventArgs = NotifyCollectionChangedEventArgsHelper.InsertRange(items, index);

            CallCollectionChanging(eventArgs);

            Insert_Core(index, items);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        private void Insert_Impl(int index, T item)
        {
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Insert(item!, index);

            CallCollectionChanging(eventArgs);

            Insert_Core(index, item);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="startIndex">挿入開始インデックス</param>
        /// <param name="items">要素</param>
        private void InsertRange_Impl(int startIndex, IEnumerable<T> items)
        {
            var itemList = items.ToArray();
            var eventArgs = NotifyCollectionChangedEventArgsHelper.InsertRange(itemList, startIndex);

            CallCollectionChanging(eventArgs);

            Insert_Core(startIndex, itemList);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        private void Overwrite_Impl(int index, IEnumerable<T> items)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・追加後の要素数チェック
             * ・itemのnullチェック
             * ・必要更新数の計算
             * を実施済み。
             */
            var list = items.ToList();

            var updateCnt = list.Count;
            if (updateCnt + index > Count) updateCnt = Count - index;

            // 上書き要素
            var replaceOldItems = Items.GetRange(index, updateCnt);
            var replaceItems = list.Take(updateCnt).ToArray();

            // 追加要素
            var insertStartIndex = index + updateCnt;
            var insertItems = list.Skip(updateCnt).ToArray();

            // 通知用EventArgs
            var setEventArgs = NotifyCollectionChangedEventArgsHelper.SetRange(replaceItems, replaceOldItems, index);
            var insertRangeEventArgs =
                NotifyCollectionChangedEventArgsHelper.InsertRange(insertItems, insertStartIndex);

            // 変更前通知
            CallCollectionChanging(setEventArgs);
            CallCollectionChanging(insertRangeEventArgs);

            // 上書き
            Set_Core(index, replaceItems);
            // 追加
            Insert_Core(insertStartIndex, insertItems);

            // 変更後通知
            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(setEventArgs);
            CallCollectionChanged(insertRangeEventArgs);
        }

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス</param>
        /// <param name="newIndex">移動先のインデックス</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        private void Move_Impl(int oldIndex, int newIndex)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * を実施済み。
             */

            /* 移動させる対象の要素を退避 */
            var movedItem = Items[oldIndex];
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Move(movedItem!, newIndex, oldIndex);

            CallCollectionChanging(eventArgs);

            Move_Core(oldIndex, newIndex, 1);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, Count - length)] 移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, Count - oldIndex)] 移動させる要素数</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        private void MoveRange_Impl(int oldIndex, int newIndex, int count)
        {
            /*
             * 呼び出し元で
             * ・index, lengthの範囲チェック
             * を実施済み。
             */

            /* 移動させる対象の要素を退避 */
            var movedItems = Items.GetRange(oldIndex, count);
            var eventArgs = NotifyCollectionChangedEventArgsHelper.MoveRange(movedItems, newIndex, oldIndex);

            CallCollectionChanging(eventArgs);

            Move_Core(oldIndex, newIndex, count);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        private void Remove_Impl(int index)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・削除後の要素数チェック
             * を実施済み。
             */
            var removeItem = this[index];
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Remove(removeItem!, index);

            CallCollectionChanging(eventArgs);

            Remove_Core(index, 1);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 指定したインデックスを起点に複数の要素を削除する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">削除要素数</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        private void RemoveRange_Impl(int index, int count)
        {
            /*
             * 呼び出し元で
             * ・index, countの範囲チェック
             * ・削除後の要素数チェック
             * を実施済み。
             */
            var removeItems = Items.GetRange(index, count);
            var eventArgs = NotifyCollectionChangedEventArgsHelper.RemoveRange(removeItems, index);

            CallCollectionChanging(eventArgs);

            Remove_Core(index, count);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">調整する要素数</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        /// <exception cref="InvalidOperationException">
        ///     （Count > length の場合限定）
        ///     MakeDefaultItem プロパティに関数がセットされていない場合、
        ///     または要素を追加した際にnullがセットされた場合
        /// </exception>
        private void AdjustLength_Impl(int length)
        {
            if (Count == length)
            {
                NotifyPropertyChanged(nameof(Count));
                NotifyPropertyChanged(ListConstant.IndexerName);
                return;
            }

            if (Count > length)
            {
                AdjustLengthIfLong_Impl(length);
                return;
            }

            AdjustLengthIfShort_Impl(length);
        }

        /// <summary>
        /// 要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">調整する要素数</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        /// <exception cref="InvalidOperationException">
        ///     要素を追加した際にnullがセットされた場合
        /// </exception>
        private void AdjustLengthIfShort_Impl(int length)
        {
            if (Count >= length) return;

            var items = Enumerable.Range(Count, length - Count)
                .Select(i =>
                {
                    var item = MakeDefaultItem(i);
                    if (item is null)
                        throw new ArgumentException(
                            ErrorMessage.NotExecute($"{nameof(MakeDefaultItem)}({i})の結果がnullのため、"));
                    return item;
                });

            var startIndex = Count;
            var itemList = items.ToArray();
            var eventArgs = NotifyCollectionChangedEventArgsHelper.InsertRange(itemList, startIndex);

            CallCollectionChanging(eventArgs);

            Insert_Core(startIndex, itemList);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">調整する要素数</param>
        /// <returns>呼び出し元で NotifyCollectionChangedEvent を発火する際に必要な引数</returns>
        private void AdjustLengthIfLong_Impl(int length)
        {
            if (Count <= length) return;

            var index = length;
            var count = Count - length;
            var removeItems = Items.GetRange(index, count);
            var eventArgs = NotifyCollectionChangedEventArgsHelper.RemoveRange(removeItems, index);

            CallCollectionChanging(eventArgs);

            Remove_Core(index, count);

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// 要素をすべて除去する。
        /// </summary>
        private void Clear_Impl()
        {
            var eventArgs = NotifyCollectionChangedEventArgsHelper.Clear();

            CallCollectionChanging(eventArgs);

            Clear_Core();

            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        #endregion

        #region List Operation Core

        /// <summary>
        /// Set 中核処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="items">要素</param>
        private void Set_Core(int index, params T[] items)
        {
            items.ForEach((item, i) => Items[index + i] = item);
        }

        /// <summary>
        /// Insert 中核処理
        /// </summary>
        /// <param name="index">挿入開始インデックス</param>
        /// <param name="items">要素</param>
        private void Insert_Core(int index, params T[] items)
        {
            Items.InsertRange(index, items);
        }

        /// <summary>
        /// Move 中核処理
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        private void Move_Core(int oldIndex, int newIndex, int count)
        {
            var movedItems = Items.GetRange(oldIndex, count);
            Items.RemoveRange(oldIndex, count);
            Items.InsertRange(newIndex, movedItems);
        }

        /// <summary>
        /// Remove 中核処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">削除要素数</param>
        private void Remove_Core(int index, int count)
        {
            Items.RemoveRange(index, count);
        }

        /// <summary>
        /// Clear 中核処理
        /// </summary>
        private void Clear_Core()
        {
            Items.Clear();
            FillMinCapacity();
        }

        #endregion

        /// <summary>
        /// 要素最小数に充足するまでデフォルト要素を追加する。
        /// </summary>
        protected void FillMinCapacity()
        {
            var shortage = GetMinCapacity() - Count;
            if (shortage <= 0) return;

            var items = Enumerable.Range(0, shortage)
                .Select(MakeDefaultItem)
                .ToArray();
            var insertIndex = Count;

            Insert_Core(insertIndex, items);
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
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Items), Items);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected RestrictedCapacityCollection(SerializationInfo info, StreamingContext context)
        {
            Items = info.GetValue<List<T>>(nameof(Items));
        }
    }
}
