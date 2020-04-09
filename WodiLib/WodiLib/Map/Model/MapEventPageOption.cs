// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageOption.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントページオプションクラス
    /// </summary>
    [Serializable]
    public class MapEventPageOption : ModelBase<MapEventPageOption>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>待機時アニメONフラグ</summary>
        public static readonly byte FlgWaitAnimationOn = 0x01;

        /// <summary>移動時アニメONフラグ</summary>
        public static readonly byte FlgMoveAnimationOn = 0x02;

        /// <summary>方向固定ONフラグ</summary>
        public static readonly byte FlgFixedDirection = 0x04;

        /// <summary>すり抜けONフラグ</summary>
        public static readonly byte FlgSkipThrough = 0x08;

        /// <summary>主人公より上ONフラグ</summary>
        public static readonly byte FlgAboveHero = 0x10;

        /// <summary>当たり判定■フラグ</summary>
        public static readonly byte FlgHitBox = 0x20;

        /// <summary>半歩上に設置ONフラグ</summary>
        public static readonly byte FlgPlaceHalfStepUp = 0x40;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private bool isWaitAnimationOn;

        /// <summary>待機時アニメフラグ</summary>
        public bool IsWaitAnimationOn
        {
            get => isWaitAnimationOn;
            set
            {
                isWaitAnimationOn = value;
                NotifyPropertyChanged();
            }
        }

        private bool isMoveAnimationOn;

        /// <summary>移動時アニメフラグ</summary>
        public bool IsMoveAnimationOn
        {
            get => isMoveAnimationOn;
            set
            {
                isMoveAnimationOn = value;
                NotifyPropertyChanged();
            }
        }

        private bool isFixedDirection;

        /// <summary>方向固定フラグ</summary>
        public bool IsFixedDirection
        {
            get => isFixedDirection;
            set
            {
                isFixedDirection = value;
                NotifyPropertyChanged();
            }
        }

        private bool isSkipThrough;

        /// <summary>すり抜けフラグ</summary>
        public bool IsSkipThrough
        {
            get => isSkipThrough;
            set
            {
                isSkipThrough = value;
                NotifyPropertyChanged();
            }
        }

        private bool isAboveHero;

        /// <summary>主人公より上フラグ</summary>
        public bool IsAboveHero
        {
            get => isAboveHero;
            set
            {
                isAboveHero = value;
                NotifyPropertyChanged();
            }
        }

        private bool isHitBox;

        /// <summary>当たり判定■フラグ</summary>
        public bool IsHitBox
        {
            get => isHitBox;
            set
            {
                isHitBox = value;
                NotifyPropertyChanged();
            }
        }

        private bool isPlaceHalfStepUp;

        /// <summary>半歩上に設置フラグ</summary>
        public bool IsPlaceHalfStepUp
        {
            get => isPlaceHalfStepUp;
            set
            {
                isPlaceHalfStepUp = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 待機時アニメ～半歩上設置フラグをセットする。
        /// </summary>
        /// <param name="optionCode">オプションコード</param>
        public void SetOptionFlag(byte optionCode)
        {
            // 待機時アニメON
            IsWaitAnimationOn = (optionCode & FlgWaitAnimationOn) != 0;
            // 移動時アニメON
            IsMoveAnimationOn = (optionCode & FlgMoveAnimationOn) != 0;
            // 方向固定ON
            IsFixedDirection = (optionCode & FlgFixedDirection) != 0;
            // すり抜けON
            IsSkipThrough = (optionCode & FlgSkipThrough) != 0;
            // 主人公より上ON
            IsAboveHero = (optionCode & FlgAboveHero) != 0;
            // 当たり判定■
            IsHitBox = (optionCode & FlgHitBox) != 0;
            // 半歩上に設置ON
            IsPlaceHalfStepUp = (optionCode & FlgPlaceHalfStepUp) != 0;
        }

        /// <summary>
        ///     オプション用のバイトデータを生成する。
        /// </summary>
        /// <returns>バイトデータ</returns>
        private byte MakeOptionByte()
        {
            byte result = 0x00;
            // 待機時アニメON
            if (IsWaitAnimationOn) result += FlgWaitAnimationOn;
            // 移動時アニメON
            if (IsMoveAnimationOn) result += FlgMoveAnimationOn;
            // 方向固定ON
            if (IsFixedDirection) result += FlgFixedDirection;
            // すり抜けON
            if (IsSkipThrough) result += FlgSkipThrough;
            // 主人公より上ON
            if (IsAboveHero) result += FlgAboveHero;
            // 当たり判定■
            if (IsHitBox) result += FlgHitBox;
            // 半歩上に設置ON
            if (IsPlaceHalfStepUp) result += FlgPlaceHalfStepUp;
            return result;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(MapEventPageOption other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsWaitAnimationOn == other.IsWaitAnimationOn
                   && IsMoveAnimationOn == other.IsMoveAnimationOn
                   && IsFixedDirection == other.IsFixedDirection
                   && IsSkipThrough == other.IsSkipThrough
                   && IsAboveHero == other.IsAboveHero
                   && IsHitBox == other.IsHitBox
                   && IsPlaceHalfStepUp == other.IsPlaceHalfStepUp;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            return new[]
            {
                MakeOptionByte()
            };
        }
    }
}