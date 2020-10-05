// ========================================
// Project Name : WodiLib
// File Name    : DBItemValuesListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Database
{
    /// <summary>
    /// DBItemValuesList バリデーション Helper クラス
    /// </summary>
    internal static class DBItemValuesListValidationHelper
    {
        /// <summary>
        /// 基準となる DBItemValuesList インスタンスの項目型情報と
        /// 指定した DBItemValueList の項目型情報が一致するか検証する。
        /// </summary>
        /// <param name="parent">基準となる DBItemValuesList インスタンス</param>
        /// <param name="checkList">チェック対象リスト</param>
        /// <exception cref="ArgumentException">
        ///     parent の項目数と checkList の項目数が異なる場合、
        ///     または parent の項目型情報と checkList の項目型情報が異なる場合
        /// </exception>
        public static void ValidateListItem(DBItemValuesList parent, IEnumerable<DBItemValue> checkList)
        {
            // 基準データがない場合はcheckList自身が基準となるためチェックしない
            if (parent.Count == 0) return;

            var baseList = parent[0];
            var items = checkList.ToList();

            // 項目数チェック
            if (baseList.Count != items.Count)
                throw new ArgumentException(
                    $"{nameof(checkList)}の項目数が異なります。");

            // 型情報チェック
            var searchError = items.Where((t, i) => t.Type != baseList[i].Type).Any();
            if (searchError)
                throw new ArgumentException(
                    $"{nameof(checkList)}中に種類の異なる項目があります。");
        }
    }
}
