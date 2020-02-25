// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgDesc.InnerDescManual.cs
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
        /// コモンイベント引数特殊指定情報内部クラス・選択肢手動生成
        /// </summary>
        [Serializable]
        internal class InnerDescManual : IInnerDesc, IEquatable<InnerDescManual>
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <inheritdoc />
            /// <summary>
            /// 引数特殊指定タイプ
            /// </summary>
            public CommonEventArgType ArgType => CommonEventArgType.Manual;

            /// <inheritdoc />
            /// <summary>
            /// DB参照時のDB種別
            /// </summary>
            /// <exception cref="PropertyException">特殊指定が「データベース参照」以外の場合</exception>
            public DBKind DatabaseDbKind => throw new PropertyException(
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

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>選択肢リスト</summary>
            private CommonEventSpecialArgCaseList ArgCaseList { get; } = new CommonEventSpecialArgCaseList();

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Constructor
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="argCaseList">[Nullable] 選択肢とその文字列リスト</param>
            public InnerDescManual(CommonEventSpecialArgCaseList argCaseList)
            {
                if (!(argCaseList is null))
                {
                    ArgCaseList = argCaseList;
                }
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
                throw new InvalidOperationException(
                    "特殊指定が「データベース参照」ではないため設定できません");
            }

            /// <inheritdoc />
            /// <summary>
            /// DB参照時の追加項目使用フラグをセットする。
            /// </summary>
            /// <param name="flag">追加項目使用フラグ</param>
            /// <exception cref="InvalidOperationException">特殊指定が「データベース参照」以外の場合</exception>
            public void SetDatabaseUseAdditionalItemsFlag(bool flag)
            {
                throw new InvalidOperationException(
                    "特殊指定が「データベース参照」ではないため設定できません");
            }

            /// <inheritdoc />
            /// <summary>
            /// 引数種別によらずすべての選択肢を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public List<CommonEventSpecialArgCase> GetAllSpecialCase()
            {
                return ArgCaseList.ToList();
            }

            /// <inheritdoc />
            /// <summary>
            /// すべての選択肢番号を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public List<int> GetAllSpecialCaseNumber()
            {
                return GetAllManualCase().Select(x => x.CaseNumber).ToList();
            }

            /// <inheritdoc />
            /// <summary>
            /// すべての選択肢文字列を取得する。
            /// </summary>
            /// <returns>すべての選択肢リスト</returns>
            public List<string> GetAllSpecialCaseDescription()
            {
                return GetAllManualCase().Select(x => x.Description).ToList();
            }

            /// <summary>
            /// 選択肢を追加する。
            /// </summary>
            /// <param name="argCase">[NotNull] 選択肢情報</param>
            /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
            public void AddSpecialCase(CommonEventSpecialArgCase argCase)
            {
                if (argCase is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(argCase)));

                ArgCaseList.Add(argCase);
            }

            /// <summary>
            /// 選択肢を追加する。
            /// </summary>
            /// <param name="argCases">[NotNull] 選択肢</param>
            /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
            public void AddRangeSpecialCase(IEnumerable<CommonEventSpecialArgCase> argCases)
            {
                if (argCases is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(argCases)));

                ArgCaseList.AddRange(argCases.ToList());
            }

            /// <summary>
            /// 選択肢を挿入する。
            /// </summary>
            /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
            /// <param name="argCase">[NotNull] 選択肢情報</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            /// <exception cref="ArgumentNullException"></exception>
            public void InsertSpecialCase(int index, CommonEventSpecialArgCase argCase)
            {
                var max = ArgCaseList.Count;
                const int min = 0;
                if (index < min || max < index)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), min, max, index));

                if (argCase is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(argCase)));

                ArgCaseList.Insert(index, argCase);
            }

            /// <summary>
            /// 選択肢を挿入する。
            /// </summary>
            /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
            /// <param name="argCases">[NotNull] 選択肢</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            /// <exception cref="ArgumentNullException"></exception>
            public void InsertRangeSpecialCase(int index, IEnumerable<CommonEventSpecialArgCase> argCases)
            {
                var max = ArgCaseList.Count;
                const int min = 0;
                if (index < min || max < index)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), min, max, index));

                if (argCases is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(argCases)));

                ArgCaseList.InsertRange(index, argCases.ToList());
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
            /// <param name="argCase">[NotNull] 更新する選択肢内容</param>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
            /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
            public void UpdateManualSpecialCase(int index, CommonEventSpecialArgCase argCase)
            {
                var max = ArgCaseList.Count;
                const int min = 0;
                if (index < min || max < index)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), min, max, index));
                if (argCase is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotEmpty(nameof(argCase)));

                ArgCaseList[index] = argCase;
            }

            /// <summary>
            /// 選択肢を削除する。
            /// </summary>
            /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
            /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
            public void RemoveSpecialCaseAt(int index)
            {
                var max = ArgCaseList.Count - 1;
                const int min = 0;
                if (index < min || max < index)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), min, max, index));

                ArgCaseList.RemoveAt(index);
            }

            /// <summary>
            /// 選択肢を範囲削除する。
            /// </summary>
            /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
            /// <param name="count">[Range(0, ManualCaseLength)] 削除数</param>
            /// <exception cref="ArgumentOutOfRangeException">index、またはcountが指定範囲以外の場合</exception>
            /// <exception cref="ArgumentException">リストの範囲を超えて削除しようとする場合</exception>
            public void RemoveSpecialCaseRange(int index, int count)
            {
                var allLength = ArgCaseList.Count;

                var indexMax = allLength - 1;
                const int min = 0;
                if (index < min || indexMax < index)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), min, indexMax, index));
                var countMax = allLength;
                if (count < min || countMax < count)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(count), min, countMax, count));

                const int listLengthMin = 0;
                if (allLength - index < count + listLengthMin)
                    throw new ArgumentException(
                        $"リストの範囲を超えて削除しようとしています。" +
                        $"{nameof(index)}:{index}, {nameof(count)}:{count}");

                ArgCaseList.RemoveRange(index, count);
            }

            /// <summary>
            /// 選択肢をクリアする。
            /// </summary>
            /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
            public void ClearSpecialCase()
            {
                ArgCaseList.Clear();
            }

            /// <summary>
            /// 値を比較する。
            /// </summary>
            /// <param name="other">比較対象</param>
            /// <returns>一致する場合、true</returns>
            public bool Equals(IInnerDesc other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                if (!(other is InnerDescManual casted)) return false;
                return Equals(casted);
            }

            /// <summary>
            /// 値を比較する。
            /// </summary>
            /// <param name="other">比較対象</param>
            /// <returns>一致する場合、true</returns>
            public bool Equals(InnerDescManual other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return ArgCaseList.Equals(other.ArgCaseList);
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Static Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// 選択肢手動生成時のすべての選択肢を取得する。
            /// </summary>
            /// <returns>すべての選択肢情報</returns>
            /// <exception cref="InvalidOperationException">特殊指定が「選択肢手動生成」以外の場合</exception>
            private IEnumerable<CommonEventSpecialArgCase> GetAllManualCase()
            {
                return ArgCaseList.ToList();
            }
        }
    }
}