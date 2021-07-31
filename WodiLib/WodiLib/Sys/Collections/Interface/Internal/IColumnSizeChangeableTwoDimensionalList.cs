// ========================================
// Project Name : WodiLib
// File Name    : IColumnSizeChangeableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     列サイズ変更可能であることを表すインタフェース。
    ///     書き込み可能でもある。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TWritable"><see cref="IWritableTwoDimensionalList{TItem,TImpl,TReadable}"/>実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableTwoDimensionalList{TItem,TImpl}"/>実装型</typeparam>
    internal interface IColumnSizeChangeableTwoDimensionalList<TItem, TImpl, out TWritable, out TReadable> :
        IEnumerable<IEnumerable<TItem>>, IEqualityComparable<TImpl>, INotifiableTwoDimensionalListChangeInternal<TItem>,
        IDeepCloneableTwoDimensionalListInternal<TImpl, TItem>
        where TImpl : IReadableTwoDimensionalList<TItem, TImpl>, IEqualityComparable<TImpl>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public TItem this[int rowIndex, int columnIndex] { get; }

        /// <summary>
        ///     空フラグ
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> == 0 かつ <see cref="ColumnCount"/> == 0 の場合に <see langword="true"/> を、
        ///     それ以外の場合に <see langword="false"/> を返す。
        /// </remarks>
        public bool IsEmpty { get; }

        /// <summary>行数</summary>
        public int RowCount { get; }

        /// <summary>列数</summary>
        public int ColumnCount { get; }

        /// <summary>
        ///     総数
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> * <see cref="ColumnCount"/> を返す。
        /// </remarks>
        public int AllCount { get; }

        /// <summary>
        ///     列容量最大値を返す。
        /// </summary>
        /// <returns>列容量最大値</returns>
        public int GetMaxColumnCapacity();

        /// <summary>
        ///     列容量最小値を返す。
        /// </summary>
        /// <returns>列容量最小値</returns>
        public int GetMinColumnCapacity();

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
        public IEnumerable<IEnumerable<TItem>> GetRow(int rowIndex, int rowCount);

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
        public IEnumerable<IEnumerable<TItem>> GetColumn(int columnIndex, int columnCount);

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
        public IEnumerable<IEnumerable<TItem>> GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount);

        /// <summary>
        ///     リストの連続した行要素を更新する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="IReadableTwoDimensionalList{TItem, TImpl}.RowCount"/> - 1)] 更新開始行インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem, TImpl}.ColumnCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public void SetRow(int rowIndex, params IEnumerable<TItem>[] items);

        /// <summary>
        ///     リストの連続した列要素を更新する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="IReadableTwoDimensionalList{TItem, TImpl}.ColumnCount"/> - 1)] 更新開始列インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem, TImpl}.RowCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public void SetColumn(int columnIndex, params IEnumerable<TItem>[] items);

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
        public void AddColumn(params IEnumerable<TItem>[] items);

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
        public void InsertColumn(int columnIndex, params IEnumerable<TItem>[] items);

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
        public void OverwriteColumn(int columnIndex, params IEnumerable<TItem>[] items);

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
        public void MoveRow(int oldRowIndex, int newRowIndex, int count = 1);

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
        public void MoveColumn(int oldColumnIndex, int newColumnIndex, int count = 1);

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
        public void RemoveColumn(int columnIndex, int count = 1);

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
        public void AdjustColumnLength(int columnLength);

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
        public void AdjustColumnLengthIfShort(int columnLength);

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
        public void AdjustColumnLengthIfLong(int columnLength);

        /// <summary>
        ///     すべての要素を初期化する。
        /// </summary>
        /// <remarks>
        ///     既存の要素はすべて除去され、<see cref="IReadableList{TItem, TImpl}.Count"/> 個の
        ///     新たなデフォルト要素が編集される。
        /// </remarks>
        public void Reset();

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<IEnumerable<TItem>>? other, IEqualityComparer<TItem>? itemComparer = null);

        /// <summary>
        ///     自身を書き込み可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TWritable"/> にキャストした自分自身</returns>
        public TWritable AsWritableList();

        /// <summary>
        ///     自身を読み取り可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TReadable"/> にキャストした自分自身</returns>
        public TReadable AsReadableList();

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
        public TItem[][] ToTwoDimensionalArray(bool isTranspose = false);
    }
}
