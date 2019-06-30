using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapTreeDataTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] TreeNodeListTestCaseSource =
        {
            new object[] {new MapTreeNodeList(), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(TreeNodeListTestCaseSource))]
        public static void TreeNodeListTest(MapTreeNodeList list, bool isError)
        {
            var instance = new MapTreeData();

            var errorOccured = false;
            try
            {
                instance.TreeNodeList = list;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.TreeNodeList;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.SequenceEqual(list));
        }
    }
}