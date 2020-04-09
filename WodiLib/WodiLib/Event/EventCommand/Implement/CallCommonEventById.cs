// ========================================
// Project Name : WodiLib
// File Name    : CallCommonEventById.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Common;
using WodiLib.Project;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・コモンイベント（ID指定）実装
    /// </summary>
    [Serializable]
    public class CallCommonEventById : CallCommonEventBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandCallCommonEventSentenceFormat
            = "■イベントの挿入{4}： {3}コモン{0}：[ {1} ]{2}";

        private const string EventCommandCallRelativeCommonEventSentenceFormat
            = "■イベントの挿入： [このｺﾓﾝから{3}]  コモン{0}：[ {1} ]{2}";

        private const string EventCommandCallMapEventSentenceFormat
            = "■イベントの挿入： {0}：[ {1} ] > ページ{2}";

        private const string EventCommandCallMapEventByVariableAddressSentenceFormat
            = "■イベントの挿入： 変数[{0}]のEv ページ{1}";


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.CallCommonEventById;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベントID
        /// </summary>
        public int EventId
        {
            get => EventIdOrName.ToInt();
            set
            {
                EventIdOrName = value;
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>イベント文字列指定フラグ</summary>
        protected override bool IsOrderByString => false;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentenceInner(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc,
            string argsCommandString, string returnVarString)
        {
            if (EventId.IsMapEventId())
            {
                return MakeEventCommandCallMapEventSentence(resolver, type, desc);
            }

            if (EventId.IsVariableAddress())
            {
                return MakeEventCommandCallMapEventByVariableAddressSentence(
                    resolver, type, desc);
            }

            if (CommonEventId.CommonEventRelativeOffset_Min <= EventId &&
                EventId <= CommonEventId.CommonEventRelativeOffset_Max)
            {
                var difference = EventId - CommonEventId.CommonEventRelativeOffset;

                int targetId = resolver.GetCorrectEventIdByRelativeId(EventId, desc.CommonEventId, type);

                return MakeEventCommandCallCommonRelativeEventSentence(
                    resolver, targetId, difference, argsCommandString);
            }

            return MakeEventCommandCallCommonEventSentence(resolver,
                argsCommandString, returnVarString);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベント呼び出し時のイベントコマンド
        /// </summary>
        private string MakeEventCommandCallCommonEventSentence(
            EventCommandSentenceResolver resolver,
            string argsCommandString, string returnVarString)
        {
            var eventName = resolver.GetCommonEventName(EventId).Item2;

            var correctEventId = EventId - 500000;

            return string.Format(EventCommandCallCommonEventSentenceFormat,
                correctEventId, eventName, argsCommandString, returnVarString, "");
        }

        /// <summary>
        /// コモンイベント相対呼び出し時のイベントコマンド
        /// </summary>
        private string MakeEventCommandCallCommonRelativeEventSentence(
            EventCommandSentenceResolver resolver, int targetCommonEventId,
            int difference, string argsCommandString)
        {
            var eventName = resolver.GetCommonEventName(targetCommonEventId).Item2;

            var diffStr = difference >= 0
                ? $"+{difference}"
                : difference.ToString();

            return string.Format(EventCommandCallRelativeCommonEventSentenceFormat,
                targetCommonEventId, eventName, argsCommandString, diffStr);
        }

        /// <summary>
        /// マップイベント呼び出し時のイベントコマンド
        /// </summary>
        private string MakeEventCommandCallMapEventSentence(
            EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            var eventIdStr = resolver.GetMapEventIdStrByNumericEventId(EventId, type, desc).Item2;
            var eventName = resolver.GetMapEventName(EventId, type, desc).Item2;

            return string.Format(EventCommandCallMapEventSentenceFormat,
                eventIdStr, eventName, Page);
        }

        /// <summary>
        /// 変数指定マップイベント呼び出し時のイベントコマンド
        /// </summary>
        private string MakeEventCommandCallMapEventByVariableAddressSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var varAddressStr = resolver.GetNumericVariableAddressString(EventId, type, desc);

            return string.Format(EventCommandCallMapEventByVariableAddressSentenceFormat,
                varAddressStr, Page);
        }
    }
}