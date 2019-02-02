// ========================================
// Project Name : WodiLib
// File Name    : KeyInputMouse.cs
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
    /// イベントコマンド・キー入力（マウス）
    /// </summary>
    public class KeyInputMouse : KeyInputBase
    {
        /// <summary>キー入力種別フラグ値</summary>
        private readonly byte FlgKeyInputType = EventCommandConstant.KeyInput.Type.Mouse;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 2)] インデックス</param>
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
                    return LeftSide;

                case 2:
                {
                    var byte0 = acceptStatus.ToByte();
                    if (IsWaitForInput) byte0 += FlgWaitForInput;
                    return new byte[] {byte0, FlgKeyInputType, 0x00, 0x00}.ToInt32(Endian.Environment);
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 2, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 2)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    LeftSide = value;
                    return;

                case 2:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    acceptStatus = new AcceptStatus(bytes[0]);
                    IsWaitForInput = (bytes[0] & FlgWaitForInput) != 0;
                    return;
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 2, index));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private AcceptStatus acceptStatus = new AcceptStatus();

        /// <summary>[NotNull] 読み込み対象</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public KeyInputMouseTarget Target
        {
            get => acceptStatus.Target;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Target)));
                acceptStatus.Target = value;
            }
        }

        /// <summary>左クリック</summary>
        public bool IsAcceptLeftClick
        {
            get => acceptStatus.IsAcceptLeftClick;
            set => acceptStatus.IsAcceptLeftClick = value;
        }

        /// <summary>右クリック</summary>
        public bool IsAcceptRightClick
        {
            get => acceptStatus.IsAcceptRightClick;
            set => acceptStatus.IsAcceptRightClick = value;
        }

        /// <summary>中クリック</summary>
        public bool IsAcceptCenterClick
        {
            get => acceptStatus.IsAcceptCenterClick;
            set => acceptStatus.IsAcceptCenterClick = value;
        }

        private class AcceptStatus
        {
            /// <summary>読み込み対象</summary>
            public KeyInputMouseTarget Target { get; set; }

            private bool isAcceptLeftClick;

            /// <summary>左クリック</summary>
            public bool IsAcceptLeftClick
            {
                get => Target == KeyInputMouseTarget.ClickState && isAcceptLeftClick;
                set => isAcceptLeftClick = value;
            }

            private bool isAcceptRightClick;

            /// <summary>右クリック</summary>
            public bool IsAcceptRightClick
            {
                get => Target == KeyInputMouseTarget.ClickState && isAcceptRightClick;
                set => isAcceptRightClick = value;
            }

            private bool isAcceptCenterClick;

            /// <summary>中クリック</summary>
            public bool IsAcceptCenterClick
            {
                get => Target == KeyInputMouseTarget.ClickState && isAcceptCenterClick;
                set => isAcceptCenterClick = value;
            }

            private const byte FlgAcceptLeftClick = 0x10;
            private const byte FlgAcceptRightClick = 0x20;
            private const byte FlgAcceptCenterClick = 0x40;

            public AcceptStatus(byte flags = 0)
            {
                var targetByte = (byte) (flags & 0x0F);
                Target = KeyInputMouseTarget.FromByte(targetByte);

                IsAcceptLeftClick = (flags & FlgAcceptLeftClick) != 0;
                IsAcceptRightClick = (flags & FlgAcceptRightClick) != 0;
                IsAcceptCenterClick = (flags & FlgAcceptCenterClick) != 0;
            }

            public byte ToByte()
            {
                var result = Target.Code;
                if (IsAcceptLeftClick) result += FlgAcceptLeftClick;
                if (IsAcceptRightClick) result += FlgAcceptRightClick;
                if (IsAcceptCenterClick) result += FlgAcceptCenterClick;
                return result;
            }
        }
    }
}