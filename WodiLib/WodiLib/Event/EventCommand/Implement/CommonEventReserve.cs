// ========================================
// Project Name : WodiLib
// File Name    : CommonEventReserve.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・コモンイベント予約
    /// </summary>
    public class CommonEventReserve : CallCommonEventBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.CommonEventReserve;

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
    }
}