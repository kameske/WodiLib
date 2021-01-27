using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventDataTest
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
        public static void CommonEventListTest(int eventLength, bool isError)
        {
            var instance = new CommonEventData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var list = MakeCommonEventList(eventLength);

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
                var setValue = instance.CommonEventList;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(list));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventData.CommonEventList)));
            }
        }


        private static CommonEventList MakeCommonEventList(int length)
        {
            if (length == -1) return null;

            var list = new List<CommonEvent>();
            for (var i = 0; i < length; i++)
            {
                list.Add(new CommonEvent());
            }

            return new CommonEventList(list);
        }
    }
}
