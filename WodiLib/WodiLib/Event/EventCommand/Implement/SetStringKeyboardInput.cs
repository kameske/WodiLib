// ========================================
// Project Name : WodiLib
// File Name    : SetStringKeyboardInput.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・文字列操作（キーボード入力）
    /// </summary>
    public class SetStringKeyboardInput : SetStringBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x04;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>右辺（文字列）</summary>
        public int RightSide { get; set; }

        /// <summary>文字数</summary>
        public int Length { get; set; }

        /// <summary>キャンセル有り</summary>
        public bool CanCancel { get; set; }

        /// <summary>左辺を置換</summary>
        public bool IsReplaceLeftSide { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>右辺設定コード値</summary>
        protected override byte RightSidePropertyCode
            => EventCommandConstant.SetString.RightSidePropertyCode.KeyboardInput;

        /// <inheritdoc />
        /// <summary>右辺特殊設定コード値</summary>
        protected override byte RightSideSpecialSettingsCode
        {
            get
            {
                byte result = 0x00;
                if (CanCancel) result += FlgCancel;
                if (IsReplaceLeftSide) result += FlgReplaceLeftSide;
                return result;
            }
            set
            {
                CanCancel = (value & FlgCancel) != 0;
                IsReplaceLeftSide = (value & FlgReplaceLeftSide) != 0;
            }
        }

        /// <inheritdoc />
        /// <summary>右辺オプション</summary>
        protected override int RightSideOption
        {
            get => Length;
            set => Length = value;
        }

        /// <inheritdoc />
        /// <summary>右辺文字列</summary>
        protected override string RightSideString
        {
            get => "";
            set { }
        }

        /// <inheritdoc />
        /// <summary>右辺置換文字列</summary>
        protected override string RightSideReplaceString
        {
            get => "";
            set { }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgCancel = 0x10;
        private const byte FlgReplaceLeftSide = 0x20;
    }
}