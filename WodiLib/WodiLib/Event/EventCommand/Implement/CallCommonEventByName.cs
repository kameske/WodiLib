// ========================================
// Project Name : WodiLib
// File Name    : CallCommonEventByName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・コモンイベント（イベント名）
    /// </summary>
    public class CallCommonEventByName : CallCommonEventBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.CallCommonEventByName;

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
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(EventName));
                EventIdOrName.Merge(value);
            }
        }

        /// <inheritdoc />
        /// <summary>イベント文字列指定フラグ</summary>
        protected override bool IsOrderByString => true;
    }
}