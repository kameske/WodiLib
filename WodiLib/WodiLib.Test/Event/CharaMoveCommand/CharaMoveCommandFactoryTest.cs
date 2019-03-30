using System;
using NUnit.Framework;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.CharaMoveCommand
{
    using Factory = CharaMoveCommandFactory;

    [TestFixture]
    public class CharaMoveCommandFactoryTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] TestCaseSource =
        {
            new object[] {CharaMoveCommandCode.MoveDown},
            new object[] {CharaMoveCommandCode.MoveLeft},
            new object[] {CharaMoveCommandCode.MoveRight},
            new object[] {CharaMoveCommandCode.MoveUp},
            new object[] {CharaMoveCommandCode.MoveLeftDown},
            new object[] {CharaMoveCommandCode.MoveRightDown},
            new object[] {CharaMoveCommandCode.MoveLeftUp},
            new object[] {CharaMoveCommandCode.MoveRightUp},
            new object[] {CharaMoveCommandCode.MoveRandom},
            new object[] {CharaMoveCommandCode.MoveTowardHero},
            new object[] {CharaMoveCommandCode.MoveAwayFromHero},
            new object[] {CharaMoveCommandCode.StepForward},
            new object[] {CharaMoveCommandCode.StepBackward},
            new object[] {CharaMoveCommandCode.Jump},
            new object[] {CharaMoveCommandCode.ApproachEvent},
            new object[] {CharaMoveCommandCode.ApproachPosition},
            new object[] {CharaMoveCommandCode.LookDown},
            new object[] {CharaMoveCommandCode.LookLeft},
            new object[] {CharaMoveCommandCode.LookRight},
            new object[] {CharaMoveCommandCode.LookUp},
            new object[] {CharaMoveCommandCode.LookLeftDown},
            new object[] {CharaMoveCommandCode.LookRightDown},
            new object[] {CharaMoveCommandCode.LookLeftUp},
            new object[] {CharaMoveCommandCode.LookRightUp},
            new object[] {CharaMoveCommandCode.TurnRight},
            new object[] {CharaMoveCommandCode.TurnLeft},
            new object[] {CharaMoveCommandCode.TurnLorR},
            new object[] {CharaMoveCommandCode.TurnRandom},
            new object[] {CharaMoveCommandCode.TurnRound},
            new object[] {CharaMoveCommandCode.TurnTail},
            new object[] {CharaMoveCommandCode.SubstituteValue},
            new object[] {CharaMoveCommandCode.AddValue},
            new object[] {CharaMoveCommandCode.SetMoveSpeed},
            new object[] {CharaMoveCommandCode.SetMoveFrequency},
            new object[] {CharaMoveCommandCode.SetAnimateSpeed},
            new object[] {CharaMoveCommandCode.SetStepHalf},
            new object[] {CharaMoveCommandCode.SetStepFull},
            new object[] {CharaMoveCommandCode.ChangePatternFirst},
            new object[] {CharaMoveCommandCode.ChangePatternSecond},
            new object[] {CharaMoveCommandCode.ChangePatternThird},
            new object[] {CharaMoveCommandCode.ChangePatternFourth},
            new object[] {CharaMoveCommandCode.ChangePatternFifth},
            new object[] {CharaMoveCommandCode.ValidReadinessAnimation},
            new object[] {CharaMoveCommandCode.InvalidReadinessAnimation},
            new object[] {CharaMoveCommandCode.ValidMoveAnimation},
            new object[] {CharaMoveCommandCode.InvalidMoveAnimation},
            new object[] {CharaMoveCommandCode.ValidFixDirection},
            new object[] {CharaMoveCommandCode.InvalidFixDirection},
            new object[] {CharaMoveCommandCode.ValidSnake},
            new object[] {CharaMoveCommandCode.InvalidSnake},
            new object[] {CharaMoveCommandCode.ValidDrawForefront},
            new object[] {CharaMoveCommandCode.InvalidDrawForefront},
            new object[] {CharaMoveCommandCode.ChangeGraphic},
            new object[] {CharaMoveCommandCode.ChangePenetration},
            new object[] {CharaMoveCommandCode.ChangeHeight},
            new object[] {CharaMoveCommandCode.PlaySE},
            new object[] {CharaMoveCommandCode.Wait},
        };

        [TestCaseSource(nameof(TestCaseSource))]
        public static void CreateRawTest(CharaMoveCommandCode code)
        {
            bool result;
            ICharaMoveCommand instance = null;
            try
            {
                instance = Factory.CreateRaw(code);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                result = false;
            }

            Assert.IsTrue(result);
            Assert.AreEqual(instance.CommandCode, code);
        }
    }
}