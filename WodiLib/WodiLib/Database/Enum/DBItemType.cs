// ========================================
// Project Name : WodiLib
// File Name    : DBItemType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// 項目の設定方法種別
    /// </summary>
    public class DBItemType : TypeSafeEnum<DBItemType>
    {
        /// <summary>数値</summary>
        public static readonly DBItemType Int;

        /// <summary>文字列</summary>
        public static readonly DBItemType String;

        static DBItemType()
        {
            Int = new DBItemType(nameof(Int), 1);
            String = new DBItemType(nameof(String), 2);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        private DBItemType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public int Code { get; }

        /// <summary>種別順列基準値</summary>
        public int TypeOrderStart => Code * 1000;

        /// <summary>
        ///     DB設定デフォルト値
        ///     取得するたびに新たなインスタンスを生成する。
        /// </summary>
        public DBItemValue DBItemDefaultValue
        {
            get
            {
                if (this == Int) return new DBValueInt(0);
                if (this == String) return new DBValueString("")!;
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>DBDataSettingType</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBItemType FromCode(int code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"{nameof(DBItemType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        /// 設定値からオブジェクトを取得する。
        /// </summary>
        /// <param name="value">設定値</param>
        /// <returns>DBDataSettingType</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBItemType FromValue(int value)
        {
            try
            {
                return AllItems.First(x => x.Code == value.SubInt(3, 1));
            }
            catch (Exception)
            {
                throw new ArgumentException($"{nameof(DBItemType)}の取得に失敗しました。条件値：{value}");
            }
        }
    }
}
