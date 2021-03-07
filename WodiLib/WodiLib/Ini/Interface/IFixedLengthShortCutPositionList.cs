// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthShortCutPositionList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Ini
{
    /// <summary>
    /// 【長さ固定】コマンド入力ウィンドウのコマンド表示順リスト
    /// </summary>
    [Obsolete("ShortCutPositionList クラスを参照してください。 Ver 2.5 で削除します。")]
    public interface IFixedLengthShortCutPositionList : IFixedLengthList<ShortCutPosition>
    {
    }
}
