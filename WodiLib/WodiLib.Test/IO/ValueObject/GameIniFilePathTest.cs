using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class GameIniFilePathTest
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
        [TestCase("Game.ini", false)]
        [TestCase("GAME.INI", false)]
        [TestCase("GameIni", false)]
        [TestCase("Game.ini.bak", false)]
        [TestCase("./game.ini", false)]
        [TestCase(@".\Data\Game.ini", false)]
        [TestCase(@"c:\MyProject\Data\Game.ini", false)]
        [TestCase(@"c:\MyProject\Data\Game.in", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            GameIniFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new GameIniFilePath(path);
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
