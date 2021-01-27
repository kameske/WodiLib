// ========================================
// Project Name : WodiLib
// File Name    : DBItemSettingDescDatabase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// データベース設定値特殊指定・データベース参照
    /// </summary>
    [Serializable]
    internal class DBItemSettingDescDatabase : DBItemSettingDescBase, IEquatable<DBItemSettingDescDatabase>,
        ISerializable, ISpecialDataSpecificationDatabaseReference
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値特殊指定タイプ
        /// </summary>
        public override DBItemSpecialSettingType SettingType => DBItemSpecialSettingType.ReferDatabase;

        /// <summary>
        /// 特殊指定が「データベース参照」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public override ISpecialDataSpecificationDatabaseReference DatabaseReferenceDesc => this;

        private DBReferType databaseDbKind = DBReferType.System;

        /// <summary>
        /// DB参照時のDB種別
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public DBReferType DatabaseReferKind
        {
            get => databaseDbKind;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DatabaseReferKind)));

                databaseDbKind = value;
                NotifyPropertyChanged();
            }
        }

        private TypeId databaseDbTypeId = 0;

        /// <inheritdoc />
        /// <summary>
        /// DB参照時のタイプID
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public TypeId DatabaseDbTypeId
        {
            get => databaseDbTypeId;
            set
            {
                databaseDbTypeId = value;
                NotifyPropertyChanged();
            }
        }

        private bool databaseUseAdditionalItemsFlag;

        /// <summary>
        /// DB参照時の追加項目使用フラグ
        /// </summary>
        /// <exception cref="PropertyException">参照種別が「データベース参照」以外の場合</exception>
        public bool DatabaseUseAdditionalItemsFlag
        {
            get => databaseUseAdditionalItemsFlag;
            set
            {
                databaseUseAdditionalItemsFlag = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        public override DBItemType DefaultType => DBItemType.Int;

        private DatabaseValueCaseList argCaseList = new DatabaseValueCaseList();

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        public override IReadOnlyDatabaseValueCaseList ArgCaseList => argCaseList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBItemSettingDescDatabase()
        {
            ClearDatabaseSpecialCase();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override IEnumerable<DatabaseValueCase> GetAllSpecialCase()
        {
            // -1～-3を使用しない場合は空リストで良い
            if (!DatabaseUseAdditionalItemsFlag) return new List<DatabaseValueCase>();
            return argCaseList.ToList();
        }

        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override IEnumerable<DatabaseValueCaseNumber> GetAllSpecialCaseNumber()
        {
            yield return DatabaseReferKind.Code;
            yield return (int) DatabaseDbTypeId;
            yield return DatabaseUseAdditionalItemsFlag ? 1 : 0;
        }

        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override IEnumerable<DatabaseValueCaseDescription> GetAllSpecialCaseDescription()
        {
            if (!DatabaseUseAdditionalItemsFlag) return new List<DatabaseValueCaseDescription>();

            return argCaseList.Select(x => x.Description)
                .ToList();
        }

        /// <summary>
        /// DB参照時の追加選択肢文字列を更新する。
        /// </summary>
        /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
        /// <param name="description">[NotNewLine] 文字列</param>
        /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">caseNumberが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">descriptionがnullの場合</exception>
        /// <exception cref="ArgumentNewLineException">descriptionが改行を含む場合</exception>
        public void UpdateDatabaseSpecialCase(int caseNumber,
            DatabaseValueCaseDescription description)
        {
            var argCase = new DatabaseValueCase(caseNumber, description);
            var innerCaseNumber = caseNumber * -1 - 1;

            argCaseList[innerCaseNumber] = argCase;
        }

        /// <summary>
        /// 指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">値種別</param>
        /// <returns>設定可能な場合true</returns>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public override bool CanSetItemType(DBItemType type)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            return type == DBItemType.Int;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool ItemEquals(IDBItemSettingDesc? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is DBItemSettingDescDatabase casted)) return false;

            return Equals(casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool ItemEquals(DBItemSettingDescBase? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is DBItemSettingDescDatabase casted)) return false;

            return Equals(casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBItemSettingDescDatabase? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return DatabaseDbTypeId == other.DatabaseDbTypeId
                   && DatabaseUseAdditionalItemsFlag == other.DatabaseUseAdditionalItemsFlag
                   && DatabaseReferKind == other.DatabaseReferKind
                   && argCaseList.Equals(other.argCaseList);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ISpecialDataSpecificationDatabaseReference other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return DatabaseDbTypeId == other.DatabaseDbTypeId
                   && DatabaseUseAdditionalItemsFlag == other.DatabaseUseAdditionalItemsFlag
                   && DatabaseReferKind == other.DatabaseReferKind
                   && GetAllSpecialCase().SequenceEqual(other.GetAllSpecialCase());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DB参照時の追加選択肢文字列をクリアする。
        /// </summary>
        /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
        private void ClearDatabaseSpecialCase()
        {
            argCaseList = new DatabaseValueCaseList(new[]
            {
                new DatabaseValueCase(-1, ""),
                new DatabaseValueCase(-2, ""),
                new DatabaseValueCase(-3, "")
            });
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(databaseDbKind), databaseDbKind.Code);
            info.AddValue(nameof(DatabaseDbTypeId), DatabaseDbTypeId);
            info.AddValue(nameof(DatabaseUseAdditionalItemsFlag), DatabaseUseAdditionalItemsFlag);
            info.AddValue(nameof(argCaseList), argCaseList);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DBItemSettingDescDatabase(SerializationInfo info, StreamingContext context)
        {
            databaseDbKind = DBReferType.FromCode(info.GetInt32(nameof(databaseDbKind)));
            DatabaseDbTypeId = info.GetInt32(nameof(DatabaseDbTypeId));
            DatabaseUseAdditionalItemsFlag = info.GetBoolean(nameof(DatabaseUseAdditionalItemsFlag));
            argCaseList = info.GetValue<DatabaseValueCaseList>(nameof(argCaseList));
        }
    }
}
