// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthAutoTileFileNameList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// 長さ固定オートタイルファイル名リスト
    /// </summary>
    [Obsolete("AutoTileFileNameList クラスを参照してください。 Ver 2.5 で削除します。")]
    public interface IFixedLengthAutoTileFileNameList : IFixedLengthList<AutoTileFileName>
    {
    }
}
