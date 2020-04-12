using System;
using Commons;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class GameIniFileReaderTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            GameIniDataTestItemGenerator.OutputFile();
        }

        [Test]
        public static void GameIniData0Test()
        {
            Common(
                GameIniDataTestItemGenerator.GenerateData0(),
                @"Dir0\Game.ini");

            Assert.True(true);
        }

        private static void Common(GameIniData resultData, string readFileName)
        {
            var filePath = $@"{CommonFileTestItemGenerator.TestWorkRootDir}\{readFileName}";
            var reader = new GameIniFileReader(filePath);

            GameIniData readData = null;

            var readResult = false;
            var errorMessage = "";
            try
            {
                readData = reader.ReadSync();
                readResult = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorMessage = ex.Message;
            }

            // 正しく読めること
            if (!readResult)
            {
                throw new InvalidOperationException(
                    $"Error Occured. Message : {errorMessage}");
            }
            Assert.NotNull(readData);

            Console.WriteLine("Read Test Clear.");

            // 意図したデータと一致すること
            var propertyInfos = typeof(GameIniData).GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                var readPropertyValue = propertyInfo.GetValue(readData);
                var answerPropertyValue = propertyInfo.GetValue(resultData);

                Assert.AreEqual(readPropertyValue, answerPropertyValue);
            }
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            GameIniDataTestItemGenerator.DeleteFile();
        }

    }
}