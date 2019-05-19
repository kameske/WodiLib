using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class MpsFilePathTest
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
        [TestCase("Map0000.mps", false)]
        [TestCase("map0123.MPS", false)]
        [TestCase("Map0000.map", true)]
        [TestCase("Map_.mps.bak", true)]
        [TestCase("./Map0002.mps", false)]
        [TestCase(@".\Data\Map0003.mps", false)]
        [TestCase(@"c:\MyProject\Data\Map0003.mps", false)]
        [TestCase(@"c:\MyProject\Data\Map0003.map", true)]
        public static void ConstructorTest(string path, bool isError)
        {
            MpsFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new MpsFilePath(path);
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
            Assert.AreEqual((string) instance, path);
        }
    }
}