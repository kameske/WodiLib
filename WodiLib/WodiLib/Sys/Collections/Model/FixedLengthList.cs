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
using System.Runtime.Serialization;

// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedParameter.Global

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    /// 要素の読み取り・更新は可能だが追加・削除は不可能。<br/>
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<T> : ModelBase<FixedLengthList<T>>, IFixedLengthCollection<T>
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
        /// 要素変更通知
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

        /*
         * 継承先のクラスで内部的に独自リストを使用したい場合、
         * Countプロパティを継承して独自リストの要素数を返すこと。
         */
        /// <summary>要素数</summary>
        public virtual int Count => Items.Length;

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException">nullをセットしようとした場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        public virtual T this[int index]
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト</summary>
        protected virtual T[] Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        protected FixedLengthList()
        {
#if DEBUG
            try
            {
                ValidateDefaultItem();
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(FixedLengthList<T>), ex);
            }
#endif

            Items = new T[GetCapacity()];
            Fill();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合、
        ///     またはinitItemsの要素数が Capacity と一致しない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        protected FixedLengthList(IEnumerable<T> initItems)
        {
#if DEBUG
            try
            {
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

            var items = initItems.ToArray();

            ListValidationHelper.ItemsHasNotNull(items);
            FixedLengthListValidationHelper.ItemCount(items.Length, GetCapacity());

            Items = new T[GetCapacity()];
            Set_Core(0, items);
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Override Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public abstract int GetCapacity();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
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
        /// <param name="newIndex">[Range(0, Count - length)] 移動先のインデックス開始位置</param>
        /// <param name="length">[Range(0, Count - oldIndex)] 移動させる要素数</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合</exception>
        public void MoveRange(int oldIndex, int newIndex, int length)
        {
            ListValidationHelper.ItemCountNotZero(Count);
            ListValidationHelper.SelectIndex(oldIndex, Count, nameof(oldIndex));
            ListValidationHelper.InsertIndex(newIndex, Count, nameof(newIndex));
            ListValidationHelper.Count(length, Count);
            ListValidationHelper.Range(oldIndex, length, Count, nameof(oldIndex));
            ListValidationHelper.Range(length, newIndex, Count, nameof(length), nameof(newIndex));

            MoveRange_Impl(oldIndex, newIndex, length);
        }

        /// <summary>
        /// すべての要素を初期化する。
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
            return ((IList<T>) Items).Contains(item);
        }

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf([AllowNull] T item)
        {
            if (item == null) return -1;
            return Array.IndexOf(Items, item);
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
        public virtual IEnumerator<T> GetEnumerator() => Items.AsEnumerable().GetEnumerator();

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(FixedLengthList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return All().SequenceEqual(other.All());
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
#pragma warning disable 618 // TODO: Ver 2.6 まで
        public bool Equals(IReadOnlyFixedLengthCollection<T>? other)
#pragma warning restore 618
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

            return this.SequenceEqual(other);
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Implements Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
            return Enumerable.Range(index, count)
                .Select(i => Items[i]);
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
            var movedItems = Items.GetRange(oldIndex, count).ToList();
            var eventArgs = NotifyCollectionChangedEventArgsHelper.MoveRange(movedItems, newIndex, oldIndex);

            CallCollectionChanging(eventArgs);

            Move_Core(oldIndex, newIndex, count);

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
        /// Move 中核処理
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        private void Move_Core(int oldIndex, int newIndex, int count)
        {
            if (oldIndex == newIndex) return;

            // ロジック簡略化のため oldIndex > newIndex を強制する
            if (oldIndex < newIndex)
            {
                Move_Core(oldIndex + count, oldIndex, newIndex - oldIndex);
                return;
            }

            var movedItems = Enumerable.Range(newIndex, oldIndex - newIndex)
                .Select(i => Items[i])
                .ToArray();
            Array.Copy(Items, oldIndex, Items, newIndex, count);
            Set_Core(newIndex + count, movedItems);
        }

        /// <summary>
        /// Clear 中核処理
        /// </summary>
        private void Clear_Core()
        {
            Enumerable.Range(0, GetCapacity())
                .ForEach(i => Items[i] = MakeDefaultItem(i));
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// デフォルト値ですべての要素を埋める。
        /// </summary>
        protected void Fill()
        {
            // 挿入時に変更通知を発火させないようにする
            var newItems = Enumerable.Range(0, GetCapacity())
                .Select(MakeDefaultItem).ToArray();
            Set_Core(0, newItems);
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
        protected FixedLengthList(SerializationInfo info, StreamingContext context)
        {
            Items = info.GetValue<T[]>(nameof(Items));
        }
    }
}
