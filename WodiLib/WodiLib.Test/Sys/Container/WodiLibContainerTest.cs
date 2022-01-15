using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class WodiLibContainerTest
    {
        private static readonly WodiLibContainerKeyName TestContainerName_CommonTest = "__wodilib_test_container__";

        private static readonly WodiLibContainerKeyName TestContainerName_HasCreateMethodTest =
            "__wodilib_test_container--has_create_method_test_";

        private static readonly WodiLibContainerKeyName TestContainerName_ChangeTestA = "__wodilib_test_container-A_";
        private static readonly WodiLibContainerKeyName TestContainerName_ChangeTestB = "__wodilib_test_container-B_";

        private static readonly WodiLibContainerKeyName TestContainerName_AddContainerIfHasMethodTest_HasKey =
            "__wodilib_test_container--add_container_if_has_test-has_key_";

        private static readonly WodiLibContainerKeyName TestContainerName_AddContainerIfHasMethodTest_NotHasKey =
            "__wodilib_test_container--add_container_if_has_test-not_has_key_";

        private static readonly WodiLibContainerKeyName TestContainerName_RegisterMethodTest_NoInitParam_NewContainer =
            "__wodilib_test_container--register_test-no_init_param-new_container_";

        private static readonly WodiLibContainerKeyName
            TestContainerName_RegisterMethodTest_NoInitParam_ExistingContainer =
                "__wodilib_test_container--register_test-no_init_param-existing_container_";

        private static readonly WodiLibContainerKeyName TestContainerName_RegisterMethodTest_HasInitParam_NewContainer =
            "__wodilib_test_container--register_test-Has_init_param-new_container_";

        private static readonly WodiLibContainerKeyName
            TestContainerName_RegisterMethodTest_HasInitParam_ExistingContainer =
                "__wodilib_test_container--register_test-Has_init_param-existing_container_";

        private static readonly WodiLibContainerKeyName TestContainerName_RegisterIfNotHasMethodTest =
            "__wodilib_test_container--register_if_not_has_test_";

        private static readonly WodiLibContainerKeyName TestContainerName_RemoveContainerIfHasMethodTest_HasKey =
            "__wodilib_test_container--remove_container_if_has_test-has_key_";

        private static readonly WodiLibContainerKeyName TestContainerName_RemoveContainerIfHasMethodTest_NotHasKey =
            "__wodilib_test_container--remove_container_if_has_test-not_has_key_";

        private static readonly WodiLibContainerKeyName TestContainerName_UnregisterMethodTest =
            "__wodilib_test_container--unregister_if_not_has_test_";

        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            // テスト用メソッドをコンテナに登録
            WodiLibContainer.Register<IContainerCreatableNoParam>(
                () => new ContainerCreatableNoParam(),
                WodiLibContainer.Lifetime.Transient,
                TestContainerName_HasCreateMethodTest
            );
            WodiLibContainer.Register<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                param => new ContainerCreatableHasParam(param),
                WodiLibContainer.Lifetime.Transient,
                TestContainerName_HasCreateMethodTest
            );
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void HasContainerTest_Validate(bool keyIsNull, bool isError)
        {
            var key = keyIsNull
                ? null
                : TestContainerName_CommonTest;

            var errorOccured = false;
            try
            {
                WodiLibContainer.HasContainer(key!);
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
        public static void HasContainerTest()
        {
            /*
             * RemoveContainerIfHasTest で使用する機能のため、
             * そちらで緑になれば良しとする。
             */
        }

        [TestCase(nameof(IContainerCreatableNoParam), true)]
        [TestCase(nameof(IContainerCreatableNoImpl), false)]
        public static void HasCreateMethodTest_NoInitParam(string testIfName, bool actual)
        {
            var expected = false;
            try
            {
                expected = testIfName switch
                {
                    nameof(IContainerCreatableNoParam) =>
                        WodiLibContainer.HasCreateMethod<IContainerCreatableNoParam>(
                            TestContainerName_HasCreateMethodTest
                        ),
                    nameof(IContainerCreatableNoImpl) => WodiLibContainer.HasCreateMethod<IContainerCreatableNoImpl>(
                        TestContainerName_HasCreateMethodTest
                    ),
                    _ => throw new ArgumentOutOfRangeException(nameof(testIfName), testIfName, null)
                };
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 結果が一致すること
            Assert.AreEqual(expected, actual);
        }

        [TestCase(nameof(IContainerCreatableHasParam), true)]
        [TestCase(nameof(IContainerCreatableHasParamNoImpl), false)]
        public static void HasCreateMethodTest_HasInitParam(string testIfName, bool actual)
        {
            var expected = false;
            try
            {
                expected = testIfName switch
                {
                    nameof(IContainerCreatableHasParam) =>
                        WodiLibContainer.HasCreateMethod<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                            TestContainerName_HasCreateMethodTest
                        ),
                    nameof(IContainerCreatableHasParamNoImpl) => WodiLibContainer
                        .HasCreateMethod<IContainerCreatableHasParamNoImpl, IContainerCreatableInitParam>(
                            TestContainerName_HasCreateMethodTest
                        ),
                    _ => throw new ArgumentOutOfRangeException(nameof(testIfName), testIfName, null)
                };
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 結果が一致すること
            Assert.AreEqual(expected, actual);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void ChangeTargetKeyTest_Validate(bool keyIsNull, bool isError)
        {
            var key = keyIsNull
                ? null
                : TestContainerName_CommonTest;

            var errorOccured = false;
            try
            {
                WodiLibContainer.ChangeTargetKey(key!);
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
        public static void ChangeTargetKeyTest()
        {
            // デフォルトキー にテスト用の情報を登録する
            WodiLibContainer.Register<IContainerCreatableCustom>(
                () => new ContainerCreatableCustom1(),
                WodiLibContainer.Lifetime.Transient,
                WodiLibContainer.DefaultKeyName
            );

            // ターゲットキー切り替え
            WodiLibContainer.ChangeTargetKey(TestContainerName_ChangeTestA);

            // IContainerCreatableCustom 生成情報が引き継がれていること
            {
                // 前提として、デフォルトキーに情報が登録されていること（登録した情報が消えていないこと）
                Assert.IsTrue(
                    WodiLibContainer.HasCreateMethod<IContainerCreatableCustom>(WodiLibContainer.DefaultKeyName)
                );
                // 前提として、現在のターゲットキー名が切り替えたキー名と一致すること
                Assert.IsTrue(((string)WodiLibContainer.TargetKeyName).Equals((string)TestContainerName_ChangeTestA));

                // キー情報が存在すること
                Assert.IsTrue(
                    WodiLibContainer.HasCreateMethod<IContainerCreatableCustom>(TestContainerName_ChangeTestA)
                );

                // ライフタイムが Transient であること（2つのインスタンスを生成して同一参照ではないことを確認）
                var instanceA = WodiLibContainer.Resolve<IContainerCreatableCustom>();
                var instanceB = WodiLibContainer.Resolve<IContainerCreatableCustom>();
                Assert.IsFalse(ReferenceEquals(instanceA, instanceB));
            }

            // デフォルトキーの情報を更新
            {
                WodiLibContainer.Register<IContainerCreatableCustom>(
                    () => new ContainerCreatableCustom2(),
                    WodiLibContainer.Lifetime.Container,
                    WodiLibContainer.DefaultKeyName
                );

                // TestContainerName_ChangeTestA の登録情報が変化していないこと（2つのインスタンスを生成して同一参照ではないことを確認）
                var instanceA = WodiLibContainer.Resolve<IContainerCreatableCustom>();
                var instanceB = WodiLibContainer.Resolve<IContainerCreatableCustom>();
                Assert.IsFalse(ReferenceEquals(instanceA, instanceB));

                // デフォルトキー の登録情報が変化していること（2つのインスタンスを生成して同一参照であること、先に生成したインスタンスとは別モノであることを確認）
                var instanceC = WodiLibContainer.Resolve<IContainerCreatableCustom>(WodiLibContainer.DefaultKeyName);
                var instanceD = WodiLibContainer.Resolve<IContainerCreatableCustom>(WodiLibContainer.DefaultKeyName);
                Assert.IsTrue(ReferenceEquals(instanceC, instanceD));
                Assert.IsFalse(ReferenceEquals(instanceA, instanceC));
                Assert.IsFalse(ReferenceEquals(instanceB, instanceC));
            }

            // ターゲットキー切り替え
            WodiLibContainer.ChangeTargetKey(TestContainerName_ChangeTestB);

            // IContainerCreatableCustom 生成情報が引き継がれていること
            {
                // 前提として、デフォルトキーに情報が登録されていること（登録した情報が消えていないこと）
                Assert.IsTrue(
                    WodiLibContainer.HasCreateMethod<IContainerCreatableCustom>(WodiLibContainer.DefaultKeyName)
                );
                // 前提として、現在のターゲットキー名が切り替えたキー名と一致すること
                Assert.IsTrue(((string)WodiLibContainer.TargetKeyName).Equals((string)TestContainerName_ChangeTestB));

                // キー情報が存在すること
                Assert.IsTrue(
                    WodiLibContainer.HasCreateMethod<IContainerCreatableCustom>(TestContainerName_ChangeTestB)
                );

                // ライフタイムが Container であること（2つのインスタンスを生成して同一参照であることを確認）
                var instanceA = WodiLibContainer.Resolve<IContainerCreatableCustom>();
                var instanceB = WodiLibContainer.Resolve<IContainerCreatableCustom>();
                Assert.IsTrue(ReferenceEquals(instanceA, instanceB));

                // デフォルトキーのインスタンスと同一参照でないこと
                //   ※ デフォルトキーのライフタイムも Container
                var instanceC = WodiLibContainer.Resolve<IContainerCreatableCustom>(WodiLibContainer.DefaultKeyName);
                Assert.IsFalse(ReferenceEquals(instanceA, instanceC));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void AddContainerIfNotHasTest_Validate(bool keyIsNull, bool isError)
        {
            var key = keyIsNull
                ? null
                : TestContainerName_CommonTest;

            var errorOccured = false;
            try
            {
                WodiLibContainer.AddContainerIfNotHas(key!);
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
        public static void AddContainerIfNotHasTest_HasKey()
        {
            var testKey = TestContainerName_AddContainerIfHasMethodTest_HasKey;

            // 事前処理：コンテナ作成
            WodiLibContainer.AddContainerIfNotHas(testKey);

            // 前提条件：コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));

            // コンテナ作成
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.AddContainerIfNotHas(testKey);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));
        }

        [Test]
        public static void AddContainerIfNotHasTest_NotHasKey()
        {
            var testKey = TestContainerName_AddContainerIfHasMethodTest_NotHasKey;

            // 前提条件：コンテナが存在しないこと
            Assert.IsFalse(WodiLibContainer.HasContainer(testKey));

            // コンテナ作成
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.AddContainerIfNotHas(testKey);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));
        }

        [Test]
        public static void RegisterTest_NoInitParam_NewContainer()
        {
            var testKey = TestContainerName_RegisterMethodTest_NoInitParam_NewContainer;

            // 前提条件：コンテナが存在しないこと
            Assert.IsFalse(WodiLibContainer.HasContainer(testKey));

            // 情報登録
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Register<IContainerCreatableNoParam>(
                        () => new ContainerCreatableNoParam(),
                        WodiLibContainer.Lifetime.Container,
                        testKey
                    );
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));

            // 情報が登録されていること & ライフタイムが Container あること ＝ 新規取得したインスタンスが既存インスタンスと同一参照であること
            var instanceA = WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            var instanceB = WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            Assert.IsTrue(ReferenceEquals(instanceA, instanceB));
        }

        [Test]
        public static void RegisterTest_NoInitParam_ExistingContainer()
        {
            var testKey = TestContainerName_RegisterMethodTest_NoInitParam_ExistingContainer;

            // 事前準備：テスト用のコンテナ作成 & 上書き前情報登録
            WodiLibContainer.AddContainerIfNotHas(testKey);
            WodiLibContainer.Register<IContainerCreatableNoParam>(
                () => new ContainerCreatableNoParam(),
                WodiLibContainer.Lifetime.Container,
                testKey
            );

            // 前提条件：コンテナが存在すること & ライフタイムが Container あること ＝ 新規取得したインスタンスが既存インスタンスと同一参照であること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));
            var instanceA = WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            var instanceB = WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            Assert.IsTrue(ReferenceEquals(instanceA, instanceB));

            // 情報登録
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Register<IContainerCreatableNoParam>(
                        () => new ContainerCreatableNoParam(),
                        WodiLibContainer.Lifetime.Transient,
                        testKey
                    );
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));
            // 情報が更新されていること（ライフタイムが Transient であること ＝ 新規取得したインスタンスが既存インスタンスと同一参照ではないこと ）
            Assert.IsTrue(WodiLibContainer.HasCreateMethod<IContainerCreatableNoParam>(testKey));
            var instanceC = WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            var instanceD = WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            Assert.IsFalse(ReferenceEquals(instanceA, instanceC));
            Assert.IsFalse(ReferenceEquals(instanceA, instanceD));
            Assert.IsFalse(ReferenceEquals(instanceC, instanceD));
        }

        [Test]
        public static void RegisterTest_HasInitParam_NewContainer()
        {
            var testKey = TestContainerName_RegisterMethodTest_HasInitParam_NewContainer;

            // 前提条件：コンテナが存在しないこと
            Assert.IsFalse(WodiLibContainer.HasContainer(testKey));

            // 情報登録
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Register<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                        p => new ContainerCreatableHasParam(p),
                        WodiLibContainer.Lifetime.Container,
                        testKey
                    );
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));

            // 情報が登録されていること & ライフタイムが Container あること ＝ 新規取得したインスタンスが既存インスタンスと同一参照であること
            var instanceA =
                WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                    new ContainerCreatableHasParam.InitParam(),
                    testKey
                );
            var instanceB =
                WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                    new ContainerCreatableHasParam.InitParam(),
                    testKey
                );
            Assert.IsTrue(ReferenceEquals(instanceA, instanceB));
        }

        [Test]
        public static void RegisterTest_HasInitParam_ExistingContainer()
        {
            var testKey = TestContainerName_RegisterMethodTest_HasInitParam_ExistingContainer;

            // 事前準備：テスト用のコンテナ作成 & 上書き前情報登録
            WodiLibContainer.AddContainerIfNotHas(testKey);
            WodiLibContainer.Register<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                p => new ContainerCreatableHasParam(p),
                WodiLibContainer.Lifetime.Container,
                testKey
            );

            // 前提条件：コンテナが存在すること & ライフタイムが Container あること ＝ 新規取得したインスタンスが既存インスタンスと同一参照であること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));
            var instanceA =
                WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                    new ContainerCreatableHasParam.InitParam(),
                    testKey
                );
            var instanceB =
                WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                    new ContainerCreatableHasParam.InitParam(),
                    testKey
                );
            Assert.IsTrue(ReferenceEquals(instanceA, instanceB));

            // 情報登録
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Register<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                        p => new ContainerCreatableHasParam(p),
                        WodiLibContainer.Lifetime.Transient,
                        testKey
                    );
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));
            // 情報が更新されていること（ライフタイムが Transient であること ＝ 新規取得したインスタンスが既存インスタンスと同一参照ではないこと ）
            Assert.IsTrue(
                WodiLibContainer.HasCreateMethod<IContainerCreatableHasParam, IContainerCreatableInitParam>(testKey)
            );
            var instanceC =
                WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                    new ContainerCreatableHasParam.InitParam(),
                    testKey
                );
            var instanceD =
                WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                    new ContainerCreatableHasParam.InitParam(),
                    testKey
                );
            Assert.IsFalse(ReferenceEquals(instanceA, instanceC));
            Assert.IsFalse(ReferenceEquals(instanceA, instanceD));
            Assert.IsFalse(ReferenceEquals(instanceC, instanceD));
        }

        [Test]
        public static void RegisterIfNotHasTest_NoInitParam()
        {
            var testKey = TestContainerName_RegisterIfNotHasMethodTest;

            // 前提条件：未登録であること
            WodiLibContainer.RemoveContainerIfHas(testKey);
            Assert.IsFalse(WodiLibContainer.HasCreateMethod<IContainerCreatableNoParam>(testKey));

            // 初回
            WodiLibContainer.Register<IContainerCreatableNoParam>(
                () => new ContainerCreatableNoParam(),
                WodiLibContainer.Lifetime.Container,
                testKey
            );

            // ライフタイムが Container であること（2つのインスタンスを生成して同一参照であることを確認）
            var instanceA =
                WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            var instanceB =
                WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            Assert.IsTrue(ReferenceEquals(instanceA, instanceB));

            // 二回目
            WodiLibContainer.RegisterIfNotHas<IContainerCreatableNoParam>(
                () => new ContainerCreatableNoParam(),
                WodiLibContainer.Lifetime.Transient,
                testKey
            );

            // 上書きされていないこと（ライフタイムが Container のままであること ＝ 新規取得したインスタンスが既存インスタンスと同一参照であること ）
            var instanceC =
                WodiLibContainer.Resolve<IContainerCreatableNoParam>(testKey);
            Assert.IsTrue(ReferenceEquals(instanceA, instanceC));
        }

        [Test]
        public static void RegisterIfNotHasTest_HasInitParam()
        {
            var testKey = TestContainerName_RegisterIfNotHasMethodTest;

            // 前提条件：未登録であること
            Assert.False(
                WodiLibContainer.HasCreateMethod<IContainerCreatableHasParam, IContainerCreatableInitParam>(testKey)
            );

            // 初回
            WodiLibContainer.Register<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                p => new ContainerCreatableHasParam(p),
                WodiLibContainer.Lifetime.Container,
                testKey
            );

            // ライフタイムが Container であること（2つのインスタンスを生成して同一参照であることを確認）
            var instanceA = WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                new ContainerCreatableHasParam.InitParam(),
                testKey
            );
            var instanceB = WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                new ContainerCreatableHasParam.InitParam(),
                testKey
            );
            Assert.IsTrue(ReferenceEquals(instanceA, instanceB));

            // 二回目
            WodiLibContainer.RegisterIfNotHas<IContainerCreatableNoParam>(
                () => new ContainerCreatableNoParam(),
                WodiLibContainer.Lifetime.Transient,
                testKey
            );

            // 上書きされていないこと（ライフタイムが Container のままであること ＝ 新規取得したインスタンスが既存インスタンスと同一参照であること ）
            var instanceC =
                WodiLibContainer.Resolve<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                    new ContainerCreatableHasParam.InitParam(),
                    testKey
                );
            Assert.IsTrue(ReferenceEquals(instanceA, instanceC));
        }

        [Test]
        public static void ResolveTest_NoInitParam()
        {
            /*
             * 他の複数テストで使用する機能のため、
             * 他のテストが緑になれば良しとする。
             */
        }

        [Test]
        public static void ResolveTest_HasInitParam()
        {
            /*
             * 他の複数テストで使用する機能のため、
             * 他のテストが緑になれば良しとする。
             */
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void RemoveContainerIfHasTest_Validate(bool keyIsNull, bool isError)
        {
            var key = keyIsNull
                ? null
                : TestContainerName_CommonTest;

            var errorOccured = false;
            try
            {
                WodiLibContainer.RemoveContainerIfHas(key!);
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
        public static void RemoveContainerIfHasTest_HasKey()
        {
            var testKey = TestContainerName_RemoveContainerIfHasMethodTest_HasKey;

            // 事前処理：コンテナ作成
            WodiLibContainer.AddContainerIfNotHas(testKey);

            // 前提条件：コンテナが存在すること
            Assert.IsTrue(WodiLibContainer.HasContainer(testKey));

            // コンテナ除去
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.RemoveContainerIfHas(testKey);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在しないこと
            Assert.IsFalse(WodiLibContainer.HasContainer(testKey));
        }

        [Test]
        public static void RemoveContainerIfHasTest_NotHasKey()
        {
            var testKey = TestContainerName_RemoveContainerIfHasMethodTest_NotHasKey;

            // 前提条件：コンテナが存在しないこと
            Assert.IsFalse(WodiLibContainer.HasContainer(testKey));

            // コンテナ除去
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.RemoveContainerIfHas(testKey);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // コンテナが存在しないこと
            Assert.IsFalse(WodiLibContainer.HasContainer(testKey));
        }

        [Test]
        public static void UnregisterTest_HasInfo_NoInitParam()
        {
            var testKey = TestContainerName_UnregisterMethodTest;

            // 事前準備：生成情報登録
            WodiLibContainer.Register<IContainerCreatableNoParam>(
                () => new ContainerCreatableNoParam(),
                WodiLibContainer.Lifetime.Transient,
                testKey
            );

            // 前提条件：情報が存在すること
            Assert.IsTrue(WodiLibContainer.HasCreateMethod<IContainerCreatableNoParam>(testKey));

            // 情報除去
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Unregister<IContainerCreatableNoParam>(testKey);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // 情報が存在しないこと
            Assert.IsFalse(WodiLibContainer.HasCreateMethod<IContainerCreatableNoParam>(testKey));
        }

        [Test]
        public static void UnregisterTest_NotHasInfo_NoInitParam()
        {
            var testKey = TestContainerName_UnregisterMethodTest;

            // 前提条件：情報が存在しないこと
            Assert.IsFalse(WodiLibContainer.HasCreateMethod<IContainerCreatableNoImpl>(testKey));

            // 情報除去
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Unregister<IContainerCreatableNoImpl>(testKey);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // 情報が存在しないこと
            Assert.IsFalse(WodiLibContainer.HasCreateMethod<IContainerCreatableNoImpl>(testKey));
        }

        [Test]
        public static void UnregisterTest_HasInfo_HasInitParam()
        {
            var testKey = TestContainerName_UnregisterMethodTest;

            // 事前準備：生成情報登録
            WodiLibContainer.Register<IContainerCreatableHasParam, IContainerCreatableInitParam>(
                p => new ContainerCreatableHasParam(p),
                WodiLibContainer.Lifetime.Transient,
                testKey
            );

            // 前提条件：情報が存在すること
            Assert.IsTrue(
                WodiLibContainer.HasCreateMethod<IContainerCreatableHasParam, IContainerCreatableInitParam>(testKey)
            );

            // 情報除去
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Unregister<IContainerCreatableHasParam, IContainerCreatableInitParam>(testKey);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // 情報が存在しないこと
            Assert.IsFalse(
                WodiLibContainer.HasCreateMethod<IContainerCreatableHasParam, IContainerCreatableInitParam>(testKey)
            );
        }

        [Test]
        public static void UnregisterTest_NotHasInfo_HasInitParam()
        {
            var testKey = TestContainerName_UnregisterMethodTest;

            // 前提条件：情報が存在しないこと
            Assert.IsFalse(
                WodiLibContainer
                    .HasCreateMethod<IContainerCreatableHasParamNoImpl, IContainerCreatableInitParam>(testKey)
            );

            // 情報除去
            {
                var errorOccured = false;
                try
                {
                    WodiLibContainer.Unregister<IContainerCreatableHasParamNoImpl, IContainerCreatableInitParam>(
                        testKey
                    );
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            // 情報が存在しないこと
            Assert.IsFalse(
                WodiLibContainer
                    .HasCreateMethod<IContainerCreatableHasParamNoImpl, IContainerCreatableInitParam>(testKey)
            );
        }

        #region テスト用クラス

        /// <summary>
        ///     テスト用インタフェース。コンストラクタ引数なし。
        /// </summary>
        private interface IContainerCreatableNoParam : IContainerCreatable
        {
            public string MyName { get; }
        }

        private class ContainerCreatableNoParam : IContainerCreatableNoParam
        {
            public string MyName { get; }

            public ContainerCreatableNoParam()
            {
                MyName = nameof(ContainerCreatableNoParam);
            }
        }

        /// <summary>
        ///     テスト用インタフェース。コンストラクタ引数あり。
        /// </summary>
        public interface IContainerCreatableHasParam : IContainerCreatable<IContainerCreatableInitParam>
        {
            public IContainerCreatableInitParam InitializedParam { get; init; }
        }

        public interface IContainerCreatableInitParam : IContainerCreatableParam
        {
            public int IntValue { get; init; }
            public int StringValue { get; init; }
        }

        public class ContainerCreatableHasParam : IContainerCreatableHasParam
        {
            public IContainerCreatableInitParam InitializedParam { get; init; }

            public ContainerCreatableHasParam(IContainerCreatableInitParam initParam)
            {
                InitializedParam = initParam;
            }

            public class InitParam : IContainerCreatableInitParam
            {
                public int IntValue { get; init; }
                public int StringValue { get; init; }
                public WodiLibContainerKeyName ContainerKeyName { get; init; }
                    = WodiLibContainer.TargetKeyName;
            }
        }

        /// <summary>
        ///     デフォルトキー切り替えテスト用インタフェース。
        /// </summary>
        public interface IContainerCreatableCustom : IContainerCreatable
        {
            public string Name { get; }
        }

        public class ContainerCreatableCustom1 : IContainerCreatableCustom
        {
            public string Name => nameof(ContainerCreatableCustom1);
        }

        public class ContainerCreatableCustom2 : IContainerCreatableCustom
        {
            public string Name => nameof(ContainerCreatableCustom2);
        }

        /// <summary>
        ///     <see cref="WodiLibContainer.HasCreateMethod{TOut}"/> テスト用インタフェース。
        ///     実装は登録しない。
        /// </summary>
        public interface IContainerCreatableNoImpl : IContainerCreatable
        {
        }

        /// <summary>
        ///     <see cref="WodiLibContainer.HasCreateMethod{TOut, TParam}"/> テスト用インタフェース
        ///     実装は登録しない。
        /// </summary>
        public interface IContainerCreatableHasParamNoImpl : IContainerCreatable<IContainerCreatableInitParam>
        {
        }

        #endregion
    }
}
