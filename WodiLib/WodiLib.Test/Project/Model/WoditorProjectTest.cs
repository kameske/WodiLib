using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Project.Model
{
    [TestFixture]
    public class WoditorProjectTest
    {
        private static WodiLibLogger logger;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            ProjectFileTestItemGenerator.OutputProjectItem();
        }

        [SetUp]
        public static void Setup()
        {
            // デバッグ情報等まで出力すると出力に時間がかかりすぎてAbortするためログレベルを抑える
            LoggerInitializer.SetupWodiLibLoggerForProjectTest();
            logger = WodiLibLogger.GetInstance(LoggerInitializer.KeyNameForDebug);
        }

        private static readonly string TestProjectDir
            = $@"{ProjectFileTestItemGenerator.TestWorkRootDir}\{Regex.Replace(ProjectFileTestItemGenerator.TestProjectZips.First().Item1, "(.*).zip", "$1")}";

        private static readonly object[] ConstructorTestCaseSource =
        {
            new object[] {null, true},
            new object[] {@"C:\NotFound", true},
            new object[] {TestProjectDir, false},
            new object[] {$@"{TestProjectDir}/", false},
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(string directory, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new WoditorProject(directory);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void ReadAllSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadAllSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 9);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.CommonEventList)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(WoditorProject.MapTreeNodeList)));
            Assert.IsTrue(changedPropertyList[2].Equals(nameof(WoditorProject.MapTreeOpenStatusList)));
            Assert.IsTrue(changedPropertyList[3].Equals(nameof(WoditorProject.TileSetSettingList)));
            Assert.IsTrue(changedPropertyList[4].Equals(nameof(WoditorProject.ChangeableDatabase)));
            Assert.IsTrue(changedPropertyList[5].Equals(nameof(WoditorProject.UserDatabase)));
            Assert.IsTrue(changedPropertyList[6].Equals(nameof(WoditorProject.SystemDatabase)));
            Assert.IsTrue(changedPropertyList[7].Equals(nameof(WoditorProject.EditorIni)));
            Assert.IsTrue(changedPropertyList[8].Equals(nameof(WoditorProject.GameIni)));
        }

        [Test]
        public static async Task ReadAllAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadAllAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 9);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.CommonEventList)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(WoditorProject.MapTreeNodeList)));
            Assert.IsTrue(changedPropertyList[2].Equals(nameof(WoditorProject.MapTreeOpenStatusList)));
            Assert.IsTrue(changedPropertyList[3].Equals(nameof(WoditorProject.TileSetSettingList)));
            Assert.IsTrue(changedPropertyList[4].Equals(nameof(WoditorProject.ChangeableDatabase)));
            Assert.IsTrue(changedPropertyList[5].Equals(nameof(WoditorProject.UserDatabase)));
            Assert.IsTrue(changedPropertyList[6].Equals(nameof(WoditorProject.SystemDatabase)));
            Assert.IsTrue(changedPropertyList[7].Equals(nameof(WoditorProject.EditorIni)));
            Assert.IsTrue(changedPropertyList[8].Equals(nameof(WoditorProject.GameIni)));
        }


        [Test]
        public static void ReadCommonEventDataSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadCommonEventDataSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.CommonEventList)));
        }


        [Test]
        public static void ReadMapTreeDataSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadMapTreeDataSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.MapTreeNodeList)));
        }


        [Test]
        public static void ReadMapTreeOpenStatusDataSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadMapTreeOpenStatusDataSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.MapTreeOpenStatusList)));
        }


        [Test]
        public static void ReadTileSetDataSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadTileSetDataSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.TileSetSettingList)));
        }


        [Test]
        public static void ReadChangeableDatabaseSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadChangeableDatabaseSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.ChangeableDatabase)));
        }


        [Test]
        public static void ReadUserDatabaseSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadUserDatabaseSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.UserDatabase)));
        }


        [Test]
        public static void ReadSystemDatabaseSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadSystemDatabaseSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.SystemDatabase)));
        }


        [Test]
        public static void ReadEditorIniSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadEditorIniSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.EditorIni)));
        }


        [Test]
        public static void ReadGameIniSyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ReadGameIniSync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.GameIni)));
        }


        [Test]
        public static async Task ReadCommonEventDataAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadCommonEventDataAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.CommonEventList)));
        }


        [Test]
        public static async Task ReadMapTreeDataAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadMapTreeDataAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.MapTreeNodeList)));
        }


        [Test]
        public static async Task ReadMapTreeOpenStatusDataAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadMapTreeOpenStatusDataAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.MapTreeOpenStatusList)));
        }


        [Test]
        public static async Task ReadTileSetDataAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadTileSetDataAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.TileSetSettingList)));
        }


        [Test]
        public static async Task ReadChangeableDatabaseAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadChangeableDatabaseAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.ChangeableDatabase)));
        }


        [Test]
        public static async Task ReadUserDatabaseAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadUserDatabaseAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.UserDatabase)));
        }


        [Test]
        public static async Task ReadSystemDatabaseAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadSystemDatabaseAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.SystemDatabase)));
        }


        [Test]
        public static async Task ReadEditorIniAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadEditorIniAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.EditorIni)));
        }


        [Test]
        public static async Task ReadGameIniAsyncTest()
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                await instance.ReadGameIniAsync();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(WoditorProject.GameIni)));
        }

        private static readonly object[] RemoveMpsFilesCacheTestCaseSource =
        {
            new object[] {(MpsFilePath) "Data/MapData/TitleMap.mps", (MpsFilePath) "Data/MapData/TitleMap.mps", false},
            new object[]
                {(MpsFilePath) "Data/MapData/TitleMap.mps", (MpsFilePath) "Data/MapData/SampleMapA.mps", false},
            new object[] {(MpsFilePath) "Data/MapData/TitleMap.mps", null, true},
        };

        [TestCaseSource(nameof(RemoveMpsFilesCacheTestCaseSource))]
        public static void RemoveMpsFilesCacheTest(MpsFilePath firstRead, MpsFilePath removePath, bool isError)
        {
            var instance = new WoditorProject(TestProjectDir);
            instance.ReadMpsFileSync(firstRead);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.RemoveMpsFilesCache(removePath);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが意図した値と一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // 削除したマップデータがプールから消えていること
                var check = instance.MpsFilesPool
                    .FirstOrDefault(x => x.Key.Equals(removePath));
                Assert.IsTrue(check.IsNull());
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ClearMpsFilesCache()
        {
            var instance = new WoditorProject(TestProjectDir);
            instance.ReadMpsFileSync("Data/MapData/TitleMap.mps");
            instance.ReadMpsFileSync("Data/MapData/SampleMapA.mps");
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ClearMpsFilesCache();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プールのデータが0件であること
            Assert.AreEqual(instance.MpsFilesPool.Count, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] GetMapEventEventCommandSentenceInfoListTestCaseSource =
        {
            new object[] {ProjectFileTestItemGenerator.TestInfoList[0]},
        };

        [TestCaseSource(nameof(GetMapEventEventCommandSentenceInfoListTestCaseSource))]
        public static void GetMapEventEventCommandSentenceInfoListSyncTest(EventCommandSentenceTestInfo testInfo)
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            IReadOnlyList<EventCommandSentenceInfo> resultList = null;

            var errorOccured = false;
            try
            {
                resultList = instance.GetMapEventEventCommandSentenceInfoListSync(
                    testInfo.MapEventInfo.FilePath, testInfo.MapEventInfo.MapEventId,
                    testInfo.MapEventInfo.PageIndex, false, false);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得したコマンド文字列が意図した文字列と一致すること
            var answerFilePath =
                $@"{ProjectFileTestItemGenerator.TestWorkRootDir}\{testInfo.MapEventInfo.EventCommandSentenceAnswerFilePath}";
            var answerLines = File.ReadAllLines(answerFilePath);

            // すべての行を比較する
            var isSuccess = true;
            var resultListCnt = resultList.Count;
            var answerListCnt = answerLines.Length;
            var length = resultListCnt > answerListCnt
                ? resultListCnt
                : answerListCnt;
            for (var i = 0; i < length; i++)
            {
                logger.Info($"{i}行目比較");
                var resultSentence = resultListCnt > i
                    ? resultList[i].Sentence.ToString()
                    : "";
                var answerSentence = answerListCnt > i
                    ? answerLines[i]
                    : "";

                logger.Info($"    変換結果：{resultSentence}");
                logger.Info($"    真値    ：{answerSentence}");

                var isEqual = resultSentence.Equals(answerSentence);

                logger.Info($"    比較結果：{isEqual}");
                isSuccess &= isEqual;
            }

            Assert.IsTrue(isSuccess);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);

            // 省略しないイベントコマンド文字列を出力
            var resultList2 = instance.GetMapEventEventCommandSentenceInfoListSync(
                testInfo.MapEventInfo.FilePath, testInfo.MapEventInfo.MapEventId,
                testInfo.MapEventInfo.PageIndex, true);
            resultList2.ForEach((result, i) =>
            {
                logger.Info($"{i}行目:{result.Sentence}");
            });
        }

        [TestCaseSource(nameof(GetMapEventEventCommandSentenceInfoListTestCaseSource))]
        public static async Task GetMapEventEventCommandSentenceInfoListAsyncTest(EventCommandSentenceTestInfo testInfo)
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            IReadOnlyList<EventCommandSentenceInfo> resultList = null;

            var errorOccured = false;
            try
            {
                resultList = await instance.GetMapEventEventCommandSentenceInfoListAsync(
                    testInfo.MapEventInfo.FilePath, testInfo.MapEventInfo.MapEventId,
                    testInfo.MapEventInfo.PageIndex, false, false);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得したコマンド文字列が意図した文字列と一致すること
            var answerFilePath =
                $@"{ProjectFileTestItemGenerator.TestWorkRootDir}\{testInfo.MapEventInfo.EventCommandSentenceAnswerFilePath}";
            var answerLines = File.ReadAllLines(answerFilePath);

            // すべての行を比較する
            var isSuccess = true;
            var resultListCnt = resultList.Count;
            var answerListCnt = answerLines.Length;
            var length = resultListCnt > answerListCnt
                ? resultListCnt
                : answerListCnt;
            for (var i = 0; i < length; i++)
            {
                logger.Info($"{i}行目比較");
                var resultSentence = resultListCnt > i
                    ? resultList[i].Sentence.ToString()
                    : "";
                var answerSentence = answerListCnt > i
                    ? answerLines[i]
                    : "";

                logger.Info($"    変換結果：{resultSentence}");
                logger.Info($"    真値    ：{answerSentence}");

                var isEqual = resultSentence.Equals(answerSentence);

                logger.Info($"    比較結果：{isEqual}");
                isSuccess &= isEqual;
            }

            Assert.IsTrue(isSuccess);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] GetCommonEventEventCommandSentenceInfoListTestCaseSource =
        {
            new object[] {ProjectFileTestItemGenerator.TestInfoList[0]},
        };

        [TestCaseSource(nameof(GetCommonEventEventCommandSentenceInfoListTestCaseSource))]
        public static void GetCommonEventEventCommandSentenceInfoListSyncTest(EventCommandSentenceTestInfo testInfo)
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            IReadOnlyList<EventCommandSentenceInfo> resultList = null;

            var errorOccured = false;
            try
            {
                resultList = instance.GetCommonEventEventCommandSentenceInfoListSync(
                    testInfo.CommonEventInfo.CommonEventId, testInfo.CommonEventInfo.MpsFilePath, false, false);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得したコマンド文字列が意図した文字列と一致すること
            var answerFilePath =
                $@"{ProjectFileTestItemGenerator.TestWorkRootDir}\{testInfo.CommonEventInfo.EventCommandSentenceAnswerFilePath}";
            var answerLines = File.ReadAllLines(answerFilePath);

            // すべての行を比較する
            var isSuccess = true;
            var resultListCnt = resultList.Count;
            var answerListCnt = answerLines.Length;
            var length = resultListCnt > answerListCnt
                ? resultListCnt
                : answerListCnt;
            for (var i = 0; i < length; i++)
            {
                logger.Info($"{i}行目比較");
                var resultSentence = resultListCnt > i
                    ? resultList[i].Sentence.ToString()
                    : "";
                var answerSentence = answerListCnt > i
                    ? answerLines[i]
                    : "";

                logger.Info($"    変換結果：{resultSentence}");
                logger.Info($"    真値    ：{answerSentence}");

                var isEqual = resultSentence.Equals(answerSentence);

                logger.Info($"    比較結果：{isEqual}");
                isSuccess &= isEqual;
            }

            Assert.IsTrue(isSuccess);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);

            // 省略しないイベントコマンド文字列を出力
            var resultList2 = instance.GetCommonEventEventCommandSentenceInfoListSync(
                testInfo.CommonEventInfo.CommonEventId, testInfo.CommonEventInfo.MpsFilePath, true);
            resultList2.ForEach((result, i) =>
            {
                logger.Info($"{i}行目:{result.Sentence}");
            });
        }

        [TestCaseSource(nameof(GetCommonEventEventCommandSentenceInfoListTestCaseSource))]
        public static async Task GetCommonEventEventCommandSentenceInfoListAsyncTest(
            EventCommandSentenceTestInfo testInfo)
        {
            var instance = new WoditorProject(TestProjectDir);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            IReadOnlyList<EventCommandSentenceInfo> resultList = null;

            var errorOccured = false;
            try
            {
                resultList = await instance.GetCommonEventEventCommandSentenceInfoListAsync(
                    testInfo.CommonEventInfo.CommonEventId, testInfo.CommonEventInfo.MpsFilePath, false, false);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得したコマンド文字列が意図した文字列と一致すること
            var answerFilePath =
                $@"{ProjectFileTestItemGenerator.TestWorkRootDir}\{testInfo.CommonEventInfo.EventCommandSentenceAnswerFilePath}";
            var answerLines = File.ReadAllLines(answerFilePath);

            // すべての行を比較する
            var isSuccess = true;
            var resultListCnt = resultList.Count;
            var answerListCnt = answerLines.Length;
            var length = resultListCnt > answerListCnt
                ? resultListCnt
                : answerListCnt;
            for (var i = 0; i < length; i++)
            {
                logger.Info($"{i}行目比較");
                var resultSentence = resultListCnt > i
                    ? resultList[i].Sentence.ToString()
                    : "";
                var answerSentence = answerListCnt > i
                    ? answerLines[i]
                    : "";

                logger.Info($"    変換結果：{resultSentence}");
                logger.Info($"    真値    ：{answerSentence}");

                var isEqual = resultSentence.Equals(answerSentence);

                logger.Info($"    比較結果：{isEqual}");
                isSuccess &= isEqual;
            }

            Assert.IsTrue(isSuccess);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            ProjectFileTestItemGenerator.DeleteProjectItem();
        }
    }
}