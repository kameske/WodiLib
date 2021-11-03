// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : AttributeDataExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    /// <see cref="AttributeDataExtension"/> 拡張クラス
    /// </summary>
    internal static class AttributeDataExtension
    {
        /// <summary>
        /// 指定したプロパティの値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>プロパティ値</returns>
        public static T? GetPropertyData<T>(this AttributeData src, string propertyName)
        {
            var initializedValue = src.GetPropertyInitializedValue<T?>(propertyName);

            if (initializedValue is not null)
            {
                return (T)initializedValue;
            }

            return src.GetPropertyDefaultValue<T?>(propertyName);
        }

        /// <summary>
        /// 引数で指定された属性プロパティ値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>属性プロパティが指定されていた場合、その値。指定されていない場合、<see langword="null"/></returns>
        private static T? GetPropertyInitializedValue<T>(this AttributeData src, string propertyName)
        {
            return (T?)src.NamedArguments
                .FirstOrDefault(arg => arg.Key.Equals(propertyName))
                .Value
                .Value;
        }

        /// <summary>
        /// <see cref="DefaultValueAttribute"/> で指定されたデフォルト値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>デフォルト指定された値</returns>
        private static T? GetPropertyDefaultValue<T>(this AttributeData src, string propertyName)
        {
            return (T?)src.AttributeClass
                ?.GetMembers()
                .FirstOrDefault(member => member.Name.Equals(propertyName))
                ?.GetAttributes()
                .FirstOrDefault(attr =>
                    attr.AttributeClass
                        ?.FullName()
                        .Equals(typeof(DefaultValueAttribute).FullName)
                    ?? false)
                ?.ConstructorArguments[0]
                .Value;
        }
    }
}
