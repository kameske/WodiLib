using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量固定のList基底クラス
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<T> : ModelBase<FixedLengthList<T>>, IFixedLengthCollection<T>,
        ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event NotifyCollectionChangedEventHandler _collectionChanged;

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
                if (ReferenceEquals(value, null))
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
        [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
        public SetItemHandlerList<T> SetItemHandlerList { get; private set; } = new SetItemHandlerList<T>();

        /// <summary>
        /// ClearItemイベントハンドラリスト
        /// </summary>
        [field: NonSerialized]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Obsolete("要素変更通知は CollectionChanged イベントを利用して取得してください。 Ver1.3 で削除します。")]
        public ClearItemHandlerList<T> ClearItemHandlerList { get; private set; } = new ClearItemHandlerList<T>();

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
            try
            {
                ValidateDefaultItem();
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(FixedLengthList<T>), ex);
            }

            Items = new T[GetCapacity()];
            Fill();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">[NotNull] 初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合、
        ///     またはinitItemsの要素数が Capacity と一致しない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public FixedLengthList(IEnumerable<T> initItems)
        {
            try
            {
                ValidateDefaultItem();
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(RestrictedCapacityCollection<T>), ex);
            }

            if (initItems is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(initItems)));

            var list = initItems.ToArray();

            if (list.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(initItems)));

            if (list.Length != GetCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.NotEqual($"{nameof(initItems)}の要素数", $"{nameof(GetCapacity)}({GetCapacity()})"));

            Items = new T[GetCapacity()];
            var insertIndex = 0;
            foreach (var item in list)
            {
                SetItem(insertIndex, item);
                insertIndex++;
            }
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

            return new List<T>(Items).GetRange(index, count);
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

            var max = Count - 1;
            const int min = 0;
            if (oldIndex < min || max < oldIndex)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(oldIndex), min, max, oldIndex));
            if (newIndex < min || max < newIndex)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(newIndex), min, max, newIndex));

            PrivateMoveItem(oldIndex, newIndex);
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
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex, length が指定範囲外の場合
        /// </exception>
        public void MoveRange(int oldIndex, int newIndex, int length)
        {
            if (Count == 0)
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute("リストの要素が0個のため"));

            const int min = 0;
            var oldIndexMax = Count - 1;
            if (oldIndex < min || oldIndexMax < oldIndex)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(oldIndex), min, oldIndexMax, oldIndex));
            var lengthMax = Count - oldIndex;
            if (length < min || lengthMax < length)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(newIndex), min, lengthMax, newIndex));
            var newIndexMax = Count - length;
            if (newIndex < min || newIndexMax < newIndex)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(newIndex), min, newIndexMax, newIndex));

            PrivateMoveItemRange(oldIndex, newIndex, length);
        }

        /// <summary>
        /// すべての要素を初期化する。
        /// </summary>
        public void Clear()
        {
            PrivateClearItems();
        }

        /// <summary>
        /// 指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">[Nullable] 対象要素</param>
        /// <returns>指定の要素が含まれる場合はtrue</returns>
        public bool Contains(T item) => ((IList<T>) Items).Contains(item);

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">[Nullable] 対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf(T item) => Array.IndexOf(Items, item);

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
        public virtual IEnumerator<T> GetEnumerator() => Items.AsEnumerable().GetEnumerator();

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(FixedLengthList<T> other)
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
        public bool Equals(IReadOnlyFixedLengthCollection<T> other)
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
        public bool Equals(IReadOnlyList<T> other)
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
        public bool Equals(IEnumerable<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return All().SequenceEqual(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Implements Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス</param>
        /// <param name="newIndex">移動先のインデックス</param>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * を実施済み。
             */
            /* 移動させる対象の要素を退避 */
            var movedItem = this[oldIndex];

            var current = oldIndex;

            while (current < newIndex)
            {
                /*
                 * oldIndex < newIndex の場合このブロックが実行される。
                 * oldIndex ～ newIndex の要素が一つ手前にずらされる。
                 */
                this[current] = this[current + 1];
                current++;
            }

            while (current > newIndex)
            {
                /*
                 * oldIndex > newIndex の場合このブロックが実行される。
                 * oldIndex ～ newIndex の要素が一つ後ろにずらされる。
                 */
                this[current] = this[current - 1];
                current--;
            }

            /* 移動させる対象の要素をあるべき場所にセットして完了。 */
            this[newIndex] = movedItem;

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
            for (var i = 0; i < GetCapacity(); i++)
            {
                Items[i] = MakeDefaultItem(i);
            }

            /*
             * 呼び出し元でイベントハンドラを実行する。
             */
        }

        /// <summary>
        /// デフォルト値チェック
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="MakeDefaultItem"/>がnullを返却する場合</exception>
        protected void ValidateDefaultItem()
        {
            var value = MakeDefaultItem(0);
            if (ReferenceEquals(value, null))
                throw new InvalidOperationException(
                    ErrorMessage.NotNull($"{nameof(MakeDefaultItem)}メソッドの返戻値"));
        }

        /// <summary>
        /// デフォルト値ですべての要素を埋める。
        /// </summary>
        protected void Fill()
        {
            // 挿入時に変更通知を発火させないよう、 PrivateSetItem() ではなく SetItem() を使用する
            for (var i = 0; i < GetCapacity(); i++)
            {
                SetItem(i, MakeDefaultItem(i));
            }
        }

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
            var ordinal = this[index];

            SetItem(index, item);

#pragma warning disable 618
            SetItemHandlerList.Execute(index, item);
#pragma warning restore 618
            NotifyPropertyChanged(ListConstant.IndexerName);
            _collectionChanged?.Invoke(this,
                NotifyCollectionChangedEventArgsHelper.Set(item, ordinal, index));
        }

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス</param>
        /// <param name="newIndex">移動先のインデックス</param>
        private void PrivateMoveItem(int oldIndex, int newIndex)
        {
            /*
             * 呼び出し元で
             * ・indexの範囲チェック
             * ・itemのnullチェック
             * を実施済み。
             */
            var moveItem = this[oldIndex];

            MoveItem(oldIndex, newIndex);

            NotifyPropertyChanged(ListConstant.IndexerName);
            _collectionChanged?.Invoke(this,
                NotifyCollectionChangedEventArgsHelper.Move(moveItem, newIndex, oldIndex));
        }

        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, Count - length)] 移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, Count - oldIndex)] 移動させる要素数</param>
        private void PrivateMoveItemRange(int oldIndex, int newIndex, int count)
        {
            /*
             * 呼び出し元で
             * ・index, lengthFの範囲チェック
             * を実施済み。
             */
            var moveItems = Items.Skip(oldIndex).Take(count).ToList();

            if (oldIndex >= newIndex)
            {
                // 前方へ移動
                var beforeIndex = oldIndex;
                var afterIndex = newIndex;
                for (var i = 0; i < count; i++)
                {
                    MoveItem(beforeIndex++, afterIndex++);
                }
            }
            else
            {
                // 後方へ移動
                var beforeIndex = oldIndex;
                var afterIndex = newIndex + count - 1;
                for (var i = 0; i < count; i++)
                {
                    MoveItem(beforeIndex, afterIndex);
                }
            }

            NotifyPropertyChanged(ListConstant.IndexerName);
            _collectionChanged?.Invoke(this,
                NotifyCollectionChangedEventArgsHelper.MoveRange(moveItems, newIndex, oldIndex));
        }

        /// <summary>
        /// 要素をすべて除去したあと、
        /// 必要に応じて最小限の要素を新たに設定する。
        /// </summary>
        private void PrivateClearItems()
        {
            ClearItems();

#pragma warning disable 618
            ClearItemHandlerList.Execute();
#pragma warning restore 618
            NotifyPropertyChanged(ListConstant.IndexerName);
            _collectionChanged?.Invoke(this,
                NotifyCollectionChangedEventArgsHelper.Clear());
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
        public void GetObjectData(SerializationInfo info, StreamingContext context)
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
#pragma warning disable 618
            SetItemHandlerList = new SetItemHandlerList<T>();
            ClearItemHandlerList = new ClearItemHandlerList<T>();
#pragma warning restore 618
        }
    }
}