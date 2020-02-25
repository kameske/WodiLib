// ========================================
// Project Name : WodiLib
// File Name    : ProjectBackupType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// バックアップ種別
    /// </summary>
    public class ProjectBackupType : TypeSafeEnum<ProjectBackupType>
    {
        /// <summary>バックアップしない</summary>
        public static readonly ProjectBackupType None;

        /// <summary>1回分のみ残す</summary>
        public static readonly ProjectBackupType Once;

        /// <summary>3回分残す</summary>
        public static readonly ProjectBackupType ThreeTimes;

        /// <summary>5回分残す</summary>
        public static readonly ProjectBackupType FiveTimes;

        /// <summary>毎月1日と15日に保存</summary>
        public static readonly ProjectBackupType Regularly;

        /// <summary>1回分のみ＋毎月1日と15日に保存</summary>
        public static readonly ProjectBackupType RegularlyAndOnce;

        /// <summary>3回分＋毎月1日と15日に保存</summary>
        public static readonly ProjectBackupType RegularlyAndThreeTimes;

        /// <summary>5回分＋毎月1日と15日に保存</summary>
        public static readonly ProjectBackupType RegularlyAndFiveTimes;


        static ProjectBackupType()
        {
            None = new ProjectBackupType(nameof(None), "0");
            Once = new ProjectBackupType(nameof(Once), "1");
            ThreeTimes = new ProjectBackupType(nameof(ThreeTimes), "3");
            FiveTimes = new ProjectBackupType(nameof(FiveTimes), "5");
            Regularly = new ProjectBackupType(nameof(Regularly), "10000");
            RegularlyAndOnce = new ProjectBackupType(nameof(RegularlyAndOnce), "10001");
            RegularlyAndThreeTimes = new ProjectBackupType(nameof(RegularlyAndThreeTimes), "10003");
            RegularlyAndFiveTimes = new ProjectBackupType(nameof(RegularlyAndFiveTimes), "10005");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public ProjectBackupType(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public string Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">[NotNull] コード</param>
        /// <returns>ProjectBackupType</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static ProjectBackupType FromCode(string code)
        {
            if (code is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(code)));

            try
            {
                return _FindFirst(x => x.Code == code);
            }
            catch
            {
                var exception = new ArgumentException($"{nameof(ProjectBackupType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        ///     対象コードが存在しない場合はデフォルト値を返す。
        /// </summary>
        /// <param name="code">[Nullable] コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        public static ProjectBackupType FromCodeOrDefault(string code)
        {
            if (code is null || code.Equals(string.Empty)) return ThreeTimes;
            return FromCode(code);
        }
    }
}