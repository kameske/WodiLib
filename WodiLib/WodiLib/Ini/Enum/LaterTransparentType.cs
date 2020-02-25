// ========================================
// Project Name : WodiLib
// File Name    : LaterTransparentType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// マップ編集時の下レイヤーの暗さ種別
    /// </summary>
    public class LaterTransparentType : TypeSafeEnum<LaterTransparentType>
    {
        /// <summary>真っ暗</summary>
        public static readonly LaterTransparentType Black;

        /// <summary>かなり暗い</summary>
        public static readonly LaterTransparentType PrettyDark;

        /// <summary>そこそこ暗い</summary>
        public static readonly LaterTransparentType SomewhatDark;

        /// <summary>うっすら暗い</summary>
        public static readonly LaterTransparentType FaintlyDarker;

        /// <summary>暗くしない</summary>
        public static readonly LaterTransparentType NoDimming;

        static LaterTransparentType()
        {
            Black = new LaterTransparentType(nameof(Black), "0");
            PrettyDark = new LaterTransparentType(nameof(PrettyDark), "1");
            SomewhatDark = new LaterTransparentType(nameof(SomewhatDark), "2");
            FaintlyDarker = new LaterTransparentType(nameof(FaintlyDarker), "3");
            NoDimming = new LaterTransparentType(nameof(NoDimming), "4");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public LaterTransparentType(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public string Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">[NotNull] コード</param>
        /// <returns>LaterTransparentType</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static LaterTransparentType FromCode(string code)
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
                var exception = new ArgumentException($"{nameof(LaterTransparentType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        ///     対象コードが存在しない場合はデフォルト値を返す。
        /// </summary>
        /// <param name="code">[Nullable] コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        public static LaterTransparentType FromCodeOrDefault(string code)
        {
            if (code is null || code.Equals(string.Empty)) return SomewhatDark;
            return FromCode(code);
        }
    }
}