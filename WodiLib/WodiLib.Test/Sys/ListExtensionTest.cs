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

        [TestCase(3, 1, 10)]
        [TestCase(4, 4, 10)]
        [TestCase(2, 5, 10)]
        public static void AdjustLengthTest(int initLength, int adjustLength, int defaultValue)
        {
            var list = new List<int>();
            for(var i=0; i<initLength; i++) list.Add(i);

            var errorOccured = false;
            try
            {
                list.AdjustLength(adjustLength, defaultValue);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

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