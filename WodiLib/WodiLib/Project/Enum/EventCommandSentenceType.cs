// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文種別
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EventCommandSentenceType : TypeSafeEnum<EventCommandSentenceType>
    {
        /// <summary>マップイベント</summary>
        public static readonly EventCommandSentenceType Map;

        /// <summary>コモンイベント</summary>
        public static readonly EventCommandSentenceType Common;

        static EventCommandSentenceType()
        {
            Map = new EventCommandSentenceType(nameof(Map));
            Common = new EventCommandSentenceType(nameof(Common));
        }

        private EventCommandSentenceType(string id) : base(id)
        {
        }
    }
}