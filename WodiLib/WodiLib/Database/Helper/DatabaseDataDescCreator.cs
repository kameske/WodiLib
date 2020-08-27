// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataDescCreator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DatabaseDataDesc 生成クラス
    /// </summary>
    internal static class DatabaseDataDescCreator
    {
        /// <summary>
        /// DataNameList インスタンスと DBItemValuesList インスタンスから DatabaseDataDesc 列挙を生成する。
        /// </summary>
        /// <param name="dataNameList">データ名リスト</param>
        /// <param name="valuesList">値リスト</param>
        /// <exception cref="ArgumentNullException">dataNameList, valuesList が null の場合</exception>
        /// <exception cref="ArgumentException">dataNameListとvaluesListの要素数が異なる場合</exception>
        public static IEnumerable<DatabaseDataDesc> CreateEnumerableDatabaseDataDesc(DataNameList dataNameList,
            DBItemValuesList valuesList)
        {
            if (dataNameList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dataNameList)));
            if (valuesList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(valuesList)));

            if (dataNameList.Count != valuesList.Count)
                throw new ArgumentException(
                    $"{nameof(dataNameList)}の要素数と{nameof(valuesList)}の要素数が一致しません。");

            for (var i = 0; i < dataNameList.Count; i++)
            {
                yield return new DatabaseDataDesc(
                    dataNameList[i],
                    valuesList[i].ToLengthChangeableItemValueList());
            }
        }
    }
}
