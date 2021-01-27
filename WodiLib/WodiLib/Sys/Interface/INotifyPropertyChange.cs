// ========================================
// Project Name : WodiLib
// File Name    : INotifyPropertyChange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    /// <see cref="INotifyPropertyChanging.PropertyChanging"/> および
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> を実装していることを示すインタフェース
    /// </summary>
    /// <remarks>
    /// <see cref="IsNotifyBeforePropertyChange"/> および
    /// <see cref="IsNotifyAfterPropertyChange"/> の設定値によって
    /// 通知イベントの有無を決定できる。
    /// </remarks>
    public interface INotifyPropertyChange : IReadOnlyNotifyPropertyChange
    {
        /// <inheritdoc cref="IReadOnlyNotifyPropertyChange.IsNotifyBeforePropertyChange" />
        public new bool IsNotifyBeforePropertyChange { get; set; }

        /// <inheritdoc cref="IReadOnlyNotifyPropertyChange.IsNotifyAfterPropertyChange" />
        public new bool IsNotifyAfterPropertyChange { get; set; }
    }

    /// <summary>
    /// <see cref="INotifyPropertyChanging.PropertyChanging"/> および
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> を実装していることを示すインタフェース
    /// </summary>
    /// <remarks>
    /// 実際に各イベントが通知されるかどうかは <see cref="IsNotifyBeforePropertyChange"/> および
    /// <see cref="IsNotifyAfterPropertyChange"/> の設定値によって決まる。
    /// </remarks>
    public interface IReadOnlyNotifyPropertyChange : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// <see cref="INotifyPropertyChanging.PropertyChanging"/> を通知するか否か。<br/>
        /// <see langword="true"/> の場合、このインスタンスはプロパティが変化する際に
        /// <see cref="INotifyPropertyChanging.PropertyChanging"/> イベントを通知する。
        /// </summary>
        public bool IsNotifyBeforePropertyChange { get; }

        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> を通知するか否か。<br/>
        /// <see langword="true"/> の場合、このインスタンスはプロパティが変化した際に
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを通知する。
        /// </summary>
        public bool IsNotifyAfterPropertyChange { get; }
    }
}
