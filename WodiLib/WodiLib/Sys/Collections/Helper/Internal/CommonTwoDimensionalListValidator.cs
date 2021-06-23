// ========================================
// Project Name : WodiLib
// File Name    : CommonTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="T">リスト内包型</typeparam>
    internal class CommonTwoDimensionalListValidator<T> : WodiLibTwoDimensionalListValidator<T>
    {
        private static Direction[] OneSideDirections { get; }
            =
            {
                Direction.Row,
                Direction.Column,
            };

        private static IEnumerable<IntOrStr> OneSideDirectionsStrings { get; }
            = OneSideDirections.Select(x => new IntOrStr(x.Id));

        protected override ITwoDimensionalListValidator<T>? BaseValidator => null;

        private string RowIndexName { get; }
        private string ColumnIndexName { get; }

        public CommonTwoDimensionalListValidator(IReadOnlyTwoDimensionalList<T> target,
            string rowName = "行", string rowIndexName = "row",
            string columnName = "列", string columnIndexName = "column") : base(target, rowName, columnName)
        {
            RowIndexName = rowIndexName;
            ColumnIndexName = columnIndexName;
        }

        public override void Constructor(T[][] initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            TwoDimensionalListValidationHelper.ItemNotNull(initItems);
            TwoDimensionalListValidationHelper.InnerItemLength(initItems);
        }

        public override void CopyTo(IEnumerable<T>[] array, int index)
        {
            ThrowHelper.ValidateArgumentNotNull(array is null, nameof(array));

            ListValidationHelper.CopyTo(array.Length, index, Target.Count);
        }

        public override void CopyTo(T[] array, int index, Direction direction)
        {
            ThrowHelper.ValidateArgumentNotNull(array is null, nameof(array));
            ThrowHelper.ValidateArgumentNotNull(direction is null, nameof(direction));

            ThrowHelper.ValidateArgumentNotMatch(!OneSideDirections.Contains(direction),
                nameof(direction), OneSideDirectionsStrings);
            ListValidationHelper.CopyTo(array.Length, index, Target.AllCount);
        }

        public override void CopyTo(T[,] array, int row, int column)
        {
            ThrowHelper.ValidateArgumentNotNull(array is null, nameof(array));

            ListValidationHelper.CopyTo(array.GetLength(0), row, Target.Count);
            ListValidationHelper.CopyTo(array.GetLength(1), column, Target.ItemCount);
        }

        public override void CopyTo(T[][] array, int row, int column)
        {
            ThrowHelper.ValidateArgumentNotNull(array is null, nameof(array));

            ListValidationHelper.CopyTo(array.Length, row, Target.Count);
            var targetItemCount = Target.ItemCount;
            foreach (var arr in array)
            {
                ListValidationHelper.CopyTo(arr.Length, column, targetItemCount);
            }
        }

        public override void Get(int row, int rowCount, int column, int columnCount, Direction direction)
        {
            ThrowHelper.ValidateArgumentNotNull(direction is null, nameof(direction));
            ListValidationHelper.SelectIndex(row, Target.Count, nameof(row));
            ListValidationHelper.Count(rowCount, Target.Count, nameof(rowCount));
            ListValidationHelper.Range(row, rowCount, Target.Count, nameof(row), nameof(rowCount));
            ListValidationHelper.SelectIndex(column, Target.ItemCount, nameof(column));
            ListValidationHelper.Count(columnCount, Target.ItemCount, nameof(columnCount));
            ListValidationHelper.Range(column, columnCount, Target.ItemCount, nameof(column), nameof(columnCount));
        }

        public override void Set(int row, int column, T[][] items, Direction direction, bool needFitItemsInnerSize)
        {
            ListValidationHelper.SelectIndex(row, Target.Count, nameof(row));

            ListValidationHelper.SelectIndex(column, Target.ItemCount, nameof(column));

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            TwoDimensionalListValidationHelper.InnerItemLength(items);

            ThrowHelper.ValidateArgumentNotNull(direction is null, nameof(direction));

            var outerLength = items.Length;
            var innerLength = items.GetInnerArrayLength();
            if (outerLength > 1 || innerLength > 1)
            {
                ThrowHelper.ValidateArgumentNotMatch(!OneSideDirections.Contains(direction),
                    nameof(direction), OneSideDirectionsStrings);
            }

            if (direction == Direction.Row || direction == Direction.None)
            {
                ListValidationHelper.Range(row, items.Length, Target.Count);
            }
            else
            {
                // direction == Column
                ListValidationHelper.Range(column, items.Length, Target.ItemCount);
            }

            if (needFitItemsInnerSize)
            {
                var itemsInnerLength = items.GetInnerArrayLength();
                TwoDimensionalListValidationHelper.SizeEqual(itemsInnerLength,
                    DetermineSecondCount(direction), countName: DetermineSecondIndexName(direction));
            }
        }

        public override void Insert(int index, T[][] items, Direction direction)
        {
            ThrowHelper.ValidateArgumentNotNull(direction is null, nameof(direction));
            ThrowHelper.ValidateArgumentNotMatch(!OneSideDirections.Contains(direction),
                nameof(direction), OneSideDirectionsStrings);

            var firstCount = DetermineFirstCount(direction);
            ListValidationHelper.InsertIndex(index, firstCount,
                DetermineFirstIndexName(direction));

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            TwoDimensionalListValidationHelper.InnerItemLength(items);
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(items[0].Length,
                    direction == Direction.Row ? Target.ItemCount : Target.Count,
                    countName: DetermineFirstIndexName(direction));
            }
        }

        public override void Overwrite(int index, T[][] items, Direction direction)
        {
            ThrowHelper.ValidateArgumentNotNull(direction is null, nameof(direction));
            ThrowHelper.ValidateArgumentNotMatch(!OneSideDirections.Contains(direction),
                nameof(direction), OneSideDirectionsStrings);

            ListValidationHelper.InsertIndex(index, DetermineFirstCount(direction),
                DetermineFirstIndexName(direction));

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            TwoDimensionalListValidationHelper.InnerItemLength(items);
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(items[0].Length,
                    direction == Direction.Row ? Target.ItemCount : Target.Count,
                    countName: DetermineFirstIndexName(direction));
            }
        }

        public override void Move(int oldIndex, int newIndex, int count, Direction direction)
        {
            ThrowHelper.ValidateArgumentNotNull(direction is null, nameof(direction));

            var firstCount = DetermineFirstCount(direction);

            TwoDimensionalListValidationHelper.LengthNotZero(firstCount, direction);
            ListValidationHelper.SelectIndex(oldIndex, firstCount,
                $"old{DetermineFirstIndexName(direction, true)}");
            ListValidationHelper.InsertIndex(newIndex, firstCount,
                $"new{DetermineFirstIndexName(direction, true)}");
            ListValidationHelper.Count(count, firstCount);
            ListValidationHelper.Range(oldIndex, count, firstCount,
                $"old{DetermineFirstIndexName(direction, true)}");
            ListValidationHelper.Range(count, newIndex, firstCount,
                nameof(count), $"new{DetermineFirstIndexName(direction, true)}");
        }

        public override void Remove(int index, int count, Direction direction)
        {
            ThrowHelper.ValidateArgumentNotNull(direction is null, nameof(direction));

            var firstCount = DetermineFirstCount(direction);
            var indexName = DetermineFirstIndexName(direction);

            ValidateTargetIsEmpty();
            ListValidationHelper.SelectIndex(index, firstCount, indexName);
            ListValidationHelper.Count(count, firstCount);
            ListValidationHelper.Range(index, count, firstCount, indexName);
        }

        public override void AdjustLength(int rowLength, int columnLength)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(rowLength < 0,
                nameof(rowLength), 0, rowLength);
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(columnLength < 0,
                nameof(columnLength), 0, columnLength);

            if (rowLength > 0) return;
            // rowLength == 0

            if (columnLength == 0) return;

            // rowLength == 0 && columnLength != 0 は不正な指定
            ThrowHelper.ValidateArgumentUnsuitable(true,
                $"{RowName}数および{ColumnName}数の指定", $"{RowName}数 == 0 かつ {ColumnName}数 != 0 の指定はできません");
        }

        public override void Reset(T[][] items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            TwoDimensionalListValidationHelper.ItemNotNull(items);
            TwoDimensionalListValidationHelper.InnerItemLength(items);
        }

        public override ITwoDimensionalListValidator<T> CreateAnotherFor(
            IReadOnlyTwoDimensionalList<T> target)
            => new CommonTwoDimensionalListValidator<T>(target, RowName, RowIndexName, ColumnName, ColumnIndexName);

        private int DetermineFirstCount(Direction direction)
        {
            return direction == Direction.Row
                ? Target.Count
                : Target.ItemCount;
        }

        private int DetermineSecondCount(Direction direction)
        {
            return direction == Direction.Row
                ? Target.ItemCount
                : Target.Count;
        }

        private string DetermineFirstIndexName(Direction direction, bool isUpperFirstChar = false)
        {
            var result = direction == Direction.Row
                ? RowIndexName
                : ColumnIndexName;

            if (!isUpperFirstChar) return result;
            return result.Length switch
            {
                0 => result,
                1 => result.ToUpper(),
                _ => result.Substring(0, 1).ToUpper() + result.Substring(1)
            };
        }

        private string DetermineSecondIndexName(Direction direction, bool isUpperFirstChar = false)
        {
            var result = direction == Direction.Row
                ? RowIndexName
                : ColumnIndexName;

            if (!isUpperFirstChar) return result;
            return result.Length switch
            {
                0 => result,
                1 => result.ToUpper(),
                _ => result.Substring(0, 1).ToUpper() + result.Substring(1)
            };
        }

        private void ValidateTargetIsEmpty()
        {
            ThrowHelper.InvalidOperationIf(Target.IsEmpty,
                () => ErrorMessage.NotExecute("空リストのため"));
        }

        private static void ValidateItemsIsEmpty(IReadOnlyCollection<T[]> items, string itemsName)
        {
            ThrowHelper.InvalidOperationIf(items.Count == 0,
                () => ErrorMessage.NotExecute($"{itemsName} の要素数が 0 のため"));
        }
    }
}
