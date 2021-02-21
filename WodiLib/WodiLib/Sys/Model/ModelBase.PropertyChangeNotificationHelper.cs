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
            /// <summary>
            ///     プロパティ変更通知フラグプロパティ名リスト
            /// </summary>
            private static readonly string[] NotifyFlagPropertyNames =
            {
                nameof(IsNotifyBeforePropertyChange),
                nameof(IsNotifyAfterPropertyChange),
            };

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Protected Delegate
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     <see cref="INotifyPropertyChange"/> が通知したプロパティ名を
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
            ///     <see cref="INotifyPropertyChange"/> が通知したプロパティ名を
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
            ///     <see cref="PropertyChangingEventArgs"/> を取得する。（条件指定なし）
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangingEventArgs? GetPropertyChangingEventArgs(
                object sender, PropertyChangingEventArgs origArgs)
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(propName);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangingEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            ///     通知許可リストから <see cref="PropertyChangingEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="allowNotifyPropertyNameList">通知許可プロパティ名リスト</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangingEventArgs? GetPropertyChangingEventArgs(
                object sender, PropertyChangingEventArgs origArgs,
                IEnumerable<string>? allowNotifyPropertyNameList)
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(propName, allowNotifyPropertyNameList);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangingEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            ///     フィルタ設定から <see cref="PropertyChangingEventArgs"/> を取得する。
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
            ///     プロパティ名 Mapper から <see cref="PropertyChangingEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="mapNotifyPropertyName">Mapper</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static IEnumerable<PropertyChangingEventArgs>? GetPropertyChangingEventArgs(
                object sender, PropertyChangingEventArgs origArgs,
                MapNotifyPropertyName? mapNotifyPropertyName)
            {
                var propName = origArgs.PropertyName;

                var notifyNames = GetPropertyNames(sender, propName, mapNotifyPropertyName);

                return notifyNames?.Select(notifyName => notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangingEventArgsCache.GetInstance(notifyName));
            }

            /// <summary>
            ///     <see cref="PropertyChangedEventArgs"/> を取得する。（条件指定なし）
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangedEventArgs? GetPropertyChangedEventArgs(
                object sender, PropertyChangedEventArgs origArgs)
            {
                var propName = origArgs.PropertyName;

                var notifyName = GetPropertyName(propName);

                if (notifyName is null) return null;

                return notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangedEventArgsCache.GetInstance(notifyName);
            }

            /// <summary>
            ///     通知許可リストから <see cref="PropertyChangedEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="allowNotifyPropertyNameList">通知許可プロパティ名リスト</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static PropertyChangedEventArgs? GetPropertyChangedEventArgs(
                object sender, PropertyChangedEventArgs origArgs,
                IEnumerable<string>? allowNotifyPropertyNameList)
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
            ///     プロパティ名 Mapper から <see cref="PropertyChangedEventArgs"/> を取得する。
            /// </summary>
            /// <param name="sender">通知送信インスタンス</param>
            /// <param name="origArgs">送信通知引数</param>
            /// <param name="mapNotifyPropertyName">Mapper</param>
            /// <returns>通知引数。通知しない場合 <see langword="null"/></returns>
            internal static IEnumerable<PropertyChangedEventArgs>? GetPropertyChangedEventArgs(
                object sender, PropertyChangedEventArgs origArgs,
                MapNotifyPropertyName? mapNotifyPropertyName)
            {
                var propName = origArgs.PropertyName;

                var notifyNames = GetPropertyNames(sender, propName, mapNotifyPropertyName);

                return notifyNames?.Select(notifyName => notifyName.Equals(propName)
                    ? origArgs
                    : PropertyChangedEventArgsCache.GetInstance(notifyName));
            }

            /// <summary>
            ///     伝播元が通知したプロパティ名をこれから通知するプロパティ名に変換する。
            /// </summary>
            /// <param name="propertyName">通知送信プロパティ</param>
            /// <param name="allowNotifyPropertyNameList">通知許可プロパティ名リスト</param>
            /// <returns></returns>
            private static string? GetPropertyName(string propertyName,
                IEnumerable<string>? allowNotifyPropertyNameList = null)
            {
                // 伝播元のプロパティ通知フラグ変更はObserverに通知しない
                if (IsNotifyFlagName(propertyName)) return null;

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
            private static string? GetPropertyName(object sender,
                string propertyName,
                FilterNotifyPropertyName? filterNotifyPropertyName)
            {
                // 伝播元のプロパティ通知フラグ変更はObserverに通知しない
                if (IsNotifyFlagName(propertyName)) return null;

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
            private static string[]? GetPropertyNames(object sender,
                string propertyName,
                MapNotifyPropertyName? mapNotifyPropertyName)
            {
                // 伝播元のプロパティ通知フラグ変更はObserverに通知しない
                if (IsNotifyFlagName(propertyName)) return null;

                // mapperなしの場合無条件に通知
                if (mapNotifyPropertyName is null) return new[] {propertyName};

                return mapNotifyPropertyName(sender, propertyName);
            }

            /// <summary>
            ///     指定したプロパティ名がプロパティ変更通知を制御するフラグのプロパティ名であるかを判定する。
            /// </summary>
            /// <param name="propertyName">判定プロパティ名</param>
            /// <returns>プロパティ変更通知を制御するフラグのプロパティ名の場合<see langword="true"/></returns>
            private static bool IsNotifyFlagName(string propertyName)
            {
                return NotifyFlagPropertyNames.Contains(propertyName);
            }
        }
    }
}
