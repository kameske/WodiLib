using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class TileSetDataFilePathTest
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
        [TestCase("TileSetData.dat", false)]
        [TestCase("TILESETDATA.DAT", false)]
        [TestCase("TileSetData_.dat", false)]
        [TestCase("TileSetData.dat.bak", false)]
        [TestCase("./TileSetData.dat", false)]
        [TestCase(@".\Data\TileSetData.dat", false)]
        [TestCase(@"c:\MyProject\Data\TileSetData.dat", false)]
        [TestCase(@"c:\MyProject\Data\TileSetData.data", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            TileSetDataFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new TileSetDataFilePath(path);
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
            var target = (TileSetDataFilePath) "test\\TileSetData.dat";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}