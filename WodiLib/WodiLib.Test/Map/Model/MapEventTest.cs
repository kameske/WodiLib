using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-1, true)]
        [TestCase(1, false)]
        [TestCase(10, false)]
        public static void SetMapEventPagesTest(int length, bool isError)
        {
            var instance = new MapEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEvent.MapEventPageList)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEvent.PageValue)));
            }
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
