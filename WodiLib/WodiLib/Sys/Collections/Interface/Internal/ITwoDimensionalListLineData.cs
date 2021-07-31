// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalListLineData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// 二次元リスト行/列データインタフェース
    /// </summary>
    /// <typeparam name="TItem">要素型</typeparam>
    /// <typeparam name="TProp">情報型</typeparam>
    internal interface ITwoDimensionalListLineData<out TItem, out TProp>
    {
        IEnumerable<TItem> Values { get; }
        TProp Properties { get; }
    }
}
