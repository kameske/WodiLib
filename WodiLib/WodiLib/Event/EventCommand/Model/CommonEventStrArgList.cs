// ========================================
// Project Name : WodiLib
// File Name    : CommonEventStrArgList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// コモンイベント・文字列引数リストオブジェクト
    /// </summary>
    internal class CommonEventStrArgList
    {
        private readonly List<IntOrStr> args = new List<IntOrStr>(4)
        {
            new IntOrStr(),
            new IntOrStr(),
            new IntOrStr(),
            new IntOrStr()
        };

        public byte ReferenceFlg
        {
            get
            {
                byte result = 0x00;
                if (args[0].InstanceIntOrStrType == IntOrStrType.Str) result += Arg1ReferFlg;
                if (args[1].InstanceIntOrStrType == IntOrStrType.Str) result += Arg2ReferFlg;
                if (args[2].InstanceIntOrStrType == IntOrStrType.Str) result += Arg3ReferFlg;
                if (args[3].InstanceIntOrStrType == IntOrStrType.Str) result += Arg4ReferFlg;
                return result;
            }
            set
            {
                if ((value & Arg1ReferFlg) == 0) args[0] = 0;
                else args[0] = "";
                if ((value & Arg2ReferFlg) == 0) args[1] = 0;
                else args[1] = "";
                if ((value & Arg3ReferFlg) == 0) args[2] = 0;
                else args[2] = "";
                if ((value & Arg4ReferFlg) == 0) args[3] = 0;
                else args[3] = "";
            }
        }


        public void Set(int index, IntOrStr value)
        {
            args[index] = value;
        }

        public IntOrStr Get(int index)
        {
            return args[index];
        }

        public bool HasStringParam(int index)
        {
            return args[index].HasStr;
        }

        private static readonly byte Arg1ReferFlg = 0x10;
        private static readonly byte Arg2ReferFlg = 0x20;
        private static readonly byte Arg3ReferFlg = 0x40;
        private static readonly byte Arg4ReferFlg = 0x80;
    }
}