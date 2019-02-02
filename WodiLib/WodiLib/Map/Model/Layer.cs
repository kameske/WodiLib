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
    public class Layer : IWodiLibObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Layer()
        {
            var initChips = new List<List<MapChip>>();
            for (var i = 0; i < MapData.MapWidthMin; i++)
            {
                var initChipsLine = new List<MapChip>();
                for (var j = 0; j < MapData.MapHeightMin; j++)
                {
                    initChipsLine.Add(new MapChip());
                }

                initChips.Add(initChipsLine);
            }

            chips = initChips;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            foreach (var chipColumn in chips)
            foreach (var chip in chipColumn)
                result.AddRange(chip.Value.ToBytes(Endian.Woditor));

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private List<List<MapChip>> chips;

        /// <summary>[NotNull] マップチップ番号リスト</summary>
        public IEnumerable<IEnumerable<MapChip>> Chips => chips;

        /// <summary>
        /// マップチップ番号リストをセットする。
        /// </summary>
        /// <param name="value">[NotNull] マップチップ番号リスト</param>
        /// <exception cref="PropertyNullException">value が null の場合</exception>
        /// <exception cref="ArgumentException">リストサイズ横または縦の要素数がマップサイズ最小以下の場合</exception>
        public void SetChips(IEnumerable<IEnumerable<MapChip>> value)
        {
            var valList = value.Select(x => x.ToList()).ToList();

            if (value == null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(Chips)));
            if (valList.Count < MapData.MapWidthMin)
                throw new ArgumentException($"マップサイズ横は{MapData.MapWidthMin}以上である必要があります。");
            var height = valList.First().Count;
            var h = 0;
            foreach (var line in valList)
            {
                if (line.Count < MapData.MapHeightMin)
                    throw new ArgumentException($"マップサイズ縦は{MapData.MapHeightMin}以上である必要があります。");
                if (line.Count != height)
                    throw new ArgumentException($"{h}行目の縦幅が他の行と異なります。（マップ縦幅はすべての行で同じにする必要があります。）");
                h++;
            }

            chips = valList;
        }

        /// <summary>[Range(20, 999999)] サイズ横</summary>
        public int Width => chips.Count;

        /// <summary>
        /// サイズ横をセットする。
        /// </summary>
        /// <param name="value">[Range[20, 999999] サイズ横</param>
        /// <exception cref="ArgumentOutOfRangeException">valueが0～999999以外の場合</exception>
        public void SetWidth(int value)
        {
            if (value < MapData.MapWidthMin || MapData.MapWidthMax < value)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(value), MapData.MapWidthMin, MapData.MapWidthMax, value));
            }

            if (chips.Count > value)
            {
                chips.RemoveRange(value, chips.Count - value);
            }
            else if (chips.Count < value)
            {
                for (var i = chips.Count; i < value; i++)
                {
                    var chipLine = new List<MapChip>();
                    for (var j = 0; j < Height; j++)
                    {
                        chipLine.Add(new MapChip());
                    }

                    chips.Add(chipLine);
                }
            }
        }

        /// <summary>[Range(15, 9999999)] サイズ縦</summary>
        public int Height => chips.First().Count;

        /// <summary>
        /// マップサイズ縦をセットする。
        /// </summary>
        /// <param name="value">[Range(15, 999999)] マップサイズ縦</param>
        /// <exception cref="ArgumentOutOfRangeException">valueが15～999999以外の場合</exception>
        public void SetHeight(int value)
        {
            if (value < MapData.MapHeightMin || MapData.MapHeightMax < value)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(value), MapData.MapHeightMin, MapData.MapHeightMax, value));
            }

            var updateList = new List<List<MapChip>>();
            foreach (var chipLine in chips)
            {
                var chipLineList = chipLine.ToList();
                if (chipLineList.Count > value)
                {
                    chipLineList.RemoveRange(value, chipLineList.Count - value);
                }
                else if (chipLineList.Count < value)
                {
                    while (chipLineList.Count < value)
                    {
                        chipLineList.Add(new MapChip());
                    }
                }

                updateList.Add(chipLineList);
            }

            chips = updateList;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップチップ番号をセットする。
        /// </summary>
        /// <param name="x">[Range(0, 現在マップサイズ横)] X座標</param>
        /// <param name="y">[Range(0, 現在マップサイズ縦)] Y座標</param>
        /// <param name="chipId">[NotNull] マップチップID</param>
        /// <exception cref="ArgumentOutOfRangeException">x, yが範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">chipIdがnullの場合</exception>
        public void SetChip(int x, int y, MapChip chipId)
        {
            if (x < 0 || Width < x)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(x), 0, Width - 1, x));
            if (y < 0 || Height < y)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(y), 0, Height - 1, y));
            if (chipId == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(chipId)));

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
    }
}