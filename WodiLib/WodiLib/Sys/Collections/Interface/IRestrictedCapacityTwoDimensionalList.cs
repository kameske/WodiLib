// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量制限のある二次元リスト
    /// </summary>
    /// <remarks>
    /// 外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    /// すべての行について列数は常に同じ値を取り続ける。<br/>
    /// 行数、列数ともに制限が設けられている。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IRestrictedCapacityTwoDimensionalList<T> : IModelBase<IRestrictedCapacityTwoDimensionalList<T>>,
        ITwoDimensionalList<T>, IReadOnlyRestrictedCapacityTwoDimensionalList<T>
    {
    }
}
