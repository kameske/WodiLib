// ========================================
// Project Name : WodiLib
// File Name    : ChoiceForkFlags.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 選択肢分岐フラグ
    /// </summary>
    [Serializable]
    public class ChoiceForkFlags : ModelBase<ChoiceForkFlags>
    {
        private bool isStopForce;

        /// <summary>強制中断フラグ</summary>
        public bool IsStopForce
        {
            get => isStopForce;
            set
            {
                isStopForce = value;
                NotifyPropertyChanged();
            }
        }

        private bool isForkLeftKey;

        /// <summary>左キー分岐</summary>
        public bool IsForkLeftKey
        {
            get => isForkLeftKey;
            set
            {
                isForkLeftKey = value;
                NotifyPropertyChanged();
            }
        }

        private bool isForkRightKey;

        /// <summary>右キー分岐</summary>
        public bool IsForkRightKey
        {
            get => isForkRightKey;
            set
            {
                isForkRightKey = value;
                NotifyPropertyChanged();
            }
        }

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
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(ChoiceForkFlags other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsStopForce == other.IsStopForce
                   && IsForkLeftKey == other.IsForkLeftKey
                   && IsForkRightKey == other.IsForkRightKey;
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