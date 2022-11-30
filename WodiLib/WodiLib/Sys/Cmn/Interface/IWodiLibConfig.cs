// ========================================
// Project Name : WodiLib
// File Name    : IWodiLibConfig.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Cmn
{
    /// <summary>
    ///     WodiLib 全体の設定クラス
    /// </summary>
    /// <remarks>
    ///     設定グループの新規作成や更新は <see cref="WodiLibConfig"/> の
    ///     各種 <see langword="static"/> メソッドを通じて行う。
    /// </remarks>
    public interface IWodiLibConfig
    {
        /// <summary>
        ///     設定キー名
        /// </summary>
        public WodiLibContainerKeyName KeyName { get; }
    }
}
