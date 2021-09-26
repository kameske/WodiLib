// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventArgsFactory.OverwriteArgsFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalCollectionChangeEventArgsFactory<T>
    {
        private static class OverwriteArgsFactory
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSingle(
                Direction execDirection, Direction notifyArgsDirection,
                T[][] target, int index, T[][] replaceNewItems, T[][] addItems)
            {
                /*
                 * 引数について
                 *      target: [行][列] の配置で渡ってくる
                 *      replaceOldItems, replaceNewItems, addItems: 転置不要な場合に
                 *          Overwrite パラメータとしてそのまま使用可能な配置
                 *          転置が必要な場合は相応の加工が必要
                 */
                var oldItems = target.ToTwoDimensionalArray();
                var needTranspose = NeedTranspose(execDirection, notifyArgsDirection);

                /*
                 * 転置が必要な場合は必ずReset
                 * 不要な場合は Add / Replace / Reset
                 */

                var row = execDirection != Direction.Column ? index : 0;
                var column = execDirection == Direction.Column ? index : 0;

                if (needTranspose)
                {
                    var fixedOldItems = oldItems.ToTransposedArray();

                    var fixedNewItems = AcquireOverwriteTransposedItems(target, index, replaceNewItems, addItems);

                    return new[]
                    {
                        TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                            fixedOldItems, fixedNewItems, row, column,
                            notifyArgsDirection)
                    };
                }

                // needTranspose == false

                var isReplace = replaceNewItems.Length > 0;
                var isAdd = addItems.Length > 0;

                if (!isReplace && !isAdd)
                {
                    // 通常ここへは来ない
                    throw new InvalidOperationException();
                }

                var replaceLength = replaceNewItems.Length;
                var replaceOldItems = target.ToTransposedArrayIf(NeedTranspose(execDirection, Direction.Row))
                    .Skip(index).Take(replaceLength).ToArray();

                var result = new List<TwoDimensionalCollectionChangeEventInternalArgs<T>>();

                if (isReplace)
                {
                    result.AddRange(SetArgsFactory.CreateSingle(execDirection, row, column, replaceOldItems,
                        replaceNewItems));
                }

                if (isAdd)
                {
                    result.AddRange(AddArgsFactory.CreateSingle(execDirection, index + replaceLength, addItems));
                }

                return result;
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateLine(
                Direction execDirection, Direction notifyArgsDirection,
                T[][] target, int index,
                T[][] replaceOldItems, T[][] replaceNewItems, T[][] addItems)
            {
                var needTranspose = NeedTranspose(execDirection, notifyArgsDirection);

                var (fixedReplaceOldItems, fixedReplaceNewItems, fixedAddItems, row, column) =
                    needTranspose
                        ? ((Func<(T[][], T[][], T[][], int, int)>)(() =>
                        {
                            var repLength = replaceNewItems.Length;
                            var repOldItems =
                                repLength == 0
                                    ? Array.Empty<T[]>()
                                    : target.ToTransposedArrayIf(NeedTranspose(execDirection, Direction.Column))
                                        .Select(line => line.Range(index, repLength))
                                        .ToTwoDimensionalArray();

                            var transposedNewItems = replaceNewItems.ToTransposedArray();
                            var transposedAddItems = addItems.ToTransposedArray();

                            return (repOldItems, transposedNewItems, transposedAddItems,
                                execDirection != Direction.Column ? index : 0,
                                execDirection == Direction.Column ? index : 0);
                        }))()
                        : (replaceOldItems, replaceNewItems, addItems,
                            execDirection != Direction.Column ? index : 0,
                            execDirection == Direction.Column ? index : 0);

                var executeRowIndexCorrect = execDirection == Direction.Row ? 1 : 0;
                var executeColIndexCorrect = execDirection == Direction.Column ? 1 : 0;
                var notifyRowIndexCorrect = notifyArgsDirection == Direction.Row ? 1 : 0;
                var notifyColIndexCorrect = notifyArgsDirection == Direction.Column ? 1 : 0;

                var replaceArgs = fixedReplaceOldItems.Zip(fixedReplaceNewItems)
                    .Select((zip, idx) => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateSetArgs(
                        row + idx * notifyRowIndexCorrect, column + idx * notifyColIndexCorrect,
                        zip.Item1, zip.Item2, notifyArgsDirection));
                var replaceLength =
                    needTranspose ? fixedReplaceOldItems.GetInnerArrayLength() : fixedReplaceOldItems.Length;
                var addArgs = fixedAddItems.Select((line, idx)
                    => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateAddArgs(
                        row + replaceLength * executeRowIndexCorrect + idx * notifyRowIndexCorrect,
                        column + replaceLength * executeColIndexCorrect + idx * notifyColIndexCorrect,
                        line, notifyArgsDirection));

                return replaceArgs.Concat(addArgs);
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateMulti(
                Direction execDirection, int index, T[][] replaceOldItems, T[][] replaceNewItems, T[][] addItems)
            {
                var row = execDirection != Direction.Column ? index : 0;
                var column = execDirection == Direction.Column ? index : 0;

                var replaceArgs =
                    SetArgsFactory.CreateMulti(execDirection, row, column, replaceOldItems, replaceNewItems);
                var addArgs = AddArgsFactory.CreateMulti(execDirection, index + replaceOldItems.Length, addItems);

                return replaceArgs.Concat(addArgs);
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateReset(
                Direction execDirection, int index, T[][] replaceOldItems, IEnumerable<T[]> replaceNewItems,
                IEnumerable<T[]> addItems)
            {
                var oldItems = replaceOldItems;
                var newItems = replaceNewItems.Concat(addItems).ToArray();

                var row = execDirection == Direction.Row ? index : 0;
                var column = execDirection == Direction.Column ? index : 0;

                return new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(oldItems, newItems, row, column,
                        execDirection)
                };
            }

            private static T[][] AcquireOverwriteTransposedItems(T[][] target,
                int index, IEnumerable<T[]> replaceItems, IEnumerable<T[]> addItems)
            {
                // パラメータの転置必要
                /*
                 * SimpleList を用いて現在の方向で Overwrite を実行し、その結果を転置することで
                 * 結果を取得する
                 */
                var list = new SimpleList<T[]>(target);

                var overwriteItems = replaceItems.Concat(addItems).ToArray();
                var overwriteParam = OverwriteParam<T[]>.Factory.Create(target, index, overwriteItems);
                list.Overwrite(index, overwriteParam);

                var transposed = list.ToTransposedArray();
                return transposed;
            }
        }
    }
}
