// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    /// キャラ動作指定コマンドFactory
    /// </summary>
    public static class CharaMoveCommandFactory
    {
        /// <summary>
        /// コード値から動作指定コマンドを返す。
        /// </summary>
        /// <param name="code">[NotNull] 動作指定コマンドコード</param>
        /// <returns>キャラ動作指定コマンドインスタンス</returns>
        /// <exception cref="ArgumentNullException">codeがnullの場合</exception>
        public static ICharaMoveCommand CreateRaw(CharaMoveCommandCode code)
        {
            if (code is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(code)));

            if (code == CharaMoveCommandCode.MoveDown) return new MoveDown();
            if (code == CharaMoveCommandCode.MoveLeft) return new MoveLeft();
            if (code == CharaMoveCommandCode.MoveRight) return new MoveRight();
            if (code == CharaMoveCommandCode.MoveUp) return new MoveUp();
            if (code == CharaMoveCommandCode.MoveLeftDown) return new MoveLeftDown();
            if (code == CharaMoveCommandCode.MoveRightDown) return new MoveRightDown();
            if (code == CharaMoveCommandCode.MoveLeftUp) return new MoveLeftUp();
            if (code == CharaMoveCommandCode.MoveRightUp) return new MoveRightUp();
            if (code == CharaMoveCommandCode.MoveRandom) return new MoveRandom();
            if (code == CharaMoveCommandCode.MoveTowardHero) return new MoveTowardHero();
            if (code == CharaMoveCommandCode.MoveAwayFromHero) return new MoveAwayFromHero();
            if (code == CharaMoveCommandCode.StepForward) return new StepForward();
            if (code == CharaMoveCommandCode.StepBackward) return new StepBackward();
            if (code == CharaMoveCommandCode.Jump) return new Jump();
            if (code == CharaMoveCommandCode.ApproachEvent) return new ApproachEvent();
            if (code == CharaMoveCommandCode.ApproachPosition) return new ApproachPosition();
            if (code == CharaMoveCommandCode.LookDown) return new LookDown();
            if (code == CharaMoveCommandCode.LookLeft) return new LookLeft();
            if (code == CharaMoveCommandCode.LookRight) return new LookRight();
            if (code == CharaMoveCommandCode.LookUp) return new LookUp();
            if (code == CharaMoveCommandCode.LookLeftDown) return new LookLeftDown();
            if (code == CharaMoveCommandCode.LookRightDown) return new LookRightDown();
            if (code == CharaMoveCommandCode.LookLeftUp) return new LookLeftUp();
            if (code == CharaMoveCommandCode.LookRightUp) return new LookRightUp();
            if (code == CharaMoveCommandCode.TurnRight) return new TurnRight();
            if (code == CharaMoveCommandCode.TurnLeft) return new TurnLeft();
            if (code == CharaMoveCommandCode.TurnLorR) return new TurnLorR();
            if (code == CharaMoveCommandCode.TurnRandom) return new TurnRandom();
            if (code == CharaMoveCommandCode.TurnRound) return new TurnRound();
            if (code == CharaMoveCommandCode.TurnTail) return new TurnTail();
            if (code == CharaMoveCommandCode.SubstituteValue) return new AssignValue();
            if (code == CharaMoveCommandCode.AddValue) return new AddValue();
            if (code == CharaMoveCommandCode.SetMoveSpeed) return new SetMoveSpeed();
            if (code == CharaMoveCommandCode.SetMoveFrequency) return new SetMoveFrequency();
            if (code == CharaMoveCommandCode.SetAnimateSpeed) return new SetAnimateSpeed();
            if (code == CharaMoveCommandCode.SetStepHalf) return new SetStepHalf();
            if (code == CharaMoveCommandCode.SetStepFull) return new SetStepFull();
            if (code == CharaMoveCommandCode.ChangePatternFirst) return new ChangePatternFirst();
            if (code == CharaMoveCommandCode.ChangePatternSecond) return new ChangePatternSecond();
            if (code == CharaMoveCommandCode.ChangePatternThird) return new ChangePatternThird();
            if (code == CharaMoveCommandCode.ChangePatternFourth) return new ChangePatternFourth();
            if (code == CharaMoveCommandCode.ChangePatternFifth) return new ChangePatternFifth();
            if (code == CharaMoveCommandCode.ValidReadinessAnimation) return new ValidReadinessAnimation();
            if (code == CharaMoveCommandCode.InvalidReadinessAnimation) return new InvalidReadinessAnimation();
            if (code == CharaMoveCommandCode.ValidMoveAnimation) return new ValidMoveAnimation();
            if (code == CharaMoveCommandCode.InvalidMoveAnimation) return new InvalidMoveAnimation();
            if (code == CharaMoveCommandCode.ValidFixDirection) return new ValidFixDirection();
            if (code == CharaMoveCommandCode.InvalidFixDirection) return new InvalidFixDirection();
            if (code == CharaMoveCommandCode.ValidSnake) return new ValidSnake();
            if (code == CharaMoveCommandCode.InvalidSnake) return new InvalidSnake();
            if (code == CharaMoveCommandCode.ValidDrawForefront) return new ValidDrawForefront();
            if (code == CharaMoveCommandCode.InvalidDrawForefront) return new InvalidDrawForefront();
            if (code == CharaMoveCommandCode.ChangeGraphic) return new ChangeGraphic();
            if (code == CharaMoveCommandCode.ChangePenetration) return new ChangePenetration();
            if (code == CharaMoveCommandCode.ChangeHeight) return new ChangeHeight();
            if (code == CharaMoveCommandCode.PlaySE) return new PlaySE();
            if (code == CharaMoveCommandCode.Wait) return new WaitMoveCommand();

            // 来ることはないはずだが念の為
            throw new ArgumentException();
        }
    }
}