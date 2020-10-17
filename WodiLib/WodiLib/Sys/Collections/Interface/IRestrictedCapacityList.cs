// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityCollection.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <inheritdoc cref="IList{T}" />
    /// <summary>
    /// 容量制限のあるListインタフェース
    /// </summary>
    public interface IRestrictedCapacityCollection<T> : IModelBase<IRestrictedCapacityCollection<T>>, IExtendedList<T>,
        IReadOnlyRestrictedCapacityCollection<T>
    {
    }
}
