// ========================================
// Project Name : WodiLib
// File Name    : NormalOrFreePosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// ピクチャ表示位置・通常または自由変形
    /// </summary>
    [Serializable]
    public class NormalOrFreePosition : ModelBase<NormalOrFreePosition>
    {
        private const string EventCommandSentenceFormatNormal = "X:{0} Y:{1}";

        private const string EventCommandSentenceFormatFree
            = "[左上X:{0} Y:{1} + 右上X:{2} Y:{3} + 左下X:{4} Y:{5} + 右下X:{6} Y:{7}]";

        private MyType type = MyType.Normal;

        private int normalPositionX;

        /// <summary>通常座標X</summary>
        public int NormalPositionX
        {
            get => normalPositionX;
            set
            {
                normalPositionX = value;
                type = MyType.Normal;
                NotifyPropertyChanged();
            }
        }


        private int normalPositionY;

        /// <summary>通常座標Y</summary>
        public int NormalPositionY
        {
            get => normalPositionY;
            set
            {
                normalPositionY = value;
                type = MyType.Normal;
                NotifyPropertyChanged();
            }
        }


        private int freePositionLeftUpX;

        /// <summary>自由変形座標左上X</summary>
        public int FreePositionLeftUpX
        {
            get => freePositionLeftUpX;
            set
            {
                freePositionLeftUpX = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


        private int freePositionLeftUpY;

        /// <summary>自由変形座標左上Y</summary>
        public int FreePositionLeftUpY
        {
            get => freePositionLeftUpY;
            set
            {
                freePositionLeftUpY = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


        private int freePositionLeftDownX;

        /// <summary>自由変形座標左下X</summary>
        public int FreePositionLeftDownX
        {
            get => freePositionLeftDownX;
            set
            {
                freePositionLeftDownX = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


        private int freePositionLeftDownY;

        /// <summary>自由変形座標左上Y</summary>
        public int FreePositionLeftDownY
        {
            get => freePositionLeftDownY;
            set
            {
                freePositionLeftDownY = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


        private int freePositionRightUpX;

        /// <summary>自由変形座標右上X</summary>
        public int FreePositionRightUpX
        {
            get => freePositionRightUpX;
            set
            {
                freePositionRightUpX = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


        private int freePositionRightUpY;

        /// <summary>自由変形座標右上Y</summary>
        public int FreePositionRightUpY
        {
            get => freePositionRightUpY;
            set
            {
                freePositionRightUpY = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


        private int freePositionRightDownX;

        /// <summary>自由変形座標右下X</summary>
        public int FreePositionRightDownX
        {
            get => freePositionRightDownX;
            set
            {
                freePositionRightDownX = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


        private int freePositionRightDownY;

        /// <summary>自由変形座標右下Y</summary>
        public int FreePositionRightDownY
        {
            get => freePositionRightDownY;
            set
            {
                freePositionRightDownY = value;
                type = MyType.Free;
                NotifyPropertyChanged();
            }
        }


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

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(NormalOrFreePosition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (type == MyType.Normal)
            {
                return NormalPositionX == other.NormalPositionX
                       && NormalPositionY == other.NormalPositionY;
            }

            return FreePositionLeftUpX == other.FreePositionLeftUpX
                   && FreePositionLeftUpY == other.FreePositionLeftUpY
                   && FreePositionLeftDownX == other.FreePositionLeftDownX
                   && FreePositionLeftDownY == other.FreePositionLeftDownY
                   && FreePositionRightUpX == other.FreePositionRightUpX
                   && FreePositionRightUpY == other.FreePositionRightUpY
                   && FreePositionRightDownX == other.FreePositionRightDownX
                   && FreePositionRightDownY == other.FreePositionRightDownY;
        }

        private enum MyType
        {
            Normal,
            Free,
        }
    }
}