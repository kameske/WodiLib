using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapSizeTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(20, 15)]
        [TestCase(20, 9999)]
        [TestCase(9999, 15)]
        [TestCase(9999, 9999)]
        public static void ConstructorTest(int width, int height)
        {
            MapSize instance = default(MapSize);

            var errorOccured = false;
            try
            {
                instance = new MapSize(width, height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // セットした値と一致すること
            Assert.AreEqual((int) instance.Width, width);
            Assert.AreEqual((int) instance.Height, height);
        }
    }
}