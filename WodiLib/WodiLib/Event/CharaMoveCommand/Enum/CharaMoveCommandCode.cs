// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandCode.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    /// 動作指定コマンドコード
    /// </summary>
    public record CharaMoveCommandCode : TypeSafeEnum<CharaMoveCommandCode>
    {
        /// <summary>下に移動 </summary>
        public static readonly CharaMoveCommandCode MoveDown;

        /// <summary>左に移動 </summary>
        public static readonly CharaMoveCommandCode MoveLeft;

        /// <summary>右に移動 </summary>
        public static readonly CharaMoveCommandCode MoveRight;

        /// <summary>上に移動 </summary>
        public static readonly CharaMoveCommandCode MoveUp;

        /// <summary>左下に移動 </summary>
        public static readonly CharaMoveCommandCode MoveLeftDown;

        /// <summary>右下に移動 </summary>
        public static readonly CharaMoveCommandCode MoveRightDown;

        /// <summary>左上に移動 </summary>
        public static readonly CharaMoveCommandCode MoveLeftUp;

        /// <summary>右上に移動 </summary>
        public static readonly CharaMoveCommandCode MoveRightUp;

        /// <summary>ランダム移動 </summary>
        public static readonly CharaMoveCommandCode MoveRandom;

        /// <summary>主人公に接近 </summary>
        public static readonly CharaMoveCommandCode MoveTowardHero;

        /// <summary>主人公から離れる </summary>
        public static readonly CharaMoveCommandCode MoveAwayFromHero;

        /// <summary>一歩前進 </summary>
        public static readonly CharaMoveCommandCode StepForward;

        /// <summary>一歩後退 </summary>
        public static readonly CharaMoveCommandCode StepBackward;

        /// <summary>ジャンプ </summary>
        public static readonly CharaMoveCommandCode Jump;

        /// <summary>イベントに接近 </summary>
        public static readonly CharaMoveCommandCode ApproachEvent;

        /// <summary>座標に接近 </summary>
        public static readonly CharaMoveCommandCode ApproachPosition;

        /// <summary>下向き </summary>
        public static readonly CharaMoveCommandCode LookDown;

        /// <summary>左向き </summary>
        public static readonly CharaMoveCommandCode LookLeft;

        /// <summary>右向き </summary>
        public static readonly CharaMoveCommandCode LookRight;

        /// <summary>上向き </summary>
        public static readonly CharaMoveCommandCode LookUp;

        /// <summary>左下向き </summary>
        public static readonly CharaMoveCommandCode LookLeftDown;

        /// <summary>右下向き </summary>
        public static readonly CharaMoveCommandCode LookRightDown;

        /// <summary>左上向き </summary>
        public static readonly CharaMoveCommandCode LookLeftUp;

        /// <summary>右上向き </summary>
        public static readonly CharaMoveCommandCode LookRightUp;

        /// <summary>右に1つ回転 </summary>
        public static readonly CharaMoveCommandCode TurnRight;

        /// <summary>左に1つ回転 </summary>
        public static readonly CharaMoveCommandCode TurnLeft;

        /// <summary>左右ランダム回転 </summary>
        public static readonly CharaMoveCommandCode TurnLorR;

        /// <summary>完全ランダム回転 </summary>
        public static readonly CharaMoveCommandCode TurnRandom;

        /// <summary>主人公の方を向く </summary>
        public static readonly CharaMoveCommandCode TurnRound;

        /// <summary>主人公の逆を向く </summary>
        public static readonly CharaMoveCommandCode TurnTail;

        /// <summary>代入 </summary>
        public static readonly CharaMoveCommandCode SubstituteValue;

        /// <summary>加算 </summary>
        public static readonly CharaMoveCommandCode AddValue;

        /// <summary>移動速度を設定 </summary>
        public static readonly CharaMoveCommandCode SetMoveSpeed;

        /// <summary>移動頻度を設定 </summary>
        public static readonly CharaMoveCommandCode SetMoveFrequency;

        /// <summary>アニメ頻度を設定 </summary>
        public static readonly CharaMoveCommandCode SetAnimateSpeed;

        /// <summary>半歩移動に設定 </summary>
        public static readonly CharaMoveCommandCode SetStepHalf;

        /// <summary>全歩移動に設定 </summary>
        public static readonly CharaMoveCommandCode SetStepFull;

        /// <summary>パターン1に変更 </summary>
        public static readonly CharaMoveCommandCode ChangePatternFirst;

        /// <summary>パターン2に変更 </summary>
        public static readonly CharaMoveCommandCode ChangePatternSecond;

        /// <summary>パターン3に変更 </summary>
        public static readonly CharaMoveCommandCode ChangePatternThird;

        /// <summary>パターン4に変更 </summary>
        public static readonly CharaMoveCommandCode ChangePatternFourth;

        /// <summary>パターン5に変更 </summary>
        public static readonly CharaMoveCommandCode ChangePatternFifth;

        /// <summary>待機時アニメON </summary>
        public static readonly CharaMoveCommandCode ValidReadinessAnimation;

        /// <summary>待機時アニメOFF </summary>
        public static readonly CharaMoveCommandCode InvalidReadinessAnimation;

        /// <summary>移動アニメON </summary>
        public static readonly CharaMoveCommandCode ValidMoveAnimation;

        /// <summary>移動アニメOFF </summary>
        public static readonly CharaMoveCommandCode InvalidMoveAnimation;

        /// <summary>向き固定ON </summary>
        public static readonly CharaMoveCommandCode ValidFixDirection;

        /// <summary>向き固定OFF </summary>
        public static readonly CharaMoveCommandCode InvalidFixDirection;

        /// <summary>すり抜けON </summary>
        public static readonly CharaMoveCommandCode ValidSnake;

        /// <summary>すり抜けOFF </summary>
        public static readonly CharaMoveCommandCode InvalidSnake;

        /// <summary>最前面表示ON </summary>
        public static readonly CharaMoveCommandCode ValidDrawForefront;

        /// <summary>最前面表示OFF </summary>
        public static readonly CharaMoveCommandCode InvalidDrawForefront;

        /// <summary>グラフィック変更 </summary>
        public static readonly CharaMoveCommandCode ChangeGraphic;

        /// <summary>不透明度設定 </summary>
        public static readonly CharaMoveCommandCode ChangePenetration;

        /// <summary>高さ変更 </summary>
        public static readonly CharaMoveCommandCode ChangeHeight;

        /// <summary>効果音再生 </summary>
        public static readonly CharaMoveCommandCode PlaySE;

        /// <summary>ウェイト </summary>
        public static readonly CharaMoveCommandCode Wait;

        /// <summary>値</summary>
        public byte Code { get; }

        static CharaMoveCommandCode()
        {
            MoveDown = new CharaMoveCommandCode(nameof(MoveDown), 0x00);
            MoveLeft = new CharaMoveCommandCode(nameof(MoveLeft), 0x01);
            MoveRight = new CharaMoveCommandCode(nameof(MoveRight), 0x02);
            MoveUp = new CharaMoveCommandCode(nameof(MoveUp), 0x03);
            MoveLeftDown = new CharaMoveCommandCode(nameof(MoveLeftDown), 0x04);
            MoveRightDown = new CharaMoveCommandCode(nameof(MoveRightDown), 0x05);
            MoveLeftUp = new CharaMoveCommandCode(nameof(MoveLeftUp), 0x06);
            MoveRightUp = new CharaMoveCommandCode(nameof(MoveRightUp), 0x07);
            MoveRandom = new CharaMoveCommandCode(nameof(MoveRandom), 0x10);
            MoveTowardHero = new CharaMoveCommandCode(nameof(MoveTowardHero), 0x11);
            MoveAwayFromHero = new CharaMoveCommandCode(nameof(MoveAwayFromHero), 0x12);
            StepForward = new CharaMoveCommandCode(nameof(StepForward), 0x13);
            StepBackward = new CharaMoveCommandCode(nameof(StepBackward), 0x14);
            Jump = new CharaMoveCommandCode(nameof(Jump), 0x15);
            ApproachEvent = new CharaMoveCommandCode(nameof(ApproachEvent), 0x35);
            ApproachPosition = new CharaMoveCommandCode(nameof(ApproachPosition), 0x36);
            LookDown = new CharaMoveCommandCode(nameof(LookDown), 0x08);
            LookLeft = new CharaMoveCommandCode(nameof(LookLeft), 0x09);
            LookRight = new CharaMoveCommandCode(nameof(LookRight), 0x0A);
            LookUp = new CharaMoveCommandCode(nameof(LookUp), 0x0B);
            LookLeftDown = new CharaMoveCommandCode(nameof(LookLeftDown), 0x0C);
            LookRightDown = new CharaMoveCommandCode(nameof(LookRightDown), 0x0D);
            LookLeftUp = new CharaMoveCommandCode(nameof(LookLeftUp), 0x0E);
            LookRightUp = new CharaMoveCommandCode(nameof(LookRightUp), 0x0F);
            TurnRight = new CharaMoveCommandCode(nameof(TurnRight), 0x16);
            TurnLeft = new CharaMoveCommandCode(nameof(TurnLeft), 0x17);
            TurnLorR = new CharaMoveCommandCode(nameof(TurnLorR), 0x18);
            TurnRandom = new CharaMoveCommandCode(nameof(TurnRandom), 0x19);
            TurnRound = new CharaMoveCommandCode(nameof(TurnRound), 0x1A);
            TurnTail = new CharaMoveCommandCode(nameof(TurnTail), 0x1B);
            SubstituteValue = new CharaMoveCommandCode(nameof(SubstituteValue), 0x1C);
            AddValue = new CharaMoveCommandCode(nameof(AddValue), 0x37);
            SetMoveSpeed = new CharaMoveCommandCode(nameof(SetMoveSpeed), 0x1D);
            SetMoveFrequency = new CharaMoveCommandCode(nameof(SetMoveFrequency), 0x1E);
            SetAnimateSpeed = new CharaMoveCommandCode(nameof(SetAnimateSpeed), 0x1F);
            SetStepHalf = new CharaMoveCommandCode(nameof(SetStepHalf), 0x30);
            SetStepFull = new CharaMoveCommandCode(nameof(SetStepFull), 0x31);
            ChangePatternFirst = new CharaMoveCommandCode(nameof(ChangePatternFirst), 0x32);
            ChangePatternSecond = new CharaMoveCommandCode(nameof(ChangePatternSecond), 0x33);
            ChangePatternThird = new CharaMoveCommandCode(nameof(ChangePatternThird), 0x34);
            ChangePatternFourth = new CharaMoveCommandCode(nameof(ChangePatternFourth), 0x38);
            ChangePatternFifth = new CharaMoveCommandCode(nameof(ChangePatternFifth), 0x39);
            ValidReadinessAnimation = new CharaMoveCommandCode(nameof(ValidReadinessAnimation), 0x20);
            InvalidReadinessAnimation = new CharaMoveCommandCode(nameof(InvalidReadinessAnimation), 0x21);
            ValidMoveAnimation = new CharaMoveCommandCode(nameof(ValidMoveAnimation), 0x22);
            InvalidMoveAnimation = new CharaMoveCommandCode(nameof(InvalidMoveAnimation), 0x23);
            ValidFixDirection = new CharaMoveCommandCode(nameof(ValidFixDirection), 0x24);
            InvalidFixDirection = new CharaMoveCommandCode(nameof(InvalidFixDirection), 0x25);
            ValidSnake = new CharaMoveCommandCode(nameof(ValidSnake), 0x26);
            InvalidSnake = new CharaMoveCommandCode(nameof(InvalidSnake), 0x27);
            ValidDrawForefront = new CharaMoveCommandCode(nameof(ValidDrawForefront), 0x28);
            InvalidDrawForefront = new CharaMoveCommandCode(nameof(InvalidDrawForefront), 0x29);
            ChangeGraphic = new CharaMoveCommandCode(nameof(ChangeGraphic), 0x2C);
            ChangePenetration = new CharaMoveCommandCode(nameof(ChangePenetration), 0x2D);
            ChangeHeight = new CharaMoveCommandCode(nameof(ChangeHeight), 0x3A);
            PlaySE = new CharaMoveCommandCode(nameof(PlaySE), 0x2E);
            Wait = new CharaMoveCommandCode(nameof(Wait), 0x2F);
        }

        private CharaMoveCommandCode(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static CharaMoveCommandCode FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc />
        public override string ToString()
            => base.ToString();
    }
}
