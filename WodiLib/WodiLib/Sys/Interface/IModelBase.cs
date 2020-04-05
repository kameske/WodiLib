// ========================================
// Project Name : WodiLib
// File Name    : IModelBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    /// 各Modelクラス基底クラスインタフェース
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IModelBase<TChild> : IEquatable<TChild>, INotifyPropertyChanged
        where TChild : IModelBase<TChild>
    {
    }
}