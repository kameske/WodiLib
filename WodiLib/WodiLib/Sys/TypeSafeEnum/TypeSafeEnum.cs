// ========================================
// Project Name : WodiLib
// File Name    : TypeSafeEnum.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// TypeSafeEnum定義クラス
    /// <para>列挙するアイテムは必ず静的コンストラクタ内で初期化すること。</para>
    /// </summary>
    /// <typeparam name="T">対象クラス</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class TypeSafeEnum<T> where T : TypeSafeEnum<T>
    {
        private static readonly EnumItemsManager EnumItems = new EnumItemsManager();


        internal TypeSafeEnum(string id)
        {
            Id = id;
            EnumItems.Add(id, this);
        }

        /// <summary>ID</summary>
        public string Id { get; }

        /// <summary>
        ///     条件に合致する最初の要素を返却する。
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>条件に合致する要素</returns>
        protected static T _FindFirst(Func<T, bool> predicate)
        {
            return EnumItems.SearchFirst(predicate);
        }

        /// <summary>
        ///     すべての要素を返却する。
        /// </summary>
        /// <returns>すべての要素</returns>
        protected static List<T> _FindAll()
        {
            return EnumItems.AllEnums;
        }


        /// <summary>
        ///     Enum実装
        /// </summary>
        private sealed class EnumItemsManager
        {
            public EnumItemsManager()
            {
                ItemDic = new Dictionary<string, TypeSafeEnum<T>>();
            }

            private Dictionary<string, TypeSafeEnum<T>> ItemDic { get; }

            /// <summary>
            ///     列挙アイテムの全リスト
            /// </summary>
            public List<T> AllEnums => ItemDic.Values.Select(item => item.ConvertToClass()).ToList();

            /// <summary>
            ///     条件に合致する最初の要素を返す。
            /// </summary>
            /// <param name="predicate">検索条件</param>
            /// <returns>条件に合致する要素</returns>
            public T SearchFirst(Func<T, bool> predicate)
            {
                var items = AllEnums.Where(predicate).ToList();

                if (!items.Any()) throw new ArgumentException();
                return items.First();
            }

            /// <summary>
            ///     アイテムを追加する。
            /// </summary>
            /// <param name="id">識別文字列</param>
            /// <param name="item">格納するインスタンス</param>
            /// <exception cref="DuplicateEnumException">IDが重複した場合</exception>
            public void Add(string id, TypeSafeEnum<T> item)
            {
                if (ItemDic.ContainsKey(id)) throw new DuplicateEnumException();
                ItemDic.Add(id, item);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ==演算子
        /// </summary>
        /// <param name="left">比較対象1</param>
        /// <param name="right">比較対象2</param>
        /// <returns>一致する場合true</returns>
        public static bool operator ==(TypeSafeEnum<T> left, TypeSafeEnum<T> right)
        {
            if (ReferenceEquals(left, right)) return true;
            if ((object) left == null || (object) right == null) return false;


            return left.Id == right.Id;
        }

        /// <summary>
        /// !=演算子
        /// </summary>
        /// <param name="left">比較対象1</param>
        /// <param name="right">比較対象2</param>
        /// <returns>一致しない場合true</returns>
        public static bool operator !=(TypeSafeEnum<T> left, TypeSafeEnum<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Equal
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合true</returns>
        public bool Equals(TypeSafeEnum<T> other)
        {
            if(other == null) return false;
            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TypeSafeEnum<T>) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        ///     TypeSafeEnumからTに変換するメソッド.
        /// </summary>
        /// <returns></returns>
        private T ConvertToClass()
        {
            return (T) this;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
}