using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageBootInfoTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] EventBootTypeTestCaseSource =
        {
            new object[] {MapEventBootType.Auto, false},
            new object[] {null, true}
        };

        [TestCaseSource(nameof(EventBootTypeTestCaseSource))]
        public static void EventBootTypeTest(MapEventBootType bootType, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventBootType = bootType;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageBootInfo.MapEventBootType)));
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(3, false)]
        [TestCase(4, true)]
        public static void SetHasEventBootConditionTest(int index, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetHasEventBootCondition(index, true);
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
                Assert.IsTrue(changedPropertyList[0].Equals($"HasEventBootCondition{index + 1}"));
            }
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition1SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventBootCondition1 = isNull ? null : new MapEventBootCondition();
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
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageBootInfo.MapEventBootCondition1)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageBootInfo.HasEventBootCondition1)));
            }
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition2SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventBootCondition2 = isNull ? null : new MapEventBootCondition();
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
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageBootInfo.MapEventBootCondition2)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageBootInfo.HasEventBootCondition2)));
            }
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition3SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventBootCondition3 = isNull ? null : new MapEventBootCondition();
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
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageBootInfo.MapEventBootCondition3)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageBootInfo.HasEventBootCondition3)));
            }
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition4SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventBootCondition4 = isNull ? null : new MapEventBootCondition();
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
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageBootInfo.MapEventBootCondition4)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageBootInfo.HasEventBootCondition4)));
            }
        }

        [TestCase(-1, false, true)]
        [TestCase(0, false, false)]
        [TestCase(0, true, true)]
        [TestCase(3, false, false)]
        [TestCase(3, true, true)]
        [TestCase(4, false, true)]
        public static void SetEventBootConditionTest(int index, bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetEventBootCondition(index, isNull ? null : new MapEventBootCondition());
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
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals($"MapEventBootCondition{index + 1}"));
                Assert.IsTrue(changedPropertyList[1].Equals($"HasEventBootCondition{index + 1}"));
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapEventPageBootInfo
            {
                MapEventBootType = MapEventBootType.Auto,
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}