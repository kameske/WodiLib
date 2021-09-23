using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class CommonFilePathTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("Common0000.common", false)]
        [TestCase("com123.COMMON", false)]
        [TestCase("Common.com", false)]
        [TestCase("CommonEvent0000.common.bak", false)]
        [TestCase("./common0000_to1234.common", false)]
        [TestCase(@".\Data\Common.Common", false)]
        [TestCase(@"c:\MyProject\Data\Common0000.common", false)]
        [TestCase(@"c:\MyProject\Data\Common0000.co", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            CommonFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonFilePath(path);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 内容が一致すること
            Assert.AreEqual((string)instance, path);
        }
    }
}
