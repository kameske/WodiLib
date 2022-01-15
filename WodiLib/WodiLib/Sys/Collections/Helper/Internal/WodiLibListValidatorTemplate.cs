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
    ///     <see cref="WodiLibListValidatorTemplate{TIn, TOut}"/> をオーバーライドしている。
    ///     入力型と出力型が同じ場合こちらを使用。
    /// </remarks>
    /// <typeparam name="T">リスト要素型</typeparam>
    public abstract class WodiLibListValidatorTemplate<T> : WodiLibListValidatorTemplate<T, T>
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="target">各種メソッドの操作対象</param>
        protected WodiLibListValidatorTemplate(IReadOnlyExtendedList<T, T> target) : base(target)
        {
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
        public virtual void InItem(NamedValue<IReadOnlyList<TIn>> items)
        {
            BaseValidator?.InItem(items);
        }

        /// <inheritdoc/>
        public virtual void Constructor(NamedValue<IReadOnlyList<TIn>> initItems)
        {
            BaseValidator?.Constructor(initItems);
        }

        /// <inheritdoc/>
        public virtual void Get(NamedValue<int> index, NamedValue<int> count)
        {
            BaseValidator?.Get(index, count);
        }

        /// <inheritdoc/>
        public virtual void Set(NamedValue<int> index, NamedValue<TIn> item)
        {
            BaseValidator?.Set(index, item);
        }

        /// <inheritdoc/>
        public virtual void Set(NamedValue<int> index, NamedValue<IReadOnlyList<TIn>> items)
        {
            BaseValidator?.Set(index, items);
        }

        /// <inheritdoc/>
        public virtual void Insert(NamedValue<int> index, NamedValue<TIn> item)
        {
            BaseValidator?.Insert(index, item);
        }

        /// <inheritdoc/>
        public virtual void Insert(NamedValue<int> index, NamedValue<IReadOnlyList<TIn>> items)
        {
            BaseValidator?.Insert(index, items);
        }

        /// <inheritdoc/>
        public virtual void Overwrite(NamedValue<int> index, NamedValue<IReadOnlyList<TIn>> items)
        {
            BaseValidator?.Overwrite(index, items);
        }

        /// <inheritdoc/>
        public virtual void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
        {
            BaseValidator?.Move(oldIndex, newIndex, count);
        }

        /// <inheritdoc/>
        public virtual void Remove(NamedValue<TIn?> item)
        {
            BaseValidator?.Remove(item);
        }

        /// <inheritdoc/>
        public virtual void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            BaseValidator?.Remove(index, count);
        }

        /// <inheritdoc/>
        public virtual void AdjustLength(NamedValue<int> length)
        {
            BaseValidator?.AdjustLength(length);
        }

        /// <inheritdoc/>
        public virtual void AdjustLengthIfShort(NamedValue<int> length)
        {
            BaseValidator?.AdjustLengthIfShort(length);
        }

        /// <inheritdoc/>
        public virtual void AdjustLengthIfLong(NamedValue<int> length)
        {
            BaseValidator?.AdjustLengthIfLong(length);
        }

        /// <inheritdoc/>
        public virtual void Reset(NamedValue<IReadOnlyList<TIn>> items)
        {
            BaseValidator?.Reset(items);
        }

        // TODO: 以下、コンパイルエラーを防ぐために一時的に旧メソッドを残しておく（あとで削除する）

        [Obsolete]
        public virtual void InItem(IReadOnlyList<TIn> items)
        {
            BaseValidator?.InItem((nameof(items), items));
        }

        [Obsolete]
        public virtual void Constructor(IReadOnlyList<TIn> initItems)
        {
            BaseValidator?.Constructor((nameof(initItems), initItems));
        }

        [Obsolete]
        public virtual void Get(int index, int count)
        {
            BaseValidator?.Get((nameof(index), index), (nameof(count), count));
        }

        [Obsolete]
        public virtual void Set(int index, TIn item)
        {
            BaseValidator?.Set((nameof(index), index), (nameof(item), item));
        }

        [Obsolete]
        public virtual void Set(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Set((nameof(index), index), (nameof(items), items));
        }

        [Obsolete]
        public virtual void Insert(int index, TIn item)
        {
            BaseValidator?.Insert((nameof(index), index), (nameof(item), item));
        }

        [Obsolete]
        public virtual void Insert(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Insert((nameof(index), index), (nameof(items), items));
        }

        [Obsolete]
        public virtual void Overwrite(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Overwrite((nameof(index), index), (nameof(items), items));
        }

        [Obsolete]
        public virtual void Move(int oldIndex, int newIndex, int count)
        {
            BaseValidator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex), (nameof(count), count));
        }

        [Obsolete]
        public virtual void Remove(TIn? item)
        {
            BaseValidator?.Remove((nameof(item), item));
        }

        [Obsolete]
        public virtual void Remove(int index, int count)
        {
            BaseValidator?.Remove((nameof(index), index), (nameof(count), count));
        }

        [Obsolete]
        public virtual void AdjustLength(int length)
        {
            BaseValidator?.AdjustLength((nameof(length), length));
        }

        [Obsolete]
        public virtual void AdjustLengthIfShort(int length)
        {
            BaseValidator?.AdjustLengthIfShort((nameof(length), length));
        }

        [Obsolete]
        public virtual void AdjustLengthIfLong(int length)
        {
            BaseValidator?.AdjustLengthIfLong((nameof(length), length));
        }

        [Obsolete]
        public virtual void Reset(IReadOnlyList<TIn> items)
        {
            BaseValidator?.Reset((nameof(items), items));
        }
    }
}
