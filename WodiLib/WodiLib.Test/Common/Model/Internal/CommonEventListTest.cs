using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventListTest
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
        public static void ConstructorTest(int listLength, bool isError)
        {
            var list = MakeCommonEventList(listLength);

            var errorOccured = false;
            try
            {
                var _ = new CommonEventList(list);
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
        public static void AddTest(bool isNull, bool isError)
        {
            var list = MakeCommonEventList(1);
            var instance = new CommonEventList(list);

            var item = isNull ? null : new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.Add(item);
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
        public static void AddRangeTest(bool isNull, bool isError)
        {
            var list = MakeCommonEventList(1);
            var instance = new CommonEventList(list);

            var items = isNull ? null : MakeCommonEventList(1);

            var errorOccured = false;
            try
            {
                instance.AddRange(items);
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
        public static void InsertTest(int defaultLength, int index, bool isNull, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);
            var instance = new CommonEventList(list);

            var item = isNull ? null : new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.Insert(index, item);
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
        public static void InsertRangeTest(int defaultLength, int index, bool isNull, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);
            var instance = new CommonEventList(list);

            var items = isNull ? null : MakeCommonEventList(1);

            var errorOccured = false;
            try
            {
                instance.InsertRange(index, items);
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
        public static void RemoveAtTest(int defaultLength, int index, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);
            var instance = new CommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.RemoveAt(index);
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
        public static void RemoveRangeTest(int defaultLength, int index, int count, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);
            var instance = new CommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.RemoveRange(index, count);
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
        public static void GetTest(int defaultLength, int index, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var instance = new CommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.Get(index);
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
        public static void GetRangeTest(int defaultLength, int index, int count, bool isError)
        {
            var list = MakeCommonEventList(defaultLength);

            var instance = new CommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.GetRange(index, count);
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
        public static void GetAllTest()
        {
            var list = MakeCommonEventList(1);

            var instance = new CommonEventList(list);

            var errorOccured = false;
            try
            {
                instance.GetAll();
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

            var instance = new CommonEventList(list);

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