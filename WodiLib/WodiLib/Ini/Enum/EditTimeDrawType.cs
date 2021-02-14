// ========================================
// Project Name : WodiLib
// File Name    : EditTimeDrawType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    ///     編集時間表示種別
    /// </summary>
    public record EditTimeDrawType : TypeSafeEnum<EditTimeDrawType>
    {
        /// <summary>OFF</summary>
        public static readonly EditTimeDrawType Off;

        /// <summary>ON</summary>
        public static readonly EditTimeDrawType On;

        /// <summary>コード値</summary>
        public string Code { get; }

        static EditTimeDrawType()
        {
            Off = new EditTimeDrawType(nameof(Off), "0");
            On = new EditTimeDrawType(nameof(On), "1");
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        private EditTimeDrawType(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>EditTimeDrawType</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static EditTimeDrawType FromCode(string code)
        {
            if (code is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(code)));

            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch
            {
                var exception = new ArgumentException($"{nameof(EditTimeDrawType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        ///     対象コードが存在しない場合はデフォルト値を返す。
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        public static EditTimeDrawType FromCodeOrDefault(string? code)
        {
            if (code is null || code.Equals(string.Empty)) return On;
            return FromCode(code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
