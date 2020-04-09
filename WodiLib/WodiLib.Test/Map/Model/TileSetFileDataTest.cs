using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TileSetFileDataTest
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
            TileSetFileDataTestItemGenerator.OutputFile();
        }

        private static readonly object[] TileSetSettingTestCaseSource =
        {
            new object[] {new TileSetSetting(), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(TileSetSettingTestCaseSource))]
        public static void TileSetSettingTest(TileSetSetting setting, bool isError)
        {
            var instance = new TileSetFileData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.TileSetSetting = setting;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.TileSetSetting;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(setting));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileSetFileData.TileSetSetting)));
            }
        }

        private static readonly object[] ToBinaryTestCaseSource =
        {
            new object[]
            {
                $@"{TileSetFileDataTestItemGenerator.TestWorkRootDir}\000_設定名.tile",
                TileSetFileDataTestItemGenerator.GenerateData0(),
                10185
            },
        };

        [TestCaseSource(nameof(ToBinaryTestCaseSource))]
        public static void ToBinaryTest(string testFilePath, TileSetFileData generatedDat, int fileSize)
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
                fileData.Select((s, index) => $"=\"[{index}] = {{byte}} {s:X}\"").ToList()
                    .ForEach(Console.WriteLine);

                Console.WriteLine();

                generatedDataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s:X}\"").ToList()
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
            var target = new TileSetFileData
            {
                TileSetSetting = new TileSetSetting
                {
                    Name = "Name",
                }
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }


        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            TileSetFileDataTestItemGenerator.DeleteFile();
        }
    }
}