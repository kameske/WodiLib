// ========================================
// Project Name : WodiLib
// File Name    : ISpecialDataSpecificationCreateOptions.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目：データ内容の特殊設定＝「選択肢を手動作成」の場合の
    /// 特殊設定内容インタフェース
    /// </summary>
    public interface ISpecialDataSpecificationCreateOptions : IEquatable<ISpecialDataSpecificationCreateOptions>
    {
        /// <summary>
        /// すべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        IEnumerable<DatabaseValueCase> GetAllSpecialCase();

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCase">選択肢情報</param>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        void AddSpecialCase(DatabaseValueCase argCase);

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCases">選択肢</param>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        void AddSpecialCaseRange(IEnumerable<DatabaseValueCase> argCases);

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
        /// <param name="argCase">選択肢情報</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        void InsertSpecialCase(int index, DatabaseValueCase argCase);

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
        /// <param name="argCases">選択肢</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     argCasesがnullの場合、
        ///     またはargCasesにnull要素が含まれる場合
        /// </exception>
        void InsertSpecialCaseRange(int index, IEnumerable<DatabaseValueCase> argCases);

        /// <summary>
        /// 選択肢を更新する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="argCase">更新する選択肢内容</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        void UpdateManualSpecialCase(int index, DatabaseValueCase argCase);

        /// <summary>
        /// 選択肢を削除する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        void RemoveSpecialCaseAt(int index);

        /// <summary>
        /// 選択肢を範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
        /// <param name="count">[Range(0, ManualCaseLength)] 削除数</param>
        /// <exception cref="ArgumentOutOfRangeException">index、またはcountが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">リストの範囲を超えて削除しようとする場合</exception>
        void RemoveSpecialCaseRange(int index, int count);

        /// <summary>
        /// 選択肢をクリアする。
        /// </summary>
        void ClearSpecialCase();
    }
}
