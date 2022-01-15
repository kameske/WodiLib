// ========================================
// Project Name : WodiLib
// File Name    : IModelBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    ///     各Modelクラス基底クラスインタフェース
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IModelBase<TChild> : IReadOnlyModelBase<TChild>
        where TChild : IModelBase<TChild>
    {
    }

    /// <summary>
    ///     【読み取り専用】各Modelクラス基底クラスインタフェース
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IReadOnlyModelBase<TChild> : INotifiablePropertyChange,
        IEqualityComparable<TChild>,
        IDeepCloneable<TChild>,
        IContainerCreatableParam
        where TChild : IReadOnlyModelBase<TChild>
    {
    }
}
