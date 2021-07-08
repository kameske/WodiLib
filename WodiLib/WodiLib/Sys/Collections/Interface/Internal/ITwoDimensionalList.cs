// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    internal interface ITwoDimensionalList<T>
        : IFixedLengthTwoDimensionalList<T>,
            ISizeChangeableTwoDimensionalList<T, ITwoDimensionalList<T>, IFixedLengthTwoDimensionalList<T>,
                IReadOnlyTwoDimensionalList<T>>
    {
    }

    /// <summary>
    ///     WodiLib内部で使用する容量固定二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    internal interface IFixedLengthTwoDimensionalList<T>
        : IReadOnlyTwoDimensionalList<T>,
            IWritableTwoDimensionalList<T, IFixedLengthTwoDimensionalList<T>, IReadOnlyTwoDimensionalList<T>>
    {
    }

    /// <summary>
    ///     【読み取り専用】二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の読み取り専用二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    internal interface IReadOnlyTwoDimensionalList<T> : IReadableTwoDimensionalList<T, IReadOnlyTwoDimensionalList<T>>
    {
    }
}
