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
    public record NotifyPropertyChangeEventType : TypeSafeEnum<NotifyPropertyChangeEventType>
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
        /// 通知フラグ
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

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}