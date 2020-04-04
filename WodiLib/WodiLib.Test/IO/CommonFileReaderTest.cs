using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class CommonFileReaderTest
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
            CommonFileTestItemGenerator.OutputFile();
        }

        [Test]
        public static void Common000Test()
        {
            Common(
                CommonFileTestItemGenerator.GenerateCommon000Data(),
                "000_コモンイベント000.common");

            Assert.True(true);
        }

        [Test]
        public static void Common003To005Test()
        {
            Common(
                CommonFileTestItemGenerator.GenerateCommon003To005Data(),
                "Common003to005_コモンイベント003.common");

            Assert.True(true);
        }

        [Test]
        public static void Common005To006Test()
        {
            Common(
                CommonFileTestItemGenerator.GenerateCommon005To006Data(),
                "Common005to006_コモンイベント005.common");

            Assert.True(true);
        }

        private static void Common(CommonFileData resultData, string readFileName)
        {
            var filePath = $@"{CommonFileTestItemGenerator.TestWorkRootDir}\{readFileName}";
            var reader = new CommonFileReader(filePath);

            CommonFileData data = null;
            var readResult = false;
            var errorMessage = "";
            try
            {
                data = reader.ReadSync();
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

            Console.WriteLine("Write Test Clear.");

            var readResultDataBytes = data.ToBinary().ToArray();

            // 元のデータと一致すること
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var bufLength = (int) stream.Length;
                var buf = new byte[bufLength];
                stream.Read(buf, 0, bufLength);

                if (readResultDataBytes.Length != bufLength)
                {
                    throw new InvalidOperationException(
                        $"Data Length Not Match. " +
                        $"(answerLength: {bufLength}, readResultLength: {readResultDataBytes.Length})");
                }

                for (long i = 0; i < 0; i++)
                {
                    if (readResultDataBytes[i] != buf[i])
                    {
                        throw new InvalidOperationException(
                            $"Data Byte Not Match. (index: {i}, answer: {buf[i]}," +
                            $" readResult: {readResultDataBytes[i]})");
                    }
                }
            }

            // 意図したデータと一致すること
            var resultDataBytes = resultData.ToBinary().ToArray();

            if (resultDataBytes.Length != readResultDataBytes.Length)
            {
                throw new InvalidOperationException(
                    $"Data Length Not Match. " +
                    $"(answerLength: {resultDataBytes.Length}, readResultLength: {readResultDataBytes.Length})");
            }

            for (long i = 0; i < 0; i++)
            {
                if (resultDataBytes[i] != readResultDataBytes[i])
                {
                    throw new InvalidOperationException(
                        $"Data Byte Not Match. (index: {i}, answer: {resultDataBytes[i]}," +
                        $" readResult: {readResultDataBytes[i]})");
                }
            }
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            CommonFileTestItemGenerator.DeleteMapFile();
        }
    }
}