// ========================================
// Project Name : WodiLib
// File Name    : NotChangeAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    ///     変更されないプロパティであることを示す属性。
    /// </summary>
    /// <remarks>
    ///     この属性が付与されたプロパティはインスタンスが変化しない。
    ///     そのため <see cref="INotifyPropertyChanging.PropertyChanging"/> および
    ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> も通知されることはない。
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class InstanceNotChangeAttribute : Attribute
    {
    }
}
