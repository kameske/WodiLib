// ========================================
// Project Name : WodiLib
// File Name    : KeyInputAutoMouse.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・自動キー入力（マウス）
    /// </summary>
    public class KeyInputAutoMouse : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.KeyInputAuto;

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                if (IsInputWheel) return 0x04;
                if (IsInputPosition) return 0x04;
                return 0x02;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 3)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode;

                case 1:
                    var byte3 = EventCommandConstant.KeyInputAuto.Type.Mouse;
                    return new byte[] {inputFlag.ToByte(), 0x00, 0x00, byte3}.ToInt32(Endian.Environment);

                case 2:
                    if (IsInputPosition) return PositionX;
                    if (IsInputWheel) return MouseWheel;
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 1, index));

                case 3:
                    if (IsInputPosition) return PositionY;
                    if (IsInputWheel) return 0;
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 2, index));

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 3, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 3)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    inputFlag = new InputFlag(bytes[0]);
                    return;
                }

                case 2:
                    if (IsInputPosition) PositionX = value;
                    else if (IsInputWheel) MouseWheel = value;
                    return;

                case 3:
                    if (IsInputPosition) PositionY = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 3, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetStringVariable(int index)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void SetStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private InputFlag inputFlag = new InputFlag();

        /// <summary>左クリック</summary>
        public bool IsInputLeftClick
        {
            get => inputFlag.IsInputLeftClick;
            set => inputFlag.IsInputLeftClick = value;
        }

        /// <summary>右クリック</summary>
        public bool IsInputRightClick
        {
            get => inputFlag.IsInputRightClick;
            set => inputFlag.IsInputRightClick = value;
        }

        /// <summary>中クリック</summary>
        public bool IsInputCenterClick
        {
            get => inputFlag.IsInputCenterClick;
            set => inputFlag.IsInputCenterClick = value;
        }

        /// <summary>ホイール入力</summary>
        public bool IsInputWheel
        {
            get => inputFlag.IsInputWheel;
            set => inputFlag.IsInputWheel = value;
        }

        /// <summary>左クリック</summary>
        public int MouseWheel { get; set; }

        /// <summary>位置</summary>
        public bool IsInputPosition
        {
            get => inputFlag.IsInputPosition;
            set => inputFlag.IsInputPosition = value;
        }

        /// <summary>X座標</summary>
        public int PositionX { get; set; }

        /// <summary>Y座標</summary>
        public int PositionY { get; set; }

        private class InputFlag
        {
            // ホイール入力と位置指定は同時には不可能

            /// <summary>左クリック</summary>
            public bool IsInputLeftClick { get; set; }

            /// <summary>右クリック</summary>
            public bool IsInputRightClick { get; set; }

            /// <summary>中クリック</summary>
            public bool IsInputCenterClick { get; set; }

            private bool isInputWheel;

            /// <summary>ホイール入力</summary>
            public bool IsInputWheel
            {
                get => isInputWheel;
                set
                {
                    isInputWheel = value;
                    if (value) isInputPosition = false;
                }
            }

            private bool isInputPosition;

            /// <summary>位置</summary>
            public bool IsInputPosition
            {
                get => isInputPosition;
                set
                {
                    isInputPosition = value;
                    if (value) isInputWheel = false;
                }
            }

            public InputFlag(byte flag = 0x00)
            {
                IsInputLeftClick = (flag & FlgLeftClick) != 0;
                IsInputRightClick = (flag & FlgRightClick) != 0;
                IsInputCenterClick = (flag & FlgCenterClick) != 0;
                IsInputWheel = (flag & FlgMouseWheel) != 0;
                IsInputPosition = (flag & FlgPosition) != 0;
            }

            private const byte FlgLeftClick = 0x01;
            private const byte FlgRightClick = 0x02;
            private const byte FlgCenterClick = 0x04;
            private const byte FlgPosition = 0x08;
            private const byte FlgMouseWheel = 0x10;

            public byte ToByte()
            {
                byte result = 0x00;
                if (IsInputLeftClick) result += FlgLeftClick;
                if (IsInputRightClick) result += FlgRightClick;
                if (IsInputCenterClick) result += FlgCenterClick;
                if (IsInputWheel) result += FlgMouseWheel;
                if (IsInputPosition) result += FlgPosition;
                return result;
            }
        }
    }
}