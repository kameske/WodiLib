// ========================================
// Project Name : WodiLib
// File Name    : DBDataSettingType.cs
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
    /// データの設定方法種別
    /// </summary>
    public class DBDataSettingType : TypeSafeEnum<DBDataSettingType>
    {
        /// <summary>手動で設定</summary>
        public static readonly DBDataSettingType Manual;

        /// <summary>最初の文字列データと同じ</summary>
        public static readonly DBDataSettingType FirstStringData;

        /// <summary>1つ前のタイプのデータIDと同じ</summary>
        public static readonly DBDataSettingType EqualBefore;

        /// <summary>指定DBの指定タイプから</summary>
        public static readonly DBDataSettingType DesignatedType;

        static DBDataSettingType()
        {
            Manual = new DBDataSettingType(nameof(Manual), 0);
            FirstStringData = new DBDataSettingType(nameof(FirstStringData), 1);
            EqualBefore = new DBDataSettingType(nameof(EqualBefore), 2);
            DesignatedType = new DBDataSettingType(nameof(DesignatedType), -1);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public DBDataSettingType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public int Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>DBDataSettingType</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBDataSettingType FromCode(int code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch
            {
                var exception = new ArgumentException($"{nameof(DBDataSettingType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        /// 設定値からオブジェクトを取得する。
        /// </summary>
        /// <param name="value">設定値</param>
        /// <returns>DBDataSettingType</returns>
        /// <exception cref="ArgumentException">不適切な値の場合</exception>
        public static DBDataSettingType FromValue(int value)
        {
            var exception = new ArgumentException($"{nameof(DBDataSettingType)}の取得に失敗しました。条件値：{value}");

            if ($"{value}".Length == 5)
            {
                // 値が5桁の場合は DesignatedType の可能性がある
                try
                {
                    var _ = DBKind.FromDBDataSettingTypeCode(value.SubInt(4, 1));
                    // DB種別が取得できる場合は DesignatedType
                    return DesignatedType;
                }
                catch
                {
                    // DB種別が取得できない場合は適切な値ではない
                    throw exception;
                }
            }

            try
            {
                return AllItems.First(x => x.Code == value);
            }
            catch
            {
                throw exception;
            }
        }
    }
}
