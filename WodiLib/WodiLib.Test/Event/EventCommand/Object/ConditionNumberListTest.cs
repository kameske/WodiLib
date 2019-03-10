using System;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ConditionNumberListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] SetConditionValueOutOfRangeTestCaseSource =
        {
            new object[] {-1, true},
            new object[] {0, true},
            new object[] {1, false},
            new object[] {3, false},
            new object[] {4, true}
        };

        [TestCaseSource(nameof(SetConditionValueOutOfRangeTestCaseSource))]
        public static void SetConditionValueOutOfRangeTest(int value, bool error)
        {
            var instance = new ConditionNumberList();
            bool isError;
            try
            {
                instance.ConditionValue = value;
                isError = false;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                isError = true;
            }

            Assert.AreEqual(isError, error);
        }

        private static readonly object[] AccessorOutOfRangeTestCaseSource =
        {
            new object[] {1, -1, true},
            new object[] {1, 0, false},
            new object[] {1, 1, true},
            new object[] {2, -1, true},
            new object[] {2, 0, false},
            new object[] {2, 1, false},
            new object[] {2, 2, true},
            new object[] {3, -1, true},
            new object[] {3, 0, false},
            new object[] {3, 2, false},
            new object[] {3, 3, true}
        };

        [TestCaseSource(nameof(AccessorOutOfRangeTestCaseSource))]
        public static void SetOutOfRangeTest(int conditionValue, int setIndex, bool error)
        {
            var instance = new ConditionNumberList();
            bool isError;
            try
            {
                instance.ConditionValue = conditionValue;
                instance.Set(setIndex, new ConditionNumberDesc());
                isError = false;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                isError = true;
            }

            Assert.AreEqual(isError, error);
        }

        [Test]
        public static void SetNullTest()
        {
            var instance = new ConditionNumberList {ConditionValue = 3};
            var isError = false;
            try
            {
                instance.Set(0, null);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                isError = true;
            }

            Assert.IsTrue(isError);
        }

        [TestCaseSource(nameof(AccessorOutOfRangeTestCaseSource))]
        public static void GetOutOfRangeTest(int conditionValue, int getIndex, bool error)
        {
            var instance = new ConditionNumberList();
            bool isError;
            try
            {
                instance.ConditionValue = conditionValue;
                var _ = instance.Get(getIndex);
                isError = false;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                isError = true;
            }

            Assert.AreEqual(isError, error);
        }

        private static readonly object[] SetLeftSideTestCaseSource =
        {
            new object[] {1, 0, 2000000, false},
            new object[] {1, 1, 2000000, true},
            new object[] {3, -1, 2000000, true},
            new object[] {3, 0, 2000000, false},
            new object[] {3, 2, 2000000, false},
            new object[] {3, 3, 2000000, true},
        };

        [TestCaseSource(nameof(SetLeftSideTestCaseSource))]
        public static void SetLeftSideTest(int length, int index, int setValue, bool isError)
        {
            var instance = new ConditionNumberList
            {
                ConditionValue = length
            };

            var errorOccured = false;
            try
            {
                instance.SetLeftSide(index, setValue);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] SetRightSideTestCaseSource =
        {
            new object[] {1, 0, 2000000, false},
            new object[] {1, 1, 2000000, true},
            new object[] {3, -1, 2000000, true},
            new object[] {3, 0, 2000000, false},
            new object[] {3, 2, 2000000, false},
            new object[] {3, 3, 2000000, true},
        };

        [TestCaseSource(nameof(SetRightSideTestCaseSource))]
        public static void SetRightSideTest(int length, int index, int setValue, bool isError)
        {
            var instance = new ConditionNumberList
            {
                ConditionValue = length
            };

            var errorOccured = false;
            try
            {
                instance.SetRightSide(index, setValue);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] SetConditionTestCaseSource =
        {
            new object[] {1, 0, NumberConditionalOperator.Not, false},
            new object[] {1, 1, NumberConditionalOperator.Not, true},
            new object[] {3, -1, NumberConditionalOperator.Not, true},
            new object[] {3, 0, NumberConditionalOperator.Not, false},
            new object[] {3, 2, NumberConditionalOperator.Not, false},
            new object[] {3, 3, NumberConditionalOperator.Not, true},
        };

        [TestCaseSource(nameof(SetConditionTestCaseSource))]
        public static void SetConditionTest(int length, int index, NumberConditionalOperator setValue, bool isError)
        {
            var instance = new ConditionNumberList
            {
                ConditionValue = length
            };

            var errorOccured = false;
            try
            {
                instance.SetCondition(index, setValue);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] SetIsNotReferXTestCaseSource =
        {
            new object[] {1, 0, true, false},
            new object[] {1, 1, true, true},
            new object[] {3, -1, true, true},
            new object[] {3, 0, true, false},
            new object[] {3, 2, true, false},
            new object[] {3, 3, true, true},
        };

        [TestCaseSource(nameof(SetIsNotReferXTestCaseSource))]
        public static void SetIsNotReferXTest(int length, int index, bool setValue, bool isError)
        {
            var instance = new ConditionNumberList
            {
                ConditionValue = length
            };

            var errorOccured = false;
            try
            {
                instance.SetIsNotReferX(index, setValue);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
    }
}