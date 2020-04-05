using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.Model
{
    [TestFixture]
    public class EditorIniDataTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] StartFlagTestCaseSource =
        {
            new object[] {(StartFlag) 0},
        };

        [TestCaseSource(nameof(StartFlagTestCaseSource))]
        public static void StartFlagTest(StartFlag startFlag)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.StartFlag = startFlag;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.StartFlag;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(startFlag));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.StartFlag)));
        }

        private static readonly object[] LastLoadFileTestCaseSource =
        {
            new object[] {null, true},
            new object[] {(LastLoadMapFilePath) "map0001.mps", false},
            new object[] {(LastLoadMapFilePath) @".\MapData\map020.mps", false},
        };

        [TestCaseSource(nameof(LastLoadFileTestCaseSource))]
        public static void LastLoadFileTest(LastLoadMapFilePath lastLoadFile, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.LastLoadFile = lastLoadFile;
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
                var setValue = instance.LastLoadFile;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(lastLoadFile));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.LastLoadFile)));
            }
        }

        private static readonly object[] MainWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(MainWindowPositionTestCaseSource))]
        public static void MainWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MainWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.MainWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.MainWindowPosition)));
        }

        private static readonly object[] MainWindowSizeTestCaseSource =
        {
            new object[] {(WindowSize) (10, 100)},
        };

        [TestCaseSource(nameof(MainWindowSizeTestCaseSource))]
        public static void MainWindowSizeTest(WindowSize size)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MainWindowSize = size;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.MainWindowSize;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(size));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.MainWindowSize)));
        }

        private static readonly object[] MapChipWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(MapChipWindowPositionTestCaseSource))]
        public static void MapChipWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapChipWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.MapChipWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.MapChipWindowPosition)));
        }

        private static readonly object[] MapEventWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(MapEventWindowPositionTestCaseSource))]
        public static void MapEventWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.MapEventWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.MapEventWindowPosition)));
        }

        private static readonly object[] MapEventWindowSizeTestCaseSource =
        {
            new object[] {(WindowSize) (10, 100)},
        };

        [TestCaseSource(nameof(MapEventWindowSizeTestCaseSource))]
        public static void MapEventWindowSizeTest(WindowSize size)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventWindowSize = size;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.MapEventWindowSize;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(size));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.MapEventWindowSize)));
        }

        private static readonly object[] MapEventInputWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(MapEventInputWindowPositionTestCaseSource))]
        public static void MapEventInputWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MapEventInputWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.MapEventInputWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.MapEventInputWindowPosition)));
        }

        private static readonly object[] CommonEventWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(CommonEventWindowPositionTestCaseSource))]
        public static void CommonEventWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CommonEventWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.CommonEventWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.CommonEventWindowPosition)));
        }

        private static readonly object[] CommonEventWindowSizeTestCaseSource =
        {
            new object[] {(WindowSize) (10, 100)},
        };

        [TestCaseSource(nameof(CommonEventWindowSizeTestCaseSource))]
        public static void CommonEventWindowSizeTest(WindowSize size)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CommonEventWindowSize = size;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.CommonEventWindowSize;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(size));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.CommonEventWindowSize)));
        }

        private static readonly object[] CommonEventInputWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(CommonEventInputWindowPositionTestCaseSource))]
        public static void CommonEventInputWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CommonEventInputWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.CommonEventInputWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.CommonEventInputWindowPosition)));
        }

        private static readonly object[] UserDbWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(UserDbWindowPositionTestCaseSource))]
        public static void UserDbWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.UserDbWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.UserDbWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.UserDbWindowPosition)));
        }

        private static readonly object[] ChangeableDbWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(ChangeableDbWindowPositionTestCaseSource))]
        public static void ChangeableDbWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ChangeableDbWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.ChangeableDbWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.ChangeableDbWindowPosition)));
        }

        private static readonly object[] SystemDbWindowPositionTestCaseSource =
        {
            new object[] {(WindowPosition) (10, 100)},
        };

        [TestCaseSource(nameof(SystemDbWindowPositionTestCaseSource))]
        public static void SystemDbWindowPositionTest(WindowPosition position)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SystemDbWindowPosition = position;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.SystemDbWindowPosition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(position));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.SystemDbWindowPosition)));
        }

        private static readonly object[] DatabaseValueNumberDrawTypeTestCaseSource =
        {
            new object[] {null, true},
            new object[] {DatabaseValueNumberDrawType.Off, false},
        };

        [TestCaseSource(nameof(DatabaseValueNumberDrawTypeTestCaseSource))]
        public static void DatabaseValueNumberDrawTypeTest(DatabaseValueNumberDrawType type, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DatabaseValueNumberDrawType = type;
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
                var setValue = instance.DatabaseValueNumberDrawType;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(type));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.DatabaseValueNumberDrawType)));
            }
        }

        private static readonly object[] EditTimeDrawTypeTestCaseSource =
        {
            new object[] {null, true},
            new object[] {EditTimeDrawType.Off, false},
        };

        [TestCaseSource(nameof(EditTimeDrawTypeTestCaseSource))]
        public static void EditTimeDrawTypeTest(EditTimeDrawType type, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.EditTimeDrawType = type;
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
                var setValue = instance.EditTimeDrawType;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(type));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.EditTimeDrawType)));
            }
        }

        private static readonly object[] EditTimeTestCaseSource =
        {
            new object[] {(WorkTime) 120},
        };

        [TestCaseSource(nameof(EditTimeTestCaseSource))]
        public static void EditTimeTest(WorkTime workTime)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.EditTime = workTime;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.EditTime;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(workTime));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.EditTime)));
        }

        private static readonly object[] NotEditTimeTestCaseSource =
        {
            new object[] {(WorkTime) 120},
        };

        [TestCaseSource(nameof(NotEditTimeTestCaseSource))]
        public static void NotEditTimeTest(WorkTime workTime)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.NotEditTime = workTime;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.NotEditTime;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(workTime));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.NotEditTime)));
        }

        [TestCase(true)]
        public static void IsShowDebugWindowTest(bool isShow)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.IsShowDebugWindow = isShow;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsShowDebugWindow;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isShow));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.IsShowDebugWindow)));
        }

        private static readonly object[] LayerTransparentTestCaseSource =
        {
            new object[] {null, true},
            new object[] {LaterTransparentType.FaintlyDarker, false},
        };

        [TestCaseSource(nameof(LayerTransparentTestCaseSource))]
        public static void LayerTransparentTest(LaterTransparentType type, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.LayerTransparent = type;
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
                var setValue = instance.LayerTransparent;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(type));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.LayerTransparent)));
            }
        }

        private static readonly object[] EventLayerOpacityTestCaseSource =
        {
            new object[] {null, true},
            new object[] {EventLayerOpacityType.Quoter, false},
        };

        [TestCaseSource(nameof(EventLayerOpacityTestCaseSource))]
        public static void EventLayerOpacityTest(EventLayerOpacityType type, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.EventLayerOpacity = type;
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
                var setValue = instance.EventLayerOpacity;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(type));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.EventLayerOpacity)));
            }
        }

        private static readonly object[] CommandColorTypeTestCaseSource =
        {
            new object[] {null, true},
            new object[] {CommandColorType.Type0, false},
        };

        [TestCaseSource(nameof(CommandColorTypeTestCaseSource))]
        public static void CommandColorTypeTest(CommandColorType type, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CommandColorType = type;
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
                var setValue = instance.CommandColorType;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(type));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.CommandColorType)));
            }
        }

        [TestCase(true)]
        public static void IsDrawBackgroundImageTest(bool isDraw)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.IsDrawBackgroundImage = isDraw;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsDrawBackgroundImage;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isDraw));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.IsDrawBackgroundImage)));
        }

        private static readonly object[] NotCopyExtListTestCaseSource =
        {
            new object[] {null, true},
            new object[] {new ExtensionList(new Extension[] {".txt", ".svg"}), false},
        };

        [TestCaseSource(nameof(NotCopyExtListTestCaseSource))]
        public static void NotCopyExtListTest(ExtensionList list, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.NotCopyExtList = list;
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
                var setValue = instance.NotCopyExtList;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(list));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.NotCopyExtList)));
            }
        }

        private static readonly object[] CommandViewTypeTestCaseSource =
        {
            new object[] {(CommandViewType) 0},
        };

        [TestCaseSource(nameof(CommandViewTypeTestCaseSource))]
        public static void CommandViewTypeTest(CommandViewType type)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CommandViewType = type;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.CommandViewType;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(type));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.CommandViewType)));
        }

        private static readonly object[] BackupTypeTestCaseSource =
        {
            new object[] {null, true},
            new object[] {ProjectBackupType.None, false},
        };

        [TestCaseSource(nameof(BackupTypeTestCaseSource))]
        public static void BackupTypeTest(ProjectBackupType type, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.BackupType = type;
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
                var setValue = instance.BackupType;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(type));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.BackupType)));
            }
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void ShortCutKeyListTest(bool isNull, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var item = isNull ? null : new EventCommandShortCutKeyList();

            var errorOccured = false;
            try
            {
                instance.ShortCutKeyList = item;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            Assert.NotNull(item);

            var setValue = instance.ShortCutKeyList;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(item));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.ShortCutKeyList)));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void CommandPositionListTest(bool isNull, bool isError)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var item = isNull ? null : new ShortCutPositionList();

            var errorOccured = false;
            try
            {
                instance.CommandPositionList = item;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            Assert.NotNull(item);

            var setValue = instance.CommandPositionList;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(item));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.CommandPositionList)));
        }

        [TestCase(true)]
        public static void IsUseExpertCommandTest(bool isUse)
        {
            var instance = new EditorIniData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.IsUseExpertCommand = isUse;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsUseExpertCommand;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isUse));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(EditorIniData.IsUseExpertCommand)));
        }

        [TestCase("Success", true)]
        [TestCase("DuplicateShortCutKeyList", false)]
        [TestCase("SetValueNotUseShortCutKeyList", false)]
        public static void ValidateTest(string testItemCode, bool answer)
        {
            var instance = new EditorIniData();
            switch (testItemCode)
            {
                case "Success":
                    // 初期状態 = 正常な状態
                    break;

                case "DuplicateShortCutKeyList":
                    instance.ShortCutKeyList[0] = EventCommandShortCutKey.A;
                    instance.ShortCutKeyList[1] = EventCommandShortCutKey.A;
                    break;

                case "SetValueNotUseShortCutKeyList":
                    instance.ShortCutKeyList[19] = EventCommandShortCutKey.A;
                    break;

                default:
                    Assert.Fail();
                    break;
            }

            var result = instance.Validate(out var errorMsg);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answer);

            // チェックOKの場合は以降のテスト不要
            if (result) return;

            // エラーメッセージが格納されていること
            Assert.IsNotEmpty(errorMsg);

            logger.Debug(errorMsg);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new EditorIniData
            {
                BackupType = ProjectBackupType.FiveTimes,
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}