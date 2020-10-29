// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// 容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    /// 要素の読み取り・更新は可能だが追加・削除は不可能。<br/>
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<T> : ExtendedListBase<T>, IFixedLengthList<T>
    {
        /*
         * このクラスの基本的な扱い方については WodiLib.Sys.RestrictedCapacityList とほぼ同様。
         * 要素数を変更可能な処理が除外されているだけ。
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// PropertyChanged イベントの上位伝播を阻止するプロパティ名リスト
        /// </summary>
        private static readonly string[] NotifyPropertyChangedDenyList = new[]
        {
            nameof(IList.Count)
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanging
        {
            add => CollectionChanging_Impl += value;
            remove => CollectionChanging_Impl -= value;
        }

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event PropertyChangedEventHandler? _propertyChanged;

        /// <summary>
        /// プロパティ変更通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        public override event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (_propertyChanged != null
                    && _propertyChanged.GetInvocationList().Contains(value)) return;
                _propertyChanged += value;
            }
            remove => _propertyChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /*
         * 継承先のクラスで内部的に独自リストを使用したい場合、
         * Countプロパティを継承して独自リストの要素数を返すこと。
         */
        /// <inheritdoc />
        public override int Count => Items.Length;

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException">nullをセットしようとした場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        public new T this[int index]
        {
            get => Get_Impl(index, 1).First();
            set => Set_Impl(index, value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Virtual Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト</summary>
        protected virtual T[] Items { get; private set; } = Array.Empty<T>();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        protected FixedLengthList()
        {
            StartObserveNotifyChangeEvent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        protected FixedLengthList(IEnumerable<T> initItems) : base(initItems)
        {
            StartObserveNotifyChangeEvent();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public abstract int GetCapacity();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// すべての列挙子を取得する。
        /// </summary>
        /// <returns>すべての列挙子</returns>
        [Obsolete("不適切なメソッドのため Ver 2.6 で削除します。")]
        public IEnumerable<T> All() => this;

        /// <inheritdoc />
        public void SetRange(int index, IEnumerable<T> items)
            => Set_Impl(index, items.ToArray());

        /// <inheritdoc />
        public void Move(int oldIndex, int newIndex)
            => Move_Impl(oldIndex, newIndex, 1);

        /// <inheritdoc />
        public void MoveRange(int oldIndex, int newIndex, int length)
            => Move_Impl(oldIndex, newIndex, length);

        /// <inheritdoc />
        public void Clear()
            => Clear_Impl();

        /// <inheritdoc />
        public void Reset(IEnumerable<T> initItems)
            => Reset_Impl(initItems.ToArray());

        /// <inheritdoc />
        public bool Contains([AllowNull] T item)
        {
            if (item is null) return false;
            return Items.Contains(item);
        }

        /// <inheritdoc />
        public int IndexOf([AllowNull] T item)
        {
            if (item is null) return -1;
            return Array.IndexOf(Items, item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int index)
            => Items.CopyTo(array, index);

        /// <inheritdoc />
        public bool Equals(IReadOnlyFixedLengthList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public override IEnumerator<T> GetEnumerator()
            => Items.AsEnumerable().GetEnumerator();

        /// <inheritdoc />
        public bool Equals(IFixedLengthList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public override bool Equals(ExtendedListBase<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Implements Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override IExtendedListValidator<T> MakeValidator()
        {
            return new FixedLengthListValidator<T>(this);
        }

        #region Action Core

        /// <inheritdoc />
        protected override void Constructor_Core(params T[] initItems)
        {
            Items = initItems;
        }

        /// <inheritdoc />
        protected override T[] Get_Core(int index, int count)
        {
            return Items.GetRange(index, count)
                .ToArray();
        }

        /// <inheritdoc />
        protected override void Set_Core(int index, params T[] items)
        {
            items.ForEach((item, i) => Items[index + i] = item);
        }

        /// <inheritdoc />
        protected override void Move_Core(int oldIndex, int newIndex, int count)
        {
            // ロジック簡略化のため oldIndex > newIndex を強制する
            if (oldIndex < newIndex)
            {
                var beforeOldIndex = oldIndex;
                var beforeNewIndex = newIndex;

                oldIndex += count;
                newIndex = beforeOldIndex;
                count = beforeNewIndex - beforeOldIndex;
            }

            var movedItems = Enumerable.Range(newIndex, oldIndex - newIndex)
                .Select(i => Items[i])
                .ToArray();
            Array.Copy(Items, oldIndex, Items, newIndex, count);
            Set_Core(newIndex + count, movedItems);
        }

        #endregion

        /// <inheritdoc />
        protected override IEnumerable<T> MakeInitItems()
        {
            return Enumerable.Range(0, GetCapacity())
                .Select(MakeDefaultItem);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Event Observe
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 変更通知の購読を開始する。
        /// </summary>
        private void StartObserveNotifyChangeEvent()
        {
            PropertyChanged_Impl += OnPropertyChanged_Impl;
        }

        /// <summary>
        /// プロパティ変更通知
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnPropertyChanged_Impl(object sender, PropertyChangedEventArgs e)
        {
            // 特定のプロパティの場合ブロック
            if (NotifyPropertyChangedDenyList.Contains(e.PropertyName)) return;

            // 上位イベントに伝播
            _propertyChanged?.Invoke(this, e);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Items), Items);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected FixedLengthList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Items = info.GetValue<T[]>(nameof(Items));
        }
    }
}
