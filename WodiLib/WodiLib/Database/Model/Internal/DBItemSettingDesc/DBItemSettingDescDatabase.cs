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
        ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 値特殊指定タイプ
        /// </summary>
        public override DBItemSpecialSettingType SettingType => DBItemSpecialSettingType.ReferDatabase;

        private DBReferType databaseDbKind = DBReferType.System;

        /// <inheritdoc />
        /// <summary>
        /// [NotNull] DB参照時のDB種別
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public override DBReferType DatabaseReferKind
        {
            get => databaseDbKind;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DatabaseReferKind)));

                databaseDbKind = value;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// DB参照時のタイプID
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public override TypeId DatabaseDbTypeId { get; set; } = 0;

        /// <inheritdoc />
        /// <summary>
        /// DB参照時の追加項目使用フラグ
        /// </summary>
        /// <exception cref="PropertyException">参照種別が「データベース参照」以外の場合</exception>
        public override bool DatabaseUseAdditionalItemsFlag { get; set; }

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        public override DBItemType DefaultType => DBItemType.Int;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>選択肢リスト</summary>
        private DatabaseValueCaseList ArgCaseList { get; set; }

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

        /// <inheritdoc />
        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCase> GetAllSpecialCase()
        {
            // -1～-3を使用しない場合は空リストで良い
            if (!DatabaseUseAdditionalItemsFlag) return new List<DatabaseValueCase>();
            return ArgCaseList.ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCaseNumber> GetAllSpecialCaseNumber()
        {
            return new List<DatabaseValueCaseNumber>
            {
                DatabaseReferKind.Code,
                (int) DatabaseDbTypeId,
                DatabaseUseAdditionalItemsFlag ? 1 : 0
            };
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCaseDescription> GetAllSpecialCaseDescription()
        {
            if (!DatabaseUseAdditionalItemsFlag) return new List<DatabaseValueCaseDescription>();

            return ArgCaseList.Select(x => x.Description)
                .ToList();
        }

        /// <summary>
        /// DB参照時の追加選択肢文字列を更新する。
        /// </summary>
        /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
        /// <param name="description">[NotNull][NotNewLine] 文字列</param>
        /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">caseNumberが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">descriptionがEmptyの場合</exception>
        /// <exception cref="ArgumentNewLineException">descriptionが改行を含む場合</exception>
        public override void UpdateDatabaseSpecialCase(int caseNumber,
            DatabaseValueCaseDescription description)
        {
            var argCase = new DatabaseValueCase(caseNumber, description);
            var innerCaseNumber = caseNumber * -1 - 1;

            ArgCaseList[innerCaseNumber] = argCase;
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

            return type == DBItemType.Int;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(IDBItemSettingDesc other)
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
        public bool Equals(DBItemSettingDescDatabase other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return DatabaseDbTypeId == other.DatabaseDbTypeId
                   && DatabaseUseAdditionalItemsFlag == other.DatabaseUseAdditionalItemsFlag
                   && DatabaseReferKind == other.DatabaseReferKind
                   && ArgCaseList.Equals(other.ArgCaseList);
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
            ArgCaseList = new DatabaseValueCaseList(new[]
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
            info.AddValue(nameof(ArgCaseList), ArgCaseList);
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
            ArgCaseList = info.GetValue<DatabaseValueCaseList>(nameof(ArgCaseList));
        }
    }
}