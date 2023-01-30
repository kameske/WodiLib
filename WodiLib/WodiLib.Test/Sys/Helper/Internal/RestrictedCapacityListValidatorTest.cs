using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class RestrictedCapacityListValidatorTest
    {
        private static Logger logger = default!;

        private static class TestInstanceConfig
        {
            public const int InitLength = 5;
            public const int MaxCapacity = 10;
            public const int MinCapacity = 1;
        }

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ConstructorTest_1TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // target
                new[]
                {
                    new RestrictedCapacityListForRestrictedCapacityListValidatorTest(),
                    null
                }
            ),
            // expectedError
            target => target is null
        );

        [TestCaseSource(nameof(ConstructorTest_1TestCaseSource))]
        public static void ConstructorTest_1(IRestrictedCapacityList<string> target, bool isError)
        {
            TestTemplate.Constructor(
                () => new RestrictedCapacityListValidator<string>(target),
                isError,
                logger
            );
        }

        private static readonly object[] ConstructorTest_2TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // target
                new[]
                {
                    null,
                    (TestInstanceConfig.MinCapacity - 1).Iterate(i => i.ToString()),
                    TestInstanceConfig.MinCapacity.Iterate(i => i.ToString()),
                    TestInstanceConfig.MaxCapacity.Iterate(i => i.ToString()),
                    (TestInstanceConfig.MaxCapacity + 1).Iterate(i => i.ToString())
                },
                // minCapacity
                new[]
                {
                    TestInstanceConfig.MinCapacity,
                    TestInstanceConfig.MaxCapacity - 1,
                    TestInstanceConfig.MaxCapacity
                },
                // maxCapacity
                new[]
                {
                    TestInstanceConfig.MinCapacity,
                    TestInstanceConfig.MaxCapacity
                }
            ),
            // expectedError
            (target, minCapacity, maxCapacity) => target is null
                                                  || minCapacity > maxCapacity
        );

        [TestCaseSource(nameof(ConstructorTest_2TestCaseSource))]
        public static void ConstructorTest_2(IEnumerable target, int minCapacity, int maxCapacity, bool isError)
        {
            TestTemplate.Constructor(
                () => new RestrictedCapacityListValidator<string>(target, minCapacity, maxCapacity),
                isError,
                logger
            );
        }

        private static readonly object[] ConstructorTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // initItems
                new[]
                {
                    new NamedValue<IEnumerable<string>?>("", null!),
                    new NamedValue<IEnumerable<string>?>(
                        "",
                        (TestInstanceConfig.MinCapacity - 1).Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>("", TestInstanceConfig.MinCapacity.Iterate(i => i.ToString())),
                    new NamedValue<IEnumerable<string>?>(
                        "",
                        TestInstanceConfig.MinCapacity.Iterate(
                            i => i % 2 == 0
                                ? i.ToString()
                                : null!
                        )
                    ),
                    new NamedValue<IEnumerable<string>?>("", TestInstanceConfig.MaxCapacity.Iterate(i => i.ToString())),
                    new NamedValue<IEnumerable<string>?>(
                        "",
                        (TestInstanceConfig.MaxCapacity + 1).Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            initItems => initItems.Value is null
                         || initItems.Value.HasNullItem()
                         || !initItems.Value.Count()
                             .IsBetween(TestInstanceConfig.MinCapacity, TestInstanceConfig.MaxCapacity)
        );

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(NamedValue<IEnumerable<string>> initItems, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Constructor(initItems),
                isError,
                logger
            );
        }

        private static readonly object[] GetTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // index
                new[]
                {
                    new NamedValue<int>("index", -1),
                    new NamedValue<int>("index", 0),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength - 1),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength)
                },
                // count
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitLength),
                    new NamedValue<int>("count", TestInstanceConfig.InitLength + 1)
                }
            ),
            // expectedError
            (index, count) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength - 1)
                              || !count.Value.IsBetween(0, TestInstanceConfig.InitLength)
                              || index.Value + count.Value > TestInstanceConfig.InitLength
        );

        [TestCaseSource(nameof(GetTestCaseSource))]
        public static void GetTest(NamedValue<int> index, NamedValue<int> count, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Get(index, count),
                isError,
                logger
            );
        }

        private static readonly object[] SetTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // index
                new[]
                {
                    new NamedValue<int>("index", -1),
                    new NamedValue<int>("index", 0),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength - 1),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength)
                },
                // items
                new[]
                {
                    new NamedValue<IEnumerable<string>?>("items", null!),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MinCapacity - 1).Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        TestInstanceConfig.MinCapacity.Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        TestInstanceConfig.MinCapacity.Iterate(
                            i => i % 2 == 0
                                ? i.ToString()
                                : null!
                        )
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        TestInstanceConfig.MaxCapacity.Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MaxCapacity + 1).Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            (index, items) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength - 1)
                              || items.Value is null
                              || items.Value.HasNullItem()
                              || index.Value + items.Value.Count() > TestInstanceConfig.InitLength
        );

        [TestCaseSource(nameof(SetTestCaseSource))]
        public static void SetTest(NamedValue<int> index, NamedValue<IEnumerable<string>> items, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Set(index, items),
                isError,
                logger
            );
        }

        private static readonly object[] InsertTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // index
                new[]
                {
                    new NamedValue<int>("index", -1),
                    new NamedValue<int>("index", 0),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength + 1)
                },
                // items
                new[]
                {
                    new NamedValue<IEnumerable<string>?>("items", null),
                    new NamedValue<IEnumerable<string>?>("items", Array.Empty<string>()),
                    new NamedValue<IEnumerable<string>?>("items", new[] { "0", null!, "2" }),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MaxCapacity - TestInstanceConfig.InitLength).Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MaxCapacity - TestInstanceConfig.InitLength + 1).Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            (index, items) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength)
                              || items.Value is null
                              || items.Value.HasNullItem()
                              || items.Value.Count() + TestInstanceConfig.InitLength > TestInstanceConfig.MaxCapacity
        );

        [TestCaseSource(nameof(InsertTestCaseSource))]
        public static void InsertTest(NamedValue<int> index, NamedValue<IEnumerable<string>> items, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Insert(index, items),
                isError,
                logger
            );
        }

        private static readonly object[] OverwriteTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // index
                new[]
                {
                    new NamedValue<int>("index", -1),
                    new NamedValue<int>("index", 0),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength + 1)
                },
                // items
                new[]
                {
                    new NamedValue<IEnumerable<string>?>("items", null),
                    new NamedValue<IEnumerable<string>?>("items", Array.Empty<string>()),
                    new NamedValue<IEnumerable<string>?>("items", new[] { "0", null!, "2" }),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MaxCapacity - TestInstanceConfig.InitLength).Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MaxCapacity - TestInstanceConfig.InitLength + 1).Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            (index, items) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength)
                              || items.Value is null
                              || items.Value.HasNullItem()
                              || items.Value.Count() + index.Value > TestInstanceConfig.MaxCapacity
        );

        [TestCaseSource(nameof(OverwriteTestCaseSource))]
        public static void OverwriteTest(NamedValue<int> index, NamedValue<IEnumerable<string>> items, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Overwrite(index, items),
                isError,
                logger
            );
        }

        private static readonly object[] MoveTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // oldIndex
                new[]
                {
                    new NamedValue<int>("oldIndex", -1),
                    new NamedValue<int>("oldIndex", 0),
                    new NamedValue<int>("oldIndex", TestInstanceConfig.InitLength - 1),
                    new NamedValue<int>("oldIndex", TestInstanceConfig.InitLength)
                },
                // newIndex
                new[]
                {
                    new NamedValue<int>("newIndex", -1),
                    new NamedValue<int>("newIndex", 0),
                    new NamedValue<int>("newIndex", TestInstanceConfig.InitLength - 1),
                    new NamedValue<int>("newIndex", TestInstanceConfig.InitLength)
                },
                // count
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", 2),
                    new NamedValue<int>("count", TestInstanceConfig.InitLength),
                    new NamedValue<int>("count", TestInstanceConfig.InitLength + 1)
                }
            ),
            // expectedError
            (oldIndex, newIndex, count) => !oldIndex.Value.IsBetween(0, TestInstanceConfig.InitLength - 1)
                                           || !newIndex.Value.IsBetween(0, TestInstanceConfig.InitLength - 1)
                                           || !count.Value.IsBetween(0, TestInstanceConfig.InitLength)
                                           || oldIndex.Value + count.Value > TestInstanceConfig.InitLength
                                           || newIndex.Value + count.Value > TestInstanceConfig.InitLength
        );

        [TestCaseSource(nameof(MoveTestCaseSource))]
        public static void MoveTest(
            NamedValue<int> oldIndex,
            NamedValue<int> newIndex,
            NamedValue<int> count,
            bool isError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Move(oldIndex, newIndex, count),
                isError,
                logger
            );
        }

        private static readonly object[] RemoveTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // index
                new[]
                {
                    new NamedValue<int>("index", -1),
                    new NamedValue<int>("index", 0),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength - 1),
                    new NamedValue<int>("index", TestInstanceConfig.InitLength)
                },
                // count
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", TestInstanceConfig.InitLength - TestInstanceConfig.MinCapacity),
                    new NamedValue<int>("count", TestInstanceConfig.InitLength - TestInstanceConfig.MinCapacity + 1)
                }
            ),
            // expectedError
            (index, count) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength - 1)
                              || !count.Value.IsBetween(0, TestInstanceConfig.InitLength)
                              || index.Value + count.Value > TestInstanceConfig.InitLength
                              || TestInstanceConfig.InitLength - count.Value < TestInstanceConfig.MinCapacity
        );

        [TestCaseSource(nameof(RemoveTestCaseSource))]
        public static void RemoveTest(NamedValue<int> index, NamedValue<int> count, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Remove(index, count),
                isError,
                logger
            );
        }

        private static readonly object[] AdjustLengthTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // length
                new[]
                {
                    new NamedValue<int>("length", TestInstanceConfig.MinCapacity - 1),
                    new NamedValue<int>("length", TestInstanceConfig.MinCapacity),
                    new NamedValue<int>("length", TestInstanceConfig.MaxCapacity),
                    new NamedValue<int>("length", TestInstanceConfig.MaxCapacity + 1)
                }
            ),
            // expectedError
            index => !index.Value.IsBetween(TestInstanceConfig.MinCapacity, TestInstanceConfig.MaxCapacity)
        );

        [TestCaseSource(nameof(AdjustLengthTestCaseSource))]
        public static void AdjustLengthTest(NamedValue<int> length, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.AdjustLength(length),
                isError,
                logger
            );
        }

        private static readonly object[] ResetTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // items
                new[]
                {
                    new NamedValue<IEnumerable<string>?>("items", null),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MinCapacity - 1).Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        TestInstanceConfig.MinCapacity.Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        TestInstanceConfig.MinCapacity.Iterate(
                            i => i % 2 == 0
                                ? i.ToString()
                                : null!
                        )
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MaxCapacity).Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.MaxCapacity + 1).Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            items => items.Value is null
                     || items.Value.HasNullItem()
                     || !items.Value.Count().IsBetween(TestInstanceConfig.MinCapacity, TestInstanceConfig.MaxCapacity)
        );

        [TestCaseSource(nameof(ResetTestCaseSource))]
        public static void ResetTest(NamedValue<IEnumerable<string>> items, bool isError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.Reset(items),
                isError,
                logger
            );
        }

        #region For TestInstance

        private static RestrictedCapacityListValidator<string> GetTestInstance()
            => TestValidatorInitializer.RestrictedCapacityListValidator;

        private static readonly RestrictedCapacityListForRestrictedCapacityListValidatorTest TestValidatorInitializer =
            new();

        internal class RestrictedCapacityListForRestrictedCapacityListValidatorTest :
            RestrictedCapacityList<string, RestrictedCapacityListForRestrictedCapacityListValidatorTest>
        {
            public RestrictedCapacityListValidator<string> RestrictedCapacityListValidator { get; private set; } =
                default!;

            public RestrictedCapacityListForRestrictedCapacityListValidatorTest() : base(
                TestInstanceConfig.InitLength.Iterate(i => i.ToString())
            )
            {
            }

            protected override string MakeDefaultItem(int index) => index.ToString();

            public override int GetMaxCapacity() => TestInstanceConfig.MaxCapacity;

            public override int GetMinCapacity() => TestInstanceConfig.MinCapacity;

            protected override IWodiLibListValidator<string> GenerateValidatorForItems()
            {
                var validator = new RestrictedCapacityListValidator<string>(this);
                RestrictedCapacityListValidator = validator;
                return validator;
            }
        }

        #endregion
    }
}
