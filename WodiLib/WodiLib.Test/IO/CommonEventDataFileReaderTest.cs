using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class CommonEventDataFileReaderTest
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            CommonEventDataFileTestItemGenerator.OutputFile();
        }

        [Test]
        public static void CommonEventData00Test()
        {
            Common(
                CommonEventDataFileTestItemGenerator.GenerateCommonEvent00Data(),
                "CommonEvent_00.dat");

            Assert.True(true);
        }

        private static void Common(CommonEventData resultData, string readFileName)
        {
            var filePath = $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{readFileName}";
            var reader = new CommonEventDatFileReader(filePath);

            var readResult = false;
            CommonEventData commonEventData = null;
            var errorMessage = "";
            try
            {
                commonEventData = reader.ReadSync();
                readResult = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }


            // 正しく読めること
            if (!readResult)
            {
                throw new InvalidOperationException(
                    $"Error Occured. Message : {errorMessage}");
            }

            Console.WriteLine("Write Test Clear.");

            var readResultDataBytes = commonEventData.ToBinary().ToArray();

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
            CommonEventDataFileTestItemGenerator.DeleteMapFile();
        }
    }
}