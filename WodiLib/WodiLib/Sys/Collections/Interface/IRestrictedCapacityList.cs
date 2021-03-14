// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限のあるListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量制限を設けた機能。
    ///     <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。<br/>
    ///     範囲操作メソッド実行時に通知される <see cref="INotifyCollectionChange.CollectionChanging"/>
    ///     および <see cref="INotifyCollectionChange.CollectionChanged"/> は
    ///     要素をいくつ変更してもそれぞれ1度だけ呼ばれる。たとえ操作した要素数が0個であっても呼ばれる。<br/>
    ///     操作前後の要素は <see cref="NotifyCollectionChangedEventArgs"/> の Items を
    ///     <see cref="IList{T}"/> にキャストすることで取り出せる。
    ///     この弊害として、WPFのUIにバインドした状態で範囲操作するメソッドを実行すると例外が発生するため注意。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IRestrictedCapacityList<T> : IModelBase<IRestrictedCapacityList<T>>,
        IReadOnlyRestrictedCapacityList<T>, IDeepCloneableRestrictedCapacityList<IRestrictedCapacityList<T>, T>
    {
        /// <inheritdoc cref="IList{T}.this"/>
        public new T this[int index] { get; set; }

        /// <inheritdoc cref="IList{T}.Count"/>
        public new int Count { get; }

        /// <summary>
        ///     リストの連続した要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        public void SetRange(int index, IEnumerable<T> items);

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     追加操作によって要素数が <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void Add(T item);

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     追加操作によって要素数が <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void AddRange(IEnumerable<T> items);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] インデックス</param>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合、
        ///     または <paramref name="item"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     追加操作によって要素数が <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void Insert(int index, T item);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     追加操作によって要素数が <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void InsertRange(int index, IEnumerable<T> items);

        /// <summary>
        ///     指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IExtendedList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     追加操作によって要素数が <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void Overwrite(int index, IEnumerable<T> items);

        /// <summary>
        ///     指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。
        /// </exception>
        public void Move(int oldIndex, int newIndex);

        /// <summary>
        ///     指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">
        ///     [Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)]
        ///     移動する項目のインデックス開始位置
        /// </param>
        /// <param name="newIndex">
        ///     [Range(0, <see cref="IReadOnlyList{T}.Count"/> - <paramref name="count"/>)]
        ///     移動先のインデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadOnlyList{T}.Count"/> - <paramref name="oldIndex"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     特定のオブジェクトで最初に出現したものを削除する。
        /// </summary>
        /// <param name="item">削除対象オブジェクト</param>
        /// <returns>
        ///     <paramref name="item"/> が存在する場合 <see langword="true"/>。それ以外の場合は <see langword="false"/>。
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     削除した結果要素数が<see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/>未満になる場合。
        /// </exception>
        public bool Remove([AllowNull] T item);

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     削除した結果要素数が<see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/>未満になる場合。
        /// </exception>
        public void RemoveAt(int index);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合、
        ///     または削除した結果要素数が<see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/>未満になる場合。
        /// </exception>
        public void RemoveRange(int index, int count);

        /// <summary>
        ///     要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/>,
        ///     <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLength(int length);

        /// <summary>
        ///     要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/>,
        ///     <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfShort(int length);

        /// <summary>
        ///     要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/>,
        ///     <see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfLong(int length);

        /// <summary>
        ///     すべての要素を初期化する。
        /// </summary>
        /// <remarks>
        ///     既存の要素はすべて除去され、<see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/> 個の
        ///     新たなデフォルト要素が編集される。
        /// </remarks>
        void Clear();

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> の要素数が
        ///     <see cref="IReadOnlyRestrictedCapacityList{T}.GetMinCapacity"/> 未満、
        ///     または<see cref="IReadOnlyRestrictedCapacityList{T}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void Reset(IEnumerable<T> initItems);
    }

    /// <summary>
    ///     【読み取り専用】容量制限のあるListインタフェース
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    public interface IReadOnlyRestrictedCapacityList<T> : IModelBase<IReadOnlyRestrictedCapacityList<T>>,
        IReadOnlyExtendedList<T>, IDeepCloneableRestrictedCapacityList<IReadOnlyRestrictedCapacityList<T>, T>
    {
        /// <summary>
        ///     容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        int GetMaxCapacity();

        /// <summary>
        ///     容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        int GetMinCapacity();
    }

    /// <summary>
    /// <see cref="IRestrictedCapacityList{T}"/> ディープクローンインタフェース
    /// </summary>
    /// <typeparam name="T">クローン返却型</typeparam>
    /// <typeparam name="TIn"><see cref="IFixedLengthList{T}"/>内包型</typeparam>
    public interface IDeepCloneableRestrictedCapacityList<out T, TIn>
        where T : IReadOnlyRestrictedCapacityList<TIn>
    {
        /// <summary>
        ///     自身の要素をコピーした新たなインスタンスを返却する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         自身の内包する要素が構造体・クラス型の場合、返却するインスタンスの要素はすべてディープコピーされた状態で格納される。
        ///         レコードの場合はシャローコピーされた要素が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="length"/> を指定した場合、返却する列挙子の要素数を指定された数にする。<br/>
        ///         <paramref name="length"/> &lt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、超過する要素は切り捨てられる。<br/>
        ///         <paramref name="length"/> &gt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、不足する要素は内包型ごとに定められたデフォルト値（
        ///         <see langword="null"/>ではない）が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="values"/> を指定した場合、<paramref name="values"/> のキーに指定されたインデックスの要素を <paramref name="values"/>
        ///         の値で上書きする。<br/>
        ///         返却する要素数を上回るインデックスが指定されている場合、その要素は無視される。
        ///     </para>
        /// </remarks>
        /// <param name="length">
        /// [Range(<typeparamref name="T"/> の <see cref="IRestrictedCapacityList{T}.GetMinCapacity"/>,
        /// <typeparamref name="T"/> の <see cref="IRestrictedCapacityList{T}.GetMaxCapacity"/>)]
        /// ディープコピー後の要素数
        /// </param>
        /// <param name="values">ディープコピー時の上書きインデックスと値のペア列挙子</param>
        /// <returns>自身をディープコピーしたインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="values"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public T DeepCloneWith(int? length = null, IReadOnlyDictionary<int, TIn>? values = null);
    }
}
