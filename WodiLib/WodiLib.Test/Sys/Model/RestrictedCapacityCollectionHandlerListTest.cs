using System;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class RestrictedCapacityCollectionHandlerListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(false, false, true)]
        [TestCase(false, true, true)]
        [TestCase(true, false, false)]
        [TestCase(true, true, false)]
        public static void SetTest(bool canDelete, bool enabled, bool isError)
        {
            var instance = new TestHandlerList
            {
                MakeTestHandler("", canDelete, enabled)
            };

            var setItem = MakeTestHandler("", true, true);

            var errorOccured = false;
            try
            {
                instance[0] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, true, true, 1)]
        [TestCase(false, false, true, 1)]
        [TestCase(true, true, false, 0)]
        [TestCase(true, false, false, 0)]
        public static void RemoveTest(bool canDelete, bool enabled, bool isError, int lengthAfterRemove)
        {
            var instance = new TestHandlerList
            {
                MakeTestHandler("", canDelete, enabled)
            };

            var errorOccured = false;
            try
            {
                instance.RemoveAt(0);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // イベントハンドラ件数が意図した件数であること
            Assert.AreEqual(instance.Count, lengthAfterRemove);
        }

        [TestCase(false, true, 1)]
        [TestCase(false, false, 1)]
        [TestCase(true, true, 0)]
        [TestCase(true, false, 0)]
        public static void ClearTest(bool canDelete, bool enabled, int lengthAfterClear)
        {
            var instance = new TestHandlerList
            {
                MakeTestHandler("", canDelete, enabled)
            };

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // イベントハンドラ件数が意図した件数であること
            Assert.AreEqual(instance.Count, lengthAfterClear);
        }

        private static TestHandler<int> MakeTestHandler(string tag, bool canDelete, bool enabled)
        {
            return new TestHandler<int>(tag, canDelete, enabled);
        }

        /// <summary>
        /// テスト用ハンドラリスト
        /// </summary>
        public class TestHandlerList : RestrictedCapacityCollectionHandlerList<int, TestHandler<int>>
        {
        }

        /// <summary>
        /// テスト用ハンドラ
        /// </summary>
        public class TestHandler<T> : RestrictedCapacityCollectionHandler<T>
        {
            public TestHandler(string tag, bool canDelete, bool enabled) : base(tag, canDelete, enabled)
            {
            }
        }
    }
}