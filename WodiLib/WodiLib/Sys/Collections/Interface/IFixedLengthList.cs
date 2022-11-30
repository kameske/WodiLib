// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     長さが固定されたListインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="ObservableCollection{T}"/> をベースに、容量を固定した機能。
    ///         <see cref="ObservableCollection{T}"/> の Read, Update 各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///         それ以外にもいくつかメソッドを追加している。
    ///         固定しているのは容量のみで、要素の入れ替えや更新は可能。
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> が変更通知を行うクラスだった場合、
    ///         通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">リスト要素型</typeparam>
    public interface IFixedLengthList<T> :
        IReadOnlyList<T>,
        IModelBase<IFixedLengthList<T>>,
        INotifyCollectionChanged
    {
        #region Properties

        /// <inheritdoc cref="IExtendedList{T}.this"/>
        public new T this[int index] { get; set; }

        #endregion

        #region CRUD

        /// <inheritdoc cref="ExtendedListInterfaceExtension.GetRange{T}"/>
        public IEnumerable<T> GetRange(int index, int count);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.SetRange{T}"/>
        public void SetRange(int index, IEnumerable<T> items);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Move{T}"/>
        public void Move(int oldIndex, int newIndex);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.MoveRange{T}"/>
        public void MoveRange(int oldIndex, int newIndex, int count);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Reset{T}"/>
        /// <exception cref="ArgumentException">
        /// <paramref name="initItems"/> の要素数が <see cref="IReadOnlyCollection{T}.Count"/> と
        /// 異なる場合。
        /// </exception>
        public void Reset(IEnumerable<T> initItems);

        /// <summary>
        /// 要素をデフォルト値で一新する。
        /// </summary>
        public void Reset();

        #endregion

        #region Validate

        /// <summary>
        ///     インデクサによる取得の検証処理。
        /// </summary>
        /// <inheritdoc cref="this[int]" path="param|exception[cref='ArgumentOutOfRangeException']"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGet(int index);

        /// <summary>
        ///     <see cref="GetRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetRange" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGetRange(int index, int count);

        /// <summary>
        ///     インデクサによる更新の検証処理。
        /// </summary>
        /// <param name="index"><inheritdoc cref="this[int]" path="param[name='index']"/></param>
        /// <param name="item">編集要素</param>
        /// <inheritdoc cref="this[int]" path="exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateSet(int index, T item);

        /// <summary>
        ///     <see cref="SetRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetRange" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateSetRange(int index, IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Move"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Move" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateMove(int oldIndex, int newIndex);

        /// <summary>
        ///     <see cref="MoveRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveRange" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateMoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     <see cref="Reset(System.Collections.Generic.IEnumerable{T})"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{T})" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateReset(IEnumerable<T> items);

        #endregion

        #region CRUD core

        /// <summary>
        ///     インデクサによる取得処理中核。
        /// </summary>
        /// <inheritdoc cref="this[int]" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T GetCore(int index);

        /// <summary>
        ///     <see cref="GetRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRange" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<T> GetRangeCore(int index, int count);

        /// <summary>
        ///     インデクサによる更新処理中核。
        /// </summary>
        /// <param name="index"><inheritdoc cref="this[int]" path="param"/></param>
        /// <param name="item">編集要素</param>
        /// <inheritdoc cref="this[int]" path="exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void SetCore(int index, T item);

        /// <summary>
        ///     <see cref="SetRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetRange" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void SetRangeCore(int index, IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Move"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Move" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void MoveCore(int oldIndex, int newIndex);

        /// <summary>
        ///     <see cref="MoveRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveRange" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void MoveRangeCore(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     <see cref="Reset(System.Collections.Generic.IEnumerable{T})"/>,
        ///     <see cref="Reset()"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{T})" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetCore(IEnumerable<T> items);

        #endregion
    }
}
