// ========================================
// Project Name : WodiLib
// File Name    : IContainerCreatable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="WodiLibContainer"/> でインスタンスを生成できることを表すインタフェース。
    /// </summary>
    public interface IContainerCreatable
    {
        /// <summary>
        /// 生成元のコンテナキー名（コンテナから生成されていない場合 <see langword="null"/>）
        /// </summary>
        public WodiLibContainerKeyName? ContainerKeyName { get; set; }
    }
}
