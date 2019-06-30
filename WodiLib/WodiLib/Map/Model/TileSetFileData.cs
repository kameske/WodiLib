// ========================================
// Project Name : WodiLib
// File Name    : TileSetFileData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイルセットファイルデータ
    /// </summary>
    public class TileSetFileData
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ヘッダ</summary>
        public static readonly byte[] Header =
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0x01, 0x00, 0x00, 0x00
        };

        /// <summary>フッタ</summary>
        public static readonly byte[] Footer =
        {
            0xCF
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private TileSetSetting tileSetSetting = new TileSetSetting();

        /// <summary>
        /// [NotNull] タイルセット設定
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public TileSetSetting TileSetSetting
        {
            get => tileSetSetting;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(TileSetSetting)));
                tileSetSetting = value;
            }
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

            // ヘッダ
            result.AddRange(Header);

            // タイルセット設定
            result.AddRange(TileSetSetting.ToBinary());

            // フッタ
            result.AddRange(Footer);

            return result.ToArray();
        }
    }
}