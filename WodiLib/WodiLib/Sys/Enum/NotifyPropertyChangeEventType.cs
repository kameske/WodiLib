// ========================================
// Project Name : WodiLib
// File Name    : NotifyPropertyChangeEventType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     プロパティ変更通知種別
    /// </summary>
    public class NotifyPropertyChangeEventType : TypeSafeEnum<NotifyPropertyChangeEventType>
    {
        /// <summary>
        ///     有効
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、プロパティ変更前/後の通知が行われる。
        /// </remarks>
        public static readonly NotifyPropertyChangeEventType Enabled;

        /// <summary>
        ///     有効
        /// </summary>
        /// <remarks>
        ///     この値が設定されている場合、プロパティ変更前/後の通知が行われない。
        /// </remarks>
        public static readonly NotifyPropertyChangeEventType Disabled;

        /// <summary>
        ///     IDからプロパティ変更通知種別を取得する。
        /// </summary>
        /// <param name="id">対象ID</param>
        /// <returns>IDから取得したインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="id"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     指定した <paramref name="id"/> に該当するプロパティ変更通知種別が存在しない場合。
        /// </exception>
        public static NotifyPropertyChangeEventType FromId(string id)
        {
            ThrowHelper.ValidateArgumentNotNull(id is null, nameof(id));
            return _FindFirst(x => x.Id.Equals(id));
        }

        /// <summary>
        ///     通知フラグ
        /// </summary>
        public bool IsNotify { get; }

        static NotifyPropertyChangeEventType()
        {
            Enabled = new NotifyPropertyChangeEventType("Enabled", true);
            Disabled = new NotifyPropertyChangeEventType("Disabled", false);
        }

        private NotifyPropertyChangeEventType(string id, bool isNotify) : base(id)
        {
            IsNotify = isNotify;
        }
    }
}
