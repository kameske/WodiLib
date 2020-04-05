// ========================================
// Project Name : WodiLib
// File Name    : MapChipList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Sys;
using ThisPropertyChangedHelper = WodiLib.Map.MapChipListCollectionChangedHelper;

namespace WodiLib.Map
{
    /// <summary>
    /// マップチップ配列クラス
    /// </summary>
    [Serializable]
    public class MapChipList : RestrictedCapacityCollection<IFixedLengthMapChipColumns>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static readonly int MaxCapacity = MapSizeWidth.MaxValue;

        /// <summary>リスト最小数</summary>
        public static readonly int MinCapacity = MapSizeWidth.MinValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>サイズ横</summary>
        public MapSizeWidth Width => Items.Count;

        // コンストラクタ中で呼ばれる場合は Items.Count == 0 となる
        /// <summary>サイズ縦</summary>
        public MapSizeHeight Height => Items.Count > 0 ? Items[0].Count : MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自身コレクション変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnThisCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ThisPropertyChangedHelper.UpdateItemPropertyChangedEvent(args, Items, OnItem0PropertyChanged);
        }

        /// <summary>
        /// 要素0のプロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnItem0PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(Count):
                    NotifyPropertyChanged(nameof(Height));
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapChipList() : this(MapSizeWidth.MinValue, MapSizeHeight.MinValue)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="width">マップサイズ横</param>
        /// <param name="height">マップサイズ縦</param>
        public MapChipList(MapSizeWidth width, MapSizeHeight height)
        {
            InitializeChips(width, height);
            CollectionChanged += OnThisCollectionChanged;
            this[0].PropertyChanged += OnItem0PropertyChanged;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mapChipList">マップチップ番号リスト</param>
        /// <exception cref="ArgumentNullException">mapChipListがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">mapChipListの横幅、または縦幅が指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">縦幅が異なる列データが存在する場合</exception>
        public MapChipList(IReadOnlyList<IReadOnlyList<MapChip>> mapChipList)
        {
            if (mapChipList is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(mapChipList)));

            var width = mapChipList.Count;
            if (width < MapSizeWidth.MinValue || MapSizeWidth.MaxValue < width)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange("mapChipListの要素数", MapSizeWidth.MinValue, MapSizeWidth.MaxValue, width));
            var height = mapChipList.First().Count;

            var chips = new List<IFixedLengthMapChipColumns>();

            var h = 0;
            foreach (var line in mapChipList)
            {
                var lineHeight = line.Count;
                if (lineHeight < MapSizeHeight.MinValue || MapSizeHeight.MaxValue < lineHeight)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange($"mapChipList[{h}の要素数", MapSizeHeight.MinValue, MapSizeHeight.MaxValue,
                            lineHeight));
                if (line.Count != height)
                    throw new ArgumentException($"{h}行目の縦幅が他の行と異なります。（マップ縦幅はすべての行で同じにする必要があります。）");

                chips.Add(new MapChipColumns(line));

                h++;
            }

            Overwrite(0, chips);

            CollectionChanged += OnThisCollectionChanged;
            this[0].PropertyChanged += OnItem0PropertyChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
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
        public void InitializeChips() => InitializeChips(Width, Height);

        /// <summary>
        /// マップチップ情報を初期化する。
        /// </summary>
        /// <param name="width">マップサイズ横</param>
        /// <param name="height">マップサイズ縦</param>
        public void InitializeChips(int width, int height)
        {
            var initChips = new List<IFixedLengthMapChipColumns>();
            for (var i = 0; i < width; i++)
            {
                var initChipsLine = new MapChipColumns();
                initChipsLine.InitializeChips(height);

                initChips.Add(initChipsLine);
            }

            Overwrite(0, initChips);

            NotifyPropertyChanged(nameof(Width));
            NotifyPropertyChanged(nameof(Height));
        }

        /// <summary>
        /// サイズ横を更新する。
        /// </summary>
        /// <param name="value">マップサイズ横</param>
        public void UpdateWidth(MapSizeWidth value)
        {
            AdjustLength(value);
            NotifyPropertyChanged(nameof(Width));
        }

        /// <summary>
        /// マップサイズ縦を更新する。
        /// </summary>
        /// <param name="value">マップサイズ縦</param>
        public void UpdateHeight(MapSizeHeight value)
        {
            foreach (var fixedLengthColumn in this)
            {
                if (!(fixedLengthColumn is MapChipColumns column))
                {
                    // 通常ここには来ない
                    throw new InvalidOperationException();
                }

                column.UpdateSize(value);
            }
        }

        /// <summary>
        /// マップサイズ縦を更新する。
        /// </summary>
        /// <param name="width">サイズ横</param>
        /// <param name="height">マップサイズ縦</param>
        public void UpdateSize(MapSizeWidth width, MapSizeHeight height)
        {
            UpdateWidth(width);
            UpdateHeight(height);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override IFixedLengthMapChipColumns MakeDefaultItem(int index)
            => new MapChipColumns(Height);

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

            foreach (var chipColumn in this)
                result.AddRange(chipColumn.ToBinary());

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
        protected MapChipList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}