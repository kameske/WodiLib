using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageGraphicInfoTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(false, 0, true)]
        [TestCase(true, 0, false)]
        [TestCase(false, 9999, true)]
        [TestCase(true, 9999, false)]
        public static void GraphicTileIdSetTest(bool isGraphicTileChip, int tileId, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = isGraphicTileChip;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var graphicTileId = (MapEventTileId) tileId;
            var errorOccured = false;
            try
            {
                instance.GraphicTileId = graphicTileId;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageGraphicInfo.GraphicTileId)));
            }
        }

        [TestCase(false, "abc", false)]
        [TestCase(true, "abc", true)]
        public static void CharaChipFileNameSetTest(bool isGraphicTileChip, string fileName, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = isGraphicTileChip;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            var charaChipName = (CharaChipFilePath) fileName;
            try
            {
                instance.CharaChipFilePath = charaChipName;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageGraphicInfo.CharaChipFilePath)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void CharaChipDrawTypeTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CharaChipDrawType = isNull ? null : PictureDrawType.Add;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageGraphicInfo.CharaChipDrawType)));
            }
        }

        [TestCase(-1, false, false)]
        [TestCase(-1, true, false)]
        [TestCase(0, false, true)]
        [TestCase(0, true, true)]
        public static void SetGraphicTileIdTest(int tileId, bool beforeIsGraphicTileChip, bool afterIsGraphicTileChip)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = beforeIsGraphicTileChip;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var mapEventTileId = (MapEventTileId) tileId;
            var errorOccured = false;
            try
            {
                instance.SetGraphicTileId(mapEventTileId);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // タイルチップ使用フラグが一致すること
            Assert.AreEqual(instance.IsGraphicTileChip, afterIsGraphicTileChip);

            // 意図したとおりプロパティ変更通知が発火していること
            if (mapEventTileId == MapEventTileId.NotUse)
            {
                Assert.AreEqual(changedPropertyList.Count, 3);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageGraphicInfo.GraphicTileId)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageGraphicInfo.IsGraphicTileChip)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(MapEventPageGraphicInfo.CharaChipFilePath)));
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageGraphicInfo.GraphicTileId)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageGraphicInfo.IsGraphicTileChip)));
            }
        }

        [TestCase("CharaChip/Hero.png", false, false)]
        [TestCase("CharaChip/Hero.png", true, false)]
        public static void SetGraphicFileNameTest(string fileName, bool beforeIsGraphicTileChip, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = beforeIsGraphicTileChip;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var charaChipFileName = (CharaChipFilePath) fileName;
            var errorOccured = false;
            try
            {
                instance.SetGraphicFileName(charaChipFileName);
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
                // タイルチップ使用フラグがfalseであること
                Assert.IsFalse(instance.IsGraphicTileChip);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageGraphicInfo.IsGraphicTileChip)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageGraphicInfo.CharaChipFilePath)));
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapEventPageGraphicInfo
            {
                InitDirection = CharaChipDirection.LeftDown
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