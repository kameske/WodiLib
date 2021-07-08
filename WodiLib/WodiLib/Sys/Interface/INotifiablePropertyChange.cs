// ========================================
// Project Name : WodiLib
// File Name    : INotifiablePropertyChange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="INotifyPropertyChanging.PropertyChanging"/> および
    ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> を実装していることを示すインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="NotifyPropertyChangingEventType"/> および
    ///     <see cref="NotifyPropertyChangedEventType"/> の設定値によって
    ///     通知イベントの有無を決定できる。
    /// </remarks>
    public interface INotifiablePropertyChange : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        ///     <see cref="INotifyPropertyChanging.PropertyChanging"/> の通知種別
        /// </summary>
        /// <exception cref="PropertyNullException">
        ///     <paramref name="value"/> が <see langword="null"/> の場合。
        /// </exception>
        public NotifyPropertyChangeEventType NotifyPropertyChangingEventType { get; set; }

        /// <summary>
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> の通知種別
        /// </summary>
        /// <exception cref="PropertyNullException">
        ///     <paramref name="value"/> が <see langword="null"/> の場合。
        /// </exception>
        public NotifyPropertyChangeEventType NotifyPropertyChangedEventType { get; set; }
    }
}
