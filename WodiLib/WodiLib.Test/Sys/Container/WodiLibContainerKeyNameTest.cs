using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class WodiLibContainerKeyNameTest
    {
        private static Logger logger = default!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase("String", false)]
        [TestCase("文字列", false)]
        [TestCase("", true)]
        [TestCase("\n", true)]
        [TestCase("あい\r\nうえお", true)]
        public static void ConstructorTest(string src, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new WodiLibContainerKeyName(src);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
    }
}
