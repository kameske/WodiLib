// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyFixedLengthCollection.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// 【読み取り専用】長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete("このインタフェースは Ver 2.6 で廃止します。代わりに IExtendedReadOnlyList<T> インタフェースを使用してください。")]
    public interface IReadOnlyFixedLengthCollection<T> : IModelBase<IReadOnlyFixedLengthCollection<T>>,
        IExtendedReadOnlyList<T>
    {
    }
}
