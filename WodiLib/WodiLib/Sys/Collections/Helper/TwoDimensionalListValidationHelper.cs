// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// 二次元リスト検証 Helper クラス
    /// </summary>
    internal static class TwoDimensionalListValidationHelper
    {
        /// <summary>
        /// 各要素が null でないことを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <param name="itemName">エラーメッセージ中の要素名</param>
        /// <typeparam name="T">リスト内包型</typeparam>
        /// <exception cref="ArgumentNullException">
        ///   target の行、または列に null 要素が存在する場合
        /// </exception>
        public static void ItemNotNull<T>(T[][] target, string itemName = "initItems")
        {
            if (target.HasNullItem())
            {
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(itemName));
            }

            if (target.Any(x => x.HasNullItem()))
            {
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList($"{itemName}の要素"));
            }
        }

        /// <summary>
        /// 各行の要素数が一致することを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <typeparam name="T">リスト内包型</typeparam>
        /// <exception cref="ArgumentException">
        ///     行数が2以上であり、かつ0行目の要素数と異なる行が存在する場合
        /// </exception>
        public static void InnerItemLength<T>(T[][] target)
        {
            if (target.Length < 2) return;

            var baseLength = target[0].Length;
            var errorRowIndex = target.Skip(1)
                .FindIndex(x => x.Length != baseLength);
            if (errorRowIndex != -1)
            {
                throw new ArgumentException(
                    $"{errorRowIndex}行目の要素数が基準要素数と異なります。");
            }
        }

        /// <summary>
        /// サイズが一致することを検証する。
        /// </summary>
        /// <param name="size">検証対象</param>
        /// <param name="count">一致すべき値</param>
        /// <param name="sizeName">エラーメッセージ中のサイズ名称</param>
        /// <param name="countName">エラーメッセージ中のカウント名称</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SizeEqual(int size, int count, string sizeName = "size", string countName = "count")
        {
            if (size != count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.NotEqual(sizeName, countName));
        }

        /// <summary>
        /// リストの要素数が0でないことを検証する。
        /// </summary>
        /// <param name="listCount">要素数</param>
        /// <param name="direction">操作方向</param>
        /// <exception cref="InvalidOperationException">
        ///     要素数が0の場合
        /// </exception>
        public static void LengthNotZero(int listCount, Direction direction)
        {
            if (listCount == 0)
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute($"{(direction == Direction.Row ? "行" : "列")}数が0のため"));
        }
    }
}
