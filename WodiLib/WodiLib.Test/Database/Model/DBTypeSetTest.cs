using System;
using System.Collections.Generic;
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
    public class DBTypeSetTest
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
            DBTypeSetFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] TypeNameTestCaseSource =
        {
            new object[] {(TypeName) "typeName", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(TypeNameTestCaseSource))]
        public static void TypeNameTest(TypeName typeName, bool isError)
        {
            var instance = new DBTypeSet();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.TypeName = typeName;
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
                var setValue = instance.TypeName;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(typeName));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBTypeSet.TypeName)));
            }
        }


        [Test]
        public static void ItemSettingListGetterTest()
        {
            var instance = new DBTypeSet();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.ItemSettingList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] MemoTestCaseSource =
        {
            new object[] {(DatabaseMemo) "Memo", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(MemoTestCaseSource))]
        public static void MemoTest(DatabaseMemo memo, bool isError)
        {
            var instance = new DBTypeSet();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Memo = memo;
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
                var setValue = instance.Memo;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(memo));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBTypeSet.Memo)));
            }
        }


        private static readonly object[] ToBinaryTestCaseSource =
        {
            new object[]
            {
                $@"{DBTypeSetFileTestItemGenerator.TestWorkRootDir}\タイプ設定_000_UDB0.dbtypeset",
                DBTypeSetFileTestItemGenerator.GenerateUDB0Data(),
                545
            },
            new object[]
            {
                $@"{DBTypeSetFileTestItemGenerator.TestWorkRootDir}\タイプ設定_000_あいうえお.dbtypeset",
                DBTypeSetFileTestItemGenerator.GenerateCDB0Data(),
                508
            },
        };

        [TestCaseSource(nameof(ToBinaryTestCaseSource))]
        public static void ToBinaryTest(string testFilePath, DBTypeSet generatedData, int fileSize)
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

        [Test]
        public static void SerializeTest()
        {
            var target = new DBTypeSet
            {
                Memo = "Memo"
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBTypeSetFileTestItemGenerator.DeleteFile();
        }
    }
}