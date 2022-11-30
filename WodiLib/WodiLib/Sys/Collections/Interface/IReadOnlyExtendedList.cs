// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自読み取り専用リストインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="IReadOnlyList{T}"/> のメソッドと ObservableCollection の機能を融合した機能。
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> が変更通知を行うクラスだった場合、
    ///         通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">リスト要素型</typeparam>
    public interface IReadOnlyExtendedList<T> :
        IReadOnlyList<T>,
        IReadOnlyModelBase<IReadOnlyExtendedList<T>>,
        INotifyCollectionChanged
    {
        /// <inheritdoc cref="ExtendedListInterfaceExtension.GetRange{T}"/>
        public IEnumerable<T> GetRange(int index, int count);

        /// <summary>
        ///     インデクサによる取得の検証処理。
        /// </summary>
        /// <inheritdoc cref="IReadOnlyList{T}.this" path="param|exception[cref='ArgumentOutOfRangeException']"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGet(int index);

        /// <summary>
        ///     <see cref="GetRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetRange" path="param|exception"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGetRange(int index, int count);

        /// <summary>
        ///     インデクサによる取得処理中核。
        /// </summary>
        /// <inheritdoc cref="IReadOnlyList{T}.this" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T GetCore(int index);
        
        /// <summary>
        ///     <see cref="GetRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRange" path="param"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<T> GetRangeCore(int index, int count);
    }
}
