// ========================================
// Project Name : WodiLib
// File Name    : WodiLibListValidatorTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     リスト編集メソッドの引数検証クラステンプレート
    /// </summary>
    /// <remarks>
    ///     継承先で定義した <see cref="BaseValidator"/> を起動するだけの実装。<br/>
    ///     <see cref="BaseValidator"/> の検証処理を拡張したいときに対象のメソッドをオーバーライドして
    ///     処理拡張を行う。
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    public abstract class WodiLibListValidatorTemplate<TIn, TOut> : IWodiLibListValidator<TIn>
        where TOut : TIn
    {
        /// <summary>
        ///     カスタマイズ元の検証処理。
        /// </summary>
        /// <remarks>
        ///     カスタマイズ元が存在しない場合 <see langword="null"/> を返却する。
        /// </remarks>
        protected abstract IWodiLibListValidator<TIn>? BaseValidator { get; }

        /// <summary>
        ///     各種メソッドの操作対象
        /// </summary>
        protected IReadOnlyExtendedList<TIn, TOut> Target { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="target">各種メソッドの操作対象</param>
        protected WodiLibListValidatorTemplate(IReadOnlyExtendedList<TIn, TOut> target)
        {
            Target = target;
        }

        /// <inheritdoc/>
        public virtual void Constructor(IReadOnlyList<TIn> initItems)
        {
            BaseValidator?.Constructor(initItems);
        }

        /// <inheritdoc/>
        public virtual void Get(int index, int count)
        {
            BaseValidator?.Get(index, count);
        }

        /// <inheritdoc/>
        public virtual void Set(int index, TIn item)
        {
            BaseValidator?.Set(index, item);
        }

        /// <inheritdoc/>
        public virtual void Set(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Set(index, items);
        }

        /// <inheritdoc/>
        public virtual void Insert(int index, TIn item)
        {
            BaseValidator?.Insert(index, item);
        }

        /// <inheritdoc/>
        public virtual void Insert(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Insert(index, items);
        }

        /// <inheritdoc/>
        public virtual void Overwrite(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Overwrite(index, items);
        }

        /// <inheritdoc/>
        public virtual void Move(int oldIndex, int newIndex, int count)
        {
            BaseValidator?.Move(oldIndex, newIndex, count);
        }

        /// <inheritdoc/>
        public virtual void Remove(TIn? item)
        {
            BaseValidator?.Remove(item);
        }

        /// <inheritdoc/>
        public virtual void Remove(int index, int count)
        {
            BaseValidator?.Remove(index, count);
        }

        /// <inheritdoc/>
        public virtual void AdjustLength(int length)
        {
            BaseValidator?.AdjustLength(length);
        }

        /// <inheritdoc/>
        public virtual void AdjustLengthIfShort(int length)
        {
            BaseValidator?.AdjustLengthIfShort(length);
        }

        /// <inheritdoc/>
        public virtual void AdjustLengthIfLong(int length)
        {
            BaseValidator?.AdjustLengthIfLong(length);
        }

        /// <inheritdoc/>
        public virtual void Reset(IReadOnlyList<TIn> items)
        {
            BaseValidator?.Reset(items);
        }
    }

    /// <summary>
    ///     リスト編集メソッドの引数検証クラステンプレート
    /// </summary>
    /// <remarks>
    ///     継承先で定義した <see cref="BaseValidator"/> を起動するだけの実装。<br/>
    ///     <see cref="BaseValidator"/> の検証処理を拡張したいときに対象のメソッドをオーバーライドして
    ///     処理拡張を行う。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    [Obsolete]
    public abstract class WodiLibListValidatorTemplate<T> : IWodiLibListValidator<T>
    {
        /// <summary>
        ///     カスタマイズ元の検証処理。
        /// </summary>
        /// <remarks>
        ///     カスタマイズ元が存在しない場合 <see langword="null"/> を返却する。
        /// </remarks>
        protected abstract IWodiLibListValidator<T>? BaseValidator { get; }

        /// <summary>
        ///     各種メソッドの操作対象
        /// </summary>
        protected IReadOnlyExtendedList<T> Target { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="target">各種メソッドの操作対象</param>
        protected WodiLibListValidatorTemplate(IReadOnlyExtendedList<T> target)
        {
            Target = target;
        }

        /// <inheritdoc/>
        public virtual void Constructor(IReadOnlyList<T> initItems)
        {
            BaseValidator?.Constructor(initItems);
        }

        /// <inheritdoc/>
        public virtual void Get(int index, int count)
        {
            BaseValidator?.Get(index, count);
        }

        /// <inheritdoc/>
        public virtual void Set(int index, T item)
        {
            BaseValidator?.Set(index, item);
        }

        /// <inheritdoc/>
        public virtual void Set(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Set(index, items);
        }

        /// <inheritdoc/>
        public virtual void Insert(int index, T item)
        {
            BaseValidator?.Insert(index, item);
        }

        /// <inheritdoc/>
        public virtual void Insert(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Insert(index, items);
        }

        /// <inheritdoc/>
        public virtual void Overwrite(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Overwrite(index, items);
        }

        /// <inheritdoc/>
        public virtual void Move(int oldIndex, int newIndex, int count)
        {
            BaseValidator?.Move(oldIndex, newIndex, count);
        }

        /// <inheritdoc/>
        public virtual void Remove(T? item)
        {
            BaseValidator?.Remove(item);
        }

        /// <inheritdoc/>
        public virtual void Remove(int index, int count)
        {
            BaseValidator?.Remove(index, count);
        }

        /// <inheritdoc/>
        public virtual void AdjustLength(int length)
        {
            BaseValidator?.AdjustLength(length);
        }

        /// <inheritdoc/>
        public virtual void AdjustLengthIfShort(int length)
        {
            BaseValidator?.AdjustLengthIfShort(length);
        }

        /// <inheritdoc/>
        public virtual void AdjustLengthIfLong(int length)
        {
            BaseValidator?.AdjustLengthIfLong(length);
        }

        /// <inheritdoc/>
        public virtual void Reset(IReadOnlyList<T> items)
        {
            BaseValidator?.Reset(items);
        }
    }
}
