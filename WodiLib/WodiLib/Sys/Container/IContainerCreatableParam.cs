// ========================================
// Project Name : WodiLib
// File Name    : IContainerCreatableParam.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="WodiLibContainer"/> でインスタンスを生成する際に使用する初期化パラメータであることを示すインタフェース。
    /// </summary>
    public interface IContainerCreatableParam : IContainerCreatableParam_Writable
    {
        /// <summary>
        ///     初期化時に指定されたコンテナキー名
        /// </summary>
        public new WodiLibContainerKeyName KeyName { get; }
    }

    /// <summary>
    ///     <see cref="WodiLibContainer"/> でインスタンスを生成する際に使用する初期化パラメータであることを示すインタフェース。
    /// </summary>
    public interface IContainerCreatableParam_Writable
    {
        /// <summary>
        ///     初期化時に指定するコンテナキー名
        /// </summary>
        public WodiLibContainerKeyName KeyName { get; set; }
    }
}
