// ========================================
// Project Name : WodiLib
// File Name    : EqualityComparerFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// WodiLib で定義したインタフェースやクラスの同値性を比較するための
    /// <see cref="IEqualityComparer{T}"/> を生成、提供するための Factory クラス。
    /// </summary>
    public static class EqualityComparerFactory
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンテナ
        /// </summary>
        private static WodiLibContainer Container { get; } = new();


    }
}
