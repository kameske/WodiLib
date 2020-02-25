// ========================================
// Project Name : WodiLib
// File Name    : MapChipColumns.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップチップ列
    /// </summary>
    [Serializable]
    public class MapChipColumns : RestrictedCapacityCollection<MapChip>, IFixedLengthMapChipColumns
        , IEquatable<MapChipColumns>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static readonly int MaxCapacity = MapSizeHeight.MaxValue;

        /// <summary>リスト最小数</summary>
        public static readonly int MinCapacity = MapSizeHeight.MinValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapChipColumns() : this(MinCapacity)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="height">初期サイズ高さ</param>
        public MapChipColumns(MapSizeHeight height)
        {
            InitializeChips(height);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="chips">[NotNull][LengthRange(MinCapacity, MaxCapacity)] マップチップリスト</param>
        /// <exception cref="ArgumentNullException">
        ///     chips が null の場合、
        ///     または chips に null 要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">chips の要素数が指定範囲外の場合</exception>
        public MapChipColumns(IReadOnlyList<MapChip> chips)
        {
            if (chips is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(chips)));
            if (chips.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(chips)));

            var length = chips.Count;
            if (length < MinCapacity || MaxCapacity < length)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(chips), MinCapacity, MaxCapacity, length));

            AdjustLength(length);
            for (var i = 0; i < length; i++)
            {
                this[i] = chips[i];
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Override Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップチップ情報を初期化する。
        /// </summary>
        public void InitializeChips() => InitializeChips(Count);

        /// <summary>
        /// マップチップ情報を初期化する。
        /// </summary>
        /// <param name="height">マップサイズ縦</param>
        public void InitializeChips(MapSizeHeight height)
        {
            AdjustLength(height);
            for (var i = 0; i < height; i++)
            {
                this[i] = MakeDefaultItem(i);
            }
        }

        /// <summary>
        /// サイズを更新する。
        /// </summary>
        /// <param name="height">更新後のサイズ</param>
        public void UpdateSize(MapSizeHeight height) => AdjustLength(height);

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(MapChipColumns other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return Equals((RestrictedCapacityCollection<MapChip>) other);
        }


        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IFixedLengthMapChipColumns other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            if (!(other is MapChipColumns casted)) return false;
            return Equals(casted);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Override Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override MapChip MakeDefaultItem(int index) => MapChip.Default;

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

            foreach (var chip in this)
                result.AddRange(((int) chip).ToBytes(Endian.Woditor));

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected MapChipColumns(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}