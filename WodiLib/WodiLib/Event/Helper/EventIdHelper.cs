// ========================================
// Project Name : WodiLib
// File Name    : EventIdHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Event
{
    /// <summary>
    /// イベントIDに関するヘルパークラス
    /// （将来EventIdオブジェクトに載せ替え）
    /// </summary>
    [Obsolete]
    public static class EventIdHelper
    {
        /// <summary>
        /// マップイベントIDかどうかを返す。
        /// </summary>
        /// <param name="eventId">イベントID</param>
        /// <returns>マップイベントIDの場合true</returns>
        public static bool IsMapEventId(int eventId)
        {
            return (EventIdConstant.MapEventId_Min <= eventId
                    && eventId <= EventIdConstant.MapEventId_Max)
                   || eventId == EventIdConstant.ThisEventId;
        }

        /// <summary>
        /// コモンイベントIDかどうかを返す。
        /// </summary>
        /// <param name="eventId">イベントID</param>
        /// <returns>イベントID絶対指定・相対指定の場合もtrue</returns>
        public static bool IsCommonEventId(int eventId)
        {
            return IsNormalCommonEventId(eventId)
                   || IsAbsoluteCommonId(eventId)
                   || IsRelativeCommonId(eventId)
                   || eventId == EventIdConstant.ThisEventId;
        }

        /// <summary>
        /// コモンイベントIDかどうかを返す。
        /// </summary>
        /// <param name="eventId">イベントID</param>
        /// <returns>イベントID絶対指定・相対指定の場合はfalse</returns>
        public static bool IsNormalCommonEventId(int eventId)
        {
            return eventId >= EventIdConstant.CommonEventId_Min &&
                   eventId <= EventIdConstant.CommonEventId_Max;
        }

        /// <summary>
        /// 絶対指定コモンイベントIDかどうかを返す。
        /// </summary>
        /// <param name="eventId">イベントID</param>
        /// <returns>絶対指定コモンイベントIDの場合true</returns>
        public static bool IsAbsoluteCommonId(int eventId)
        {
            return EventIdConstant.CommonEventOffset <= eventId
                   && eventId <= EventIdConstant.CommonEventOffset + 9999;
        }

        /// <summary>
        /// 相対指定コモンイベントIDかどうかを返す。
        /// </summary>
        /// <param name="eventId">イベントID</param>
        /// <returns>相対指定コモンイベントIDの場合true</returns>
        public static bool IsRelativeCommonId(int eventId)
        {
            return eventId >= EventIdConstant.CommonEventRelativeOffset_Min
                   && eventId <= EventIdConstant.CommonEventRelativeOffset_Max;
        }
    }
}