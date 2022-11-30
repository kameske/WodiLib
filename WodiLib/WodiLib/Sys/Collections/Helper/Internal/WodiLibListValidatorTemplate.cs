// ========================================
// Project Name : WodiLib
// File Name    : WodiLibListValidatorTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

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
    /// <typeparam name="T">リスト要素型</typeparam>
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
        protected TargetAdapter Target { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="target">各種メソッドの操作対象</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target"/> が <see langword="null"/> の場合。
        /// </exception>
        protected WodiLibListValidatorTemplate(TargetAdapter target)
        {
            ThrowHelper.ValidateArgumentNotNull(target is null, nameof(target));

            Target = target;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="target">各種メソッドの操作対象</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target"/> が <see langword="null"/> の場合。
        /// </exception>
        protected WodiLibListValidatorTemplate(IRestrictedCapacityList<T> target) : this(new TargetAdapter(target))
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="target">各種メソッドの操作対象</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target"/> が <see langword="null"/> の場合。
        /// </exception>
        protected WodiLibListValidatorTemplate(IFixedLengthList<T> target) : this(
            new TargetAdapter(target, target.Count)
        )
        {
        }

        /// <inheritdoc/>
        public virtual void Constructor(NamedValue<IEnumerable<T>> initItems)
            => BaseValidator?.Constructor(initItems);

        /// <inheritdoc/>
        public virtual void Get(NamedValue<int> index, NamedValue<int> count)
            => BaseValidator?.Get(index, count);

        /// <inheritdoc/>
        public virtual void Set(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
            => BaseValidator?.Set(index, items);

        /// <inheritdoc/>
        public virtual void Insert(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
            => BaseValidator?.Insert(index, items);

        /// <inheritdoc/>
        public virtual void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
            => BaseValidator?.Overwrite(index, items);

        /// <inheritdoc/>
        public virtual void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
            => BaseValidator?.Move(oldIndex, newIndex, count);

        /// <inheritdoc/>
        public virtual void Remove(NamedValue<int> index, NamedValue<int> count)
            => BaseValidator?.Remove(index, count);

        /// <inheritdoc/>
        public virtual void AdjustLength(NamedValue<int> length)
            => BaseValidator?.AdjustLength(length);

        /// <inheritdoc/>
        public virtual void Reset(NamedValue<IEnumerable<T>> items)
            => BaseValidator?.Reset(items);

        /// <inheritdoc/>
        public void Clear()
            => BaseValidator?.Clear();

        /// <summary>
        /// 検証対象Adapter
        /// </summary>
        protected class TargetAdapter
        {
            /*
             * 検証処理で必要なメソッド等あればここに定義を追加する。
             */

            private readonly Func<int> getCount;
            private readonly Func<int> getMaxCapacity;
            private readonly Func<int> getMinCapacity;

            private readonly IEnumerable<T> targetItems;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="target"/> が <see langword="null"/> の場合。
            /// </exception>
            public TargetAdapter(IRestrictedCapacityList<T> target)
            {
                ThrowHelper.ValidateArgumentNotNull(target is null, nameof(target));

                getCount = () => target.Count;
                getMaxCapacity = target.GetMaxCapacity;
                getMinCapacity = target.GetMinCapacity;
                targetItems = target;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="capacity">容量</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="target"/> が <see langword="null"/> の場合。
            /// </exception>
            public TargetAdapter(IFixedLengthList<T> target, int capacity)
            {
                ThrowHelper.ValidateArgumentNotNull(target is null, nameof(target));

                getCount = () => target.Count;
                getMaxCapacity = () => capacity;
                getMinCapacity = () => capacity;
                targetItems = target;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="capacityGetter">容量取得</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="target"/>, <paramref name="capacityGetter"/> が <see langword="null"/> の場合。
            /// </exception>
            public TargetAdapter(IFixedLengthList<T> target, Func<int> capacityGetter)
            {
                ThrowHelper.ValidateArgumentNotNull(target is null, nameof(target));
                ThrowHelper.ValidateArgumentNotNull(capacityGetter is null, nameof(capacityGetter));

                getCount = () => target.Count;
                getMaxCapacity = capacityGetter;
                getMinCapacity = capacityGetter;
                targetItems = target;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="minCapacity">最小容量</param>
            /// <param name="maxCapacity">最大容量</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="target"/> が <see langword="null"/> の場合。
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="maxCapacity"/> が <paramref name="minCapacity"/> 未満の場合。
            /// </exception>
            public TargetAdapter(IEnumerable<T> target, int minCapacity, int maxCapacity)
            {
                ThrowHelper.ValidateArgumentNotNull(target is null, nameof(target));
                ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                    minCapacity > maxCapacity,
                    nameof(maxCapacity),
                    nameof(minCapacity),
                    maxCapacity
                );

                getCount = () =>
                {
                    var array = target.ToArray();
                    return array.Length;
                };
                getMaxCapacity = () => maxCapacity;
                getMinCapacity = () => minCapacity;
                targetItems = target;
            }

            /// <inheritdoc cref="ICollection{T}.Count"/>
            public int Count => getCount();

            /// <inheritdoc cref="IRestrictedCapacityList{T}.GetMaxCapacity"/>
            public int GetMaxCapacity() => getMaxCapacity();

            /// <inheritdoc cref="IRestrictedCapacityList{T}.GetMinCapacity"/>
            public int GetMinCapacity() => getMinCapacity();

            /// <summary>
            /// 指定された要素のインデックスを取得する。
            /// </summary>
            /// <returns>指定された要素が存在しない場合、-1</returns>
            public int FindIndex(T item) => targetItems.FindIndex(
                targetItem => ReferenceEquals(item, targetItem)
            );
        }
    }
}
