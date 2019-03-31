// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialNumberArgDesc.InnerDescDatabase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.Common
{
    public partial class CommonEventSpecialNumberArgDesc
    {
        /// <summary>
        /// コモンイベント引数特殊指定情報内部クラス・データベース参照
        /// </summary>
        internal class InnerDescDatabase : IInnerDesc
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <inheritdoc />
            /// <summary>
            /// 引数特殊指定タイプ
            /// </summary>
            public CommonEventArgType ArgType => CommonEventArgType.ReferDatabase;

            private DBKind databaseDbKind = DBKind.Changeable;

            /// <inheritdoc />
            /// <summary>
            /// DB参照時のDB種別
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            public DBKind DatabaseDbKind
            {
                get => databaseDbKind;
                private set
                {
                    if (value == null)
                        throw new PropertyNullException(
                            ErrorMessage.NotNull(nameof(DatabaseDbKind)));
                    databaseDbKind = value;
                }
            }

            /// <inheritdoc />
            /// <summary>
            /// DB参照時のタイプID
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            public TypeId DatabaseDbTypeId { get; private set; }

            /// <inheritdoc />
            /// <summary>
            /// DB参照時の追加項目使用フラグ
            /// </summary>
            /// <exception cref="PropertyException">参照種別が「データベース参照」以外の場合</exception>
            public bool DatabaseUseAdditionalItemsFlag { get; private set; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>選択肢リスト</summary>
            private CommonEventSpecialArgCaseList ArgCaseList { get; set; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Constructor
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public InnerDescDatabase()
            {
                ClearDatabaseSpecialCase();
            }

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
                if (dbKind == null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(dbKind)));

                DatabaseDbKind = dbKind;
                DatabaseDbTypeId = dbTypeId;
            }

            /// <inheritdoc />
            /// <summary>
            /// DB参照時の追加項目使用フラグをセットする。
            /// </summary>
            /// <param name="flag">追加項目使用フラグ</param>
            /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            public void SetDatabaseUseAdditionalItemsFlag(bool flag)
            {
                DatabaseUseAdditionalItemsFlag = flag;
            }

            /// <inheritdoc />
            /// <summary>
            /// 引数種別によらずすべての選択肢を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public List<CommonEventSpecialArgCase> GetAllSpecialCase()
            {
                // -1～-3を使用しない場合は空リストで良い
                if (!DatabaseUseAdditionalItemsFlag) return new List<CommonEventSpecialArgCase>();
                return ArgCaseList.GetAllCase();
            }

            /// <inheritdoc />
            /// <summary>
            /// すべての選択肢番号を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public List<int> GetAllSpecialCaseNumber()
            {
                return new List<int>
                {
                    DatabaseDbKind.SpecialArgCode,
                    DatabaseDbTypeId,
                    DatabaseUseAdditionalItemsFlag ? 1 : 0
                };
            }

            /// <inheritdoc />
            /// <summary>
            /// すべての選択肢文字列を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public List<string> GetAllSpecialCaseDescription()
            {
                if (!DatabaseUseAdditionalItemsFlag) return new List<string>();

                return ArgCaseList.GetAllCase()
                    .Select(x => x.Description).ToList();
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
                var argCase = new CommonEventSpecialArgCase(caseNumber, description);
                var innerCaseNumber = caseNumber * -1 - 1;

                ArgCaseList.Update(innerCaseNumber, argCase);
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

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// DB参照時の追加選択肢文字列をクリアする。
            /// </summary>
            /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            private void ClearDatabaseSpecialCase()
            {
                ArgCaseList = new CommonEventSpecialArgCaseList(
                    new CommonEventSpecialArgCase(-1, ""),
                    new CommonEventSpecialArgCase(-2, ""),
                    new CommonEventSpecialArgCase(-3, "")
                );
            }
        }
    }
}