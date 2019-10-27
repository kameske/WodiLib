// ========================================
// Project Name : WodiLib
// File Name    : NormalOrFreePosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Project;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// ピクチャ表示位置・通常または自由変形
    /// </summary>
    public class NormalOrFreePosition
    {
        private const string EventCommandSentenceFormatNormal = "X:{0} Y:{1}";

        private const string EventCommandSentenceFormatFree
            = "[左上X:{0} Y:{1} + 右上X:{2} Y:{3} + 左下X:{4} Y:{5} + 右下X:{6} Y:{7}]";


        /// <summary>通常座標X</summary>
        public int NormalPositionX { get; set; }

        /// <summary>通常座標Y</summary>
        public int NormalPositionY { get; set; }

        /// <summary>自由変形座標左上X</summary>
        public int FreePositionLeftUpX { get; set; }

        /// <summary>自由変形座標左上Y</summary>
        public int FreePositionLeftUpY { get; set; }

        /// <summary>自由変形座標左下X</summary>
        public int FreePositionLeftDownX { get; set; }

        /// <summary>自由変形座標左上Y</summary>
        public int FreePositionLeftDownY { get; set; }

        /// <summary>自由変形座標右上X</summary>
        public int FreePositionRightUpX { get; set; }

        /// <summary>自由変形座標右上Y</summary>
        public int FreePositionRightUpY { get; set; }

        /// <summary>自由変形座標右下X</summary>
        public int FreePositionRightDownX { get; set; }

        /// <summary>自由変形座標右下Y</summary>
        public int FreePositionRightDownY { get; set; }

        /// <summary>
        /// 通常座標のイベントコマンド文を取得する。
        /// </summary>
        /// <returns>イベントコマンド文</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetEventCommandSentenceNormal(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var xStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                NormalPositionX, type, desc);
            var yStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                NormalPositionY, type, desc);

            return string.Format(EventCommandSentenceFormatNormal,
                xStr, yStr);
        }

        /// <summary>
        /// 自由変形座標のイベントコマンド文を取得する。
        /// </summary>
        /// <returns>イベントコマンド文</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetEventCommandSentenceFree(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var leftUpXStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionLeftUpX, type, desc);
            var leftUpYStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionLeftUpY, type, desc);
            var rightUpXStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionRightUpX, type, desc);
            var rightUpYStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionRightUpY, type, desc);
            var leftDownXStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionLeftDownX, type, desc);
            var leftDownYStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionLeftDownY, type, desc);
            var rightDownXStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionRightDownX, type, desc);
            var rightDownYStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                FreePositionRightDownY, type, desc);

            return string.Format(EventCommandSentenceFormatFree,
                leftUpXStr, leftUpYStr, rightUpXStr, rightUpYStr,
                leftDownXStr, leftDownYStr, rightDownXStr, rightDownYStr);
        }
    }
}