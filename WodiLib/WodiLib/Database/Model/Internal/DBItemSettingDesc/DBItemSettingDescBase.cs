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

        /// <inheritdoc />
        /// <summary>
        /// [MotNull] DB参照時のDB種別
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public virtual DBReferType DatabaseReferKind
        {
            get => throw new PropertyAccessException(
                "特殊指定が「データベース参照」ではないため取得できません");
            set => throw new PropertyAccessException(
                "特殊指定が「データベース参照」ではないため設定できません");
        }

        /// <inheritdoc />
        /// <summary>
        /// DB参照時のタイプID
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public virtual TypeId DatabaseDbTypeId
        {
            get => throw new PropertyAccessException(
                "特殊指定が「データベース参照」ではないため取得できません");
            set => throw new PropertyAccessException(
                "特殊指定が「データベース参照」ではないため設定できません");
        }

        /// <inheritdoc />
        /// <summary>
        /// DB参照時の追加項目使用フラグ
        /// </summary>
        /// <exception cref="PropertyException">参照種別が「データベース参照」以外の場合</exception>
        public virtual bool DatabaseUseAdditionalItemsFlag
        {
            get => throw new PropertyAccessException(
                "特殊指定が「データベース参照」ではないため取得できません");
            set => throw new PropertyAccessException(
                "特殊指定が「データベース参照」ではないため設定できません");
        }

        /// <summary>
        /// [MotNull] ファイル読み込み時の初期フォルダ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public virtual DBSettingFolderName FolderName
        {
            get => throw new PropertyAccessException(
                "特殊指定が「ファイル読み込み」ではないため取得できません");
            set => throw new PropertyAccessException(
                "特殊指定が「ファイル読み込み」ではないため設定できません");
        }

        /// <summary>
        /// ファイル読み込み時の保存時にフォルダ名省略フラグ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        public virtual bool OmissionFolderNameFlag
        {
            get => throw new PropertyAccessException(
                "特殊指定が「ファイル読み込み」ではないため取得できません");
            set => throw new PropertyAccessException(
                "特殊指定が「ファイル読み込み」ではないため設定できません");
        }

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

        /// <inheritdoc />
        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public abstract List<DatabaseValueCase> GetAllSpecialCase();

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public abstract List<DatabaseValueCaseNumber> GetAllSpecialCaseNumber();

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public abstract List<DatabaseValueCaseDescription> GetAllSpecialCaseDescription();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCase">[NotEmpty] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        public virtual void AddSpecialCase(DatabaseValueCase argCase)
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCases">[NotNull] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public virtual void AddRangeSpecialCase(IEnumerable<DatabaseValueCase> argCases)
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
        /// <param name="argCase">[NotEmpty] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public virtual void InsertSpecialCase(int index, DatabaseValueCase argCase)
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
        /// <param name="argCases">[NotNull] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public virtual void InsertRangeSpecialCase(int index, IEnumerable<DatabaseValueCase> argCases)
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
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
        public virtual void UpdateDatabaseSpecialCase(int caseNumber,
            DatabaseValueCaseDescription description)
        {
            throw new InvalidOperationException(
                "特殊指定が「データベース参照」ではないため処理できません");
        }

        /// <summary>
        /// 選択肢を更新する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="argCase">[NotEmpty] 更新する選択肢内容</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public virtual void UpdateManualSpecialCase(int index, DatabaseValueCase argCase)
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
        }

        /// <summary>
        /// 選択肢を削除する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        public virtual void RemoveSpecialCaseAt(int index)
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
        }

        /// <summary>
        /// 選択肢を範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="count">[Range(0, 選択肢数)] 削除数</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">最大数を超えて削除しようとする場合</exception>
        public virtual void RemoveSpecialCaseRange(int index, int count)
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
        }

        /// <summary>
        /// 選択肢をクリアする。
        /// </summary>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        public virtual void ClearSpecialCase()
        {
            throw new InvalidOperationException(
                "特殊指定が「手動生成」ではないため処理できません");
        }

        /// <summary>
        /// ファイル読み込み時のデフォルトフォルダを更新する。
        /// </summary>
        /// <param name="folderName">[NotNull] デフォルトフォルダ</param>
        /// <exception cref="InvalidOperationException">特殊指定が「フォルダ読み込み」以外の場合</exception>
        /// <exception cref="ArgumentNullException">folderNameがnullの場合</exception>
        public virtual void UpdateDefaultFolder(DBSettingFolderName folderName)
        {
            throw new InvalidOperationException(
                "特殊指定が「ファイル読み込み」ではないため処理できません");
        }

        /// <summary>
        /// ファイル読み込み時のフォルダ名省略フラグを更新する。
        /// </summary>
        /// <param name="flag">フォルダ名省略フラグ</param>
        /// <exception cref="InvalidOperationException">特殊指定が「フォルダ読み込み」以外の場合</exception>
        public virtual void UpdateOmissionFolderNameFlag(bool flag)
        {
            throw new InvalidOperationException(
                "特殊指定が「ファイル読み込み」ではないため処理できません");
        }

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