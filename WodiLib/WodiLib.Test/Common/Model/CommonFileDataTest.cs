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

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(999, false)]
        public static void SetCommonEventListTest(int commonEventLength, bool isError)
        {
            var instance = new CommonFileData();

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
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(999)]
        public static void GetAllCommonEventTest(int commonEventLength)
        {
            var instance = new CommonFileData();
            instance.SetCommonEventList(MakeCommonEventList(commonEventLength));

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