using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class ListExtensionTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(3, 1, -1, true)]
        [TestCase(4, 4, -1, true)]
        [TestCase(2, 5, -1, true)]
        [TestCase(3, 1, 10, false)]
        [TestCase(4, 4, 10, false)]
        [TestCase(2, 5, 10, false)]
        public static void AdjustLengthTest(int initLength, int adjustLength, int defaultValue, bool isError)
        {
            var list = new List<int>();
            for (var i = 0; i < initLength; i++) list.Add(i);


            var makeDefaultValueFunc = defaultValue == -1
                ? null
                : new Func<int, int>(i => defaultValue);

            var errorOccured = false;
            try
            {
                list.AdjustLength(adjustLength, makeDefaultValueFunc);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 長さが意図した値であること
            Assert.AreEqual(list.Count, adjustLength);

            // 手を加えなかった部分の値がそのままであること
            var checkLength = initLength < adjustLength ? initLength : adjustLength;
            for (var i = 0; i < checkLength; i++)
            {
                Assert.AreEqual(list[i], i);
            }

            // 手を加えた部分の値が defaultValue と一致すること
            var addLength = adjustLength - initLength;
            if (addLength < 0) addLength = 0;
            for (var i = 0; i < addLength; i++)
            {
                Assert.AreEqual(list[i + checkLength], defaultValue);
            }
        }
    }
}