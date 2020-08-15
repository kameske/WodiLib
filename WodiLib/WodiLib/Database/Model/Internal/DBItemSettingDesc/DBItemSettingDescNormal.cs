// ========================================
// Project Name : WodiLib
// File Name    : DBItemSettingDescNormal.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// データベース設定値特殊指定・特殊な指定方法を使用しない
    /// </summary>
    [Serializable]
    internal class DBItemSettingDescNormal : DBItemSettingDescBase, IEquatable<DBItemSettingDescNormal>,
        ISpecialDataSpecificationNormal
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 特殊指定が「特殊な設定方法を使用しない」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「特殊な設定方法を使用しない」以外の場合</exception>
        public override ISpecialDataSpecificationNormal NormalDesc => this;

        /// <inheritdoc />
        /// <summary>
        /// 値特殊指定タイプ
        /// </summary>
        public override DBItemSpecialSettingType SettingType => DBItemSpecialSettingType.Normal;

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        public override DBItemType DefaultType => DBItemType.Int;

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        public override IReadOnlyDatabaseValueCaseList ArgCaseList { get; }
            = new DatabaseValueCaseList();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override IEnumerable<DatabaseValueCase> GetAllSpecialCase()
        {
            return ArgCaseList;
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override IEnumerable<DatabaseValueCaseNumber> GetAllSpecialCaseNumber()
        {
            return ArgCaseList.Select(x => x.CaseNumber)
                .ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override IEnumerable<DatabaseValueCaseDescription> GetAllSpecialCaseDescription()
        {
            return ArgCaseList.Select(x => x.Description)
                .ToList();
        }

        /// <summary>
        /// 指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">[NotNull] 値種別</param>
        /// <returns>設定可能な場合true</returns>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public override bool CanSetItemType(DBItemType type)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            return type == DBItemType.Int
                   || type == DBItemType.String;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(IDBItemSettingDesc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is DBItemSettingDescNormal casted)) return false;

            return Equals(casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(DBItemSettingDescBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is DBItemSettingDescNormal casted)) return false;

            return Equals(casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBItemSettingDescNormal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return true;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ISpecialDataSpecificationNormal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return true;
        }
    }
}
