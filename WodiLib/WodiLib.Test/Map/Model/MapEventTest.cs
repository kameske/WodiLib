using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(-1, true)]
        [TestCase(1, false)]
        [TestCase(10, false)]
        public static void SetMapEventPagesTest(int length, bool isError)
        {
            var instance = new MapEvent();
            var setPages = length == -1 ? null : GenerateMapEventOnePageList(length);

            var errorOccured = false;
            try
            {
                instance.MapEventPageList = setPages;
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
                // ページ数が正しく取得できること
                Assert.AreEqual(instance.PageValue, length);
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapEvent
            {
                EventName = "Name",
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }

        private static MapEventPageList GenerateMapEventOnePageList(int length)
        {
            var list = new List<MapEventPage>();
            for (var i = 0; i < length; i++)
            {
                list.Add(new MapEventPage());
            }

            return new MapEventPageList(list);
        }
    }
}