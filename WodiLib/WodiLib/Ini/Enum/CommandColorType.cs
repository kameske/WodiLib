// ========================================
// Project Name : WodiLib
// File Name    : CommandColorType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// イベントコマンド配色
    /// </summary>
    public class CommandColorType : TypeSafeEnum<CommandColorType>
    {
        /// <summary>旧カラーセット（Ver2.00より前）</summary>
        public static readonly CommandColorType Old;

        /// <summary>タイプ0</summary>
        public static readonly CommandColorType Type0;

        /// <summary>タイプ1</summary>
        public static readonly CommandColorType Type1;

        static CommandColorType()
        {
            Old = new CommandColorType(nameof(Old), "__Old__");
            Type0 = new CommandColorType(nameof(Type0), "0");
            Type1 = new CommandColorType(nameof(Type1), "1");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public CommandColorType(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public string Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">[NotNull] コード</param>
        /// <returns>CommandColorType</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static CommandColorType FromCode(string code)
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
                var exception = new ArgumentException($"{nameof(CommandColorType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        ///     対象コードが存在しない場合はデフォルト値を返す。
        /// </summary>
        /// <param name="code">[Nullable] コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        public static CommandColorType FromCodeOrDefault(string code)
        {
            if (code is null || code.Equals(string.Empty)) return Type1;
            return FromCode(code);
        }
    }
}