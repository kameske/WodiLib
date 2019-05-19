using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseMergedDataReaderTest
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            DatabaseMergedDataTestItemGenerator.OutputFile();
        }

        private static readonly object[] DBDataReadTestCaseSource =
        {
            new object[] {DatabaseMergedDataTestItemGenerator.GenerateCDB0MergedData(), DBKind.Changeable},
            new object[] {DatabaseMergedDataTestItemGenerator.GenerateUDB0MergedData(), DBKind.User},
        };

        [TestCaseSource(nameof(DBDataReadTestCaseSource))]
        public static void DBDataReadTest(DatabaseMergedData resultData, DBKind dbKind)
        {
            DatabaseMergedDataReader reader = null;

            DatabaseDatFilePath datFilePath = null;
            DatabaseProjectFilePath projectFilePath = null;

            if (dbKind == DBKind.User)
            {
                datFilePath =
                    (UserDatabaseDatFilePath) $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\Database.dat";
                projectFilePath =
                    (UserDatabaseProjectFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\Database.project";
                reader = new DatabaseMergedDataReader(
                    (UserDatabaseDatFilePath) datFilePath,
                    (UserDatabaseProjectFilePath) projectFilePath);
            }
            else if (dbKind == DBKind.Changeable)
            {
                datFilePath =
                    (ChangeableDatabaseDatFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\CDatabase.dat";
                projectFilePath =
                    (ChangeableDatabaseProjectFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\CDatabase.project";
                reader = new DatabaseMergedDataReader(
                    (ChangeableDatabaseDatFilePath) datFilePath,
                    (ChangeableDatabaseProjectFilePath) projectFilePath);
            }
            else if (dbKind == DBKind.System)
            {
                datFilePath =
                    (SystemDatabaseDatFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\SysDatabase.dat";
                projectFilePath =
                    (SystemDatabaseProjectFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\SysDatabase.project";
                reader = new DatabaseMergedDataReader(
                    (SystemDatabaseDatFilePath) datFilePath,
                    (SystemDatabaseProjectFilePath) projectFilePath);
            }
            else
            {
                Assert.Fail();
            }

            Assert.NotNull(datFilePath);
            Assert.NotNull(projectFilePath);
            Assert.NotNull(reader);

            var readResult = false;
            var errorMessage = "";
            try
            {
                reader.ReadSync();
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

            Console.WriteLine("Read Test Clear.");

            {
                // DatabaseDat 一致チェック
                var readResultDataBytes = reader.Data.GenerateDatabaseDat().ToBinary();

                // 元のデータと一致すること
                using (var stream = new FileStream(datFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
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
                var resultDataBytes = resultData.GenerateDatabaseDat().ToBinary().ToArray();

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

            {
                // DatabaseProject 一致チェック
                var readResultDataBytes = reader.Data.GenerateDatabaseProject().ToBinary();

                // 元のデータと一致すること
                using (var stream = new FileStream(projectFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
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
                var resultDataBytes = resultData.GenerateDatabaseProject().ToBinary().ToArray();

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

            Assert.True(true);
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DatabaseMergedDataTestItemGenerator.DeleteFile();
        }
    }
}