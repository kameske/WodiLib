// ========================================
// Project Name : WodiLib
// File Name    : DBItemSpecialSettingDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目特殊指定情報クラス
    /// </summary>
    [Serializable]
    public class DBItemSpecialSettingDesc : ModelBase<DBItemSpecialSettingDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目特殊指定タイプ
        /// </summary>
        public DBItemSpecialSettingType SettingType => InnerDesc.SettingType;

        /// <summary>
        /// 特殊指定が「データベース参照」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public ISpecialDataSpecificationDatabaseReference DatabaseReferenceDesc => InnerDesc.DatabaseReferenceDesc;

        /// <summary>
        /// [NotNull] DB参照時のDB種別
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        [Obsolete("DatabaseReferenceDesc プロパティを参照してください。Ver 1.6 で削除します。")]
        public DBReferType DatabaseReferKind
        {
            get => DatabaseReferenceDesc.DatabaseReferKind;
            set => DatabaseReferenceDesc.DatabaseReferKind = value;
        }

        /// <summary>
        /// DB参照時のタイプID
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        [Obsolete("DatabaseReferenceDesc プロパティを参照してください。Ver 1.6 で削除します。")]
        public TypeId DatabaseDbTypeId
        {
            get => DatabaseReferenceDesc.DatabaseDbTypeId;
            set => DatabaseReferenceDesc.DatabaseDbTypeId = value;
        }

        /// <summary>
        /// DB参照時の追加項目使用フラグ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        [Obsolete("DatabaseReferenceDesc プロパティを参照してください。Ver 1.6 で削除します。")]
        public bool DatabaseUseAdditionalItemsFlag
        {
            get => DatabaseReferenceDesc.DatabaseUseAdditionalItemsFlag;
            set => DatabaseReferenceDesc.DatabaseUseAdditionalItemsFlag = value;
        }

        /// <summary>
        /// 特殊指定が「ファイル読み込み」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        public ISpecialDataSpecificationLoadFile LoadFileDesc => InnerDesc.LoadFileDesc;

        /// <summary>
        /// ファイル読み込み時の初期フォルダ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        [Obsolete("LoadFileDesc プロパティを参照してください。Ver 1.6 で削除します。")]
        public DBSettingFolderName FolderName
        {
            get => LoadFileDesc.FolderName;
            set => LoadFileDesc.FolderName = value;
        }

        /// <summary>
        /// ファイル読み込み時の保存時にフォルダ名省略フラグ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        [Obsolete("LoadFileDesc プロパティを参照してください。Ver 1.6 で削除します。")]
        public bool OmissionFolderNameFlag
        {
            get => LoadFileDesc.OmissionFolderNameFlag;
            set => LoadFileDesc.OmissionFolderNameFlag = value;
        }

        /// <summary>
        /// 特殊指定が「手動設定」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「手動設定」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ISpecialDataSpecificationCreateOptions ManualDesc => InnerDesc.ManualDesc;

        /// <summary>
        /// 特殊指定が「特殊な設定方法を使用しない」の場合の特殊設定情報
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「特殊な設定方法を使用しない」以外の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ISpecialDataSpecificationNormal NormalDesc => InnerDesc.NormalDesc;

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        public IReadOnlyDatabaseValueCaseList ArgCaseList => InnerDesc.ArgCaseList;

        private DBValueInt initValue = 0;

        /// <summary>
        /// 項目の初期値
        /// </summary>
        public DBValueInt InitValue
        {
            get => initValue;
            set
            {
                initValue = value;
                NotifyPropertyChanged();
            }
        }

        private ItemMemo itemMemo = "";

        /// <summary>
        /// 項目メモ
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ItemMemo ItemMemo
        {
            get => itemMemo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemMemo)));

                itemMemo = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        internal DBItemType DefaultType => InnerDesc.DefaultType;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private IDBItemSettingDesc innerDesc = new DBItemSettingDescNormal();

        /// <summary>
        /// 情報内部クラス
        /// </summary>
        private IDBItemSettingDesc InnerDesc
        {
            get => innerDesc;
            set
            {
                innerDesc.PropertyChanged -= OnInnerDescPropertyChanged;
                innerDesc = value;
                innerDesc.PropertyChanged += OnInnerDescPropertyChanged;
                NotifyPropertyChanged(nameof(SettingType));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 情報内部クラスプロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnInnerDescPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(IDBItemSettingDesc.DatabaseReferenceDesc):
                case nameof(IDBItemSettingDesc.LoadFileDesc):
                case nameof(IDBItemSettingDesc.ManualDesc):
                case nameof(IDBItemSettingDesc.NormalDesc):
                    NotifyPropertyChanged(args.PropertyName);
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBItemSpecialSettingDesc()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">[NotNull] 特殊指定タイプ</param>
        /// <param name="argCases">[Nullable] 選択肢リスト</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public DBItemSpecialSettingDesc(DBItemSpecialSettingType type,
            IEnumerable<DatabaseValueCase> argCases)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            var argCaseList = argCases is null
                ? null
                : new DatabaseValueCaseList(argCases);

            InnerDesc = DBItemSettingDescFactory.Create(type, argCaseList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 引数特殊指定タイプを変更する。
        /// </summary>
        /// <param name="type">[NotNull] 特殊指定タイプ</param>
        /// <param name="argCases">[Nullable] 選択肢リスト</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public void ChangeValueType(DBItemSpecialSettingType type,
            IEnumerable<DatabaseValueCase> argCases)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            var argCaseList = argCases is null
                ? null
                : new DatabaseValueCaseList(argCases);

            InnerDesc = DBItemSettingDescFactory.Create(type, argCaseList);
        }

        /// <summary>
        /// すべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public IEnumerable<DatabaseValueCase> GetAllSpecialCase()
        {
            return InnerDesc.GetAllSpecialCase();
        }

        /// <summary>
        /// すべての選択肢番号を取得する。
        /// <para>特殊指定が「データベース参照」の場合、返却されるリストは[-1, -2, -3]ではなく[DB種別コード, DBタイプID, -1～-3追加フラグ]。</para>
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public IEnumerable<DatabaseValueCaseNumber> GetAllSpecialCaseNumber()
        {
            return InnerDesc.GetAllSpecialCaseNumber();
        }

        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public IEnumerable<DatabaseValueCaseDescription> GetAllSpecialCaseDescription()
        {
            return InnerDesc.GetAllSpecialCaseDescription();
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCase">[NotNull] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        [Obsolete("ManualDesc.AddSpecialCase(DatabaseValueCase) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void AddSpecialCase(DatabaseValueCase argCase)
            => ManualDesc.AddSpecialCase(argCase);

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCases">[NotNull] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        [Obsolete("ManualDesc.AddSpecialCaseRange(IEnumerable<DatabaseValueCase>) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void AddSpecialCaseRange(IEnumerable<DatabaseValueCase> argCases)
            => ManualDesc.AddSpecialCaseRange(argCases);

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
        /// <param name="argCase">[NotNull] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        [Obsolete("ManualDesc.InsertSpecialCase(int, DatabaseValueCase) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void InsertSpecialCase(int index, DatabaseValueCase argCase)
            => ManualDesc.InsertSpecialCase(index, argCase);

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
        /// <param name="argCases">[NotNull] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        [Obsolete("ManualDesc.InsertSpecialCase(int, DatabaseValueCase) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void InsertSpecialCaseRange(int index, IEnumerable<DatabaseValueCase> argCases)
            => ManualDesc.InsertSpecialCaseRange(index, argCases);

        /// <summary>
        /// DB参照時の追加選択肢文字列を更新する。
        /// </summary>
        /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
        /// <param name="description">[NotNull][NotNewLine] 文字列</param>
        /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">caseNumberが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">descriptionがEmptyの場合</exception>
        /// <exception cref="ArgumentNewLineException">descriptionが改行を含む場合</exception>
        [Obsolete("DatabaseReferenceDesc.UpdateDatabaseSpecialCase(int, string) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void UpdateDatabaseSpecialCase(int caseNumber, string description)
            => DatabaseReferenceDesc.UpdateDatabaseSpecialCase(caseNumber, description);

        /// <summary>
        /// 選択肢を更新する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="argCase">[NotEmpty] 更新する選択肢内容</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        [Obsolete("ManualDesc.UpdateManualSpecialCase(int, DatabaseValueCase) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void UpdateManualSpecialCase(int index, DatabaseValueCase argCase)
            => ManualDesc.UpdateManualSpecialCase(index, argCase);

        /// <summary>
        /// 選択肢を削除する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        [Obsolete("ManualDesc.RemoveSpecialCaseAt(int) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void RemoveAt(int index)
            => ManualDesc.RemoveSpecialCaseAt(index);

        /// <summary>
        /// 選択肢を範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="count">[Range(0, 選択肢数)] 削除数</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">最大数を超えて削除しようとする場合</exception>
        [Obsolete("ManualDesc.RemoveSpecialCaseRange(int, int) メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void RemoveRange(int index, int count)
            => ManualDesc.RemoveSpecialCaseRange(index, count);

        /// <summary>
        /// 選択肢をクリアする。
        /// </summary>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        [Obsolete("ManualDesc.ClearSpecialCase() メソッドを使用してください。 Ver 1.6 で削除します。")]
        public void Clear()
            => ManualDesc.ClearSpecialCase();

        /// <summary>
        /// 指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">[NotNull] 値種別</param>
        /// <returns>設定可能な場合true</returns>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public bool CanSetItemType(DBItemType type)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            return InnerDesc.CanSetItemType(type);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(DBItemSpecialSettingDesc other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return SettingType == other.SettingType
                   && InitValue == other.InitValue
                   && ItemMemo == other.ItemMemo
                   && InnerDesc.Equals(other.InnerDesc);
        }
    }
}
