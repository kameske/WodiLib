// ========================================
// Project Name : WodiLib
// File Name    : Layer.cs
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
    /// Layer実装クラス
    /// </summary>
    public class Layer
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private MapChipList chips;

        /// <summary>[NotNull] マップチップ番号リスト</summary>
        public IEnumerable<IEnumerable<MapChip>> Chips => chips.Chips;

        /// <summary>
        /// マップチップ番号リストをセットする。
        /// </summary>
        /// <param name="value">[NotNull] マップチップ番号リスト</param>
        /// <exception cref="PropertyNullException">value が null の場合</exception>
        /// <exception cref="ArgumentException">リストサイズ横または縦の要素数がマップサイズ最小以下の場合</exception>
        public void SetChips(IEnumerable<IEnumerable<MapChip>> value)
        {
            var mapChipList = value as IEnumerable<MapChip>[] ?? value.ToArray();
            var valList = mapChipList.Select(x => x.ToList()).ToList();

            if (value == null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(Chips)));
            if (valList.Count < MapSizeWidth.MinValue)
                throw new ArgumentException($"マップサイズ横は{MapSizeWidth.MinValue}以上である必要があります。");
            var height = valList.First().Count;
            var h = 0;
            foreach (var line in valList)
            {
                if (line.Count < MapSizeHeight.MinValue)
                    throw new ArgumentException($"マップサイズ縦は{MapSizeHeight.MinValue}以上である必要があります。");
                if (line.Count != height)
                    throw new ArgumentException($"{h}行目の縦幅が他の行と異なります。（マップ縦幅はすべての行で同じにする必要があります。）");
                h++;
            }

            chips = new MapChipList(mapChipList);
        }

        /// <summary>サイズ横</summary>
        public MapSizeWidth Width => chips.Width;

        /// <summary>
        /// マップサイズ横を更新する。
        /// </summary>
        /// <param name="value">サイズ横</param>
        /// <exception cref="ArgumentOutOfRangeException">valueが0～999999以外の場合</exception>
        public void UpdateWidth(MapSizeWidth value)
        {
            chips.UpdateWidth(value);
        }

        /// <summary>[Range(15, 9999999)] サイズ縦</summary>
        public MapSizeHeight Height => chips.Height;

        /// <summary>
        /// マップサイズ縦を更新する。
        /// </summary>
        /// <param name="value">マップサイズ縦</param>
        public void UpdateHeight(MapSizeHeight value)
        {
            chips.UpdateHeight(value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップチップ番号をセットする。
        /// </summary>
        /// <param name="x">[Range(0, 現在マップサイズ横)] X座標</param>
        /// <param name="y">[Range(0, 現在マップサイズ縦)] Y座標</param>
        /// <param name="chipId">マップチップID</param>
        /// <exception cref="ArgumentOutOfRangeException">x, yが範囲外の場合</exception>
        public void SetChip(int x, int y, MapChip chipId)
        {
            chips.UpdateChip(x, y, chipId);
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
            return chips.GetChip(x, y);
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

            foreach (var chipColumn in Chips)
            foreach (var chip in chipColumn)
                result.AddRange(((int) chip).ToBytes(Endian.Woditor));

            return result.ToArray();
        }
    }
}