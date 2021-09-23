using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapTreeOpenStatusDataTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] StatusListTestCaseSource =
        {
            new object[] { new MapTreeOpenStatusList(), false },
            new object[] { null, true }
        };

        [TestCaseSource(nameof(StatusListTestCaseSource))]
        public static void StatusListTest(MapTreeOpenStatusList statusList, bool isError)
        {
            var instance = new MapTreeOpenStatusData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.StatusList = statusList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.StatusList;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(statusList));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapTreeOpenStatusData.StatusList)));
            }
        }
    }
}
