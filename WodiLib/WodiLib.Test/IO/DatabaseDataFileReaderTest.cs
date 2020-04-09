using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseDataFileReaderTest
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
            DatabaseDatFileTestItemGenerator.OutputFile();
        }


        private static readonly object[] DatabaseDatReadTestCaseSource =
        {
            new object[]
            {
                DatabaseDatFileTestItemGenerator.GenerateDataBaseDat0Data(),
                (UserDatabaseDatFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\Database0.dat",
                DBKind.User
            },
            new object[]
            {
                DatabaseDatFileTestItemGenerator.GenerateCDatabaseData0Data(),
                (ChangeableDatabaseDatFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\CDatabase0.dat",
                DBKind.Changeable
            },
        };

        [TestCaseSource(nameof(DatabaseDatReadTestCaseSource))]
        public static void DatabaseDatReadTest(DatabaseDat resultDat, DatabaseDatFilePath readFileName, DBKind dbKind)
        {
            var reader = new DatabaseDatFileReader(readFileName, dbKind);

            DatabaseDat data = null;
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
            var resultDataBytes = resultDat.ToBinary().ToArray();

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