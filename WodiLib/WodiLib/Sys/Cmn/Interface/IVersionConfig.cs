// ========================================
// Project Name : WodiLib
// File Name    : IVersionConfig.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Cmn
{
    /// <summary>
    ///     WodiLib全体のバージョン設定インタフェース
    /// </summary>
    /// <remarks>
    ///     バージョン設定の新規作成や更新は <see cref="VersionConfig"/> の
    ///     各種 <see langword="static"/> メソッドを通じて行う。
    /// </remarks>
    public interface IVersionConfig
    {
        /// <summary>
        ///     設定キー名
        /// </summary>
        public WodiLibContainerKeyName KeyName { get; }

        /// <summary>
        ///     設定バージョン
        /// </summary>
        public WoditorVersion Version { get; }
    }
}
