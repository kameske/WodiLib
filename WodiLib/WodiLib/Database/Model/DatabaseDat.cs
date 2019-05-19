// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDat.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBデータ（XXXDatabase.dat）
    /// </summary>
    public class DatabaseDat
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルヘッダ
        /// </summary>
        public static readonly byte[] Header =
        {
            0x00, 0x57, 0x00, 0x00, 0x4F, 0x4C, 0x00, 0x46, 0x4D, 0x00, 0xC1
        };

        /// <summary>
        /// ファイルフッタ
        /// </summary>
        public static readonly byte[] Footer =
        {
            0xC1
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[Nullable] DB種別</summary>
        public DBKind DBKind { get; set; }

        private DBDataSettingList settingList = new DBDataSettingList();

        /// <summary>
        /// [NotNull] タイプ設定リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBDataSettingList SettingList
        {
            get => settingList;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SettingList)));
                settingList = value;
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

            // DBデータ設定
            result.AddRange(SettingList.ToBinary());

            // ファイルフッタ
            result.AddRange(Footer);

            return result.ToArray();
        }
    }
}