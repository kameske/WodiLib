// ========================================
// Project Name : WodiLib
// File Name    : DBItemSettingDescManual.cs
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
    /// データベース設定値特殊指定・ファイル読み込み
    /// </summary>
    [Serializable]
    internal class DBItemSettingDescLoadFile : DBItemSettingDescBase, IEquatable<DBItemSettingDescLoadFile>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 値特殊指定タイプ
        /// </summary>
        public override DBItemSpecialSettingType SettingType => DBItemSpecialSettingType.LoadFile;

        private DBSettingFolderName folderName = "";

        /// <summary>
        /// [MotNull] ファイル読み込み時の初期フォルダ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public override DBSettingFolderName FolderName
        {
            get => folderName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(FolderName));

                folderName = value;
                NotifyPropertyChanged();
            }
        }

        private bool omissionFolderNameFlag;

        /// <summary>
        /// ファイル読み込み時の保存時にフォルダ名省略フラグ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        public override bool OmissionFolderNameFlag
        {
            get => omissionFolderNameFlag;
            set
            {
                omissionFolderNameFlag = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        public override DBItemType DefaultType => DBItemType.String;

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        public override IReadOnlyDatabaseValueCaseList ArgCaseList { get; }
            = new DatabaseValueCaseList();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="infoSet">[Nullable] 初期フォルダと保存時にフォルダ名省略フラグのセット</param>
        /// <exception cref="ArgumentException">
        ///     infoSetがnullではない、かつinfoSet.CaseNumberが0,1以外の場合
        /// </exception>
        public DBItemSettingDescLoadFile(DatabaseValueCase infoSet = null)
        {
            if (infoSet is null)
            {
                FolderName = "";
                OmissionFolderNameFlag = false;
                return;
            }

            if (infoSet.CaseNumber != 0
                && infoSet.CaseNumber != 1)
                throw new ArgumentException(
                    ErrorMessage.OutOfRange(nameof(infoSet.CaseNumber),
                        0, 1, infoSet.CaseNumber));

            FolderName = new DBSettingFolderName(infoSet.Description);
            OmissionFolderNameFlag = infoSet.CaseNumber == 1;
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
            var omissionFlagValue = OmissionFolderNameFlag ? 1 : 0;
            return new List<DatabaseValueCase>
            {
                new DatabaseValueCase(omissionFlagValue, (string) FolderName)
            };
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCaseNumber> GetAllSpecialCaseNumber()
        {
            return GetAllCase().Select(x => x.CaseNumber).ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCaseDescription> GetAllSpecialCaseDescription()
        {
            return GetAllCase().Select(x => x.Description).ToList();
        }

        /// <summary>
        /// ファイル読み込み時のデフォルトフォルダを更新する。
        /// </summary>
        /// <param name="folderName">[NotNull] デフォルトフォルダ</param>
        /// <exception cref="InvalidOperationException">特殊指定が「フォルダ読み込み」以外の場合</exception>
        /// <exception cref="ArgumentNullException">folderNameがnullの場合</exception>
        public override void UpdateDefaultFolder(DBSettingFolderName folderName)
        {
            if (folderName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(folderName)));

            FolderName = folderName;
        }

        /// <summary>
        /// ファイル読み込み時のフォルダ名省略フラグを更新する。
        /// </summary>
        /// <param name="flag">フォルダ名省略フラグ</param>
        /// <exception cref="InvalidOperationException">特殊指定が「フォルダ読み込み」以外の場合</exception>
        public override void UpdateOmissionFolderNameFlag(bool flag)
        {
            OmissionFolderNameFlag = flag;
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

            return type == DBItemType.String;
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

            if (!(other is DBItemSettingDescLoadFile casted)) return false;

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

            if (!(other is DBItemSettingDescLoadFile casted)) return false;

            return Equals(casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBItemSettingDescLoadFile other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return FolderName == other.FolderName
                   && OmissionFolderNameFlag == other.OmissionFolderNameFlag;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// すべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢情報</returns>
        private IEnumerable<DatabaseValueCase> GetAllCase()
        {
            var omissionFlagValue = OmissionFolderNameFlag ? 1 : 0;
            var caseList = new List<DatabaseValueCase>
            {
                new DatabaseValueCase(omissionFlagValue, (string) FolderName)
            };
            return caseList;
        }
    }
}