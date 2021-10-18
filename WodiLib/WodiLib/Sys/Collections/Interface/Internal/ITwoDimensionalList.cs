// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    internal interface ITwoDimensionalList<TIn, TOut> :
        IReadableTwoDimensionalList<TOut>,
        IWritableTwoDimensionalList<TIn, TOut>,
        ISizeChangeableTwoDimensionalList<TIn, TOut>,
        INotifiableTwoDimensionalListChangeInternal<TOut>,
        IEqualityComparable<ITwoDimensionalList<TIn, TOut>>,
        IDeepCloneableTwoDimensionalListInternal<ITwoDimensionalList<TIn, TOut>, TIn>
        where TOut : TIn
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public new TOut this[int rowIndex, int columnIndex] { get; set; }

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<IEnumerable<TIn>>? other,
            IEqualityComparer<TIn>? itemComparer = null);

        /// <summary>
        ///     自身の全要素を簡易コピーした二次元配列を返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         返す二次元配列の状態は <paramref name="isTranspose"/> によって変化する。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="false"/> の場合、
        ///         自身の要素をそのまま返す。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="true"/> の場合、
        ///         自身を転置した状態の要素を返す。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="isTranspose">転置フラグ</param>
        /// <returns>自身の全要素簡易コピー配列</returns>
        public TOut[][] ToTwoDimensionalArray(bool isTranspose = false);
    }

    /// <summary>
    ///     WodiLib内部で使用する二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="T">リスト要素型</typeparam>
    [Obsolete]
    internal interface ITwoDimensionalList<T> :
        IReadableTwoDimensionalList<T, ITwoDimensionalList<T>>,
        IWritableTwoDimensionalList<T, ITwoDimensionalList<T>, ITwoDimensionalList<T>>,
        IRowSizeChangeableTwoDimensionalList<T, ITwoDimensionalList<T>, ITwoDimensionalList<T>,
            ITwoDimensionalList<T>>,
        IColumnSizeChangeableTwoDimensionalList<T, ITwoDimensionalList<T>, ITwoDimensionalList<T>,
            ITwoDimensionalList<T>>,
        ISizeChangeableTwoDimensionalList<T, ITwoDimensionalList<T>, ITwoDimensionalList<T>,
            ITwoDimensionalList<T>, ITwoDimensionalList<T>, ITwoDimensionalList<T>>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public new T this[int rowIndex, int columnIndex] { get; }

        /// <summary>
        ///     空フラグ
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> == 0 かつ <see cref="ColumnCount"/> == 0 の場合に <see langword="true"/> を、
        ///     それ以外の場合に <see langword="false"/> を返す。
        /// </remarks>
        public new bool IsEmpty { get; }

        /// <summary>行数</summary>
        public new int RowCount { get; }

        /// <summary>列数</summary>
        public new int ColumnCount { get; }

        /// <summary>
        ///     総数
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> * <see cref="ColumnCount"/> を返す。
        /// </remarks>
        public new int AllCount { get; }

        /// <summary>
        ///     行容量最大値を返す。
        /// </summary>
        /// <returns>行容量最大値</returns>
        public new int GetMaxRowCapacity();

        /// <summary>
        ///     行容量最小値を返す。
        /// </summary>
        /// <returns>行容量最小値</returns>
        public new int GetMinRowCapacity();

        /// <summary>
        ///     列容量最大値を返す。
        /// </summary>
        /// <returns>列容量最大値</returns>
        public new int GetMaxColumnCapacity();

        /// <summary>
        ///     列容量最小値を返す。
        /// </summary>
        /// <returns>列容量最小値</returns>
        public new int GetMinColumnCapacity();

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは行要素を、内側シーケンスは列要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="RowCount"/>)] 行数</param>
        /// <returns>指定行範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="rowCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public new IEnumerable<IEnumerable<T>> GetRow(int rowIndex, int rowCount);

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ColumnCount"/>)] 列数</param>
        /// <returns>指定列範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="columnCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public new IEnumerable<IEnumerable<T>> GetColumn(int columnIndex, int columnCount);

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="RowCount"/>)] 行数</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ColumnCount"/>)] 列数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="columnCount"/>,
        ///     <paramref name="rowIndex"/>, <paramref name="rowCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public new IEnumerable<IEnumerable<T>> GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount);

        /// <summary>
        ///     リストの連続した行要素を更新する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 更新開始行インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="ColumnCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public new void SetRow(int rowIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     リストの連続した列要素を更新する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 更新開始列インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="RowCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public new void SetColumn(int columnIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     行の末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxRowCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="ColumnCount"/> と異なる場合。
        /// </exception>
        public new void AddRow(params IEnumerable<T>[] items);

        /// <summary>
        ///     列の末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxColumnCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="ColumnCount"/> と異なる場合。
        /// </exception>
        public new void AddColumn(params IEnumerable<T>[] items);

        /// <summary>
        ///     指定した行インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/>)] 行インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxRowCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="ColumnCount"/> と異なる場合。
        /// </exception>
        public new void InsertRow(int rowIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     指定した列インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxColumnCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="ColumnCount"/> と異なる場合。
        /// </exception>
        public new void InsertColumn(int columnIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     指定した行インデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IExtendedList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/>)] 行インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を超える場合、
        ///     または <paramref name="items"/> のいずれかの要素の要素数が
        ///     <see cref="ColumnCount"/> と異なる場合。
        /// </exception>
        public new void OverwriteRow(int rowIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     指定した列インデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IExtendedList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって列数が <see cref="GetMaxColumnCapacity"/> を超える場合、
        ///     または <paramref name="items"/> のいずれかの要素の要素数が
        ///     <see cref="ColumnCount"/> と異なる場合。
        /// </exception>
        public new void OverwriteColumn(int columnIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     指定した行番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRowIndex">
        ///     [Range(0, <see cref="RowCount"/> - 1)]
        ///     移動する項目の行番号開始位置
        /// </param>
        /// <param name="newRowIndex">
        ///     [Range(0, <see cref="RowCount"/> - 1)]
        ///     移動先の行番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="RowCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRowIndex"/>, <paramref name="newRowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public new void MoveRow(int oldRowIndex, int newRowIndex, int count = 1);

        /// <summary>
        ///     指定した列番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumnIndex">
        ///     [Range(0, <see cref="ColumnCount"/> - 1)]
        ///     移動する項目の列番号開始位置
        /// </param>
        /// <param name="newColumnIndex">
        ///     [Range(0, <see cref="ColumnCount"/> - 1)]
        ///     移動先の列番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ColumnCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumnIndex"/>, <paramref name="newColumnIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public new void MoveColumn(int oldColumnIndex, int newColumnIndex, int count = 1);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="RowCount"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合、
        ///     または 操作によって要素数が <see cref="GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public new void RemoveRow(int rowIndex, int count = 1);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ColumnCount"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合、
        ///     または 操作によって要素数が <see cref="GetMinColumnCapacity"/> を下回る場合。
        /// </exception>
        public new void RemoveColumn(int columnIndex, int count = 1);

        /// <summary>
        ///     行数および列数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>,
        ///     <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="GetMinColumnCapacity"/>,
        ///     <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせ、
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>,
        ///     <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="GetMinColumnCapacity"/>,
        ///     <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustLengthIfShort(int rowLength, int columnLength);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせ、
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>,
        ///     <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="GetMinColumnCapacity"/>,
        ///     <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustLengthIfLong(int rowLength, int columnLength);

        /// <summary>
        ///     行数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustRowLength(int rowLength);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustRowLengthIfShort(int rowLength);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustRowLengthIfLong(int rowLength);

        /// <summary>
        ///     列数を指定の数に合わせる。
        /// </summary>
        /// <param name="columnLength">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustColumnLength(int columnLength);

        /// <summary>
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="columnLength">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustColumnLengthIfShort(int columnLength);

        /// <summary>
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="columnLength">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public new void AdjustColumnLengthIfLong(int columnLength);

        /// <summary>
        ///     すべての要素を初期化する。
        /// </summary>
        /// <remarks>
        ///     既存の要素はすべて除去され、<see cref="GetMinRowCapacity"/> 行 <see cref="GetMinColumnCapacity"/> 列の
        ///     新たなデフォルト要素が編集される。
        /// </remarks>
        public new void Reset();

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/>中に<see langword="null"/>要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> のすべての要素の要素数が統一されていない場合。
        /// </exception>
        public new void Reset(IEnumerable<IEnumerable<T>> initItems);

        /// <summary>
        ///     自分自身を初期化する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         既存の要素はすべて除去され、
        ///         <typeparam name="T">
        ///             のデフォルト要素で埋められた
        ///         </typeparam>
        ///         <see cref="GetMinRowCapacity"/> 行
        ///         <see cref="GetMinColumnCapacity"/> 列の
        ///         二次元リストとなる。
        ///     </para>
        /// </remarks>
        public new void Clear();

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public new bool ItemEquals(IEnumerable<IEnumerable<T>>? other,
            IEqualityComparer<T>? itemComparer = null);

        /// <summary>
        ///     自身を行数変更可能型にキャストする。
        /// </summary>
        /// <returns>自分自身</returns>
        public new ITwoDimensionalList<T> AsRowSizeChangeableList();

        /// <summary>
        ///     自身を列数変更可能型にキャストする。
        /// </summary>
        /// <returns>自分自身</returns>
        public new ITwoDimensionalList<T> AsColumnSizeChangeableList();

        /// <summary>
        ///     自身を書き込み可能型にキャストする。
        /// </summary>
        /// <returns>自分自身</returns>
        public new ITwoDimensionalList<T> AsWritableList();

        /// <summary>
        ///     自身を読み取り可能型にキャストする。
        /// </summary>
        /// <returns>自分自身</returns>
        public new ITwoDimensionalList<T> AsReadableList();

        /// <summary>
        ///     自身の全要素を簡易コピーした二次元配列を返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         返す二次元配列の状態は <paramref name="isTranspose"/> によって変化する。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="false"/> の場合、
        ///         自身の要素をそのまま返す。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="true"/> の場合、
        ///         自身を転置した状態の要素を返す。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="isTranspose">転置フラグ</param>
        /// <returns>自身の全要素簡易コピー配列</returns>
        public new T[][] ToTwoDimensionalArray(bool isTranspose = false);
    }
}
