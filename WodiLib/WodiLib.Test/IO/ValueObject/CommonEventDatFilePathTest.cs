using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class CommonEventDatFilePathTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("CommonEvent.dat", false)]
        [TestCase("commonevent.dat", false)]
        [TestCase("CommonEven.dat", true)]
        [TestCase("CommonEven.dat.bak", true)]
        [TestCase("./CommonEvent.dat", false)]
        [TestCase(@".\Data\CommonEvent.dat", false)]
        [TestCase(@"c:\MyProject\Data\CommonEvent.dat", false)]
        [TestCase(@"c:\MyProject\Data\Common_Event.dat", true)]
        public static void ConstructorTest(string path, bool isError)
        {
            CommonEventDatFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonEventDatFilePath(path);

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