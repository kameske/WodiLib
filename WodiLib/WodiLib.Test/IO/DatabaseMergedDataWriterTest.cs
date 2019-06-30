using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseMergedDataWriterTest
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            var outputDir = $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\DatabaseMergedDataWriterTest";
            // テスト用ファイル出力
            outputDir.CreateDirectoryIfNeed();
        }

        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                DatabaseMergedDataTestItemGenerator.GenerateCDB0MergedData(),
                DBKind.Changeable
            },
            new object[]
            {
                DatabaseMergedDataTestItemGenerator.GenerateUDB0MergedData(),
                DBKind.User
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DatabaseMergedData outputDat, DBKind dbKind)
        {
            DatabaseMergedDataWriter writer = null;
            if (dbKind == DBKind.User)
            {
                var datFilePath =
                    (ChangeableDatabaseDatFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.dat";
                var projectFilePath =
                    (ChangeableDatabaseProjectFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.project";
                writer = new DatabaseMergedDataWriter(outputDat, datFilePath, projectFilePath);
            }
            else if (dbKind == DBKind.System)
            {
                var datFilePath =
                    (ChangeableDatabaseDatFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\DatabaseMergedDataWriterTest\SysDatabase.dat";
                var projectFilePath =
                    (ChangeableDatabaseProjectFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\DatabaseMergedDataWriterTest\SysDatabase.project";
                writer = new DatabaseMergedDataWriter(outputDat, datFilePath, projectFilePath);
            }
            else if (dbKind == DBKind.Changeable)
            {
                var datFilePath =
                    (ChangeableDatabaseDatFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.dat";
                var projectFilePath =
                    (ChangeableDatabaseProjectFilePath)
                    $@"{DatabaseMergedDataTestItemGenerator.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.project";
                writer = new DatabaseMergedDataWriter(outputDat, datFilePath, projectFilePath);
            }

            Assert.NotNull(writer);

            var isSuccess = false;
            var errorMessage = "";

            try
            {
                writer.WriteSync();
                isSuccess = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            // 出力成功すること
            if (!isSuccess)
            {
                throw new InvalidOperationException(
                    $"Error message: {errorMessage}");
            }

            Assert.True(true);
        }
    }
}