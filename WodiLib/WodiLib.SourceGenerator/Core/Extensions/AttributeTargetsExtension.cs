// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : AttributeTargetsExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="AttributeTargets"/> 拡張クラス
    /// </summary>
    internal static class AttributeTargetsExtension
    {
        /// <returns>ソースコード文字列</returns>
        public static string ToSource(this AttributeTargets src)
            => AllTargets().Where(target => (target & src) != 0)
                .Select(target => $"{typeof(AttributeTargets).FullName}.{target.ToString()}")
                .Join(" | ");

        /// <returns>すべての <see cref="AttributeTargets"/> 要素</returns>
        private static IEnumerable<AttributeTargets> AllTargets()
        {
            yield return AttributeTargets.Assembly;
            yield return AttributeTargets.Module;
            yield return AttributeTargets.Class;
            yield return AttributeTargets.Struct;
            yield return AttributeTargets.Enum;
            yield return AttributeTargets.Constructor;
            yield return AttributeTargets.Method;
            yield return AttributeTargets.Property;
            yield return AttributeTargets.Field;
            yield return AttributeTargets.Event;
            yield return AttributeTargets.Interface;
            yield return AttributeTargets.Parameter;
            yield return AttributeTargets.Delegate;
            yield return AttributeTargets.ReturnValue;
            yield return AttributeTargets.GenericParameter;
        }
    }
}
