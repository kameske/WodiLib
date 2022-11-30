// ========================================
// Project Name : WodiLib
// File Name    : ModelBase.PropertyChangeNotificationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys
{
    public abstract partial class ModelBase<TChild>
    {
        /// <summary>
        ///     <see cref="ModelBase{TChild}"/> のプロパティ変更通知Helperクラス
        /// </summary>
        protected static class PropertyChangeNotificationHelper
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Protected Delegate
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     <see cref="INotifyPropertyChanged"/> が通知したプロパティ名を
            ///     自身が通知するかを決定する。
            /// </summary>
            /// <remarks>
            ///     <see langword="false"/> を返却した場合、そのプロパティは通知しない。<br/>
            ///     通知プロパティ名を変化させたい場合は <see cref="MapNotifyPropertyName"/> を使用する。
            /// </remarks>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="propertyName">通知プロパティ名</param>
            public delegate bool FilterNotifyPropertyName(object sender, string propertyName);

            /// <summary>
            ///     <see cref="INotifyPropertyChanged"/> が通知したプロパティ名を
            ///     自身がどのように通知するかを決定する。
            /// </summary>
            /// <remarks>
            ///     <see langword="null"/> を返却した場合、そのプロパティは通知しない。
            /// </remarks>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="propertyName">通知プロパティ名</param>
            public delegate string[]? MapNotifyPropertyName(object sender, string propertyName);

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Static Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     通知許可リストから <see cref="PropertyChangedEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="allowNotifyPropertyNameList">通知許可プロパティ名リスト</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangedEventArgs? GetPropertyChangedEventArgs(
                object sender,
                PropertyChangedEventArgs origArgs,
                IEnumerable<string>? allowNotifyPropertyNameList
            )
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(propName, allowNotifyPropertyNameList);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangedEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            ///     フィルタ設定から <see cref="PropertyChangedEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="filterNotifyPropertyName">フィルタ</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangedEventArgs? GetPropertyChangedEventArgs(
                object sender,
                PropertyChangedEventArgs origArgs,
                FilterNotifyPropertyName? filterNotifyPropertyName
            )
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(sender, propName, filterNotifyPropertyName);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangedEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            ///     プロパティ名 Mapper から <see cref="PropertyChangedEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="mapNotifyPropertyName">Mapper</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static IEnumerable<PropertyChangedEventArgs>? GetPropertyChangedEventArgs(
                object sender,
                PropertyChangedEventArgs origArgs,
                MapNotifyPropertyName? mapNotifyPropertyName
            )
            {
                var propName = origArgs.PropertyName;

                var notifyNames = GetPropertyNames(sender, propName, mapNotifyPropertyName);

                return notifyNames?.Select(
                    notifyName => notifyName.Equals(propName)
                        ? origArgs
                        : PropertyChangedEventArgsCache.GetInstance(notifyName)
                );
            }

            /// <summary>
            ///     伝播元が通知したプロパティ名をこれから通知するプロパティ名に変換する。
            /// </summary>
            /// <param name="propertyName">通知送信プロパティ</param>
            /// <param name="allowNotifyPropertyNameList">通知許可プロパティ名リスト</param>
            /// <returns></returns>
            private static string? GetPropertyName(
                string propertyName,
                IEnumerable<string>? allowNotifyPropertyNameList = null
            )
            {
                // 許可リストなしの場合無条件に通知
                if (allowNotifyPropertyNameList is null) return propertyName;

                // 許可リストに含まれない場合通知しない
                if (!allowNotifyPropertyNameList.Contains(propertyName)) return null;

                return propertyName;
            }

            /// <summary>
            ///     伝播元が通知したプロパティ名をこれから通知するプロパティ名に変換する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="propertyName">通知送信プロパティ</param>
            /// <param name="filterNotifyPropertyName">フィルタ</param>
            /// <returns></returns>
            private static string? GetPropertyName(
                object sender,
                string propertyName,
                FilterNotifyPropertyName? filterNotifyPropertyName
            )
            {
                // filterなしの場合無条件に通知
                if (filterNotifyPropertyName is null) return propertyName;

                return filterNotifyPropertyName(sender, propertyName)
                    ? propertyName
                    : null;
            }

            /// <summary>
            ///     伝播元が通知したプロパティ名をこれから通知するプロパティ名配列に変換する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="propertyName">通知送信プロパティ</param>
            /// <param name="mapNotifyPropertyName">Mapper</param>
            /// <returns></returns>
            private static string[]? GetPropertyNames(
                object sender,
                string propertyName,
                MapNotifyPropertyName? mapNotifyPropertyName
            )
            {
                // mapperなしの場合無条件に通知
                if (mapNotifyPropertyName is null) return new[] { propertyName };

                return mapNotifyPropertyName(sender, propertyName);
            }
        }
    }
}
