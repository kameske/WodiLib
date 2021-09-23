// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ObjectType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Core.Enums
{
    /// <summary>
    ///     オブジェクト種別
    /// </summary>
    internal enum ObjectType
    {
        /// <summary>インタフェース</summary>
        Interface,

        /// <summary>クラス</summary>
        Class,

        /// <summary>構造体</summary>
        Struct,

        /// <summary>レコード</summary>
        Record,

        /// <summary>プロパティ</summary>
        Property,

        /// <summary>その他</summary>
        Other
    }

    /// <summary>
    ///     <see cref="ObjectType"/> 拡張クラス
    /// </summary>
    internal static class ObjectTypeExtension
    {
        /// <summary>自身が型を示すオブジェクトである場合<see langword="true"/></summary>
        public static bool IsTypeDefinition(this ObjectType src)
        {
            return src is ObjectType.Interface or ObjectType.Class or ObjectType.Record or ObjectType.Struct;
        }
    }
}
