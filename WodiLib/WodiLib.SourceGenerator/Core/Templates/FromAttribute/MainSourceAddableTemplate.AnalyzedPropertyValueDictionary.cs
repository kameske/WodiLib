// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MainSourceAddableTemplate.AnalyzedPropertyValueDictionary.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal abstract partial class MainSourceAddableTemplate
    {
        /// <summary>
        ///     プロパティ値ディクショナリラッパー
        /// </summary>
        internal class AnalyzedPropertyValueDictionary
        {
            private Dictionary<string, WorkInfo> InfoDict { get; } = new();

            /// <summary>
            ///     デフォルト値情報をセットする。
            /// </summary>
            /// <param name="attrName">属性名</param>
            /// <param name="defaultValues">デフォルト値ディクショナリ</param>
            /// <param name="parentAttrName">
            ///     継承元属性名。<paramref name="attrName"/> がSourceGeneratorで定義された属性の場合<see langword="null"/>
            /// </param>
            public void Set(string attrName, Dictionary<string, PropertyValue> defaultValues, string? parentAttrName)
            {
                var info = parentAttrName is null
                    ? new WorkInfo(defaultValues, true, null)
                    : new WorkInfo(defaultValues, false, parentAttrName);

                InfoDict[attrName] = info;
            }

            /// <summary>
            ///     デフォルト値情報を取得する。
            /// </summary>
            /// <param name="attrName">属性名</param>
            /// <returns></returns>
            public SourceAttributeDefaultValueMap Get(string attrName)
            {
                var info = GetWorkInfo(attrName);
                return info.ToDictionary();
            }

            /// <summary>
            ///     デフォルト値情報を取得する。
            ///     親クラスの情報とマージされていない場合はマージしながら取得する。
            /// </summary>
            /// <param name="attrName">属性名</param>
            /// <returns>デフォルト値情報</returns>
            private WorkInfo GetWorkInfo(string attrName)
            {
                var current = InfoDict[attrName];
                if (current.IsRootAttribute || current.IsMerged) return current;

                var parentAttrName = current.ParentTypeFullName;
                var parentInfo = GetWorkInfo(parentAttrName);
                current.Merge(parentInfo);

                return current;
            }

            /// <summary>
            ///     プロパティ値ワークデータ
            /// </summary>
            private class WorkInfo
            {
                public bool IsRootAttribute { get; }
                public bool IsMerged { get; private set; }
                public string ParentTypeFullName { get; }
                private Dictionary<string, PropertyValue> DefaultValues { get; }

                private object DefaultValuesLock { get; } = new();

                public WorkInfo(IDictionary<string, PropertyValue> defaultValues, bool isRootAttribute,
                    string? parentTypeName)
                {
                    DefaultValues = new Dictionary<string, PropertyValue>(defaultValues);
                    IsRootAttribute = isRootAttribute;
                    IsMerged = IsRootAttribute;
                    ParentTypeFullName = IsRootAttribute ? "" : parentTypeName!;
                }

                public void Merge(WorkInfo other)
                {
                    lock (DefaultValuesLock)
                    {
                        var addValues =
                            other.DefaultValues.Where(otherPair => !DefaultValues.ContainsKey(otherPair.Key));
                        foreach (var otherPair in addValues)
                        {
                            DefaultValues[otherPair.Key] = otherPair.Value;
                        }
                    }

                    IsMerged = true;
                }

                public SourceAttributeDefaultValueMap ToDictionary()
                    => new(DefaultValues);
            }
        }
    }
}
