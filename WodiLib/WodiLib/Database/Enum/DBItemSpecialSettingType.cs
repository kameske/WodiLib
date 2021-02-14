// ========================================
// Project Name : WodiLib
// File Name    : DBItemSpecialSettingType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目値特殊指定タイプ
    /// </summary>
    public record DBItemSpecialSettingType : TypeSafeEnum<DBItemSpecialSettingType>
    {
        /// <summary>特殊な指定方法を使用しない</summary>
        public static readonly DBItemSpecialSettingType Normal;

        /// <summary>ファイル読み込み</summary>
        public static readonly DBItemSpecialSettingType LoadFile;

        /// <summary>データベース参照</summary>
        public static readonly DBItemSpecialSettingType ReferDatabase;

        /// <summary>選択肢を手動生成</summary>
        public static readonly DBItemSpecialSettingType Manual;

        static DBItemSpecialSettingType()
        {
            Normal = new DBItemSpecialSettingType(nameof(Normal), 0x00);
            LoadFile = new DBItemSpecialSettingType(nameof(LoadFile), 0x01);
            ReferDatabase = new DBItemSpecialSettingType(nameof(ReferDatabase), 0x02);
            Manual = new DBItemSpecialSettingType(nameof(Manual), 0x03);
        }

        private DBItemSpecialSettingType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public byte Code { get; }

        /// <summary>
        /// コード値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static DBItemSpecialSettingType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
