using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class TileSetFilePathTest
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
        [TestCase("test.tile", false)]
        [TestCase("TILEDATA.TILE", false)]
        [TestCase("TileData.tile.bak", false)]
        [TestCase("./TileData.tile", false)]
        [TestCase(@".\Data\TileData.tile", false)]
        [TestCase(@"c:\MyProject\Data\TileData.tile", false)]
        [TestCase(@"c:\MyProject\Data\TileData.tail", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            TileSetFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new TileSetFilePath(path);
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
            var target = (TileSetFilePath) "test\\test_tile_set.tile";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}