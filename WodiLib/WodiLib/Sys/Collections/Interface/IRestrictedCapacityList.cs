// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityList.cs
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
    [Obsolete("不適切な名前のため Ver 2.6 で削除します。 IRestrictedCapacityList<T> を使用してください。")]
    public interface IRestrictedCapacityCollection<T> : IRestrictedCapacityList<T>
    {
    }

    /// <summary>
    /// 容量制限のあるListインタフェース
    /// </summary>
    public interface IRestrictedCapacityList<T> : IModelBase<IRestrictedCapacityList<T>>, IExtendedList<T>,
        IReadOnlyRestrictedCapacityList<T>
    {
    }
}
