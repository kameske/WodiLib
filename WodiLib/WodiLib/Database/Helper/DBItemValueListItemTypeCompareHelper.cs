// ========================================
// Project Name : WodiLib
// File Name    : DBItemValueListItemTypeCompareHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;

namespace WodiLib.Database
{
    /// <summary>
    /// DBItemValueList の項目種別を比較するためのHelperクラス
    /// </summary>
    internal static class DBItemValueListItemTypeCompareHelper
    {
        /// <summary>
        /// DBItemValueList インスタンス同士の項目種別を比較する。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>すべての項目の型情報が一致する場合True</returns>
        public static bool Compare(IFixedLengthDBItemValueList left, IFixedLengthDBItemValueList right)
        {
            if (left.Count != right.Count) return false;

            var leftItemTypes = left.Select(x => x.Type);
            var rightItemTypes = right.Select(x => x.Type);

            return leftItemTypes.SequenceEqual(rightItemTypes);
        }

        /// <summary>
        /// DatabaseDataDesc インスタンス同士の項目種別を比較する。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>すべての項目の型情報が一致する場合True</returns>
        public static bool Compare(DatabaseDataDesc left, DatabaseDataDesc right)
            => Compare(left.ItemValueList, right.ItemValueList);
    }
}
