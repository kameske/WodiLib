// ========================================
// Project Name : WodiLib
// File Name    : DBItemValueListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBItemValueList の項目種別を比較するためのHelperクラス
    /// </summary>
    internal static class DBItemValueListValidationHelper
    {
        /// <summary>
        /// DBItemValueList インスタンス同士の項目種別を比較する。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <exception cref="InvalidOperationException">値型情報が一致しない場合</exception>
        public static void ItemTypeIsSame(IFixedLengthDBItemValueList left, IFixedLengthDBItemValueList right)
        {
            if (left.Count != right.Count) ThrowItemTypeIsNotSameException();

            var leftItemTypes = left.Select(x => x.Type);
            var rightItemTypes = right.Select(x => x.Type);

            var isSame = leftItemTypes.SequenceEqual(rightItemTypes);
            if (!isSame) ThrowItemTypeIsNotSameException();
        }

        /// <summary>
        /// DatabaseDataDesc インスタンス同士の項目種別を比較する。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <exception cref="InvalidOperationException">値型情報が一致しない場合</exception>
        public static void ItemTypeIsSame(DatabaseDataDesc left, DatabaseDataDesc right)
            => ItemTypeIsSame(left.ItemValueList, right.ItemValueList);

        /// <summary>
        /// 値型情報が一致しない場合の例外を発生させる
        /// </summary>
        /// <exception cref="InvalidOperationException">値型情報が一致しない場合の例外。必ず発生する。</exception>
        [DoesNotReturn]
        private static void ThrowItemTypeIsNotSameException()
        {
            throw new InvalidOperationException(
                ErrorMessage.NotEqual($"{nameof(DatabaseDataDescList)}の値型情報", "セットしようとした要素の値型情報"));
        }
    }
}
