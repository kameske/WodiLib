using System;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class EditorIniFileReaderTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            EditorIniDataTestItemGenerator.OutputFile();
        }

        [Test]
        public static void EditorIniData0Test()
        {
            Common(
                EditorIniDataTestItemGenerator.GenerateData0(),
                @"Dir0\Editor.ini");

            Assert.True(true);
        }

        private static void Common(EditorIniData resultData, string readFileName)
        {
            var filePath = $@"{CommonFileTestItemGenerator.TestWorkRootDir}\{readFileName}";
            var reader = new EditorIniFileReader(filePath);

            EditorIniData readData = null;

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
            var propertyInfos = typeof(EditorIniData).GetProperties();

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
            EditorIniDataTestItemGenerator.DeleteFile();
        }

    }
}