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

        [TestCase(false, false, true, true)]
        [TestCase(false, false, false, true)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, false, true)]
        [TestCase(true, false, true, false)]
        [TestCase(true, false, false, false)]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, false)]
        public static void SetTest(bool canDelete, bool enabled, bool canChangeEnabled, bool isError)
        {
            var instance = new TestHandlerList
            {
                MakeTestHandler("", canDelete, enabled, canChangeEnabled)
            };

            var setItem = MakeTestHandler("", true, true, true);

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

        [TestCase(false, true, true, true, 1)]
        [TestCase(false, true, false, true, 1)]
        [TestCase(false, false, true, true, 1)]
        [TestCase(false, false, false, true, 1)]
        [TestCase(true, true, true, false, 0)]
        [TestCase(true, true, false, false, 0)]
        [TestCase(true, false, true, false, 0)]
        [TestCase(true, false, false, false, 0)]
        public static void RemoveTest(bool canDelete, bool enabled, bool canChangeEnabled,
            bool isError, int lengthAfterRemove)
        {
            var instance = new TestHandlerList
            {
                MakeTestHandler("", canDelete, enabled, canChangeEnabled)
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

        [TestCase(false, true, true, 1)]
        [TestCase(false, true, false, 1)]
        [TestCase(false, false, true, 1)]
        [TestCase(false, false, false, 1)]
        [TestCase(true, true, true, 0)]
        [TestCase(true, true, false, 0)]
        [TestCase(true, false, true, 0)]
        [TestCase(true, false, false, 0)]
        public static void ClearTest(bool canDelete, bool enabled, bool canChangeEnabled, int lengthAfterClear)
        {
            var instance = new TestHandlerList
            {
                MakeTestHandler("", canDelete, enabled, canChangeEnabled)
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

        private static TestHandler<int> MakeTestHandler(string tag, bool canDelete, bool enabled, bool canChangeEnabled)
        {
            return new TestHandler<int>(tag, canDelete, enabled, canChangeEnabled);
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
            public TestHandler(string tag, bool canDelete, bool enabled,
                bool canChangeEnabled) : base(tag, canDelete, enabled, canChangeEnabled)
            {
            }
        }
    }
}