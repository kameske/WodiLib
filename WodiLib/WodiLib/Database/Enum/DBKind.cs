// ========================================
// Project Name : WodiLib
// File Name    : DBKind.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
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

        static DBKind()
        {
            Changeable = new DBKind(nameof(Changeable), 0x00, 10, 2);
            User = new DBKind(nameof(User), 0x02, 11, 1);
            System = new DBKind(nameof(System), 0x01, 13, 0);
        }

        private DBKind(string id, byte code, int targetCode, int specialArgCode) : base(id)
        {
            Code = code;
            TargetCode = targetCode;
            SpecialArgCode = specialArgCode;
        }

        /// <summary>対象DBコード</summary>
        public int TargetCode { get; }

        /// <summary>コード値</summary>
        public byte Code { get; }

        /// <summary>引数特殊指定DBコード</summary>
        public int SpecialArgCode { get; }

        /// <summary>
        ///     DB値からオブジェクトを取得する。
        /// </summary>
        /// <param name="num">DB値</param>
        /// <returns>DBType</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBKind FromTargetCode(int num)
        {
            try
            {
                return _FindFirst(x => x.TargetCode == num);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"DBKindの取得に失敗しました。条件値：{num}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象DBコードからオブジェクトを取得する。
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
                var exception = new ArgumentException($"DBKindの取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     引数特殊指定値からオブジェクトを取得する。
        /// </summary>
        /// <param name="num">引数特殊指定値から</param>
        /// <returns>DBKind</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBKind FromSpecialArgCode(int num)
        {
            try
            {
                return _FindFirst(x => x.SpecialArgCode == num);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"DBKindの取得に失敗しました。条件値：{num}");
                throw exception;
            }
        }
    }
}