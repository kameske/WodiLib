// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     長さが固定されたListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量を固定した機能。
    ///     <see cref="ObservableCollection{T}"/> の Read, Update 各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。
    ///     固定しているのは容量のみで、要素の入れ替えや更新は可能。<br/>
    ///     範囲操作メソッド実行時に通知される <see cref="INotifyCollectionChange.CollectionChanging"/>
    ///     および <see cref="INotifyCollectionChange.CollectionChanged"/> は
    ///     要素をいくつ変更してもそれぞれ1度だけ呼ばれる。たとえ操作した要素数が0個であっても呼ばれる。<br/>
    ///     操作前後の要素は <see cref="NotifyCollectionChangedEventArgs"/> の Items を
    ///     <see cref="IList{T}"/> にキャストすることで取り出せる。
    ///     この弊害として、WPFのUIにバインドした状態で範囲操作するメソッドを実行すると例外が発生するため注意。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IFixedLengthList<T> : IModelBase<IFixedLengthList<T>>,
        IReadOnlyFixedLengthList<T>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException">nullをセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合。</exception>
        public new T this[int index] { get; set; }

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.IsNotifyBeforeCollectionChange"/>
        public new bool IsNotifyBeforeCollectionChange { get; set; }

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.IsNotifyAfterCollectionChange"/>
        public new bool IsNotifyAfterCollectionChange { get; set; }

        /// <summary>
        ///     リストの連続した要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        public void SetRange(int index, IEnumerable<T> items);

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
        ///     すべての要素を初期化する。
        /// </summary>
        /// <remarks>
        ///     既存の要素はすべて除去され、<see cref="IReadOnlyFixedLengthList{T}.GetCapacity"/> 個の
        ///     新たなデフォルト要素が編集される。
        /// </remarks>
        public void Clear();

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
        ///     <see cref="IReadOnlyFixedLengthList{T}.GetCapacity"/> と一致しない場合。
        /// </exception>
        public void Reset(IEnumerable<T> initItems);

        /// <inheritdoc cref="IReadOnlyFixedLengthList{T}.DeepCloneWith(IEnumerable{KeyValuePair{int, T}})"/>
        public new IFixedLengthList<T> DeepCloneWith(IEnumerable<KeyValuePair<int, T>>? values = null);

        /// <inheritdoc cref="IReadOnlyFixedLengthList{T}.DeepCloneWith(int?,IEnumerable{KeyValuePair{int, T}})"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new IFixedLengthList<T> DeepCloneWith(int? length = null,
            IEnumerable<KeyValuePair<int, T>>? values = null);
    }

    /// <summary>
    ///     【読み取り専用】長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyFixedLengthList<T> : IModelBase<IReadOnlyFixedLengthList<T>>,
        IReadOnlyExtendedList<T>
    {
        /// <summary>
        ///     容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public int GetCapacity();

        /// <summary>
        ///     自身の要素をコピーした新たなインスタンスを返却する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         自身の内包する要素が構造体・クラス型の場合、返却するインスタンスの要素はすべてディープコピーされた状態で格納される。
        ///         レコードの場合はシャローコピーされた要素が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="values"/> を指定した場合、<paramref name="values"/> のキーに指定されたインデックスの要素を <paramref name="values"/>
        ///         の値で上書きする。<br/>
        ///         返却する要素数を上回るインデックスが指定されている場合、その要素は無視される。
        ///     </para>
        /// </remarks>
        /// <param name="values">ディープコピー時の上書き要素</param>
        /// <returns>自身をディープコピーしたインスタンス</returns>
        public IReadOnlyFixedLengthList<T> DeepCloneWith(IEnumerable<KeyValuePair<int, T>>? values = null);

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.DeepCloneWith" select="summary"/>
        /// <remarks>
        ///     <para>
        ///         自身の内包する要素が構造体・クラス型の場合、返却するインスタンスの要素はすべてディープコピーされた状態で格納される。
        ///         レコードの場合はシャローコピーされた要素が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="length"/> は無視される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="values"/> を指定した場合、<paramref name="values"/> のキーに指定されたインデックスの要素を <paramref name="values"/>
        ///         の値で上書きする。<br/>
        ///         返却する要素数を上回るインデックスが指定されている場合、その要素は無視される。
        ///     </para>
        /// </remarks>
        /// <param name="length">未使用</param>
        /// <param name="values">ディープコピー時の上書き要素</param>
        /// <returns>自身をディープコピーしたインスタンス</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new IReadOnlyFixedLengthList<T> DeepCloneWith(int? length = null,
            IEnumerable<KeyValuePair<int, T>>? values = null);
    }
}
