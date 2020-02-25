// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueNumberDrawType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// データの設定方法種別
    /// </summary>
    public class DatabaseValueNumberDrawType : TypeSafeEnum<DatabaseValueNumberDrawType>
    {
        /// <summary>手動で設定</summary>
        public static readonly DatabaseValueNumberDrawType Off;

        /// <summary>最初の文字列データと同じ</summary>
        public static readonly DatabaseValueNumberDrawType On;

        static DatabaseValueNumberDrawType()
        {
            Off = new DatabaseValueNumberDrawType(nameof(Off), "0");
            On = new DatabaseValueNumberDrawType(nameof(On), "1");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public DatabaseValueNumberDrawType(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public string Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">[NotNull] コード</param>
        /// <returns>DatabaseValueNumberDrawType</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static DatabaseValueNumberDrawType FromCode(string code)
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
                var exception = new ArgumentException($"{nameof(DatabaseValueNumberDrawType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        ///     対象コードが存在しない場合はデフォルト値を返す。
        /// </summary>
        /// <param name="code">[Nullable] コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        public static DatabaseValueNumberDrawType FromCodeOrDefault(string code)
        {
            if (code is null || code.Equals(string.Empty)) return Off;
            return FromCode(code);
        }
    }
}