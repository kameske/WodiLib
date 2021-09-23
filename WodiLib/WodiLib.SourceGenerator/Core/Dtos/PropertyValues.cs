// ========================================
// Project Name : WodiLib
// File Name    : PropertyValues.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Dtos
{
    /// <summary>
    ///     属性プロパティ値
    /// </summary>
    internal class PropertyValues
    {
        /// <summary>対象シンボル</summary>
        public INamedTypeSymbol? TargetSymbol => WorkResult.TargetSymbol;

        /// <summary>名前空間</summary>
        public string Namespace => WorkResult.Namespace;

        /// <summary>プロパティクラス名</summary>
        public string Name => WorkResult.ClassName;

        /// <summary>プロパティクラス名（フル）</summary>
        public string FullName => WorkResult.FullName;

        /// <summary>解析結果</summary>
        public SyntaxWorkResult WorkResult { get; }

        /// <summary>プロパティ設定値</summary>
        private IReadOnlyDictionary<string, PropertyValue> PropertyValueDict { get; }

        /// <summary>プロパティデフォルト値</summary>
        private IReadOnlyDictionary<string, PropertyValue> DefaultValueDict { get; }

        public PropertyValues(
            SyntaxWorkResult workResult,
            IReadOnlyDictionary<string, PropertyValue> defaultValueDict)
        {
            WorkResult = workResult;
            PropertyValueDict = workResult.MakePropertyValuesDict();
            DefaultValueDict = defaultValueDict;
        }

        public string? this[string key] =>
            PropertyValueDict.ContainsKey(key) ? PropertyValueDict[key].ToValueString()
            : DefaultValueDict.ContainsKey(key) ? DefaultValueDict[key].ToValueString()
            : null;

        /// <summary>
        ///     指定したキー名の値が <see langword="null"/> であるかを返す。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <returns>値が <see langword="null"/> の場合 <see langword="true"/></returns>
        public bool IsNull(string key)
        {
            var val = this[key];
            return val is null or "null";
        }

        /// <summary>
        ///     指定したキー名の値が <see langword="null"/> でないかを返す。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <returns>値が <see langword="null"/> の場合 <see langword="false"/></returns>
        public bool IsNotNull(string key)
            => !IsNull(key);

        /// <summary>
        ///     指定したキー名のいずれかの値が <see langword="null"/> ではないかどうかを返す。
        /// </summary>
        /// <param name="keys">キー名</param>
        /// <returns>いずれかの値が <see langword="null"/> ではない場合 <see langword="true"/></returns>
        public bool IsNotNullAny(params string[] keys)
            => keys.Any(IsNotNull);

        public bool IsPropertyInitialized(string propertyName)
            => PropertyValueDict.ContainsKey(propertyName);

        /// <summary>
        ///     指定したキー名の値が <see langword="null"/> ではない場合はキーの値を、
        ///     <see langword="null"/> の場合は <paramref name="ifNullValue"/> を返す。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <param name="ifNullValue">キー名の値が <see langword="null"/> の場合の返戻値</param>
        /// <returns>プロパティ値</returns>
        public string GetOrDefault(string key, string ifNullValue)
        {
            return this[key] ?? ifNullValue;
        }

        /// <summary>
        ///     指定したキー名の <cee cref="PropertyValue"/> インスタンスを取得する。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <returns><cee cref="PropertyValue"/> インスタンス</returns>
        public string[]? GetArrayValue(string key)
            => PropertyValueDict.ContainsKey(key) ? PropertyValueDict[key].ToValueStrings()
                : DefaultValueDict.ContainsKey(key) ? DefaultValueDict[key].ToValueStrings()
                : null;

        public override string ToString()
            => $"FullName: {FullName}, "
               + $"PropertyValueDict: {PropertyValueDict.Description()}"
               + $"DefaultValueDict: {DefaultValueDict.Description()}";
    }
}
