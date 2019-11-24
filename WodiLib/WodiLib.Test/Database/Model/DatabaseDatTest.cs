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
    public class DatabaseDatTest
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

        [Test]
        public static void ConstructorTest()
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseDat();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [Test]
        public static void SettingListTest()
        {
            var instance = new DatabaseDat();

            var errorOccured = false;
            try
            {
                var _ = instance.SettingList;
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
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\DataBase0.dat",
                DatabaseDatFileTestItemGenerator.GenerateDataBaseDat0Data(),
                455
            },
            new object[]
            {
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\CDataBase0.dat",
                DatabaseDatFileTestItemGenerator.GenerateCDatabaseData0Data(),
                269
            },
        };

        [TestCaseSource(nameof(ToBinaryTestCaseSource))]
        public static void ToBinaryTest(string testFilePath, DatabaseDat generatedDat, int fileSize)
        {
            var generatedDataBuf = generatedDat.ToBinary();

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

        [Test]
        public static void SerializeTest()
        {
            var target = new DatabaseDat
            {
                DBKind = DBKind.System,
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }


        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DatabaseDatFileTestItemGenerator.DeleteFile();
        }
    }
}