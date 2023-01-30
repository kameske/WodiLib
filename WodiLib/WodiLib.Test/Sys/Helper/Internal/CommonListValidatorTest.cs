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
    public class CommonListValidatorTest
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
                    new RestrictedCapacityListForCommonListValidatorTest(),
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
                () => new CommonListValidator<string>(target),
                isError,
                logger
            );
        }

        private static readonly object[] ConstructorTest_2TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // target
                new[]
                {
                    new FixedLengthListForConstructorTest(),
                    null
                },
                // capacity
                new[]
                {
                    TestInstanceConfig.MinCapacity
                }
            ),
            // expectedError
            (target, _) => target is null
        );

        [TestCaseSource(nameof(ConstructorTest_2TestCaseSource))]
        public static void ConstructorTest_2(IFixedLengthList<string> target, int capacity, bool isError)
        {
            TestTemplate.Constructor(
                () => new CommonListValidator<string>(target, capacity),
                isError,
                logger
            );
        }

        private static readonly object[] ConstructorTest_3TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // target
                new[]
                {
                    new FixedLengthListForConstructorTest(),
                    null
                },
                // capacityGetter
                new[]
                {
                    new Func<int>(() => TestInstanceConfig.MinCapacity),
                    null
                }
            ),
            // expectedError
            (target, capacityGetter) => target is null || capacityGetter is null
        );

        [TestCaseSource(nameof(ConstructorTest_3TestCaseSource))]
        public static void ConstructorTest_3(IFixedLengthList<string> target, Func<int> capacityGetter, bool isError)
        {
            TestTemplate.Constructor(
                () => new CommonListValidator<string>(target, capacityGetter),
                isError,
                logger
            );
        }

        private static readonly object[] ConstructorTest_4TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // target
                new[]
                {
                    new ReadOnlyListForConstructorTest(),
                    null
                }
            ),
            // expectedError
            target => target is null
        );

        [TestCaseSource(nameof(ConstructorTest_4TestCaseSource))]
        public static void ConstructorTest_4(IReadOnlyExtendedList<string> target, bool isError)
        {
            TestTemplate.Constructor(
                () => new CommonListValidator<string>(target),
                isError,
                logger
            );
        }

        private static readonly object[] ConstructorTest_5TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // target
                new[]
                {
                    10.Iterate(i => i.ToString()),
                    null
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
            (target, minCapacity, maxCapacity) => target is null || target.HasNullItem() || minCapacity > maxCapacity
        );

        [TestCaseSource(nameof(ConstructorTest_5TestCaseSource))]
        public static void ConstructorTest_5(IEnumerable target, int minCapacity, int maxCapacity, bool isError)
        {
            TestTemplate.Constructor(
                () => new CommonListValidator<string>(target, minCapacity, maxCapacity),
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
                    new NamedValue<IEnumerable<string>?>("", Array.Empty<string>()),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", null!, "2" })
                }
            ),
            // expectedError
            initItems => initItems.Value is null || initItems.Value.HasNullItem()
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
                    new NamedValue<IEnumerable<string>?>("items", null),
                    new NamedValue<IEnumerable<string>?>("items", Array.Empty<string>()),
                    new NamedValue<IEnumerable<string>?>("items", new[] { "0", null!, "2" }),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        TestInstanceConfig.InitLength.Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.InitLength + 1).Iterate(i => i.ToString())
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
                        TestInstanceConfig.InitLength.Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            (index, items) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength)
                              || items.Value is null
                              || items.Value.HasNullItem()
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
                        TestInstanceConfig.InitLength.Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            (index, items) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength)
                              || items.Value is null
                              || items.Value.HasNullItem()
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
                    new NamedValue<int>("count", TestInstanceConfig.InitLength),
                    new NamedValue<int>("count", TestInstanceConfig.InitLength + 1)
                }
            ),
            // expectedError
            (index, count) => !index.Value.IsBetween(0, TestInstanceConfig.InitLength - 1)
                              || !count.Value.IsBetween(0, TestInstanceConfig.InitLength)
                              || index.Value + count.Value > TestInstanceConfig.InitLength
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
                    new NamedValue<int>("length", -1),
                    new NamedValue<int>("length", 0),
                    new NamedValue<int>("length", TestInstanceConfig.InitLength - 1),
                    new NamedValue<int>("length", TestInstanceConfig.InitLength),
                    new NamedValue<int>("length", TestInstanceConfig.InitLength + 2)
                }
            ),
            // expectedError
            index => index.Value < 0
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
                    new NamedValue<IEnumerable<string?>?>("items", null),
                    new NamedValue<IEnumerable<string?>?>("items", Array.Empty<string>()),
                    new NamedValue<IEnumerable<string?>?>("items", new[] { "0", null, "2" }),
                    new NamedValue<IEnumerable<string?>?>("items", 3.Iterate(i => i.ToString()))
                }
            ),
            // expectedError
            items => items.Value is null || items.Value.HasNullItem()
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

        private static CommonListValidator<string> GetTestInstance()
            => TestValidatorInitializer.CommonListValidator;

        private static readonly RestrictedCapacityListForCommonListValidatorTest TestValidatorInitializer = new();

        internal class RestrictedCapacityListForCommonListValidatorTest :
            RestrictedCapacityList<string, RestrictedCapacityListForCommonListValidatorTest>
        {
            public CommonListValidator<string> CommonListValidator { get; private set; } = default!;

            public RestrictedCapacityListForCommonListValidatorTest() : base(
                TestInstanceConfig.InitLength.Iterate(i => i.ToString())
            )
            {
            }

            protected override string MakeDefaultItem(int index) => index.ToString();

            public override int GetMaxCapacity() => TestInstanceConfig.MaxCapacity;

            public override int GetMinCapacity() => TestInstanceConfig.MinCapacity;

            protected override IWodiLibListValidator<string> GenerateValidatorForItems()
            {
                var validator = new CommonListValidator<string>(this);
                CommonListValidator = validator;
                return validator;
            }
        }

        internal class FixedLengthListForConstructorTest : FixedLengthList<string, FixedLengthListForConstructorTest>
        {
            public FixedLengthListForConstructorTest() : base(TestInstanceConfig.MinCapacity)
            {
            }

            protected override string MakeDefaultItem(int index) => index.ToString();

            protected override IWodiLibListValidator<string> GenerateValidatorForItems()
            {
                return new CommonListValidator<string>(this, TestInstanceConfig.MinCapacity);
            }
        }

        internal class ReadOnlyListForConstructorTest : ReadOnlyExtendedList<string, ReadOnlyListForConstructorTest>
        {
            public ReadOnlyListForConstructorTest() : base(TestInstanceConfig.MinCapacity.Iterate(i => i.ToString()))
            {
            }
        }

        #endregion
    }
}
