// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量制限のある二次元リスト基底クラス
    /// </summary>
    /// <remarks>
    /// 外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    /// すべての行について列数は常に同じ値を取り続ける。<br/>
    /// 行数、列数ともに制限が設けられている。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public abstract class RestrictedCapacityTwoDimensionalList<T> : TwoDimensionalListBase<T>,
        IRestrictedCapacityTwoDimensionalList<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="ITwoDimensionalList{T}.this" />
        public new T this[int row, int column]
        {
            get => Get_Impl(row, 1, column, 1).First().First();
            set => Set_Impl(row, column, new[] {new[] {value}});
        }

        /// <inheritdoc />
        public override int RowCount => Items.Count;

        /// <inheritdoc />
        public override int ColumnCount
        {
            get
            {
                if (Items.Count == 0) return 0;
                return Items[0].Count;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト実体</summary>
        protected virtual List<List<T>> Items { get; } = new List<List<T>>();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected RestrictedCapacityTwoDimensionalList()
        {
        }

        /// <summary>
        /// コンストラクタ（初期値指定）
        /// </summary>
        /// <param name="initItems">初期要素</param>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">initItemsの要素数が不適切な場合</exception>
        /// <exception cref="ArgumentException">
        ///     initItems中に要素数の異なるリストがある場合
        /// </exception>
        protected RestrictedCapacityTwoDimensionalList(IEnumerable<IEnumerable<T>> initItems) : base(initItems)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public abstract int GetMaxRowCapacity();

        /// <inheritdoc />
        public abstract int GetMinRowCapacity();

        /// <inheritdoc />
        public abstract int GetMaxColumnCapacity();

        /// <inheritdoc />
        public abstract int GetMinColumnCapacity();

        /// <inheritdoc />
        public void AddRow(IEnumerable<T> rowItems)
            => InsertRow_Impl(RowCount, new[] {rowItems.ToArray()});

        /// <inheritdoc />
        public void AddColumn(IEnumerable<T> columnItems)
            => InsertColumn_Impl(ColumnCount, new[] {columnItems.ToArray()});

        /// <inheritdoc />
        public void AddRowRange(IEnumerable<IEnumerable<T>> items)
            => InsertRow_Impl(RowCount, items.ToTwoDimensionalArray());

        /// <inheritdoc />
        public void AddColumnRange(IEnumerable<IEnumerable<T>> items)
            => InsertColumn_Impl(ColumnCount, items.ToTwoDimensionalArray());

        /// <inheritdoc />
        public void InsertRow(int row, IEnumerable<T> rowItems)
            => InsertRow_Impl(row, new[] {rowItems.ToArray()});

        /// <inheritdoc />
        public void InsertColumn(int column, IEnumerable<T> columnItems)
            => InsertColumn_Impl(column, new[] {columnItems.ToArray()});

        /// <inheritdoc />
        public void InsertRowRange(int row, IEnumerable<IEnumerable<T>> items)
            => InsertRow_Impl(row, items.ToTwoDimensionalArray());

        /// <inheritdoc />
        public void InsertColumnRange(int column, IEnumerable<IEnumerable<T>> items)
            => InsertColumn_Impl(column, items.ToTwoDimensionalArray());

        /// <inheritdoc />
        public void OverwriteRow(int row, IEnumerable<IEnumerable<T>> items)
            => OverwriteRow_Impl(row, items.ToTwoDimensionalArray());

        /// <inheritdoc />
        public void OverwriteColumn(int column, IEnumerable<IEnumerable<T>> items)
            => OverwriteColumn_Impl(column, items.ToTwoDimensionalArray());

        /// <inheritdoc />
        public void MoveRow(int oldRow, int newRow)
            => MoveRow_Impl(oldRow, newRow, 1);

        /// <inheritdoc />
        public void MoveColumn(int oldColumn, int newColumn)
            => MoveColumn_Impl(oldColumn, newColumn, 1);

        /// <inheritdoc />
        public void MoveRowRange(int oldRow, int newRow, int count)
            => MoveRow_Impl(oldRow, newRow, count);

        /// <inheritdoc />
        public void MoveColumnRange(int oldColumn, int newColumn, int count)
            => MoveColumn_Impl(oldColumn, newColumn, count);

        /// <inheritdoc />
        public void RemoveRow(int row)
            => RemoveRow_Impl(row, 1);

        /// <inheritdoc />
        public void RemoveColumn(int column)
            => RemoveColumn_Impl(column, 1);

        /// <inheritdoc />
        public void RemoveRowRange(int row, int count)
            => RemoveRow_Impl(row, count);

        /// <inheritdoc />
        public void RemoveColumnRange(int column, int count)
            => RemoveColumn_Impl(column, count);

        /// <inheritdoc />
        public void AdjustLength(int rowLength, int columnLength)
            => AdjustLength_Impl(rowLength, columnLength);

        /// <inheritdoc />
        public void AdjustLengthIfShort(int rowLength, int columnLength)
            => AdjustLengthIfShort_Impl(rowLength, columnLength);

        /// <inheritdoc />
        public void AdjustLengthIfLong(int rowLength, int columnLength)
            => AdjustLengthIfLong_Impl(rowLength, columnLength);

        /// <inheritdoc />
        public void AdjustRowLength(int length)
            => AdjustRowLength_Impl(length);

        /// <inheritdoc />
        public void AdjustColumnLength(int length)
            => AdjustColumnLength_Impl(length);

        /// <inheritdoc />
        public void AdjustRowLengthIfShort(int length)
            => AdjustRowLengthIfShort_Impl(length);

        /// <inheritdoc />
        public void AdjustColumnLengthIfShort(int length)
            => AdjustColumnLengthIfShort_Impl(length);

        /// <inheritdoc />
        public void AdjustRowLengthIfLong(int length)
            => AdjustRowLengthIfLong_Impl(length);

        /// <inheritdoc />
        public void AdjustColumnLengthIfLong(int length)
            => AdjustColumnLengthIfLong_Impl(length);

        /// <inheritdoc />
        public void Clear()
            => Clear_Impl();

        /// <inheritdoc />
        public void Reset(IEnumerable<IEnumerable<T>> initItems)
            => Reset_Impl(initItems.ToTwoDimensionalArray());

        /// <inheritdoc />
        public override IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <inheritdoc />
        public bool ItemEquals(IRestrictedCapacityTwoDimensionalList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(other.AsEnumerable());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override ITwoDimensionalListValidator<T> MakeValidator()
        {
            return new RestrictedCapacityTwoDimensionalListValidator<T>(this);
        }

        #region Action Core

        /// <inheritdoc />
        protected override void Constructor_Core(params T[][] initItems)
        {
            Items.Clear();

            Items.AddRange(initItems.Select(row => row.ToList()));
        }

        /// <inheritdoc />
        protected override IEnumerable<IEnumerable<T>> Get_Core(int row, int rowCount,
            int column, int columnCount)
        {
            if (rowCount == 0) return new List<IEnumerable<T>>();

            return Items.GetRange(row, rowCount)
                .Select(innerItems => innerItems.GetRange(column, columnCount));
        }

        /// <inheritdoc />
        protected override IEnumerable<IEnumerable<T>> GetRow_Core(int row, int count)
        {
            if (count == 0) return new List<IEnumerable<T>>();

            return Items.GetRange(row, count);
        }

        /// <inheritdoc />
        protected override IEnumerable<IEnumerable<T>> GetColumn_Core(int column, int count)
        {
            if (count == 0) return Enumerable.Empty<IEnumerable<T>>();

            return Enumerable.Range(column, count).Select(j =>
                Enumerable.Range(0, RowCount).Select(i =>
                    Items[i][j]));
        }

        /// <inheritdoc />
        protected override void Set_Core(int row, int column, T[][] items)
        {
            items.ForEach((rowItems, rIdx) =>
            {
                rowItems.ForEach((item, cIdx) => { Items[row + rIdx][column + cIdx] = item; });
            });
        }

        /// <inheritdoc />
        protected override void InsertRow_Core(int row, T[][] items)
        {
            if (IsEmpty)
            {
                Reset_Core(items);
            }
            else
            {
                Items.InsertRange(row, items.Select(line => line.ToList()));
            }
        }

        /// <inheritdoc />
        protected override void InsertColumn_Core(int column, T[][] items)
        {
            var transposedItems = items.ToTransposedArray();
            if (IsEmpty)
            {
                Reset_Core(transposedItems);
            }
            else
            {
                transposedItems.ForEach((line, i) => { Items[i].InsertRange(column, line); });
            }
        }

        /// <inheritdoc />
        protected override void MoveRow_Core(int oldRow, int newRow, int count)
        {
            var moveItems = Items.GetRange(oldRow, count);
            Items.RemoveRange(oldRow, count);
            Items.InsertRange(newRow, moveItems);
        }

        /// <inheritdoc />
        protected override void MoveColumn_Core(int oldColumn, int newColumn, int count)
        {
            Items.ForEach(line =>
            {
                var moveItems = line.GetRange(oldColumn, count);
                line.RemoveRange(oldColumn, count);
                line.InsertRange(newColumn, moveItems);
            });
        }

        /// <inheritdoc />
        protected override void RemoveRow_Core(int row, int count)
        {
            var removedRowCount = RowCount - count;
            if (removedRowCount == 0)
            {
                Items.Clear();
            }
            else
            {
                Items.RemoveRange(row, count);
            }
        }

        /// <inheritdoc />
        protected override void RemoveColumn_Core(int column, int count)
        {
            Items.ForEach(line => { line.RemoveRange(column, count); });
        }

        /// <inheritdoc />
        protected override void Reset_Core(T[][] items)
        {
            if (items.Length == 0)
            {
                Items.Clear();
                return;
            }

            Items.Clear();
            Items.AddRange(items.Select(line => new List<T>(line)));
        }

        /// <inheritdoc />
        protected override void Clear_Core()
        {
            Items.Clear();
        }

        #endregion

        /// <inheritdoc />
        protected override T[][] MakeInitItems()
        {
            if (GetMinRowCapacity() == 0 || GetMinColumnCapacity() == 0)
            {
                return Array.Empty<T[]>();
            }

            return Enumerable.Range(0, GetMinRowCapacity()).Select(i =>
                Enumerable.Range(0, GetMinColumnCapacity()).Select(j =>
                    MakeDefaultItem(i, j)).ToArray()
            ).ToArray();
        }
    }
}
