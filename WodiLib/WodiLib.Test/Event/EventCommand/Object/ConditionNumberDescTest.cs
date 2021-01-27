using System;
using Commons;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ConditionNumberDescTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] TestCaseSource =
        {
            new object[] {false, NumberConditionalOperator.Above, (byte) 0x00},
            new object[] {false, NumberConditionalOperator.Greater, (byte) 0x01},
            new object[] {false, NumberConditionalOperator.Equal, (byte) 0x02},
            new object[] {false, NumberConditionalOperator.Less, (byte) 0x03},
            new object[] {false, NumberConditionalOperator.Below, (byte) 0x04},
            new object[] {false, NumberConditionalOperator.Not, (byte) 0x05},
            new object[] {false, NumberConditionalOperator.BitAnd, (byte) 0x06},
            new object[] {true, NumberConditionalOperator.Above, (byte) 0x10},
            new object[] {true, NumberConditionalOperator.Greater, (byte) 0x11},
            new object[] {true, NumberConditionalOperator.Equal, (byte) 0x12},
            new object[] {true, NumberConditionalOperator.Less, (byte) 0x13},
            new object[] {true, NumberConditionalOperator.Below, (byte) 0x14},
            new object[] {true, NumberConditionalOperator.Not, (byte) 0x15},
            new object[] {true, NumberConditionalOperator.BitAnd, (byte) 0x16}
        };

        [TestCaseSource(nameof(TestCaseSource))]
        public static void ToConditionFlagTest(bool isNotReferX, NumberConditionalOperator op, byte byteFlag)
        {
            var instance = new ConditionNumberDesc
            {
                IsNotReferX = isNotReferX,
                Condition = op
            };
            Assert.AreEqual(instance.ToConditionFlag(), byteFlag);
        }

        [Test]
        public static void NullGuardTest()
        {
            bool isError;
            var instance = new ConditionNumberDesc();
            try
            {
                instance.Condition = null;
                isError = false;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                isError = true;
            }

            Assert.IsTrue(isError);
        }
    }
}
