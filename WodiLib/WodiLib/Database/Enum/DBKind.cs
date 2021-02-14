// ========================================
// Project Name : WodiLib
// File Name    : DBKind.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB種別
    /// </summary>
    public record DBKind : TypeSafeEnum<DBKind>
    {
        /// <summary>可変DB</summary>
        public static readonly DBKind Changeable;

        /// <summary>ユーザDB</summary>
        public static readonly DBKind User;

        /// <summary>システムDB</summary>
        public static readonly DBKind System;

        static DBKind()
        {
            Changeable = new DBKind(nameof(Changeable), 0x00, 10,
                2, 3, "可変");
            User = new DBKind(nameof(User), 0x02, 11,
                1, 2, "ﾕｰｻﾞ");
            System = new DBKind(nameof(System), 0x01, 13,
                0, 1, "ｼｽﾃﾑ");
        }

        private DBKind(string id, byte code, int targetCode, int specialArgCode,
            int dBDataSettingTypeCode, string eventCommandSentence) : base(id)
        {
            Code = code;
            TargetCode = targetCode;
            SpecialArgCode = specialArgCode;
            DBDataSettingTypeCode = dBDataSettingTypeCode;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>対象DBコード</summary>
        public int TargetCode { get; }

        /// <summary>コード値</summary>
        public byte Code { get; }

        /// <summary>引数特殊指定DBコード</summary>
        public int SpecialArgCode { get; }

        /// <summary>DBデータ設定種別コード</summary>
        public int DBDataSettingTypeCode { get; }

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        /// <summary>
        ///     DB値からオブジェクトを取得する。
        /// </summary>
        /// <param name="num">DB値</param>
        /// <returns>DBKind</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBKind FromTargetCode(int num)
        {
            try
            {
                return AllItems.First(x => x.TargetCode == num);
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
                return AllItems.First(x => x.Code == code);
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
        /// <param name="num">引数特殊指定値</param>
        /// <returns>DBKind</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBKind FromSpecialArgCode(int num)
        {
            try
            {
                return AllItems.First(x => x.SpecialArgCode == num);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"DBKindの取得に失敗しました。条件値：{num}");
                throw exception;
            }
        }

        /// <summary>
        ///     DBデータ種別設定コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="num">引数特殊指定値</param>
        /// <returns>DBKind</returns>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DBKind FromDBDataSettingTypeCode(int num)
        {
            try
            {
                return AllItems.First(x => x.DBDataSettingTypeCode == num);
            }
            catch (Exception)
            {
                throw new ArgumentException($"DBKindの取得に失敗しました。条件値：{num}");
            }
        }
    }
}
