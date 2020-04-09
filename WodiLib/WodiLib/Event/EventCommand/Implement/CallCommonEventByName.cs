// ========================================
// Project Name : WodiLib
// File Name    : CallCommonEventByName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・コモンイベント（イベント名）
    /// </summary>
    [Serializable]
    public class CallCommonEventByName : CallCommonEventBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■イベントの挿入[名]： {3}[\"{0}\"] <コモンEv {1}> {2}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.CallCommonEventByName;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[NotNull] コモンイベント名</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string EventName
        {
            get => EventIdOrName.ToStr();
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(EventName));
                EventIdOrName.Merge(value);
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>イベント文字列指定フラグ</summary>
        protected override bool IsOrderByString => true;

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
            var id = resolver.GetCommonEventIdString(EventName);

            return string.Format(EventCommandSentenceFormat,
                EventName, id, argsCommandString, returnVarString);
        }
    }
}