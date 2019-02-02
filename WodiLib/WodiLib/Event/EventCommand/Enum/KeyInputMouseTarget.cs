// ========================================
// Project Name : WodiLib
// File Name    : KeyInputMouseTarget.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// キー入力（マウス）取得対象
    /// </summary>
    public class KeyInputMouseTarget : TypeSafeEnum<KeyInputMouseTarget>
    {
        /// <summary>クリック状態</summary>
        public static readonly KeyInputMouseTarget ClickState;

        /// <summary>マウスX座標</summary>
        public static readonly KeyInputMouseTarget MouseXPosition;

        /// <summary>マウスY座標</summary>
        public static readonly KeyInputMouseTarget MouseYPosition;

        /// <summary>ホイール回転量</summary>
        public static readonly KeyInputMouseTarget WheelDelta;

        /// <summary>値</summary>
        public byte Code { get; }

        static KeyInputMouseTarget()
        {
            ClickState = new KeyInputMouseTarget(nameof(ClickState), 0x00);
            MouseXPosition = new KeyInputMouseTarget(nameof(MouseXPosition), 0x01);
            MouseYPosition = new KeyInputMouseTarget(nameof(MouseYPosition), 0x02);
            WheelDelta = new KeyInputMouseTarget(nameof(WheelDelta), 0x03);
        }

        private KeyInputMouseTarget(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static KeyInputMouseTarget FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}