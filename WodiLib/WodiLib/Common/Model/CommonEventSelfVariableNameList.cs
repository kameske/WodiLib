// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSelfVariableNameList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベントセルフ変数名リスト
    /// </summary>
    internal class CommonEventSelfVariableNameList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大値</summary>
        public static readonly int ListMax = 100;

        /// <summary>リスト最小値</summary>
        public static readonly int ListMin = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 変数名リスト
        /// </summary>
        private List<string> VariableNameList { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEventSelfVariableNameList()
        {
            VariableNameList = new List<string>();
            for (var i = 0; i < ListMax; i++)
            {
                VariableNameList.Add("");
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="src">[NotNull] 変数名リスト</param>
        /// <exception cref="ArgumentNullException">srcがnullの場合</exception>
        /// <exception cref="ArgumentException">srcの要素数が100以外の場合</exception>
        public CommonEventSelfVariableNameList(List<string> src)
        {
            if (src == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(src)));

            var count = ListMax;
            if (src.Count != count)
                throw new ArgumentException(
                    $"{nameof(src)}の要素数は{count}である必要があります。");

            VariableNameList = src;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 変数名を更新する。
        /// </summary>
        /// <param name="number">[Range(0, 99)] 変数番号</param>
        /// <param name="variableName">[NotNull] 変数名</param>
        /// <exception cref="ArgumentOutOfRangeException">numberが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">variableNameがnullの場合</exception>
        public void UpdateVariableName(int number, string variableName)
        {
            int max = ListMax - 1;
            if (number < ListMin || max < number)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(number), ListMin, max, number));
            if (variableName == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(variableName)));

            VariableNameList[number] = variableName;
        }

        /// <summary>
        /// 変数名を取得する。
        /// </summary>
        /// <param name="number">[Range(0, 99)] 変数番号</param>
        /// <returns>変数名</returns>
        /// <exception cref="ArgumentOutOfRangeException">numberが指定範囲以外の場合</exception>
        public string GetVariableName(int number)
        {
            int max = ListMax - 1;
            if (number < ListMin || max < number)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(number), ListMin, max, number));

            return VariableNameList[number];
        }

        /// <summary>
        /// すべての変数名を取得する。
        /// </summary>
        /// <returns>変数名の集合</returns>
        public IEnumerable<string> GetAllName()
        {
            return VariableNameList;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            foreach (var varName in VariableNameList)
            {
                result.AddRange(new WoditorString(varName).StringByte);
            }

            return result.ToArray();
        }
    }
}