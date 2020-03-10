using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDataFileReaderTest
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            DBDataFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] DBDataReadTestCaseSource =
        {
            new object[] {DBDataFileTestItemGenerator.GenerateUDB0DBData(), "UDB0_データ_001to003_7.dbdata"},
            new object[] {DBDataFileTestItemGenerator.GenerateCDB0DBData(), "あいうえお_データ_000to000_a.dbdata"},
        };

        [TestCaseSource(nameof(DBDataReadTestCaseSource))]
        public static void DBDataReadTest(DBData resultData, string readFileName)
        {
            var filePath = $@"{DBDataFileTestItemGenerator.TestWorkRootDir}\{readFileName}";
            var reader = new DBDataFileReader(filePath);

            var readResult = false;
            DBData data = null;
            var errorMessage = "";
            try
            {
                data = reader.ReadSync();
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

            Assert.True(true);
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBDataFileTestItemGenerator.DeleteFile();
        }
    }
}