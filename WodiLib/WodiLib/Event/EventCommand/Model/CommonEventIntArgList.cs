// ========================================
// Project Name : WodiLib
// File Name    : CommonEventIntArgList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// コモンイベント・数値引数リストオブジェクト
    /// </summary>
    internal class CommonEventIntArgList
    {
        private readonly int[] args =
        {
            0, 0, 0, 0
        };

        public void Set(int index, int value)
        {
            args[index] = value;
        }

        public int Get(int index)
        {
            return args[index];
        }
    }
}