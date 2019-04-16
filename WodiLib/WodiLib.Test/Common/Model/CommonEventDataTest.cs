using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventDataTest
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
        public static void CommonEventListTest(int eventLength, bool isError)
        {
            var instance = new CommonEventData();
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

            if (errorOccured) return;

            var setValue = instance.CommonEventList;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(list));
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