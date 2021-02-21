// ========================================
// Project Name : WodiLib
// File Name    : DoubleSequenceEqualityComparer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    ///     二重シーケンス比較
    /// </summary>
    /// <remarks>
    ///     状態を持たないのでシングルトンクラスとする。
    ///     インスタンスが必要なときは <see cref="GetInstance"/> メソッドで取得。
    /// </remarks>
    internal class DoubleSequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<IEnumerable<T>>>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Singleton
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static DoubleSequenceEqualityComparer<T>? _instance;

        /// <summary>
        ///     インスタンスを取得する。
        /// </summary>
        /// <returns></returns>
        public static DoubleSequenceEqualityComparer<T> GetInstance()
            => _instance ??= new DoubleSequenceEqualityComparer<T>();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public bool Equals(IEnumerable<IEnumerable<T>>? left, IEnumerable<IEnumerable<T>>? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null ^ right is null) return false;
            var lArray = left!.ToArray();
            var rArray = right!.ToArray();

            if (lArray.Length != rArray.Length) return false;

            if (lArray.Length == 0 && rArray.Length == 0) return true;

            return lArray.Zip(rArray, (l2, r2) => (l2, r2))
                .All(tuple => tuple.l2.SequenceEqual(tuple.r2));
        }

        /// <inheritdoc/>
        public int GetHashCode(IEnumerable<IEnumerable<T>> obj)
        {
            return obj.GetHashCode();
        }
    }
}
