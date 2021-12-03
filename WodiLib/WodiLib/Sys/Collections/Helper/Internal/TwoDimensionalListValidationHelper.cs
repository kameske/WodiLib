// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト検証 Helper クラス
    /// </summary>
    internal static class TwoDimensionalListValidationHelper
    {
        /// <summary>
        ///     各要素が null でないことを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <param name="itemName">エラーメッセージ中の要素名</param>
        /// <typeparam name="T">リスト内包型</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="target"/> の行、または列に <see langword="true"/> 要素が存在する場合
        /// </exception>
        public static void ItemNotNull<T>(IEnumerable<IEnumerable<T>> target,
            string itemName = "initItems")
        {
            var targetArray = target.ToTwoDimensionalArray();

            ThrowHelper.ValidateArgumentItemsHasNotNull(targetArray.HasNullItem(), itemName);
            ThrowHelper.ValidateArgumentItemsHasNotNull(targetArray.Any(x => x.HasNullItem()), $"{itemName}の要素");
        }

        /// <summary>
        ///     各行の要素数が一致することを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <typeparam name="T">リスト内包型</typeparam>
        /// <exception cref="ArgumentException">
        ///     行数が2以上であり、かつ0行目の要素数と異なる行が存在する場合
        /// </exception>
        public static void InnerItemLength<T>(IEnumerable<IEnumerable<T>> target)
        {
            var targetArray = target.ToTwoDimensionalArray();

            if (targetArray.Length < 2) return;

            var baseLength = targetArray[0].Length;
            var errorRowIndex = targetArray.Skip(1)
                .FindIndex(x => x.Length != baseLength);
            ThrowHelper.ValidateTwoDimListInnerItemLength(errorRowIndex != -1, errorRowIndex);
        }

        /// <summary>
        ///     サイズが一致することを検証する。
        /// </summary>
        /// <param name="size">検証対象</param>
        /// <param name="count">一致すべき値</param>
        /// <param name="sizeName">エラーメッセージ中のサイズ名称</param>
        /// <param name="countName">エラーメッセージ中のカウント名称</param>
        /// <exception cref="ArgumentException"></exception>
        public static void SizeEqual(int size, int count, string sizeName = "size", string countName = "count")
        {
            ThrowHelper.ValidateArgumentNotEqual(size != count, sizeName, countName);
        }

        /// <summary>
        ///     リストの要素数が0でないことを検証する。
        /// </summary>
        /// <param name="listCount">要素数</param>
        /// <param name="lineName">行または列の名前</param>
        /// <exception cref="InvalidOperationException">
        ///     要素数が0の場合
        /// </exception>
        public static void LengthNotZero(int listCount, string lineName)
        {
            ThrowHelper.InvalidOperationIf(listCount == 0,
                () => ErrorMessage.NotExecute($"{lineName}数が0のため"));
        }
    }
}
