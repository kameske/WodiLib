using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonFileDataTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void CommonEventListTest(bool isNull, bool isError)
        {
            var instance = new CommonFileData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var list = isNull
                ? null
                : new CommonEventList();

            var errorOccured = false;
            try
            {
                instance.CommonEventList = list;
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
                var getList = instance.CommonEventList;

                // セットした値と取得した値が一致すること
                Assert.NotNull(getList);
                Assert.IsTrue(getList.Equals(list));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonFileData.CommonEventList)));
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(999, false)]
        public static void SetCommonEventListTest(int commonEventLength, bool isError)
        {
            var instance = new CommonFileData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetCommonEventList(MakeCommonEventList(commonEventLength));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (isError)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonFileData.CommonEventList)));
            }
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(999)]
        public static void GetAllCommonEventTest(int commonEventLength)
        {
            var instance = new CommonFileData();
            instance.SetCommonEventList(MakeCommonEventList(commonEventLength));
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.GetAllCommonEvent();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得件数が意図した値と一致すること
            var eventsLength = instance.GetAllCommonEvent().Count();
            Assert.AreEqual(eventsLength, commonEventLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new CommonFileData();
            target.SetCommonEventList(MakeCommonEventList(1));
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static List<CommonEvent> MakeCommonEventList(int length)
        {
            if (length == -1) return null;

            // yieldを使用したリスト返却
            IEnumerable<CommonEvent> FMakeList()
            {
                for (var i = 0; i < length; i++)
                {
                    yield return new CommonEvent();
                }
            }

            return FMakeList().ToList();
        }
    }
}