// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialNumberArgDesc.IInnerDesc.cs
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
        internal interface IInnerDesc : IModelBase<IInnerDesc>
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// 引数特殊指定タイプ
            /// </summary>
            CommonEventArgType ArgType { get; }

            /// <summary>
            /// DB参照時のDB種別
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            DBKind DatabaseUseDbKind { get; }

            /// <summary>
            /// DB参照時のタイプID
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            TypeId DatabaseDbTypeId { get; }

            /// <summary>
            /// DB参照時の追加項目使用フラグ
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            bool DatabaseUseAdditionalItemsFlag { get; }

            /// <summary>
            /// 【読み取り専用】選択肢情報リスト
            /// </summary>
            IReadOnlyCommonEventSpecialArgCaseList SpecialArgCaseList { get; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// DB参照時の参照DBをセットする。
            /// </summary>
            /// <param name="dbKind">[NotNull] DB種別</param>
            /// <param name="dbTypeId">タイプID</param>
            /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
            void SetDatabaseRefer(DBKind dbKind, TypeId dbTypeId);

            /// <summary>
            /// DB参照時の追加項目使用フラグをセットする。
            /// </summary>
            /// <param name="flag">追加項目使用フラグ</param>
            /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            void SetDatabaseUseAdditionalItemsFlag(bool flag);

            /// <summary>
            /// 引数種別によらずすべての選択肢を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            IReadOnlyList<CommonEventSpecialArgCase> GetAllSpecialCase();

            /// <summary>
            /// すべての選択肢番号を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            IReadOnlyList<int> GetAllSpecialCaseNumber();

            /// <summary>
            /// すべての選択肢文字列を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            IReadOnlyList<string> GetAllSpecialCaseDescription();

            /// <summary>
            /// 選択肢を追加する。
            /// </summary>
            /// <param name="argCase">[NotEmpty] 追加する選択肢</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
            void AddSpecialCase(CommonEventSpecialArgCase argCase);

            /// <summary>
            /// 選択肢を追加する。
            /// </summary>
            /// <param name="argCases">[NotNull] 追加する選択肢</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
            void AddRangeSpecialCase(IEnumerable<CommonEventSpecialArgCase> argCases);

            /// <summary>
            /// 選択肢を挿入する。
            /// </summary>
            /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
            /// <param name="argCase">[NotEmpty] 追加する選択肢内容</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
            /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
            void InsertSpecialCase(int index, CommonEventSpecialArgCase argCase);

            /// <summary>
            /// 選択肢を挿入する。
            /// </summary>
            /// <param name="index">[Range(0, 選択肢数)] 追加する選択肢</param>
            /// <param name="argCases">[NotNull] 追加する選択肢</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
            /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
            void InsertRangeSpecialCase(int index, IEnumerable<CommonEventSpecialArgCase> argCases);

            /// <summary>
            /// DB参照時の追加選択肢文字列を更新する。
            /// </summary>
            /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
            /// <param name="description">[NotNull][NotNewLine] 文字列</param>
            /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">caseNumberが指定範囲外の場合</exception>
            /// <exception cref="ArgumentNullException">descriptionがEmptyの場合</exception>
            /// <exception cref="ArgumentNewLineException">descriptionが改行を含む場合</exception>
            void UpdateDatabaseSpecialCase(int caseNumber, string description);

            /// <summary>
            /// 選択肢を更新する。
            /// </summary>
            /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
            /// <param name="argCase">[NotEmpty] 更新する選択肢内容</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
            /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
            void UpdateManualSpecialCase(int index, CommonEventSpecialArgCase argCase);

            /// <summary>
            /// 選択肢を削除する。
            /// </summary>
            /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
            void RemoveSpecialCaseAt(int index);

            /// <summary>
            /// 選択肢を範囲削除する。
            /// </summary>
            /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
            /// <param name="count">[Range(0, 選択肢数-1)] 削除数</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
            /// <exception cref="ArgumentException">最大数を超えて削除しようとする場合</exception>
            void RemoveSpecialCaseRange(int index, int count);

            /// <summary>
            /// 選択肢をクリアする。
            /// </summary>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            void ClearSpecialCase();
        }
    }
}