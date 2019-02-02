// ========================================
// Project Name : WodiLib
// File Name    : DBKind.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// DB種別
    /// </summary>
    public class DBKind : TypeSafeEnum<DBKind>
    {
        /// <summary>可変DB</summary>
        public static readonly DBKind Changeable;

        /// <summary>ユーザDB</summary>
        public static readonly DBKind User;

        /// <summary>システムDB</summary>
        public static readonly DBKind System;

        /// <summary>Null</summary>
        public static readonly DBKind Null;

        static DBKind()
        {
            Changeable = new DBKind(nameof(Changeable), 0x00, 10);
            User = new DBKind(nameof(User), 0x02, 11);
            System = new DBKind(nameof(System), 0x01, 13);
            Null = new DBKind(nameof(Null), 0xFF, 0);
        }

        private DBKind(string id, byte code, int value) : base(id)
        {
            Code = code;
            Value = value;
        }

        /// <summary>DB値</summary>
        public int Value { get; }

        /// <summary>コード値</summary>
        public byte Code { get; }

        /// <summary>
        ///     DB値からオブジェクトを取得する。
        /// </summary>
        /// <param name="num">DB値</param>
        /// <returns>DBType</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBKind FromInt(int num)
        {
            try
            {
                return _FindFirst(x => x.Value == num);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"DBTypeの取得に失敗しました。条件値：{num}");
                throw exception;
            }
        }

        /// <summary>
        ///     DBコードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">DBコード</param>
        /// <returns>DBType</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBKind FromCode(byte code)
        {
            try
            {
                return _FindFirst(x => x.Code == code);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"DBTypeの取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }
    }
}