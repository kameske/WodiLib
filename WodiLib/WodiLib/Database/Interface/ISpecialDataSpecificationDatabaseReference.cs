// ========================================
// Project Name : WodiLib
// File Name    : ISpecialDataSpecificationDatabaseReference.cs
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
    /// DB項目：データ内容の特殊設定＝「データベース参照」の場合の
    /// 特殊設定内容インタフェース
    /// </summary>
    public interface ISpecialDataSpecificationDatabaseReference : IEquatable<ISpecialDataSpecificationDatabaseReference>
    {
        /// <summary>
        /// DB種別
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        DBReferType DatabaseReferKind { get; set; }

        /// <summary>
        /// タイプID
        /// </summary>
        TypeId DatabaseDbTypeId { get; set; }

        /// <summary>
        /// 追加項目使用フラグ
        /// </summary>
        bool DatabaseUseAdditionalItemsFlag { get; set; }

        /// <summary>
        /// すべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        IEnumerable<DatabaseValueCase> GetAllSpecialCase();

        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        IEnumerable<DatabaseValueCaseDescription> GetAllSpecialCaseDescription();

        /// <summary>
        /// 追加選択肢文字列を更新する。
        /// </summary>
        /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
        /// <param name="description">[NotNewLine] 文字列</param>
        /// <exception cref="ArgumentOutOfRangeException">caseNumberが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">descriptionがnullの場合</exception>
        /// <exception cref="ArgumentNewLineException">descriptionが改行を含む場合</exception>
        void UpdateDatabaseSpecialCase(int caseNumber, DatabaseValueCaseDescription description);
    }
}
