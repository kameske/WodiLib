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
using System.ComponentModel;
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
    public abstract class RestrictedCapacityCollection<T> : IRestrictedCapacityCollection<T>, IReadOnlyList<T>,
        IEquatable<RestrictedCapacityCollection<T>>, IEquatable<IFixedLengthCollection<T>>, ISerializable
    {
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
                if (value == null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(value)));

                var max = Count - 1;
                const int min = 0;
                if (index < min || max < index)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), min, max, index));

                PrivateSetItem(index, value);
            }
        }

        /// <summary>
        /// SetItemイベントハンドラリスト
        /// </summary>
        [field: NonSerialized]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public SetItemHandlerList<T> SetItemHandlerList { get; private set; } = new SetItemHandlerList<T>();

        /// <summary>
        /// InsertItemイベントハンドラリスト
        /// </summary>
        [field: NonSerialized]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public InsertItemHandlerList<T> InsertItemHandlerList { get; private set; } = new InsertItemHandlerList<T>();

        /// <summary>
        /// RemoveItemイベントハンドラリスト
        /// </summary>
        [field: NonSerialized]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public RemoveItemHandlerList<T> RemoveItemHandlerList { get; private set; } = new RemoveItemHandlerList<T>();

        /// <summary>
        /// ClearItemイベントハンドラリスト
        /// </summary>
        [field: NonSerialized]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ClearItemHandlerList<T> ClearItemHandlerList { get; private set; } = new ClearItemHandlerList<T>();

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
        public RestrictedCapacityCollection()
        {
            try
            {
                ValidateCapacity();
                ValidateDefaultItem();
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(RestrictedCapacityCollection<T>), ex);
            }

            FillMinCapacity();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">[NotNull] 初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public RestrictedCapacityCollection(IReadOnlyCollection<T> list)
        {
            try
            {
                ValidateCapacity();
                ValidateDefaultItem();
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(RestrictedCapacityCollection<T>), ex);
            }

            if (list is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(list)));

            if (list.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(list)));

            var cnt = list.Count;
            if (cnt < GetMinCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));
            if (cnt > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var insertIndex = 0;
            foreach (var item in list)
            {
                PrivateInsertItem(insertIndex, item);
                insertIndex++;
            }
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
            var indexMax = Count - 1;
            const int min = 0;
            if (index < min || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, indexMax, index));

            var countMax = Count;
            if (count < min || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, countMax, count));

            if (Count - index < count)
                throw new ArgumentException(
                    $"{nameof(index)}および{nameof(count)}が有効な範囲を示していません。");

            return Items.GetRange(index, count);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(item)));

            var addedLength = Count + 1;
            if (addedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var index = Count;
            PrivateInsertItem(index, item);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void AddRange(IReadOnlyCollection<T> items)
        {
            if (items is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));

            if (items.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(items)));

            var addedLength = Count + items.Count;
            if (addedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var insertIndex = Count;
            foreach (var item in items)
            {
                PrivateInsertItem(insertIndex, item);
                insertIndex++;
            }
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="item">[NotNull] 挿入する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void Insert(int index, T item)
        {
            var max = Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (item == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));

            var insertedLength = Count + 1;
            if (insertedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            PrivateInsertItem(index, item);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        public void InsertRange(int index, IReadOnlyCollection<T> items)
        {
            var max = Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (items is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));

            if (items.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(items)));

            var addedLength = Count + items.Count;
            if (addedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var insertIndex = index;

            foreach (var item in items)
            {
                Insert(insertIndex, item);
                insertIndex++;
            }
        }

        /// <summary>
        /// 指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="list">[NotNull] 上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">listがnullの場合</exception>
        /// <exception cref="ArgumentException">list中にnull要素が含まれる場合</exception>
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
        public void Overwrite(int index, IReadOnlyList<T> list)
        {
            var indexMax = Count;
            const int indexMin = 0;
            if (index < indexMin || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), indexMin, indexMax, index));

            if (list is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(list)));
            if (list.HasNullItem())
                throw new ArgumentException(
                    ErrorMessage.NotNullInList(nameof(list)));

            var updateCnt = list.Count;
            if (updateCnt + index > Count) updateCnt = Count - index;
            var insertCnt = list.Count - updateCnt;

            if (insertCnt > 0 && Count + insertCnt > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            for (var i = 0; i < updateCnt; i++)
            {
                this[index + i] = list[i];
            }

            var addList = list.Skip(updateCnt);
            AddRange(addList.ToList());
        }

        /// <summary>
        /// 特定のオブジェクトを要素として持つとき、最初に出現したものを削除する。
        /// </summary>
        /// <param name="item">[Nullable] 削除する要素</param>
        /// <returns>削除成否</returns>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinValue未満になる場合</exception>
        public bool Remove(T item)
        {
            if (item == null) return false;

            var index = Items.IndexOf(item);
            if (index < 0) return false;

            var removedLength = Count - 1;
            if (removedLength < GetMinCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));

            PrivateRemoveItem(index);

            return true;
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinValue未満になる場合</exception>
        public void RemoveAt(int index)
        {
            var max = Count - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            var removedLength = Count - 1;
            if (removedLength < GetMinCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));

            PrivateRemoveItem(index);
        }

        /// <summary>
        /// 要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinValue未満になる場合</exception>
        public void RemoveRange(int index, int count)
        {
            var indexMax = Count - 1;
            const int min = 0;

            if (index < min || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, indexMax, index));

            var countMax = Count;

            if (count < min || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, countMax, count));

            if (Count - index < count)
                throw new ArgumentException(
                    $"{nameof(index)}および{nameof(count)}が有効な範囲を示していません。");

            var removedLength = Count - count;
            if (removedLength < GetMinCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));

            for (var i = 0; i < count; i++)
            {
                PrivateRemoveItem(index);
            }
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


            var count = Count;

            if (count == length) return;

            if (count < length)
            {
                // 長さが足りないので追加
                //   途中でMakeDefaultItemの結果がnullとなった場合に備え、予めすべてのデフォルト要素を取得してから処理する

                var addItemList = new List<T>();

                for (var i = Count; i < length; i++)
                {
                    var addItem = MakeDefaultItem(i);
                    if (addItem == null)
                        throw new ArgumentException(
                            ErrorMessage.NotExecute($"{nameof(MakeDefaultItem)}({i})の結果がnullのため、"));

                    addItemList.Add(addItem);
                }

                AddRange(addItemList);

                return;
            }

            // 長すぎるので除去
            RemoveRange(length, Count - length);
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
            if (Count >= length) return;
            AdjustLength(length);
        }

        /// <summary>
        /// すべての要素を削除し、最小の要素数だけ初期化する。
        /// </summary>
        public void Clear()
        {
            PrivateClearItems();

            FillMinCapacity();
        }

        /// <summary>
        /// 指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">[Nullable] 対象要素</param>
        /// <returns>指定の要素が含まれる場合はtrue</returns>
        public bool Contains(T item) => Items.Contains(item);

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">[Nullable] 対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf(T item) => Items.IndexOf(item);

        /// <summary>
        /// すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">[NotNull] コピー先の配列</param>
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
        public bool Equals(RestrictedCapacityCollection<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Items.SequenceEqual(other.Items);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IFixedLengthCollection<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Items.SequenceEqual(other.ToList());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Implements Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        bool ICollection<T>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) Items).GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定したインデックス位置にある要素を置き換える。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected virtual void SetItem(int index, T item)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・itemのnullチェック
             * を実施済み。
             */
            Items[index] = item;
            /*
             * 呼び出し元でイベントハンドラを実行する。
             */
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected virtual void InsertItem(int index, T item)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・追加後の要素数チェック
             * ・itemのnullチェック
             * を実施済み。
             */
            Items.Insert(index, item);
            /*
             * 呼び出し元でイベントハンドラを実行する。
             */
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">インデックス</param>
        protected virtual void RemoveItem(int index)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・削除後の要素数チェック
             * を実施済み。
             */
            Items.RemoveAt(index);
            /*
             * 呼び出し元でイベントハンドラを実行する。
             */
        }

        /// <summary>
        /// 要素をすべて除去したあと、
        /// 必要に応じて最小限の要素を新たに設定する。
        /// </summary>
        protected virtual void ClearItems()
        {
            Items.Clear();
            /*
             * 呼び出し元でイベントハンドラを実行する。
             *
             * 呼び出し元で
             * ・最小要素数リストの再作成
             * を実施する。
             */
        }

        /// <summary>
        /// 容量上下限チェック
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     容量下限が 0 未満の場合、
        ///     または容量上限が容量下限未満の場合
        /// </exception>
        protected void ValidateCapacity()
        {
            var maxCapacity = GetMaxCapacity();
            var minCapacity = GetMinCapacity();

            if (minCapacity < 0)
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最小容量", 0, maxCapacity));

            if (maxCapacity < minCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最大容量", $"最小容量（{minCapacity}）", maxCapacity));
        }

        /// <summary>
        /// デフォルト値チェック
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="MakeDefaultItem"/>がnullを返却する場合</exception>
        protected void ValidateDefaultItem()
        {
            var value = MakeDefaultItem(0);
            if (value == null)
                throw new InvalidOperationException(
                    ErrorMessage.NotNull($"{nameof(MakeDefaultItem)}メソッドの返戻値"));
        }

        /// <summary>
        /// 要素最小数に充足するまでデフォルト要素を追加する。
        /// </summary>
        protected void FillMinCapacity()
        {
            var shortage = GetMinCapacity() - Count;
            if (shortage <= 0) return;

            var insertIndex = Count;
            for (var i = 0; i < shortage; i++)
            {
                PrivateInsertItem(insertIndex, MakeDefaultItem(insertIndex));
                insertIndex++;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected abstract T MakeDefaultItem(int index);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定したインデックス位置にある要素を置き換える。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        private void PrivateSetItem(int index, T item)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・itemのnullチェック
             * を実施済み。
             */
            SetItem(index, item);
            SetItemHandlerList.Execute(index, item);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        private void PrivateInsertItem(int index, T item)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・追加後の要素数チェック
             * ・itemのnullチェック
             * を実施済み。
             */
            InsertItem(index, item);
            InsertItemHandlerList.Execute(index, item);
        }


        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">インデックス</param>
        private void PrivateRemoveItem(int index)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・削除後の要素数チェック
             * を実施済み。
             */
            RemoveItem(index);
            RemoveItemHandlerList.Execute(index);
        }

        /// <summary>
        /// 要素をすべて除去したあと、
        /// 必要に応じて最小限の要素を新たに設定する。
        /// </summary>
        private void PrivateClearItems()
        {
            ClearItems();
            ClearItemHandlerList.Execute();
            /*
             * 呼び出し元で
             * ・最小要素数リストの再作成
             * を実施する。
             */
        }

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
            SetItemHandlerList = new SetItemHandlerList<T>();
            InsertItemHandlerList = new InsertItemHandlerList<T>();
            RemoveItemHandlerList = new RemoveItemHandlerList<T>();
            ClearItemHandlerList = new ClearItemHandlerList<T>();
        }
    }
}