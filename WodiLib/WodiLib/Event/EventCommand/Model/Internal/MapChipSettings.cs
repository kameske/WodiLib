// ========================================
// Project Name : WodiLib
// File Name    : MapChipSettings.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// マップチップ通行設定
    /// </summary>
    [Serializable]
    internal class MapChipSettings : ModelBase<MapChipSettings>
    {
        private bool isNoUp;

        /// <summary>↑不能</summary>
        public bool IsNoUp
        {
            get => isNoUp;
            set
            {
                isNoUp = value;
                NotifyPropertyChanged();
            }
        }

        private bool isNoLeft;

        /// <summary>←不能</summary>
        public bool IsNoLeft
        {
            get => isNoLeft;
            set
            {
                isNoLeft = value;
                NotifyPropertyChanged();
            }
        }

        private bool isNoRight;

        /// <summary>→不能</summary>
        public bool IsNoRight
        {
            get => isNoRight;
            set
            {
                isNoRight = value;
                NotifyPropertyChanged();
            }
        }

        private bool isNoDown;

        /// <summary>↓不能</summary>
        public bool IsNoDown
        {
            get => isNoDown;
            set
            {
                isNoDown = value;
                NotifyPropertyChanged();
            }
        }

        private bool isAboveHero;

        /// <summary>主人公の上</summary>
        public bool IsAboveHero
        {
            get => isAboveHero;
            set
            {
                isAboveHero = value;
                NotifyPropertyChanged();
            }
        }

        private bool isHalfTrans;

        /// <summary>下半身透過</summary>
        public bool IsHalfTrans
        {
            get => isHalfTrans;
            set
            {
                isHalfTrans = value;
                NotifyPropertyChanged();
            }
        }

        private bool isMatchLowerLayer;

        /// <summary>下レイヤー依存</summary>
        public bool IsMatchLowerLayer
        {
            get => isMatchLowerLayer;
            set
            {
                isMatchLowerLayer = value;
                NotifyPropertyChanged();
            }
        }

        private bool isCounter;

        /// <summary>カウンター</summary>
        public bool IsCounter
        {
            get => isCounter;
            set
            {
                isCounter = value;
                NotifyPropertyChanged();
            }
        }

        public MapChipSettings(int? flags = null)
        {
            if (!flags.HasValue) return;
            var tmpFlags = flags;
            IsNoDown = (tmpFlags & NoDownFlg) != 0;
            IsNoLeft = (tmpFlags & NoLeftFlg) != 0;
            IsNoRight = (tmpFlags & NoRightFlg) != 0;
            IsNoUp = (tmpFlags & NoUpFlg) != 0;
            IsAboveHero = (tmpFlags & AboveHeroFlg) != 0;
            IsHalfTrans = (tmpFlags & HalfTransFlg) != 0;
            IsMatchLowerLayer = (tmpFlags & MatchLowerLayerFlg) != 0;
            IsCounter = (tmpFlags & CounterFlg) != 0;
        }

        public int ToInt()
        {
            int result = 0;
            if (IsNoDown) result += NoDownFlg;
            if (IsNoLeft) result += NoLeftFlg;
            if (IsNoRight) result += NoRightFlg;
            if (IsNoUp) result += NoUpFlg;
            if (IsAboveHero) result += AboveHeroFlg;
            if (IsHalfTrans) result += HalfTransFlg;
            if (IsMatchLowerLayer) result += MatchLowerLayerFlg;
            if (IsCounter) result += CounterFlg;
            return result;
        }

        public override bool Equals(MapChipSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsNoUp == other.IsNoUp
                   && IsNoLeft == other.IsNoLeft
                   && IsNoRight == other.IsNoRight
                   && IsNoDown == other.IsNoDown
                   && IsAboveHero == other.IsAboveHero
                   && IsHalfTrans == other.IsHalfTrans
                   && IsMatchLowerLayer == other.IsMatchLowerLayer
                   && IsCounter == other.IsCounter;
        }

        private static readonly int NoDownFlg = 1;
        private static readonly int NoLeftFlg = 2;
        private static readonly int NoRightFlg = 4;
        private static readonly int NoUpFlg = 8;
        private static readonly int AboveHeroFlg = 16;
        private static readonly int HalfTransFlg = 64;
        private static readonly int MatchLowerLayerFlg = 512;
        private static readonly int CounterFlg = 128;
    }
}