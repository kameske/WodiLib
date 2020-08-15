// ========================================
// Project Name : WodiLib
// File Name    : IDBItemSettingDesc.cs
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
    /// データベース設定値特殊指定インタフェース
    /// </summary>
    internal interface IDBItemSettingDesc : IModelBase<IDBItemSettingDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値特殊指定タイプ
        /// </summary>
        DBItemSpecialSettingType SettingType { get; }

        /// <summary>
        /// 特殊指定が「データベース参照」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        ISpecialDataSpecificationDatabaseReference DatabaseReferenceDesc { get; }

        /// <summary>
        /// 特殊指定が「ファイル読み込み」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        ISpecialDataSpecificationLoadFile LoadFileDesc { get; }

        /// <summary>
        /// 特殊指定が「手動設定」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「手動設定」以外の場合</exception>
        ISpecialDataSpecificationCreateOptions ManualDesc { get; }

        /// <summary>
        /// 特殊指定が「特殊な設定方法を使用しない」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「特殊な設定方法を使用しない」以外の場合</exception>
        ISpecialDataSpecificationNormal NormalDesc { get; }

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        DBItemType DefaultType { get; }

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        IReadOnlyDatabaseValueCaseList ArgCaseList { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        IEnumerable<DatabaseValueCase> GetAllSpecialCase();

        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        IEnumerable<DatabaseValueCaseNumber> GetAllSpecialCaseNumber();

        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        IEnumerable<DatabaseValueCaseDescription> GetAllSpecialCaseDescription();

        /// <summary>
        /// 指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">[NotNull] 値種別</param>
        /// <returns>設定可能な場合true</returns>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        bool CanSetItemType(DBItemType type);
    }
}
