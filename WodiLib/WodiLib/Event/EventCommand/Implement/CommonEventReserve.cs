// ========================================
// Project Name : WodiLib
// File Name    : CommonEventReserve.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Cmn;
using WodiLib.Common;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・コモンイベント予約
    /// </summary>
    [Serializable]
    public class CommonEventReserve : CallCommonEventBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormatCallById
            = "■次イベントの予約： {2}コモン{0}：[ {1} ]";

        private const string EventCommandSentenceRelativeId = "[このｺﾓﾝから{0}]  ";

        private const string EventCommandSentenceFormatCallByVariableAddress
            = "■次イベントの予約： 変数[{0}]のEv";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.CommonEventReserve;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.YellowGreen;

        /// <inheritdoc />
        protected override string MakeEventCommandMainSentenceInner(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc, string argsCommandString,
            string returnVarString)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));

            return EventId.IsVariableAddressSimpleCheck()
                ? MakeEventCommandMainSentenceCallByVariableAddress(resolver, type, desc)
                : MakeEventCommandMainSentenceCallById(resolver, type, desc);
        }

        private string MakeEventCommandMainSentenceCallById(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var correctEventId = resolver.GetCorrectEventIdByRelativeId(EventId, desc.CommonEventId, type);

            var getEventNameResult = GetEventName(resolver, type, desc, correctEventId);
            var eventName = getEventNameResult.Item1
                ? getEventNameResult.Item2
                : string.Empty;

            var relativeStr = string.Empty;
            if (CommonEventId.CommonEventRelativeOffset_Min <= EventId &&
                EventId <= CommonEventId.CommonEventRelativeOffset_Max)
            {
                var difference = EventId - CommonEventId.CommonEventRelativeOffset;
                var differenceStr = difference >= 0
                    ? $"+{difference}"
                    : difference.ToString();
                relativeStr = string.Format(EventCommandSentenceRelativeId, differenceStr);
            }

            return string.Format(EventCommandSentenceFormatCallById,
                correctEventId, eventName, relativeStr);
        }

        private string MakeEventCommandMainSentenceCallByVariableAddress(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var idStr = resolver.GetNumericVariableAddressStringIfVariableAddress(EventId, type, desc);

            return string.Format(EventCommandSentenceFormatCallByVariableAddress,
                idStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベントID</summary>
        public int EventId
        {
            get => EventIdOrName.ToInt();
            set => EventIdOrName = value;
        }

        /// <inheritdoc />
        /// <summary>イベント文字列指定フラグ</summary>
        protected override bool IsOrderByString => false;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private (bool, string) GetEventName(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc,
            int correctId)
        {
            if (EventId.IsMapEventId())
            {
                return resolver.GetMapEventIdStrByNumericEventId(EventId, type, desc);
            }

            if (EventId.IsVariableAddressSimpleCheck())
            {
                var result = resolver.GetNumericVariableAddressString(EventId, type, desc);
                return (true, result);
            }

            return resolver.GetCommonEventName(correctId);
        }
    }
}