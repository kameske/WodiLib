using System;
using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class EventBootConditionTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] SetCriteriaOperatorTestCaseSource =
        {
            new object[] {CriteriaOperator.Above, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(SetCriteriaOperatorTestCaseSource))]
        public static void SetCriteriaOperatorTest(CriteriaOperator operation, bool isError)
        {
            var instance = new MapEventBootCondition();
            var errorOccured = false;
            try
            {
                instance.Operation = operation;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapEventBootCondition
            {
                UseCondition = true
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}