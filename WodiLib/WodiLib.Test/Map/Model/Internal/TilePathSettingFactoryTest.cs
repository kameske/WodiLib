using System;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TilePathSettingFactoryTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(0x00, false)] // 通行可
        [TestCase(0x0F, false)] // 通行不可
        [TestCase(0x20, true)] // 部分的通行不可（不可方向未設定）
        [TestCase(0x22, false)] // 部分的通行不可（左下）
        [TestCase(0x02_00, false)] // 下レイヤー依存
        [TestCase(0x90, false)] // キャラより上 & カウンター
        [TestCase(0x02_40, false)] // 下レイヤー依存 & 下半身透明
        [TestCase(0x08_00, false)] // 該当なし（通行可）
        public static void CreateTestA(int code, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = TilePathSettingFactory.Create(code);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        // 引数にテストコード値指定＝ITilePathSetting が internal のため引数に指定できないことの対処
        [TestCase(0, "null, null, true")]
        [TestCase(1, "Allow, null, true")]
        [TestCase(2, "null, Allow, true")]
        [TestCase(3, "Allow, Allow, false")]
        [TestCase(4, "Allow, Deny, false")]
        [TestCase(5, "Allow, PartialDeny, false")]
        [TestCase(6, "Deny, Dependent, false")]
        [TestCase(7, "Deny, Deny, false")]
        [TestCase(8, "Deny, PartialDeny, false")]
        [TestCase(6, "PartialDeny, Dependent, false")]
        [TestCase(7, "PartialDeny, Deny, false")]
        [TestCase(8, "PartialDeny, PartialDeny, false")]
        public static void CreateTest2(int testCode, string description)
        {
            var (pathPermission, src, isError, resultType, resultFlagsCode) = MakeCreateTest2Args(testCode);

            ITilePathSetting result = null;

            var errorOccured = false;
            try
            {
                result = TilePathSettingFactory.Create(pathPermission, src);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 結果が意図した値であること
            Assert.AreEqual(result.GetType(), resultType);

            if (result.GetType() != typeof(TilePathSettingPartialDeny) &&
                result.GetType() != typeof(TilePathSettingDeny))
            {
                // CannotPassingFlags が意図した値であること
                Assert.AreEqual(result.CannotPassingFlags.ToCode(), resultFlagsCode);
            }

            if (result.GetType() == typeof(TilePathSettingPartialDeny))
            {
                // ImpassableFlags が意図した値であること
                Assert.AreEqual(result.ImpassableFlags.ToCode(), resultFlagsCode);
            }
        }

        private static (TilePathPermission pathPermission, ITilePathSetting src, bool isError,
            Type resultType, int resultFlagsCode)
            MakeCreateTest2Args(int testCode)
        {
            switch (testCode)
            {
                case 0:
                    return (null, null, true, null, 0);
                case 1:
                    return (TilePathPermission.Allow, null, true, null, 0);
                case 2:
                    return (null, new TilePathSettingAllow(), true, null, 0);
                case 3:
                    return (TilePathPermission.Allow, new TilePathSettingAllow(0x03), false,
                        typeof(TilePathSettingAllow), 0x03);
                case 4:
                    return (TilePathPermission.Deny, new TilePathSettingAllow(0x03), false, typeof(TilePathSettingDeny),
                        0x0F);
                case 5:
                    return (TilePathPermission.PartialDeny, new TilePathSettingAllow(0x03), false,
                        typeof(TilePathSettingPartialDeny), 0x0F);
                case 6:
                    return (TilePathPermission.Dependent, new TilePathSettingDeny(), false,
                        typeof(TilePathSettingDependent), 0x00);
                case 7:
                    return (TilePathPermission.Deny, new TilePathSettingDeny(), false, typeof(TilePathSettingDeny),
                        0x0F);
                case 8:
                    return (TilePathPermission.PartialDeny, new TilePathSettingDeny(), false,
                        typeof(TilePathSettingPartialDeny), 0x0F);
                case 9:
                    return (TilePathPermission.Dependent, new TilePathSettingPartialDeny(0x03), false,
                        typeof(TilePathSettingDependent), 0x00);
                case 10:
                    return (TilePathPermission.Deny, new TilePathSettingPartialDeny(0x03), false,
                        typeof(TilePathSettingDeny), 0x0F);
                case 11:
                    return (TilePathPermission.PartialDeny, new TilePathSettingPartialDeny(0x03), false,
                        typeof(TilePathSettingPartialDeny), 0x03);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}