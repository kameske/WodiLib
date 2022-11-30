// ========================================
// Project Name : WodiLib
// File Name    : DelegateMakeListDefaultItem.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自リスト内にてデフォルト要素を取得する必要がある場合に
    ///     デフォルト要素を生成する処理。
    /// </summary>
    public delegate T DelegateMakeListDefaultItem<out T>(int index);
}
