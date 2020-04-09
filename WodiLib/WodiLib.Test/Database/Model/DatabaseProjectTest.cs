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
    public class DatabaseProjectTest
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
            DatabaseProjectFileTestItemGenerator.OutputFile();
        }

        [Test]
        public static void ConstructorTest()
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseProject();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void TypeSettingListTest(bool isSetNull, bool isError)
        {
            var instance = new DatabaseProject();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var typeSettingList = isSetNull ? null : new DBTypeSettingList();

            var errorOccured = false;
            try
            {
                instance.TypeSettingList = typeSettingList;
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
                var propertyValue = instance.TypeSettingList;

                Assert.NotNull(typeSettingList);
                Assert.NotNull(propertyValue);

                // 取得した値が意図した値であること
                Assert.IsTrue(propertyValue.Equals(typeSettingList));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseProject.TypeSettingList)));
            }
        }

        private static readonly object[] ToBinaryTestCaseSource =
        {
            new object[]
            {
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\DataBase0.project",
                DatabaseProjectFileTestItemGenerator.GenerateDatabase0Project(),
                1256
            },
            new object[]
            {
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\CDataBase0.project",
                DatabaseProjectFileTestItemGenerator.GenerateCDatabase0Project(),
                664
            },
        };

        [TestCaseSource(nameof(ToBinaryTestCaseSource))]
        public static void ToBinaryTest(string testFilePath, DatabaseProject generatedData, int fileSize)
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
                    .ForEach(s => Console.WriteLine(s));

                Console.WriteLine();

                generatedDataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\"").ToList()
                    .ForEach(s => Console.WriteLine(s));

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
            var target = new DatabaseProject
            {
                DBKind = DBKind.System
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
            DatabaseProjectFileTestItemGenerator.DeleteFile();
        }
    }
}