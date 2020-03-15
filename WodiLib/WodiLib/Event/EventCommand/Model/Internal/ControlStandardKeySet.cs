// ========================================
// Project Name : WodiLib
// File Name    : ControlStandardKeySet.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Text;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 基本キーセット
    /// </summary>
    [Serializable]
    internal class ControlStandardKeySet : IEquatable<ControlStandardKeySet>
    {
        private static class InputKeyString
        {
            public const string Ok = "決定  ";
            public const string Cancel = "ｷｬﾝｾﾙ  ";
            public const string Sub = "サブ  ";
            public const string Down = "↓ｷｰ  ";
            public const string Left = "←ｷｰ  ";
            public const string Right = "→ｷｰ  ";
            public const string Up = "↑ｷｰ  ";
        }

        public bool Ok { get; set; }
        public bool Cancel { get; set; }
        public bool Sub { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }

        private static byte FlgOk => 0x01;
        private static byte FlgCancel => 0x02;
        private static byte FlgSub => 0x04;
        private static byte FlgDown => 0x10;
        private static byte FlgLeft => 0x20;
        private static byte FlgRight => 0x40;
        private static byte FlgUp => 0x80;

        public ControlStandardKeySet(byte? flg = null)
        {
            if (flg is null) return;
            byte tFlg = (byte) flg;
            Ok = (tFlg & FlgOk) != 0;
            Cancel = (tFlg & FlgCancel) != 0;
            Sub = (tFlg & FlgSub) != 0;
            Down = (tFlg & FlgDown) != 0;
            Left = (tFlg & FlgLeft) != 0;
            Right = (tFlg & FlgRight) != 0;
            Up = (tFlg & FlgUp) != 0;
        }

        public byte ToByte()
        {
            byte result = 0x00;
            if (Ok) result += FlgOk;
            if (Cancel) result += FlgCancel;
            if (Sub) result += FlgSub;
            if (Down) result += FlgDown;
            if (Left) result += FlgLeft;
            if (Right) result += FlgRight;
            if (Up) result += FlgUp;
            return result;
        }

        public string MakeEventCommandTargetKeySentence()
        {
            var builder = new StringBuilder();
            if (Ok) builder.Append(InputKeyString.Ok);
            if (Cancel) builder.Append(InputKeyString.Cancel);
            if (Sub) builder.Append(InputKeyString.Sub);
            if (Down) builder.Append(InputKeyString.Down);
            if (Left) builder.Append(InputKeyString.Left);
            if (Right) builder.Append(InputKeyString.Right);
            if (Up) builder.Append(InputKeyString.Up);
            return builder.ToString();
        }

        public bool Equals(ControlStandardKeySet other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Ok == other.Ok
                   && Cancel == other.Cancel
                   && Sub == other.Sub
                   && Down == other.Down
                   && Left == other.Left
                   && Right == other.Right
                   && Up == other.Up;
        }
    }
}