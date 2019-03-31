// ========================================
// Project Name : WodiLib
// File Name    : MapChipList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップチップ配列クラス
    /// </summary>
    public class MapChipList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private List<List<MapChip>> chips;

        /// <summary>[NotNull] マップチップ番号リスト</summary>
        public IEnumerable<IEnumerable<MapChip>> Chips => chips;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapChipList()
        {
            InitializeChips(MapSizeWidth.MinValue, MapSizeHeight.MinValue);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mapChipList">マップチップ番号リスト</param>
        /// <exception cref="ArgumentNullException">mapChipListがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">mapChipListの横幅、または縦幅が指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">縦幅が異なる列データが存在する場合</exception>
        public MapChipList(IEnumerable<IEnumerable<MapChip>> mapChipList)
        {
            if (mapChipList == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(Chips)));
            var valList = mapChipList.Select(x => x.ToList()).ToList();

            var width = valList.Count;
            if (width < MapSizeWidth.MinValue || MapSizeWidth.MaxValue < width)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange("mapChipListの要素数", MapSizeWidth.MinValue, MapSizeWidth.MaxValue, width));
            var height = valList.First().Count;
            var h = 0;
            foreach (var line in valList)
            {
                var lineHeight = line.Count;
                if (lineHeight < MapSizeHeight.MinValue || MapSizeHeight.MaxValue < lineHeight)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange($"mapChipList[{h}の要素数", MapSizeHeight.MinValue, MapSizeHeight.MaxValue,
                            lineHeight));
                if (line.Count != height)
                    throw new ArgumentException($"{h}行目の縦幅が他の行と異なります。（マップ縦幅はすべての行で同じにする必要があります。）");
                h++;
            }

            chips = valList;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップチップ情報を初期化する。
        /// </summary>
        /// <param name="width">マップサイズ横</param>
        /// <param name="height">マップサイズ縦</param>
        public void InitializeChips(int width, int height)
        {
            var initChips = new List<List<MapChip>>();
            for (var i = 0; i < width; i++)
            {
                var initChipsLine = new List<MapChip>();
                for (var j = 0; j < height; j++)
                {
                    initChipsLine.Add(MapChip.Default);
                }

                initChips.Add(initChipsLine);
            }

            chips = initChips;
        }

        /// <summary>サイズ横</summary>
        public MapSizeWidth Width => chips.Count;

        /// <summary>
        /// サイズ横を更新する。
        /// </summary>
        /// <param name="value">サイズ横</param>
        /// <exception cref="ArgumentOutOfRangeException">valueが0～999999以外の場合</exception>
        public void UpdateWidth(MapSizeWidth value)
        {
            var intValue = (int) value;

            if (chips.Count > intValue)
            {
                chips.RemoveRange(intValue, chips.Count - intValue);
            }
            else if (chips.Count < intValue)
            {
                for (var i = chips.Count; i < intValue; i++)
                {
                    var chipLine = new List<MapChip>();
                    for (var j = 0; j < (int) Height; j++)
                    {
                        chipLine.Add(MapChip.Default);
                    }

                    chips.Add(chipLine);
                }
            }
        }

        /// <summary>サイズ縦</summary>
        public MapSizeHeight Height => chips.First().Count;

        /// <summary>
        /// マップサイズ縦を更新する。
        /// </summary>
        /// <param name="value">マップサイズ縦</param>
        /// <exception cref="ArgumentOutOfRangeException">valueが15～999999以外の場合</exception>
        public void UpdateHeight(MapSizeHeight value)
        {
            var intValue = (int) value;

            var updateList = new List<List<MapChip>>();
            foreach (var chipLine in chips)
            {
                var chipLineList = chipLine.ToList();
                if (chipLineList.Count > intValue)
                {
                    chipLineList.RemoveRange(intValue, chipLineList.Count - intValue);
                }
                else if (chipLineList.Count < intValue)
                {
                    while (chipLineList.Count < intValue)
                    {
                        chipLineList.Add(MapChip.Default);
                    }
                }

                updateList.Add(chipLineList);
            }

            chips = updateList;
        }

        /// <summary>
        /// マップチップ番号を更新する。
        /// </summary>
        /// <param name="x">[Range(0, 現在マップサイズ横)] X座標</param>
        /// <param name="y">[Range(0, 現在マップサイズ縦)] Y座標</param>
        /// <param name="chipId">マップチップID</param>
        /// <exception cref="ArgumentOutOfRangeException">x, yが範囲外の場合</exception>
        public void UpdateChip(int x, int y, MapChip chipId)
        {
            if (x < 0 || Width < x)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(x), 0, Width - 1, x));
            if (y < 0 || Height < y)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(y), 0, Height - 1, y));

            chips[x][y] = chipId;
        }

        /// <summary>
        /// マップチップ番号を取得する。
        /// </summary>
        /// <param name="x">[Range(0, 現在マップサイズ横-1)] X座標</param>
        /// <param name="y">[Range(0, 現在マップサイズ縦-1)] Y座標</param>
        /// <returns>マップチップ番号</returns>
        /// <exception cref="ArgumentOutOfRangeException">x, yが範囲外の場合</exception>
        public MapChip GetChip(int x, int y)
        {
            if (x < 0 || Width < x)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(x), 0, Width - 1, x));
            if (y < 0 || Height < y)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(y), 0, Height - 1, y));
            return chips[x][y];
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

            foreach (var chipColumn in chips)
            foreach (var chip in chipColumn)
                result.AddRange(((int) chip).ToBytes(Endian.Woditor));

            return result.ToArray();
        }
    }
}