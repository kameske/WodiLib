using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapTreeOpenStatusDataTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] StatusListTestCaseSource =
        {
            new object[] {new MapTreeOpenStatusList(), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(StatusListTestCaseSource))]
        public static void StatusListTest(MapTreeOpenStatusList statusList, bool isError)
        {
            var instance = new MapTreeOpenStatusData();

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

            if (errorOccured) return;

            var setValue = instance.StatusList;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.SequenceEqual(statusList));
        }
    }
}