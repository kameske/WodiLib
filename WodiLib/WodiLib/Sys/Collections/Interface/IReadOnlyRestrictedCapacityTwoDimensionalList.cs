// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyRestrictedCapacityTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量制限のある読み取り専用二次元リスト
    /// </summary>
    /// <remarks>
    /// 外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    /// すべての行について列数は常に同じ値を取り続ける。<br/>
    /// 行数、列数ともに制限が設けられている。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyRestrictedCapacityTwoDimensionalList<T> : IReadOnlyTwoDimensionalList<T>
    {
        /// <summary>
        /// 最大行数を取得する。
        /// </summary>
        /// <returns>最大行数</returns>
        public int GetMaxRowCapacity();

        /// <summary>
        /// 最小行数を取得する。
        /// </summary>
        /// <returns>最小行数</returns>
        public int GetMinRowCapacity();

        /// <summary>
        /// 最大列数を取得する。
        /// </summary>
        /// <returns>最大列数</returns>
        public int GetMaxColumnCapacity();

        /// <summary>
        /// 最小列数を取得する。
        /// </summary>
        /// <returns>最小列数</returns>
        public int GetMinColumnCapacity();
    }
}
