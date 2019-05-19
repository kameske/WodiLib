// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProject.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBプロジェクトデータ（XXXDatabase.project）
    /// </summary>
    public class DatabaseProject
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[Nullable] DB種別</summary>
        public DBKind DBKind { get; set; }

        private DBTypeSettingList typeSettingList = new DBTypeSettingList();

        /// <summary>
        /// [NotNull] タイプ設定リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBTypeSettingList TypeSettingList
        {
            get => typeSettingList;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(TypeSettingList)));
                typeSettingList = value;
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

            result.AddRange(TypeSettingList.ToBinary());

            return result.ToArray();
        }
    }
}