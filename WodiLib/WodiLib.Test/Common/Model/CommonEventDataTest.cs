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
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(9999, false)]
        public static void SetCommonEventListTest(int eventLength, bool isError)
        {
            var list = MakeCommonEventList(eventLength);

            var instance = new CommonEventData();

            var errorOccured = false;
            try
            {
                instance.SetCommonEventList(list);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void AddCommonEventTest(bool isNull, bool isError)
        {
            var commonEvent = isNull ? null : new CommonEvent();

            var instance = new CommonEventData();

            var errorOccured = false;
            try
            {
                instance.AddCommonEvent(commonEvent);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void AddCommonEventRangeTest(bool isNull, bool isError)
        {
            var commonEvent = isNull ? null : MakeCommonEventList(1);

            var instance = new CommonEventData();

            var errorOccured = false;
            try
            {
                instance.AddCommonEventRange(commonEvent);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, false, true)]
        [TestCase(1, -1, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 0, true, true)]
        [TestCase(1, 1, false, false)]
        [TestCase(1, 1, true, true)]
        [TestCase(1, 2, false, true)]
        [TestCase(1, 2, true, true)]
        [TestCase(3, -1, false, true)]
        [TestCase(3, -1, true, true)]
        [TestCase(3, 0, false, false)]
        [TestCase(3, 0, true, true)]
        [TestCase(3, 3, false, false)]
        [TestCase(3, 3, true, true)]
        [TestCase(3, 4, false, true)]
        [TestCase(3, 4, true, true)]
        public static void InsertCommonEventTest(int defaultLength, int index, bool isNull, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var insertEvent = isNull ? null : new CommonEvent();

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.InsertCommonEvent(index, insertEvent);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, false, true)]
        [TestCase(1, -1, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 0, true, true)]
        [TestCase(1, 1, false, false)]
        [TestCase(1, 1, true, true)]
        [TestCase(1, 2, false, true)]
        [TestCase(1, 2, true, true)]
        [TestCase(3, -1, false, true)]
        [TestCase(3, -1, true, true)]
        [TestCase(3, 0, false, false)]
        [TestCase(3, 0, true, true)]
        [TestCase(3, 3, false, false)]
        [TestCase(3, 3, true, true)]
        [TestCase(3, 4, false, true)]
        [TestCase(3, 4, true, true)]
        public static void InsertCommonEventRangeTest(int defaultLength, int index, bool isNull, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var insertEvents = isNull ? null : MakeCommonEventList(1);

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.InsertCommonEventRange(index, insertEvents);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, true)]
        [TestCase(1, 0, true)]
        [TestCase(1, 1, true)]
        [TestCase(4, -1, true)]
        [TestCase(4, 0, false)]
        [TestCase(4, 3, false)]
        [TestCase(4, 4, true)]
        public static void RemoveCommonEventAtTest(int defaultLength, int index, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.RemoveCommonEventAt(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, -1, true)]
        [TestCase(1, -1, 0, true)]
        [TestCase(1, -1, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, true)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, true)]
        [TestCase(1, 1, 1, true)]
        [TestCase(4, -1, -1, true)]
        [TestCase(4, -1, 0, true)]
        [TestCase(4, -1, 3, true)]
        [TestCase(4, -1, 4, true)]
        [TestCase(4, 0, -1, true)]
        [TestCase(4, 0, 0, false)]
        [TestCase(4, 0, 3, false)]
        [TestCase(4, 0, 4, true)]
        [TestCase(4, 1, -1, true)]
        [TestCase(4, 1, 0, false)]
        [TestCase(4, 1, 1, false)]
        [TestCase(4, 1, 3, false)]
        [TestCase(4, 1, 4, true)]
        [TestCase(4, 3, -1, true)]
        [TestCase(4, 3, 0, false)]
        [TestCase(4, 3, 1, false)]
        [TestCase(4, 3, 2, true)]
        [TestCase(4, 4, -1, true)]
        [TestCase(4, 4, 0, true)]
        [TestCase(4, 4, 1, true)]
        public static void RemoveCommonEventRangeTest(int defaultLength, int index, int count, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.RemoveCommonEventRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(4, -1, true)]
        [TestCase(4, 0, false)]
        [TestCase(4, 3, false)]
        [TestCase(4, 4, true)]
        public static void GetCommonEventTest(int defaultLength, int index, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.GetCommonEvent(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, -1, true)]
        [TestCase(1, -1, 0, true)]
        [TestCase(1, -1, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 0, 2, true)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, true)]
        [TestCase(1, 1, 1, true)]
        [TestCase(4, -1, -1, true)]
        [TestCase(4, -1, 0, true)]
        [TestCase(4, -1, 4, true)]
        [TestCase(4, 0, -1, true)]
        [TestCase(4, 0, 0, false)]
        [TestCase(4, 0, 4, false)]
        [TestCase(4, 0, 5, true)]
        [TestCase(4, 3, -1, true)]
        [TestCase(4, 3, 0, false)]
        [TestCase(4, 3, 1, false)]
        [TestCase(4, 3, 2, true)]
        [TestCase(4, 4, -1, true)]
        [TestCase(4, 4, 0, true)]
        [TestCase(4, 4, 1, true)]
        public static void GetCommonEventRangeTest(int defaultLength, int index, int count, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.GetCommonEventRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void GetAllCommonEventTest()
        {
            var list = MakeCommonEventList(1);

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, false);
        }

        [TestCase(1, -1, false, true)]
        [TestCase(1, -1, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 0, true, true)]
        [TestCase(1, 1, false, true)]
        [TestCase(1, 1, true, true)]
        [TestCase(4, -1, false, true)]
        [TestCase(4, -1, true, true)]
        [TestCase(4, 0, false, false)]
        [TestCase(4, 0, true, true)]
        [TestCase(4, 3, false, false)]
        [TestCase(4, 3, true, true)]
        [TestCase(4, 4, false, true)]
        [TestCase(4, 4, true, true)]
        public static void UpdateTest(int defaultLength, int index, bool isNull, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var updateItem = isNull ? null : new CommonEvent();

            var instance = new CommonEventData();
            instance.SetCommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.Update(index, updateItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }


        private static List<CommonEvent> MakeCommonEventList(int length)
        {
            if (length == -1) return null;

            var list = new List<CommonEvent>();
            for (var i = 0; i < length; i++)
            {
                list.Add(new CommonEvent());
            }

            return list;
        }
    }
}