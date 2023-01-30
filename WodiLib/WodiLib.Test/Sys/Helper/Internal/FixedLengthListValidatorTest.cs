using System;
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
    public class FixedLengthListValidatorTest
    {
        private static Logger logger = default!;

        private static class TestInstanceConfig
        {
            public const int Capacity = 5;
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
                    new FixedLengthListForTest(),
                    null
                },
                // capacity
                new[]
                {
                    TestInstanceConfig.Capacity
                }
            ),
            // expectedError
            (target, _) => target is null
        );

        [TestCaseSource(nameof(ConstructorTest_1TestCaseSource))]
        public static void ConstructorTest_1(IFixedLengthList<string> target, int capacity, bool isError)
        {
            TestTemplate.Constructor(
                () => new FixedLengthListValidator<string>(target, capacity),
                isError,
                logger
            );
        }

        private static readonly object[] ConstructorTest_2TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // target
                new[]
                {
                    new FixedLengthListForTest(),
                    null
                },
                // capacityGetter
                new[]
                {
                    new Func<int>(() => TestInstanceConfig.Capacity),
                    null
                }
            ),
            // expectedError
            (target, capacityGetter) => target is null || capacityGetter is null
        );

        [TestCaseSource(nameof(ConstructorTest_2TestCaseSource))]
        public static void ConstructorTest_2(IFixedLengthList<string> target, Func<int> capacityGetter, bool isError)
        {
            TestTemplate.Constructor(
                () => new FixedLengthListValidator<string>(target, capacityGetter),
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
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", null!, "2", "3", "4" }),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", "2", "3", "4" }),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", "2", "3", "4", "5" }),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", "2", "3", "4", "5", "6" })
                }
            ),
            // expectedError
            initItems => initItems.Value is null
                         || initItems.Value.HasNullItem()
                         || initItems.Value.Count() != TestInstanceConfig.Capacity
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
                    new NamedValue<int>("index", TestInstanceConfig.Capacity - 1),
                    new NamedValue<int>("index", TestInstanceConfig.Capacity)
                },
                // count
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", 1),
                    new NamedValue<int>("count", TestInstanceConfig.Capacity),
                    new NamedValue<int>("count", TestInstanceConfig.Capacity + 1)
                }
            ),
            // expectedError
            (index, count) => !index.Value.IsBetween(0, TestInstanceConfig.Capacity - 1)
                              || !count.Value.IsBetween(0, TestInstanceConfig.Capacity)
                              || index.Value + count.Value > TestInstanceConfig.Capacity
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
                    new NamedValue<int>("index", TestInstanceConfig.Capacity - 1),
                    new NamedValue<int>("index", TestInstanceConfig.Capacity)
                },
                // items
                new[]
                {
                    new NamedValue<IEnumerable<string>?>("items", null),
                    new NamedValue<IEnumerable<string>?>("items", Array.Empty<string>()),
                    new NamedValue<IEnumerable<string>?>("items", new[] { "0", null!, "2" }),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        TestInstanceConfig.Capacity.Iterate(i => i.ToString())
                    ),
                    new NamedValue<IEnumerable<string>?>(
                        "items",
                        (TestInstanceConfig.Capacity + 1).Iterate(i => i.ToString())
                    )
                }
            ),
            // expectedError
            (index, items) => !index.Value.IsBetween(0, TestInstanceConfig.Capacity - 1)
                              || items.Value is null
                              || items.Value.HasNullItem()
                              || index.Value + items.Value.Count() > TestInstanceConfig.Capacity
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

        private static readonly object[] MoveTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // oldIndex
                new[]
                {
                    new NamedValue<int>("oldIndex", -1),
                    new NamedValue<int>("oldIndex", 0),
                    new NamedValue<int>("oldIndex", TestInstanceConfig.Capacity - 1),
                    new NamedValue<int>("oldIndex", TestInstanceConfig.Capacity)
                },
                // newIndex
                new[]
                {
                    new NamedValue<int>("newIndex", -1),
                    new NamedValue<int>("newIndex", 0),
                    new NamedValue<int>("newIndex", TestInstanceConfig.Capacity - 1),
                    new NamedValue<int>("newIndex", TestInstanceConfig.Capacity)
                },
                // count
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", 2),
                    new NamedValue<int>("count", TestInstanceConfig.Capacity),
                    new NamedValue<int>("count", TestInstanceConfig.Capacity + 1)
                }
            ),
            // expectedError
            (oldIndex, newIndex, count) => !oldIndex.Value.IsBetween(0, TestInstanceConfig.Capacity - 1)
                                           || !newIndex.Value.IsBetween(0, TestInstanceConfig.Capacity - 1)
                                           || !count.Value.IsBetween(0, TestInstanceConfig.Capacity)
                                           || oldIndex.Value + count.Value > TestInstanceConfig.Capacity
                                           || newIndex.Value + count.Value > TestInstanceConfig.Capacity
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

        private static readonly object[] ResetTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // items
                new[]
                {
                    new NamedValue<IEnumerable<string>?>("", null!),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", null!, "2", "3", "4" }),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", "2", "3", "4" }),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", "2", "3", "4", "5" }),
                    new NamedValue<IEnumerable<string>?>("", new[] { "1", "2", "3", "4", "5", "6" })
                }
            ),
            // expectedError
            initItems => initItems.Value is null
                         || initItems.Value.HasNullItem()
                         || initItems.Value.Count() != TestInstanceConfig.Capacity
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

        private static FixedLengthListValidator<string> GetTestInstance()
            => TestValidatorInitializer.FixedLengthListValidator;

        private static readonly FixedLengthListForTest TestValidatorInitializer = new();

        internal class FixedLengthListForTest : FixedLengthList<string, FixedLengthListForTest>
        {
            public FixedLengthListValidator<string> FixedLengthListValidator { get; private set; } = default!;

            public FixedLengthListForTest() : base(TestInstanceConfig.Capacity)
            {
            }

            protected override string MakeDefaultItem(int index) => index.ToString();

            protected override IWodiLibListValidator<string> GenerateValidatorForItems()
            {
                FixedLengthListValidator = new FixedLengthListValidator<string>(this, TestInstanceConfig.Capacity);
                return FixedLengthListValidator;
            }
        }

        internal class ReadOnlyListForConstructorTest : ReadOnlyExtendedList<string, ReadOnlyListForConstructorTest>
        {
            public ReadOnlyListForConstructorTest() : base(TestInstanceConfig.Capacity.Iterate(i => i.ToString()))
            {
            }
        }

        #endregion
    }
}
