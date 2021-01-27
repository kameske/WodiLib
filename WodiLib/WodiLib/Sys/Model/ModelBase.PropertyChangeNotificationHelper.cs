// ========================================
// Project Name : WodiLib
// File Name    : ModelBase.PropertyChangeNotificationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Sys
{
    public abstract partial class ModelBase<TChild>
    {
        /// <summary>
        /// <see cref="ModelBase{TChild}"/> のプロパティ変更通知Helperクラス
        /// </summary>
        protected static class PropertyChangeNotificationHelper
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Protected Delegate
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// <see cref="INotifyPropertyChange"/> が通知したプロパティ名を
            /// 自身が通知するかを決定する。
            /// </summary>
            /// <remarks>
            /// <see langword="false"/> を返却した場合、そのプロパティは通知しない。<br/>
            /// 通知プロパティ名を変化させたい場合は <see cref="MapNotifyPropertyName"/> を使用する。
            /// </remarks>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="propertyName">通知プロパティ名</param>
            public delegate bool FilterNotifyPropertyName(object sender, string propertyName);

            /// <summary>
            /// <see cref="INotifyPropertyChange"/> が通知したプロパティ名を
            /// 自身がどのように通知するかを決定する。
            /// </summary>
            /// <remarks>
            /// <see langword="null"/> を返却した場合、そのプロパティは通知しない。
            /// </remarks>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="propertyName">通知プロパティ名</param>
            public delegate string? MapNotifyPropertyName(object sender, string propertyName);

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Static Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// フィルタ設定から <see cref="PropertyChangingEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="filterNotifyPropertyName">フィルタ</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangingEventArgs? GetPropertyChangingEventArgs(
                object sender, PropertyChangingEventArgs origArgs,
                FilterNotifyPropertyName? filterNotifyPropertyName)
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(sender, propName, filterNotifyPropertyName);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangingEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            /// プロパティ名 Mapper から <see cref="PropertyChangingEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="mapNotifyPropertyName">Mapper</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangingEventArgs? GetPropertyChangingEventArgs(
                object sender, PropertyChangingEventArgs origArgs,
                MapNotifyPropertyName? mapNotifyPropertyName)
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(sender, propName, mapNotifyPropertyName);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangingEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            /// フィルタ設定から <see cref="PropertyChangedEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="filterNotifyPropertyName">フィルタ</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangedEventArgs? GetPropertyChangedEventArgs(
                object sender, PropertyChangedEventArgs origArgs,
                FilterNotifyPropertyName? filterNotifyPropertyName)
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(sender, propName, filterNotifyPropertyName);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangedEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            /// プロパティ名 Mapper から <see cref="PropertyChangedEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="mapNotifyPropertyName">Mapper</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangedEventArgs? GetPropertyChangedEventArgs(
                object sender, PropertyChangedEventArgs origArgs,
                MapNotifyPropertyName? mapNotifyPropertyName)
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(sender, propName, mapNotifyPropertyName);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangedEventArgsCache.GetInstance(notifyName);
            }

            private static string? GetPropertyName(object sender,
                string propertyName,
                FilterNotifyPropertyName? filterNotifyPropertyName)
            {
                // 伝播元のプロパティ通知フラグ変更はObserverに通知しない
                switch (propertyName)
                {
                    case null:
                    case nameof(IsNotifyBeforePropertyChange):
                    case nameof(IsNotifyAfterPropertyChange):
                        return null;
                }

                // filterなしの場合無条件に通知
                if (filterNotifyPropertyName is null) return propertyName;

                return filterNotifyPropertyName(sender, propertyName)
                    ? propertyName
                    : null;
            }

            private static string? GetPropertyName(object sender,
                string propertyName,
                MapNotifyPropertyName? mapNotifyPropertyName)
            {
                // 伝播元のプロパティ通知フラグ変更はObserverに通知しない
                switch (propertyName)
                {
                    case null:
                    case nameof(IsNotifyBeforePropertyChange):
                    case nameof(IsNotifyAfterPropertyChange):
                        return null;
                }

                // mapperなしの場合無条件に通知
                if (mapNotifyPropertyName is null) return propertyName;

                return mapNotifyPropertyName(sender, propertyName);
            }
        }
    }
}
