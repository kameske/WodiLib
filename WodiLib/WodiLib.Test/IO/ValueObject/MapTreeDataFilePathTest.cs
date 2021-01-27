using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class MapTreeDataFilePathTest
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
        [TestCase("MapTree.dat", false)]
        [TestCase("MAPTREE.DAT", false)]
        [TestCase("Map_Tree.dat", false)]
        [TestCase("MapTree.dat.bak", false)]
        [TestCase("./MapTree.dat", false)]
        [TestCase(@".\Data\MapTree.dat", false)]
        [TestCase(@"c:\MyProject\Data\MapTree.dat", false)]
        [TestCase(@"c:\MyProject\Data\MapTree.data", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            MapTreeDataFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new MapTreeDataFilePath(path);
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
