// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgDesc.InnerDescNormal.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.Common
{
    public partial class CommonEventSpecialNumberArgDesc
    {
        /// <summary>
        /// コモンイベント引数特殊指定情報内部クラス・データベース参照・特殊な指定方法を使用しない
        /// </summary>
        [Serializable]
        internal class InnerDescNormal : ModelBase<InnerDescNormal>, IInnerDesc
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <inheritdoc />
            /// <summary>
            /// 引数特殊指定タイプ
            /// </summary>
            public CommonEventArgType ArgType => CommonEventArgType.Normal;

            /// <inheritdoc />
            /// <summary>
            /// DB参照時のDB種別
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            public DBKind DatabaseUseDbKind => throw new PropertyException(
                "特殊指定が「データベース参照」ではないため取得できません");

            /// <inheritdoc />
            /// <summary>
            /// DB参照時のタイプID
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            public TypeId DatabaseDbTypeId => throw new PropertyException(
                "特殊指定が「データベース参照」ではないため取得できません");

            /// <inheritdoc />
            /// <summary>
            /// DB参照時の追加項目使用フラグ
            /// </summary>
            /// <exception cref="PropertyException">参照種別が「データベース参照」以外の場合</exception>
            public bool DatabaseUseAdditionalItemsFlag => throw new PropertyException(
                "特殊指定が「データベース参照」ではないため取得できません");

            private CommonEventSpecialArgCaseList ArgCaseList { get; } = new CommonEventSpecialArgCaseList();

            /// <summary>
            /// 【読み取り専用】選択肢情報リスト
            /// </summary>
            public IReadOnlyCommonEventSpecialArgCaseList SpecialArgCaseList => ArgCaseList;

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <inheritdoc />
            /// <summary>
            /// DB参照時の参照DBをセットする。
            /// </summary>
            /// <param name="dbKind">[NotNull] DB種別</param>
            /// <param name="dbTypeId">タイプID</param>
            /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
            public void SetDatabaseRefer(DBKind dbKind, TypeId dbTypeId)
            {
                throw new InvalidOperationException(
                    "特殊指定が「データベース参照」ではないため取得できません");
            }

            /// <inheritdoc />
            /// <summary>
            /// DB参照時の追加項目使用フラグをセットする。
            /// </summary>
            /// <param name="flag">追加項目使用フラグ</param>
            /// <exception cref="T:System.InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            public void SetDatabaseUseAdditionalItemsFlag(bool flag)
            {
                throw new InvalidOperationException(
                    "特殊指定が「データベース参照」ではないため取得できません");
            }

            /// <inheritdoc />
            /// <summary>
            /// 引数種別によらずすべての選択肢を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public IReadOnlyList<CommonEventSpecialArgCase> GetAllSpecialCase()
            {
                // 空リストでよい
                return new List<CommonEventSpecialArgCase>();
            }

            /// <inheritdoc />
            /// <summary>
            /// すべての選択肢番号を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public IReadOnlyList<int> GetAllSpecialCaseNumber()
            {
                // 空リストでよい
                return new List<int>();
            }

            /// <inheritdoc />
            /// <summary>
            /// すべての選択肢文字列を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public IReadOnlyList<string> GetAllSpecialCaseDescription()
            {
                // 空リストでよい
                return new List<string>();
            }

            /// <summary>
            /// 選択肢を追加する。
            /// </summary>
            /// <param name="argCase">[NotEmpty] 追加する選択肢</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
            public void AddSpecialCase(CommonEventSpecialArgCase argCase)
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
            public void AddRangeSpecialCase(IEnumerable<CommonEventSpecialArgCase> argCases)
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
            public void InsertSpecialCase(int index, CommonEventSpecialArgCase argCase)
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
            public void InsertRangeSpecialCase(int index, IEnumerable<CommonEventSpecialArgCase> argCases)
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
            public void UpdateDatabaseSpecialCase(int caseNumber, string description)
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
            public void UpdateManualSpecialCase(int index, CommonEventSpecialArgCase argCase)
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
            public void RemoveSpecialCaseAt(int index)
            {
                throw new InvalidOperationException(
                    "特殊指定が「手動生成」ではないため処理できません");
            }

            /// <summary>
            /// 選択肢を範囲削除する。
            /// </summary>
            /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
            /// <param name="count">[Range(0, 選択肢数-1)] 削除数</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
            /// <exception cref="ArgumentException">最大数を超えて削除しようとする場合</exception>
            public void RemoveSpecialCaseRange(int index, int count)
            {
                throw new InvalidOperationException(
                    "特殊指定が「手動生成」ではないため処理できません");
            }

            /// <summary>
            /// 選択肢をクリアする。
            /// </summary>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            public void ClearSpecialCase()
            {
                throw new InvalidOperationException(
                    "特殊指定が「手動生成」ではないため処理できません");
            }

            /// <summary>
            /// 値を比較する。
            /// </summary>
            /// <param name="other">比較対象</param>
            /// <returns>一致する場合、true</returns>
            public bool Equals(IInnerDesc other)
            {
                if (ReferenceEquals(this, other)) return true;
                if (ReferenceEquals(null, other)) return false;
                if (!(other is InnerDescNormal casted)) return false;
                return Equals((IEquatable<InnerDescNormal>) casted);
            }

            /// <summary>
            /// 値を比較する。
            /// </summary>
            /// <param name="other">比較対象</param>
            /// <returns>一致する場合、true</returns>
            public override bool Equals(InnerDescNormal other)
            {
                if (ReferenceEquals(this, other)) return true;
                if (ReferenceEquals(null, other)) return false;
                return true;
            }
        }
    }
}