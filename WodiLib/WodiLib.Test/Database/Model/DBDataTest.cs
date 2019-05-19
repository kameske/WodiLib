using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBDataTest
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
            DBDataFileTestItemGenerator.OutputFile();
        }

        [Test]
        public static void DataDescListTest()
        {
            var instance = new DBData();

            var errorOccured = false;
            try
            {
                var _ = instance.DataDescList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] ToBinaryTestCaseSource =
        {
            new object[]
            {
                $@"{DBDataFileTestItemGenerator.TestWorkRootDir}\あいうえお_データ_000to000_a.dbdata",
                DBDataFileTestItemGenerator.GenerateCDB0DBData(),
                96
            },
            new object[]
            {
                $@"{DBDataFileTestItemGenerator.TestWorkRootDir}\UDB0_データ_001to003_7.dbdata",
                DBDataFileTestItemGenerator.GenerateUDB0DBData(),
                198
            },
        };

        [TestCaseSource(nameof(ToBinaryTestCaseSource))]
        public static void ToBinaryTest(string testFilePath, DBData generatedData, int fileSize)
        {
            var generatedDataBuf = generatedData.ToBinary();

            using (var fs = new FileStream(testFilePath, FileMode.Open))
            {
                var length = (int) fs.Length;
                // ファイルサイズが規定でない場合誤作動防止の為テスト失敗にする
                Assert.AreEqual(length, fileSize);


                var fileData = new byte[length];
                fs.Read(fileData, 0, length);

                // binデータ出力
                fileData.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\"").ToList()
                    .ForEach(Console.WriteLine);

                Console.WriteLine();

                generatedDataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\"").ToList()
                    .ForEach(Console.WriteLine);

                for (var i = 0; i < generatedDataBuf.Length; i++)
                {
                    if (i == fileData.Length)
                        Assert.Fail(
                            $"データ帳が異なります。（期待値：{fileData.Length}, 実際：{generatedDataBuf.Length}）");

                    if (fileData[i] != generatedDataBuf[i])
                        Assert.Fail(
                            $"offset: {i} のバイナリが異なります。（期待値：{fileData[i]}, 実際：{generatedDataBuf[i]}）");
                }

                if (fileData.Length != generatedDataBuf.Length)
                    Assert.Fail(
                        $"データ帳が異なります。（期待値：{fileData.Length}, 実際：{generatedDataBuf.Length}）");
            }
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBDataFileTestItemGenerator.DeleteFile();
        }
    }
}