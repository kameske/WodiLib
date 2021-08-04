// ========================================
// Project Name : WodiLib
// File Name    : IContainerCreatable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="WodiLibContainer"/> でインスタンスを生成できることを表すインタフェース。
    /// </summary>
    /// <remarks>
    ///     インスタンス生成時に初期化パラメータが不要な場合はこのインタフェースを実装する。<br/>
    ///     初期化パラメータが必要な場合は <see cref="IContainerCreatable{TParam}"/> を実装する。
    /// </remarks>
    public interface IContainerCreatable
    {
    }

    /// <summary>
    ///     <see cref="WodiLibContainer"/> でインスタンスを生成できることを表すインタフェース。
    /// </summary>
    /// <remarks>
    ///     インスタンス生成時に初期化パラメータが不要な場合はこのインタフェースを実装する。<br/>
    ///     初期化パラメータが必要な場合は <see cref="IContainerCreatable"/> を実装する。
    /// </remarks>
    /// <typeparam name="TParam">インスタンス作成パラメータ定義型</typeparam>
    public interface IContainerCreatable<TParam>
        where TParam : IContainerCreatableParam
    {
    }

    /// <summary>
    ///     SourceGenerator用
    ///     パラメータと出力インタフェースを紐付ける。
    ///     これを付与した実装クラスにパラメータを用いたコンストラクタを実装する。
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    /// <typeparam name="TIf"></typeparam>
    internal interface IContainerCreatableInternal<TParam, TIf> : IContainerCreatable<TParam>
        where TParam : IContainerCreatableParam
    {
    }
}
