// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgCaseList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// 選択肢情報リスト
    /// </summary>
    internal class CommonEventSpecialArgCaseList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 選択肢個数
        /// </summary>
        public int Count => specialArgCases.Count;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private readonly List<CommonEventSpecialArgCase> specialArgCases = new List<CommonEventSpecialArgCase>();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        public CommonEventSpecialArgCaseList(params CommonEventSpecialArgCase[] cases)
        {
            specialArgCases.AddRange(cases);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 選択肢番号から選択肢文字列を取得する。
        /// </summary>
        /// <param name="caseNumber">選択肢番号</param>
        /// <returns>選択肢文字列。番号に対応した情報が存在しない場合null。</returns>
        public string GetDescriptionForCaseNumber(int caseNumber)
        {
            var info = GetForCaseNumber(caseNumber);
            if (info.IsEmpty()) return null;
            return GetForCaseNumber(caseNumber).Description;
        }

        /// <summary>
        /// 選択肢番号から選択肢情報を取得する。
        /// </summary>
        /// <param name="caseNumber">選択肢番号</param>
        /// <returns>選択肢情報。情報が存在しない場合CommonEventSpecialArgCase.Empty</returns>
        public CommonEventSpecialArgCase GetForCaseNumber(int caseNumber)
        {
            var result = specialArgCases.FirstOrDefault(x => x.CaseNumber == caseNumber);
            return result != default(CommonEventSpecialArgCase) ? result : CommonEventSpecialArgCase.Empty;
        }

        /// <summary>
        /// すべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢情報</returns>
        public List<CommonEventSpecialArgCase> GetAllCase()
        {
            return specialArgCases;
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCase">[NotEmpty] 選択肢情報</param>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        public void Add(CommonEventSpecialArgCase argCase)
        {
            if (argCase == CommonEventSpecialArgCase.Empty)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCase)));

            specialArgCases.Add(argCase);
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCases">[NotNull] 選択肢</param>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        public void AddRange(IEnumerable<CommonEventSpecialArgCase> argCases)
        {
            if (argCases == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCases)));

            specialArgCases.AddRange(argCases);
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
        /// <param name="argCase">[NotEmpty] 選択肢情報</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Insert(int index, CommonEventSpecialArgCase argCase)
        {
            var max = Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (argCase == CommonEventSpecialArgCase.Empty)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCase)));

            specialArgCases.Insert(index, argCase);
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
        /// <param name="argCases">[NotNull] 選択肢</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void InsertRange(int index, IEnumerable<CommonEventSpecialArgCase> argCases)
        {
            var max = Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (argCases == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCases)));

            specialArgCases.InsertRange(index, argCases);
        }

        /// <summary>
        /// 選択肢を更新する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
        /// <param name="argCase">[NotEmpty] 選択肢情報</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">argCaseがEmptyの場合</exception>
        public void Update(int index, CommonEventSpecialArgCase argCase)
        {
            var max = Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));
            if (argCase == CommonEventSpecialArgCase.Empty)
                throw new ArgumentNullException(
                    ErrorMessage.NotEmpty(nameof(argCase)));

            specialArgCases[index] = argCase;
        }

        /// <summary>
        /// 選択肢を削除する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public void RemoveAt(int index)
        {
            var max = Count - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            specialArgCases.RemoveAt(index);
        }

        /// <summary>
        /// 選択肢を範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
        /// <param name="count">[Range(0, ManualCaseLength)] 削除数</param>
        /// <exception cref="ArgumentOutOfRangeException">index、またはcountが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">リストの範囲を超えて削除しようとする場合</exception>
        public void RemoveRange(int index, int count)
        {
            var allLength = Count;

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

            specialArgCases.RemoveRange(index, count);
        }

        /// <summary>
        /// 選択肢をクリアする。
        /// </summary>
        public void Clear()
        {
            specialArgCases.Clear();
        }
    }
}