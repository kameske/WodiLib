// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandCodeConstant.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Event
{
    /// <summary>
    ///     動作指定コマンドコード
    /// </summary>
    [Obsolete]
    public static class CharaMoveCommandCodeConstant
    {
        /// <summary>下に移動 </summary>
        public static readonly byte MoveDown = 0x00;

        /// <summary>左に移動 </summary>
        public static readonly byte MoveLeft = 0x01;

        /// <summary>右に移動 </summary>
        public static readonly byte MoveRight = 0x02;

        /// <summary>上に移動 </summary>
        public static readonly byte MoveUp = 0x03;

        /// <summary>左下に移動 </summary>
        public static readonly byte MoveLeftDown = 0x04;

        /// <summary>右下に移動 </summary>
        public static readonly byte MoveRightDown = 0x05;

        /// <summary>左上に移動 </summary>
        public static readonly byte MoveLeftUp = 0x06;

        /// <summary>右上に移動 </summary>
        public static readonly byte MoveRightUp = 0x07;

        /// <summary>ランダム移動 </summary>
        public static readonly byte MoveRandom = 0x10;

        /// <summary>主人公に接近 </summary>
        public static readonly byte MoveTowardHero = 0x11;

        /// <summary>主人公から離れる </summary>
        public static readonly byte MoveAwayFromHero = 0x12;

        /// <summary>一歩前進 </summary>
        public static readonly byte StepForward = 0x13;

        /// <summary>一歩後退 </summary>
        public static readonly byte StepBackward = 0x14;

        /// <summary>ジャンプ </summary>
        public static readonly byte Jump = 0x15;

        /// <summary>イベントに接近 </summary>
        public static readonly byte ApproachEvent = 0x35;

        /// <summary>座標に接近 </summary>
        public static readonly byte ApproachPosition = 0x36;

        /// <summary>下向き </summary>
        public static readonly byte LookDown = 0x08;

        /// <summary>左向き </summary>
        public static readonly byte LookLeft = 0x09;

        /// <summary>右向き </summary>
        public static readonly byte LookRight = 0x0A;

        /// <summary>上向き </summary>
        public static readonly byte LookUp = 0x0B;

        /// <summary>左下向き </summary>
        public static readonly byte LookLeftDown = 0x0C;

        /// <summary>右下向き </summary>
        public static readonly byte LookRightDown = 0x0D;

        /// <summary>左上向き </summary>
        public static readonly byte LookLeftUp = 0x0E;

        /// <summary>右上向き </summary>
        public static readonly byte LookRightUp = 0x0F;

        /// <summary>右に1つ回転 </summary>
        public static readonly byte TurnRight = 0x16;

        /// <summary>左に1つ回転 </summary>
        public static readonly byte TurnLeft = 0x17;

        /// <summary>左右ランダム回転 </summary>
        public static readonly byte TurnLorR = 0x18;

        /// <summary>完全ランダム回転 </summary>
        public static readonly byte TurnRandom = 0x19;

        /// <summary>主人公の方を向く </summary>
        public static readonly byte TurnRound = 0x1A;

        /// <summary>主人公の逆を向く </summary>
        public static readonly byte TurnTail = 0x1B;

        /// <summary>代入 </summary>
        public static readonly byte SubstituteValue = 0x1C;

        /// <summary>加算 </summary>
        public static readonly byte AddValue = 0x37;

        /// <summary>移動速度を設定 </summary>
        public static readonly byte SetMoveSpeed = 0x1D;

        /// <summary>移動頻度を設定 </summary>
        public static readonly byte SetMoveFrequency = 0x1E;

        /// <summary>アニメ頻度を設定 </summary>
        public static readonly byte SetAnimateSpeed = 0x1F;

        /// <summary>半歩移動に設定 </summary>
        public static readonly byte SetStepHalf = 0x30;

        /// <summary>全歩移動に設定 </summary>
        public static readonly byte SetStepFull = 0x31;

        /// <summary>パターン1に変更 </summary>
        public static readonly byte ChangePatternFirst = 0x32;

        /// <summary>パターン2に変更 </summary>
        public static readonly byte ChangePatternSecond = 0x33;

        /// <summary>パターン3に変更 </summary>
        public static readonly byte ChangePatternThird = 0x34;

        /// <summary>パターン4に変更 </summary>
        public static readonly byte ChangePatternFourth = 0x38;

        /// <summary>パターン5に変更 </summary>
        public static readonly byte ChangePatternFifth = 0x39;

        /// <summary>待機時アニメON </summary>
        public static readonly byte ValidReadinessAnimation = 0x20;

        /// <summary>待機時アニメOFF </summary>
        public static readonly byte InvalidReadinessAnimation = 0x21;

        /// <summary>移動アニメON </summary>
        public static readonly byte ValidMoveAnimation = 0x22;

        /// <summary>移動アニメOFF </summary>
        public static readonly byte InvalidMoveAnimation = 0x23;

        /// <summary>向き固定ON </summary>
        public static readonly byte ValidFixDirection = 0x24;

        /// <summary>向き固定OFF </summary>
        public static readonly byte InvalidFixDirection = 0x25;

        /// <summary>すり抜けON </summary>
        public static readonly byte ValidSnake = 0x26;

        /// <summary>すり抜けOFF </summary>
        public static readonly byte InvalidSnake = 0x27;

        /// <summary>最前面表示ON </summary>
        public static readonly byte ValidDrawForefront = 0x28;

        /// <summary>最前面表示OFF </summary>
        public static readonly byte InvalidDrawForefront = 0x29;

        /// <summary>グラフィック変更 </summary>
        public static readonly byte ChangeGraphic = 0x2C;

        /// <summary>不透明度設定 </summary>
        public static readonly byte ChangePenetration = 0x2D;

        /// <summary>高さ変更 </summary>
        public static readonly byte ChangeHeight = 0x3A;

        /// <summary>効果音再生 </summary>
        public static readonly byte PlaySE = 0x2E;

        /// <summary>ウェイト </summary>
        public static readonly byte Wait = 0x2F;
    }
}