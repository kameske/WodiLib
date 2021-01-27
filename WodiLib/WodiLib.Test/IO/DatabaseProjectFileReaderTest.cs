using System;
using System.IO;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseProjectFileReaderTest
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
            DatabaseProjectFileTestItemGenerator.OutputFile();
        }


        [Test]
        public static void Database0DatTest()
        {
            Common(
                DatabaseProjectFileTestItemGenerator.GenerateDatabase0Project(),
                (UserDatabaseProjectFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\Database0.project",
                DBKind.User);

            Assert.True(true);
        }

        [Test]
        public static void CDatabase0DatTest()
        {
            Common(
                DatabaseProjectFileTestItemGenerator.GenerateCDatabase0Project(),
                (ChangeableDatabaseProjectFilePath)
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\CDatabase0.project",
                DBKind.Changeable);

            Assert.True(true);
        }


        private static void Common(DatabaseProject resultData, DatabaseProjectFilePath readFileName, DBKind dbKind)
        {
            var reader = new DatabaseProjectFileReader(readFileName, dbKind);

            var readResult = false;
            DatabaseProject data = null;
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
            using (var stream = new FileStream(readFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
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
            DatabaseDatFileTestItemGenerator.DeleteFile();
        }
    }
}
