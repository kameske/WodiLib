// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageOptionConstant.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントページオプション定数クラス
    /// </summary>
    public static class MapEventPageOptionConstant
    {
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
    }
}