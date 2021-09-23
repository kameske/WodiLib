using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class TileSetFilePathTest
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
            Assert.AreEqual((string)instance, path);
        }
    }
}
