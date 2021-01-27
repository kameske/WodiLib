// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialNumberArgDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント数値引数特殊指定情報クラス
    /// </summary>
    [Serializable]
    public partial class CommonEventSpecialNumberArgDesc : ModelBase<CommonEventSpecialNumberArgDesc>,
        ICommonEventSpecialArgDesc
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventArgName argName = "";

        /// <summary>
        /// 引数名
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventArgName ArgName
        {
            get => argName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ArgName)));
                argName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 引数特殊指定タイプ
        /// </summary>
        public CommonEventArgType ArgType => InnerDesc.ArgType;

        /// <summary>
        /// DB参照時のDB種別
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public DBKind DatabaseUseDbKind => InnerDesc.DatabaseUseDbKind;

        /// <summary>
        /// DB参照時のタイプID
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public TypeId DatabaseDbTypeId => InnerDesc.DatabaseDbTypeId;


        /// <summary>
        /// DB参照時の追加項目使用フラグ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
        public bool DatabaseUseAdditionalItemsFlag => InnerDesc.DatabaseUseAdditionalItemsFlag;

        private CommonEventNumberArgInitValue initValue;

        /// <summary>
        /// 数値引数の初期値
        /// </summary>
        public CommonEventNumberArgInitValue InitValue
        {
            get => initValue;
            set
            {
                initValue = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 【読み取り専用】選択肢情報リスト
        /// </summary>
        public IReadOnlyCommonEventSpecialArgCaseList SpecialArgCaseList
            => InnerDesc.SpecialArgCaseList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private IInnerDesc innerDesc = new InnerDescNormal();

        /// <summary>
        /// 情報内部クラス
        /// </summary>
        private IInnerDesc InnerDesc
        {
            get => innerDesc;
            set
            {
                innerDesc.PropertyChanged -= InnerDescNotifyChanged;
                innerDesc = value;
                innerDesc.PropertyChanged += InnerDescNotifyChanged;
                NotifyPropertyChanged(nameof(ArgType));
                NotifyPropertyChanged(nameof(DatabaseUseDbKind));
                NotifyPropertyChanged(nameof(DatabaseDbTypeId));
                NotifyPropertyChanged(nameof(DatabaseUseAdditionalItemsFlag));
                NotifyPropertyChanged(nameof(SpecialArgCaseList));
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
        private void InnerDescNotifyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(IInnerDesc.ArgType):
                case nameof(IInnerDesc.DatabaseUseDbKind):
                case nameof(IInnerDesc.DatabaseDbTypeId):
                case nameof(IInnerDesc.DatabaseUseAdditionalItemsFlag):
                    NotifyPropertyChanged(args.PropertyName);
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 引数特殊指定タイプを変更する。
        /// </summary>
        /// <param name="type">特殊指定タイプ</param>
        /// <param name="argCases">選択肢リスト</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public void ChangeArgType(CommonEventArgType type,
            IEnumerable<CommonEventSpecialArgCase>? argCases)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            var argCaseList = argCases is null
                ? null
                : new CommonEventSpecialArgCaseList(argCases);

            InnerDesc = InnerDescFactory.Create(type, argCaseList);
        }

        /// <summary>
        /// DB参照時の参照DBをセットする。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="dbTypeId">タイプID</param>
        /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="ArgumentNullException">dbKingがnullの場合</exception>
        public void SetDatabaseRefer(DBKind dbKind, TypeId dbTypeId)
        {
            InnerDesc.SetDatabaseRefer(dbKind, dbTypeId);
        }

        /// <summary>
        /// DB参照時の追加項目使用フラグをセットする。
        /// </summary>
        /// <param name="flag">追加項目使用フラグ</param>
        /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
        public void SetDatabaseUseAdditionalItemsFlag(bool flag)
        {
            InnerDesc.SetDatabaseUseAdditionalItemsFlag(flag);
        }

        /// <summary>
        /// すべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public IReadOnlyList<CommonEventSpecialArgCase> GetAllSpecialCase()
        {
            return InnerDesc.GetAllSpecialCase();
        }

        /// <summary>
        /// すべての選択肢番号を取得する。
        /// <para>特殊指定が「データベース参照」の場合、返却されるリストは[-1, -2, -3]ではなく[DB種別コード, DBタイプID, -1～-3追加フラグ]。</para>
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public IReadOnlyList<int> GetAllSpecialCaseNumber()
        {
            return InnerDesc.GetAllSpecialCaseNumber();
        }

        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public IReadOnlyList<string> GetAllSpecialCaseDescription()
        {
            return InnerDesc.GetAllSpecialCaseDescription();
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ICommonEventSpecialArgDesc? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is CommonEventSpecialNumberArgDesc casted)) return false;

            return Equals((IEquatable<CommonEventSpecialNumberArgDesc>) casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool ItemEquals(CommonEventSpecialNumberArgDesc? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ArgName.Equals(other.ArgName)
                   && InitValue.Equals(other.InitValue)
                   && InnerDesc.Equals(other.InnerDesc);
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCase">[NotEmpty] 追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        public void AddSpecialCase(CommonEventSpecialArgCase argCase)
        {
            InnerDesc.AddSpecialCase(argCase);
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCases">追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public void AddRangeSpecialCase(IEnumerable<CommonEventSpecialArgCase> argCases)
        {
            InnerDesc.AddRangeSpecialCase(argCases);
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
        /// <param name="argCase">追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public void InsertSpecialCase(int index, CommonEventSpecialArgCase argCase)
        {
            InnerDesc.InsertSpecialCase(index, argCase);
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
        /// <param name="argCases">追加する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public void InsertRangeSpecialCase(int index, IEnumerable<CommonEventSpecialArgCase> argCases)
        {
            InnerDesc.InsertRangeSpecialCase(index, argCases);
        }

        /// <summary>
        /// DB参照時の追加選択肢文字列を更新する。
        /// </summary>
        /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
        /// <param name="description">[NotNewLine] 文字列</param>
        /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">caseNumberが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">descriptionがEmptyの場合</exception>
        /// <exception cref="ArgumentNewLineException">descriptionが改行を含む場合</exception>
        public void UpdateDatabaseSpecialCase(int caseNumber, string description)
        {
            InnerDesc.UpdateDatabaseSpecialCase(caseNumber, description);
        }

        /// <summary>
        /// 選択肢を更新する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="argCase">[NotEmpty] 更新する選択肢内容</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public void UpdateManualSpecialCase(int index, CommonEventSpecialArgCase argCase)
        {
            InnerDesc.UpdateManualSpecialCase(index, argCase);
        }

        /// <summary>
        /// 選択肢を削除する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        public void RemoveAt(int index)
        {
            InnerDesc.RemoveSpecialCaseAt(index);
        }

        /// <summary>
        /// 選択肢を範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="count">[Range(0, 選択肢数-1)] 削除数</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">最大数を超えて削除しようとする場合</exception>
        public void RemoveRange(int index, int count)
        {
            InnerDesc.RemoveSpecialCaseRange(index, count);
        }

        /// <summary>
        /// 選択肢をクリアする。
        /// </summary>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        public void Clear()
        {
            InnerDesc.ClearSpecialCase();
        }
    }
}
