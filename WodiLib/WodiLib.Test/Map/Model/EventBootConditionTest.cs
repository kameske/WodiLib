using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class EventBootConditionTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] SetCriteriaOperatorTestCaseSource =
        {
            new object[] { CriteriaOperator.Above, false },
            new object[] { null, true }
        };

        [TestCaseSource(nameof(SetCriteriaOperatorTestCaseSource))]
        public static void SetCriteriaOperatorTest(CriteriaOperator operation, bool isError)
        {
            var instance = new MapEventBootCondition();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventBootCondition.Operation)));
            }
        }
    }
}
