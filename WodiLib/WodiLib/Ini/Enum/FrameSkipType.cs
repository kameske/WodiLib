// ========================================
// Project Name : WodiLib
// File Name    : FrameSkipType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// フレームスキップ種別
    /// </summary>
    public class FrameSkipType : TypeSafeEnum<FrameSkipType>
    {
        /// <summary>高スペック</summary>
        public static readonly FrameSkipType HighSpec;

        /// <summary>中スペック</summary>
        public static readonly FrameSkipType MiddleSpec;

        /// <summary>低スペック</summary>
        public static readonly FrameSkipType LowSpec;

        static FrameSkipType()
        {
            HighSpec = new FrameSkipType(nameof(HighSpec), "0");
            MiddleSpec = new FrameSkipType(nameof(MiddleSpec), "1");
            LowSpec = new FrameSkipType(nameof(LowSpec), "2");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public FrameSkipType(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public string Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">[NotNull] コード</param>
        /// <returns>FrameSkipType</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static FrameSkipType FromCode(string code)
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
                var exception = new ArgumentException($"{nameof(FrameSkipType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }
    }
}