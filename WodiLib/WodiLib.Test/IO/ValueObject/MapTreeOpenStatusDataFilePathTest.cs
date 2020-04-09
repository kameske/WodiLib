using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class MapTreeOpenStatusDataFilePathTest
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
        [TestCase("MapTreeOpenStatus.dat", false)]
        [TestCase("MAPTREEOPENSTATUS.DAT", false)]
        [TestCase("MapTreeOpenStatus_.dat", false)]
        [TestCase("MapTreeOpenStatus.dat.bak", false)]
        [TestCase("./MapTreeOpenStatus.dat", false)]
        [TestCase(@".\Data\MapTreeOpenStatus.dat", false)]
        [TestCase(@"c:\MyProject\Data\MapTreeOpenStatus.dat", false)]
        [TestCase(@"c:\MyProject\Data\MapTreeOpenStatus.data", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            MapTreeOpenStatusDataFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new MapTreeOpenStatusDataFilePath(path);
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

        [Test]
        public static void SerializeTest()
        {
            var target = (MapTreeOpenStatusDataFilePath) "test\\MapTreeOpenStatus.dat";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}