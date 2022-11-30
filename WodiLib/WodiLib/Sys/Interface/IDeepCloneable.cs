// ========================================
// Project Name : WodiLib
// File Name    : IDeepCloneable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     ディープクローン可能であることを示すインタフェース
    /// </summary>
    public interface IDeepCloneable<out T> : IDeepCloneable
    {
        /// <summary>
        ///     自身のディープコピーを作成する。
        /// </summary>
        /// <returns>自身をディープコピーしたインスタンス</returns>
        new T DeepClone();
    }

    /// <summary>
    ///     ディープクローン可能であることを示すインタフェース（非 Generic 版）
    /// </summary>
    public interface IDeepCloneable
    {
        /// <summary>
        ///     自身のディープコピーを作成する。
        /// </summary>
        /// <returns>自身をディープコピーしたインスタンス</returns>
        public object DeepClone();
    }
}
