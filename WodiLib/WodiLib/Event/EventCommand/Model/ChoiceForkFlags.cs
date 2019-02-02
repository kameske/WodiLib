// ========================================
// Project Name : WodiLib
// File Name    : ChoiceForkFlags.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 選択肢分岐フラグ
    /// </summary>
    public class ChoiceForkFlags
    {
        /// <summary>強制中断フラグ</summary>
        public bool IsStopForce { get; set; }

        /// <summary>左キー分岐</summary>
        public bool IsForkLeftKey { get; set; }

        /// <summary>右キー分岐</summary>
        public bool IsForkRightKey { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="src">[CanBeNull] バイトコード</param>
        public ChoiceForkFlags(byte? src = null)
        {
            if (!src.HasValue) return;
            IsStopForce = (src & StopForceFlag) != 0;
            IsForkLeftKey = (src & ForkLeftKeyFlag) != 0;
            IsForkRightKey = (src & ForkRightKeyFlag) != 0;
        }

        /// <summary>
        /// バイトに変換する。
        /// </summary>
        /// <returns>バイトデータ</returns>
        public byte ToByte()
        {
            byte result = 0x00;
            if (IsStopForce) result += StopForceFlag;
            if (IsForkLeftKey) result += ForkLeftKeyFlag;
            if (IsForkRightKey) result += ForkRightKeyFlag;
            return result;
        }

        /// <summary>強制中断フラグ</summary>
        private const byte StopForceFlag = 0x04;

        /// <summary>左キー分岐フラグ</summary>
        private const byte ForkLeftKeyFlag = 0x01;

        /// <summary>右キー分岐フラグ</summary>
        private const byte ForkRightKeyFlag = 0x02;
    }
}