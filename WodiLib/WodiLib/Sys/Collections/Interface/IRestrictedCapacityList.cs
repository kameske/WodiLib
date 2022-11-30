// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityList.cs
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
    ///     容量制限のあるListインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="ObservableCollection{T}"/> をベースに、容量制限を設けた機能。
    ///         <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///         それ以外にもいくつかメソッドを追加している。
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> が変更通知を行うクラスだった場合、
    ///         通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    public interface IRestrictedCapacityList<T> :
        IReadOnlyList<T>,
        IModelBase<IRestrictedCapacityList<T>>,
        INotifyCollectionChanged
    {
        #region Properties

        /// <inheritdoc cref="IExtendedList{T}.this"/>
        public new T this[int index] { get; set; }

        #endregion

        #region Get Config

        /// <summary>
        ///     容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public int GetMaxCapacity();

        /// <summary>
        ///     容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public int GetMinCapacity();

        #endregion

        #region CRUD

        /// <inheritdoc cref="ExtendedListInterfaceExtension.GetRange{T}"/>
        public IEnumerable<T> GetRange(int index, int count);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.SetRange{T}"/>
        public void SetRange(int index, IEnumerable<T> items);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Add{T}"/>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public void Add(T item);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.AddRange{T}"/>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public void AddRange(IEnumerable<T> items);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Insert{T}"/>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public void Insert(int index, T item);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.InsertRange{T}"/>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void InsertRange(int index, IEnumerable<T> items);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Overwrite{T}"/>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void Overwrite(int index, IEnumerable<T> items);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Move{T}"/>
        public void Move(int oldIndex, int newIndex);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.MoveRange{T}"/>
        public void MoveRange(int oldIndex, int newIndex, int count);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Remove{T}"/>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public void Remove(int index);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.RemoveRange{T}"/>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveRange(int index, int count);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLength{T}" path="/summary"/>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLength{T}" path="/exception"/>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        public void AdjustLength(int length);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLengthIfShort{T}" path="/summary"/>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLengthIfShort{T}" path="/exception"/>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        public void AdjustLengthIfShort(int length);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLengthIfLong{T}" path="/summary"/>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLengthIfLong{T}" path="/exception"/>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        public void AdjustLengthIfLong(int length);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Reset{T}"/>
        public void Reset(IEnumerable<T> initItems);

        /// <inheritdoc cref="ExtendedListInterfaceExtension.Clear{T}"/>
        public void Clear();

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
        ///     <see cref="Add"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Add" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdd(T item);

        /// <summary>
        ///     <see cref="AddRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddRange" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAddRange(IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Insert"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Insert" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateInsert(int index, T item);

        /// <summary>
        ///     <see cref="InsertRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertRange" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateInsertRange(int index, IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Overwrite"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Overwrite" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateOverwrite(int index, IEnumerable<T> items);

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
        ///     <see cref="Remove"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Remove" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateRemove(int index);

        /// <summary>
        ///     <see cref="RemoveRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveRange" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateRemoveRange(int index, int count);

        /// <summary>
        ///     <see cref="AdjustLength"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustLength" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdjustLength(int length);

        /// <summary>
        ///     <see cref="AdjustLengthIfShort"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustLengthIfShort" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdjustLengthIfShort(int length);

        /// <summary>
        ///     <see cref="AdjustLengthIfLong"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustLengthIfLong" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdjustLengthIfLong(int length);

        /// <summary>
        ///     <see cref="Reset"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateReset(IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Clear"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateClear();

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
        ///     <see cref="Add"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Add" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddCore(T item);

        /// <summary>
        ///     <see cref="AddRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddRange" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddRangeCore(IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Insert"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Insert" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void InsertCore(int index, T item);

        /// <summary>
        ///     <see cref="InsertRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertRange" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void InsertRangeCore(int index, IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Overwrite"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Overwrite" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void OverwriteCore(int index, IEnumerable<T> items);

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
        ///     <see cref="Remove"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Remove" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void RemoveCore(int index);

        /// <summary>
        ///     <see cref="RemoveRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveRange" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void RemoveRangeCore(int index, int count);

        /// <summary>
        ///     <see cref="AdjustLength"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustLength" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AdjustLengthCore(int length);

        /// <summary>
        ///     <see cref="AdjustLengthIfShort"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustLengthIfShort" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AdjustLengthIfShortCore(int length);

        /// <summary>
        ///     <see cref="AdjustLengthIfLong"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustLengthIfLong" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AdjustLengthIfLongCore(int length);

        /// <summary>
        ///     <see cref="Reset"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Reset" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetCore(IEnumerable<T> items);

        /// <summary>
        ///     <see cref="Clear"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ClearCore();

        #endregion
    }
}
