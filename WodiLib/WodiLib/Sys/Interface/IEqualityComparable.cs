// ========================================
// Project Name : WodiLib
// File Name    : IEqualityComparable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// 同値比較可能な可変クラスであることを示すインタフェース。<br/>
    /// このインタフェースを継承するインタフェースおよびクラスは
    /// <see cref="EqualityComparerFactory"/> で <see cref="IEqualityComparer{T}"/> を
    /// 生成可能。また、 <see cref="IEqualityComparer{T}"/> を通さずとも
    /// <see cref="ItemEquals"/> メソッドにより同値比較可能。
    /// </summary>
    public interface IEqualityComparable<in T>
        where T : IEqualityComparable<T>
    {
        /// <summary>
        /// 現在のオブジェクトが、同じ型の別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals([AllowNull] T other);
    }
}
