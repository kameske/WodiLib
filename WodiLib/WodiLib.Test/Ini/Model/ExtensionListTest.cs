using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Ini;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.Model
{
    [TestFixture]
    public class ExtensionListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new ExtensionList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, ExtensionList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new ExtensionList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, ExtensionList.MinCapacity);
        }

        [Test]
        public static void ToStringItems()
        {
            var extStringList = new List<string> {".A", ".B", "CCC"};
            var extList = extStringList.Select(x => (Extension) x).ToList();

            var instance = new ExtensionList(extList);

            var allExtString = instance.ToStringItems();

            var answer = string.Join(",", extStringList);

            Assert.IsTrue(allExtString.Equals(answer));
        }
    }
}