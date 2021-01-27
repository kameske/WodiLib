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
    /// 各Modelクラス基底クラスインタフェース
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IModelBase<in TChild> : IReadOnlyModelBase<TChild>, INotifyPropertyChange
        where TChild : IModelBase<TChild>
    {
    }

    /// <summary>
    /// 【読み取り専用】各Modelクラス基底クラスインタフェース
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IReadOnlyModelBase<in TChild> : IReadOnlyNotifyPropertyChange,
        IEqualityComparable<TChild>
        where TChild : IReadOnlyModelBase<TChild>
    {
    }
}
