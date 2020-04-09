// ========================================
// Project Name : WodiLib
// File Name    : Layer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// Layer実装クラス
    /// </summary>
    [Serializable]
    public class Layer : ModelBase<Layer>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private MapChipList chips = new MapChipList(MapSizeWidth.MinValue, MapSizeHeight.MinValue);

        /// <summary>[NotNull] マップチップ番号リスト</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapChipList Chips
        {
            get => chips;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Chips)));
                chips.PropertyChanged -= OnChipsPropertyChanged;
                chips = value;
                chips.PropertyChanged += OnChipsPropertyChanged;
                NotifyPropertyChanged();
            }
        }

        /// <summary>[Range(20, 99999)] サイズ横</summary>
        public MapSizeWidth Width => chips.Width;

        /// <summary>[Range(15, 99999)] サイズ縦</summary>
        public MapSizeHeight Height => chips.Height;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップサイズ横を更新する。
        /// </summary>
        /// <param name="value">サイズ横</param>
        public void UpdateWidth(MapSizeWidth value)
        {
            chips.UpdateWidth(value);
        }

        /// <summary>
        /// マップサイズ縦を更新する。
        /// </summary>
        /// <param name="value">マップサイズ縦</param>
        public void UpdateHeight(MapSizeHeight value)
        {
            chips.UpdateHeight(value);
        }

        /// <summary>
        /// マップサイズ縦を更新する。
        /// </summary>
        /// <param name="width">サイズ横</param>
        /// <param name="height">マップサイズ縦</param>
        public void UpdateSize(MapSizeWidth width, MapSizeHeight height)
        {
            chips.UpdateWidth(width);
            chips.UpdateHeight(height);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(Layer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return chips.Equals(other.chips);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップチッププロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnChipsPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(MapChipList.Width):
                case nameof(MapChipList.Height):
                    NotifyPropertyChanged(args.PropertyName);
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Layer()
        {
            chips.PropertyChanged += OnChipsPropertyChanged;
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