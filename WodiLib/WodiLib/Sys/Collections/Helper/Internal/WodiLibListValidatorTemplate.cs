// ========================================
// Project Name : WodiLib
// File Name    : WodiLibListValidatorTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// リスト編集メソッドの引数検証クラステンプレート
    /// </summary>
    /// <remarks>
    /// 継承先で定義した <see cref="BaseValidator"/> を起動するだけの実装。<br/>
    /// <see cref="BaseValidator"/> の検証処理を拡張したいときに対象のメソッドをオーバーライドして
    /// 処理拡張を行う。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    internal abstract class WodiLibListValidatorTemplate<T> : IWodiLibListValidator<T>
    {
        /// <summary>
        /// カスタマイズ元の検証処理。
        /// </summary>
        /// <remarks>
        /// カスタマイズ元が存在しない場合 <see langword="null"/> を返却する。
        /// </remarks>
        protected abstract IWodiLibListValidator<T>? BaseValidator { get; }

        protected IReadOnlyExtendedList<T> Target { get; }

        protected WodiLibListValidatorTemplate(IReadOnlyExtendedList<T> target)
        {
            Target = target;
        }

        public virtual void Constructor(IReadOnlyList<T> initItems)
        {
            BaseValidator?.Constructor(initItems);
        }

        public virtual void Get(int index, int count)
        {
            BaseValidator?.Get(index, count);
        }

        public virtual void Set(int index, T item)
        {
            BaseValidator?.Set(index, item);
        }

        public virtual void Set(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Set(index, items);
        }

        public virtual void Insert(int index, T item)
        {
            BaseValidator?.Insert(index, item);
        }

        public virtual void Insert(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Insert(index, items);
        }

        public virtual void Overwrite(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Overwrite(index, items);
        }

        public virtual void Move(int oldIndex, int newIndex, int count)
        {
            BaseValidator?.Move(oldIndex, newIndex, count);
        }

        public virtual void Remove([AllowNull] T item)
        {
            BaseValidator?.Remove(item);
        }

        public virtual void Remove(int index, int count)
        {
            BaseValidator?.Remove(index, count);
        }

        public virtual void AdjustLength(int length)
        {
            BaseValidator?.AdjustLength(length);
        }

        public virtual void AdjustLengthIfShort(int length)
        {
            BaseValidator?.AdjustLengthIfShort(length);
        }

        public virtual void AdjustLengthIfLong(int length)
        {
            BaseValidator?.AdjustLengthIfLong(length);
        }

        public virtual void Reset(IReadOnlyList<T> items)
        {
            BaseValidator?.Reset(items);
        }
    }
}
