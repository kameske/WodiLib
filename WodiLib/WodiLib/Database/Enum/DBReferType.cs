// ========================================
// Project Name : WodiLib
// File Name    : DBReferType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using Commons;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目特殊指定「DB参照」の参照先
    /// </summary>
    public class DBReferType : TypeSafeEnum<DBReferType>
    {
        /// <summary>可変DB</summary>
        public static readonly DBReferType Changeable;

        /// <summary>ユーザDB</summary>
        public static readonly DBReferType User;

        /// <summary>システムDB</summary>
        public static readonly DBReferType System;

        /// <summary>コモンイベント</summary>
        public static readonly DBReferType CommonEvent;

        static DBReferType()
        {
            Changeable = new DBReferType(nameof(Changeable), 2);
            User = new DBReferType(nameof(User), 1);
            System = new DBReferType(nameof(System), 0);
            CommonEvent = new DBReferType(nameof(CommonEvent), 3);
        }

        private DBReferType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>DBデータ設定種別コード</summary>
        public int Code { get; }

        /// <summary>
        ///     DBデータ種別設定コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">引数特殊指定値</param>
        /// <returns>DBReferType</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBReferType FromCode(int code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch (Exception)
            {
                throw new ArgumentException($"DBReferTypeの取得に失敗しました。条件値：{code}");
            }
        }
    }
}