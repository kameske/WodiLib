// ========================================
// Project Name : WodiLib
// File Name    : ChoiceCaseList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 選択肢リスト
    /// </summary>
    public class ChoiceCaseList
    {
        private readonly List<string> caseList = new List<string>
        {
            "", "", "", "", "",
            "", "", "", "", ""
        };

        private int caseValue = 1;

        /// <summary>
        /// [Range(1, 10)] 選択肢数
        /// </summary>
        /// <exception cref="PropertyOutOfRangeException">1～10以外の値を設定した場合</exception>
        public int CaseValue
        {
            get => caseValue;
            set
            {
                if (value < 1 || 10 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseValue), 1, 10, value));
                caseValue = value;
            }
        }

        /// <summary>
        /// 選択肢番号を指定して文字列を取得する。
        /// </summary>
        /// <param name="index">[Range(0, CaseValue - 1)] 選択肢番号</param>
        /// <returns>選択肢番号に対応した文字列</returns>
        /// <exception cref="ArgumentOutOfRangeException">0～選択肢最大番号以外の値を設定した場合</exception>
        public string Get(int index)
        {
            if (index < 0 || CaseValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, CaseValue - 1, index));
            return caseList[index];
        }

        /// <summary>
        /// 選択肢番号を指定して内容を更新する。
        /// </summary>
        /// <param name="index">[Range(0, CaseValue - 1)] 選択肢番号</param>
        /// <param name="src">[NotNull] 更新文字列</param>
        /// <exception cref="ArgumentOutOfRangeException">0～選択肢最大番号以外の値を設定した場合</exception>
        /// <exception cref="ArgumentNullException">srcがnullの場合</exception>
        public void Set(int index, string src)
        {
            if (index < 0 || CaseValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, CaseValue - 1, index));
            if (src == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(src)));
            caseList[index] = src;
        }
    }
}