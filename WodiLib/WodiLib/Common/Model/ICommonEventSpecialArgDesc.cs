// ========================================
// Project Name : WodiLib
// File Name    : ICommonEventSpecialArgDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント引数特殊指定情報インタフェース
    /// </summary>
    internal interface ICommonEventSpecialArgDesc
    {
        /// <summary>
        /// [NotNull] 引数名
        /// </summary>
        string ArgName { get; }

        /// <summary>
        /// 引数特殊指定タイプ
        /// </summary>
        CommonEventArgType ArgType { get; }

        /// <summary>
        /// 数値引数の初期値
        /// </summary>
        /// <exception cref="InvalidOperationException">文字列引数の場合</exception>
        int InitValue { get; }

        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        List<CommonEventSpecialArgCase> GetAllSpecialCase();

        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        List<int> GetAllSpecialCaseNumber();

        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        List<string> GetAllSpecialCaseDescription();
    }
}