// ========================================
// Project Name : WodiLib
// File Name    : DBItemSettingDescBase.cs
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
    /// データベース設定値特殊指定・基底クラス
    /// </summary>
    [Serializable]
    internal abstract class DBItemSettingDescBase : ModelBase<DBItemSettingDescBase>,
        IDBItemSettingDesc
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 値特殊指定タイプ
        /// </summary>
        public abstract DBItemSpecialSettingType SettingType { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Virtual Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 特殊指定が「データベース参照」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public virtual ISpecialDataSpecificationDatabaseReference DatabaseReferenceDesc =>
            throw new PropertyAccessException(
                "特殊指定が「データベース参照」ではないため取得できません");

        /// <summary>
        /// 特殊指定が「ファイル読み込み」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        public virtual ISpecialDataSpecificationLoadFile LoadFileDesc =>
            throw new PropertyAccessException(
                "特殊指定が「ファイル読み込み」ではないため取得できません");

        /// <summary>
        /// 特殊指定が「手動設定」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「手動設定」以外の場合</exception>
        public virtual ISpecialDataSpecificationCreateOptions ManualDesc =>
            throw new PropertyAccessException(
                "特殊指定が「手動設定」ではないため取得できません");

        /// <summary>
        /// 特殊指定が「特殊な設定方法を使用しない」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「特殊な設定方法を使用しない」以外の場合</exception>
        public virtual ISpecialDataSpecificationNormal NormalDesc =>
            throw new PropertyAccessException(
                "特殊指定が「特殊な設定方法を使用しない」ではないため取得できません");

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        public abstract DBItemType DefaultType { get; }

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        public abstract IReadOnlyDatabaseValueCaseList ArgCaseList { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public abstract IEnumerable<DatabaseValueCase> GetAllSpecialCase();

        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public abstract IEnumerable<DatabaseValueCaseNumber> GetAllSpecialCaseNumber();

        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public abstract IEnumerable<DatabaseValueCaseDescription> GetAllSpecialCaseDescription();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">[NotNull] 値種別</param>
        /// <returns>設定可能な場合true</returns>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public abstract bool CanSetItemType(DBItemType type);

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public abstract bool Equals(IDBItemSettingDesc other);
    }
}
