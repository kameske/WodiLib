// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthMapChipColumns.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Map
{
    /// <summary>
    /// 長さ固定マップチップリスト
    /// </summary>
    public interface IFixedLengthMapChipColumns : IFixedLengthList<MapChip>,
        IEquatable<IFixedLengthMapChipColumns>
    {
        /// <summary>
        /// マップチップ情報を初期化する。
        /// </summary>
        void InitializeChips();

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        byte[] ToBinary();
    }
}
