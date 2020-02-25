using System;
using System.Collections;
using System.Collections.Generic;
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
    public abstract class FixedLengthList<T> : IFixedLengthCollection<T>, IReadOnlyList<T>,
        IEquatable<FixedLengthList<T>>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
        public T this[int index]
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
        public SetItemHandlerList<T> SetItemHandlerList { get; private set; } = new SetItemHandlerList<T>();

        /// <summary>
        /// ClearItemイベントハンドラリスト
        /// </summary>
        [field: NonSerialized]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
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
        public FixedLengthList()
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
        /// <param name="list">[NotNull] 初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合、
        ///     またはlistの要素数が Capacity と一致しない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public FixedLengthList(IReadOnlyCollection<T> list)
        {
            try
            {
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

            if (list.Count != GetCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.NotEqual($"{nameof(list)}の要素数", $"{nameof(GetCapacity)}({GetCapacity()})"));

            Items = new T[GetCapacity()];
            var insertIndex = 0;
            foreach (var item in list)
            {
                PrivateSetItem(insertIndex, item);
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
        public bool Equals(FixedLengthList<T> other)
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
            for (var i = 0; i < GetCapacity(); i++)
            {
                PrivateSetItem(i, MakeDefaultItem(i));
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
            SetItem(index, item);
            SetItemHandlerList.Execute(index, item);
        }

        /// <summary>
        /// 要素をすべて除去したあと、
        /// 必要に応じて最小限の要素を新たに設定する。
        /// </summary>
        private void PrivateClearItems()
        {
            ClearItems();
            ClearItemHandlerList.Execute();
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
            SetItemHandlerList = new SetItemHandlerList<T>();
            ClearItemHandlerList = new ClearItemHandlerList<T>();
        }
    }
}