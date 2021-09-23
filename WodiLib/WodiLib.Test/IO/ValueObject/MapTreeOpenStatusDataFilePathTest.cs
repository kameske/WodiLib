using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class MapTreeOpenStatusDataFilePathTest
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
            Assert.AreEqual((string)instance, path);
        }
    }
}
