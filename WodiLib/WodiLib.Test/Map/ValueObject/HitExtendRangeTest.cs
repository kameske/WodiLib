using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class HitExtendRangeTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(0, 0, false)]
        [TestCase(250, 0, false)]
        [TestCase(251, 0, false)]
        [TestCase(0, 250, false)]
        [TestCase(250, 250, false)]
        [TestCase(251, 250, false)]
        [TestCase(0, 251, false)]
        [TestCase(250, 251, false)]
        [TestCase(251, 251, false)]
        public static void ConstructorTest(byte width, byte height, bool isError)
        {
            HitExtendRange instance = default(HitExtendRange);

            var errorOccured = false;
            try
            {
                instance = new HitExtendRange(width, height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // セットした値と一致すること
            Assert.AreEqual(instance.Width, width);
            Assert.AreEqual(instance.Height, height);
        }
    }
}