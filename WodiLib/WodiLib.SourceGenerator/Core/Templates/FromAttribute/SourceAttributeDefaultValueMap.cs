// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceAttributeDefaultValueMap.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     属性デフォルト値マップ
    /// </summary>
    internal class SourceAttributeDefaultValueMap
    {
        /// <summary>
        ///     プロパティデフォルト値
        /// </summary>
        /// <param name="propertyName"></param>
        public PropertyValue this[string propertyName]
            => Impl[propertyName];

        private readonly Dictionary<string, PropertyValue> Impl;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="src">デフォルト値ディクショナリ</param>
        public SourceAttributeDefaultValueMap(Dictionary<string, PropertyValue> src)
        {
            Impl = src;
        }

        public IReadOnlyDictionary<string, PropertyValue> ToReadOnlyDictionary() => Impl;
    }
}
