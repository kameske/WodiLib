using System;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ConditionStringListTest
    {
        private static readonly object[] SetConditionValueOutOfRangeTestCaseSource =
        {
            new object[] {-1, true},
            new object[] {0, true},
            new object[] {1, false},
            new object[] {4, false},
            new object[] {5, true}
        };
        [TestCaseSource(nameof(SetConditionValueOutOfRangeTestCaseSource))]
        public static void SetConditionValueOutOfRangeTest(int value, bool error)
        {
            var instance = new ConditionStringList();
            bool isError;
            try
            {
                instance.ConditionValue = value;
                isError = false;
            }
            catch (Exception)
            {
                isError = true;
            }

            Assert.AreEqual(isError, error);
        }

        private static readonly object[] AccessorOutOfRangeTestCaseSource =
        {
            new object[] {1, -1, true},
            new object[] {1, 0, false},
            new object[] {1, 1, true},
            new object[] {2, -1, true},
            new object[] {2, 0, false},
            new object[] {2, 1, false},
            new object[] {2, 2, true},
            new object[] {4, -1, true},
            new object[] {4, 0, false},
            new object[] {4, 3, false},
            new object[] {4, 4, true}
        };
        [TestCaseSource(nameof(AccessorOutOfRangeTestCaseSource))]
        public static void SetOutOfRangeTest(int conditionValue, int setIndex, bool error)
        {
            var instance = new ConditionStringList();
            bool isError;
            try
            {
                instance.ConditionValue = conditionValue;
                instance.Set(setIndex, new ConditionStringDesc());
                isError = false;
            }
            catch (Exception)
            {
                isError = true;
            }

            Assert.AreEqual(isError, error);
        }

        [Test]
        public static void SetNullText()
        {
            var instance = new ConditionStringList
            {
                ConditionValue = 4
            };
            var isError = false;
            try
            {
                instance.Set(0, null);
            }
            catch (ArgumentNullException)
            {
                isError = true;
            }
            Assert.IsTrue(isError);
        }

        private static readonly object[] SetLeftSideTestCaseSource =
        {
            new object[] {1, 0, 100, false},
            new object[] {1, 1, 100, true},
            new object[] {4, -1, 100, true},
            new object[] {4, 0, 100, false},
            new object[] {4, 3, 100, false},
            new object[] {4, 4, 100, true},
        };
        [TestCaseSource(nameof(SetLeftSideTestCaseSource))]
        public static void SetLeftSideTest(int conditionValue, int index, int leftSide, bool isError)
        {
            var instance = new ConditionStringList
            {
                ConditionValue = conditionValue
            };
            var errorOccured = false;
            try
            {
                instance.SetLeftSide(index, leftSide);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // セットしたLeftSideだけが変化していること
            for (var i = 0; i < instance.ConditionValue; i++)
            {
                var getValue = instance.Get(i).LeftSide;
                if(i == index) Assert.AreEqual(getValue, leftSide);
                else Assert.AreNotEqual(getValue, leftSide);
            }
        }

        private static readonly object[] SetRightSideTestCaseSource =
        {
            new object[] {1, 0, 100, false},
            new object[] {1, 1, 100, true},
            new object[] {4, -1, 100, true},
            new object[] {4, 0, 100, false},
            new object[] {4, 3, 100, false},
            new object[] {4, 4, 100, true},
        };
        [TestCaseSource(nameof(SetRightSideTestCaseSource))]
        public static void SetRightSideTest(int conditionValue, int index, int rightSide, bool isError)
        {
            var instance = new ConditionStringList
            {
                ConditionValue = conditionValue
            };
            var errorOccured = false;
            try
            {
                instance.SetRightSide(index, rightSide);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] MergeRightSideTestCaseSource =
        {
            new object[] {1, 0, 100, false},
            new object[] {1, 1, 100, true},
            new object[] {4, -1, 100, true},
            new object[] {4, 0, 100, false},
            new object[] {4, 3, 100, false},
            new object[] {4, 4, 100, true},
        };
        [TestCaseSource(nameof(MergeRightSideTestCaseSource))]
        public static void MergeRightSideTest(int conditionValue, int index, int rightSide, bool isError)
        {
            var instance = new ConditionStringList
            {
                ConditionValue = conditionValue
            };
            var errorOccured = false;
            try
            {
                instance.MergeRightSide(index, rightSide);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

        }

        private static readonly object[] MergeRightSideNonCheckIndexTestCaseSource =
        {
            new object[] {1, 0, 100, false},
            new object[] {1, 1, 100, false},
            new object[] {1, 3, 100, false},
            new object[] {1, 4, 100, true},
            new object[] {4, -1, 100, true},
            new object[] {4, 0, 100, false},
            new object[] {4, 3, 100, false},
            new object[] {4, 4, 100, true},
        };
        [TestCaseSource(nameof(MergeRightSideNonCheckIndexTestCaseSource))]
        public static void MergeRightSideNonCheckIndexTest(int conditionValue, int index, int rightSide, bool isError)
        {
            var instance = new ConditionStringList
            {
                ConditionValue = conditionValue
            };
            var errorOccured = false;
            try
            {
                instance.MergeRightSideNonCheckIndex(index, rightSide);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

        }

        private static readonly object[] SetConditionTestCaseSource =
        {
            new object[] {1, 0, StringConditionalOperator.StartWith, false},
            new object[] {1, 1, StringConditionalOperator.StartWith, true},
            new object[] {4, -1, StringConditionalOperator.StartWith, true},
            new object[] {4, 0, StringConditionalOperator.StartWith, false},
            new object[] {4, 3, StringConditionalOperator.StartWith, false},
            new object[] {4, 4, StringConditionalOperator.StartWith, true},
        };
        [TestCaseSource(nameof(SetConditionTestCaseSource))]
        public static void SetConditionTest(int conditionValue, int index, StringConditionalOperator condition, bool isError)
        {
            var instance = new ConditionStringList
            {
                ConditionValue = conditionValue
            };
            var errorOccured = false;
            try
            {
                instance.SetCondition(index, condition);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // セットしたConditionだけが変化していること
            for (var i = 0; i < instance.ConditionValue; i++)
            {
                var getValue = instance.Get(i).Condition;
                if(i == index) Assert.AreEqual(getValue, condition);
                else Assert.AreNotEqual(getValue, condition);
            }
        }

        private static readonly object[] SetIsUseNumberVariableTestCaseSource =
        {
            new object[] {1, 0, true, false},
            new object[] {1, 1, true, true},
            new object[] {4, -1, true, true},
            new object[] {4, 0, true, false},
            new object[] {4, 3, true, false},
            new object[] {4, 4, true, true},
        };
        [TestCaseSource(nameof(SetIsUseNumberVariableTestCaseSource))]
        public static void SetIsUseNumberVariableTest(int conditionValue, int index, bool isUseNumberVariable, bool isError)
        {
            var instance = new ConditionStringList
            {
                ConditionValue = conditionValue
            };
            var errorOccured = false;
            try
            {
                instance.SetIsUseNumberVariable(index, isUseNumberVariable);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // セットしたIsUseNumberVariableだけが変化していること
            for (var i = 0; i < instance.ConditionValue; i++)
            {
                var getValue = instance.Get(i).IsUseNumberVariable;
                if(i == index) Assert.AreEqual(getValue, isUseNumberVariable);
                else Assert.AreNotEqual(getValue, isUseNumberVariable);
            }
        }

        [TestCaseSource(nameof(AccessorOutOfRangeTestCaseSource))]
        public static void GetOutOfRangeTest(int conditionValue, int getIndex, bool error)
        {
            var instance = new ConditionStringList();
            bool isError;
            try
            {
                instance.ConditionValue = conditionValue;
                var _ = instance.Get(getIndex);
                isError = false;
            }
            catch (Exception)
            {
                isError = true;
            }

            Assert.AreEqual(isError, error);
        }

        private static readonly object[] SearchUseNumberVariableForRightSideMaxTestCaseSource =
        {
            new object[] {1, false, false, false, false, 1},
            new object[] {1, true, false, false, false, 0},
            new object[] {2, false, false, false, false, 2},
            new object[] {2, true, false, false, false, 2},
            new object[] {2, false, true, false, false, 1},
            new object[] {2, true, true, false, false, 0},
            new object[] {3, false, false, false, false, 3},
            new object[] {3, true, false, false, false, 3},
            new object[] {3, false, true, false, false, 3},
            new object[] {3, false, false, true, false, 2},
            new object[] {3, true, true, false, false, 3},
            new object[] {3, true, false, true, false, 2},
            new object[] {3, false, true, true, false, 1},
            new object[] {3, true, true, true, false, 0},
            new object[] {4, false, false, false, false, 4},
            new object[] {4, true, false, false, false, 4},
            new object[] {4, false, true, false, false, 4},
            new object[] {4, false, false, true, false, 4},
            new object[] {4, false, false, false, true, 3},
            new object[] {4, true, true, false, false, 4},
            new object[] {4, true, false, true, false, 4},
            new object[] {4, true, false, false, true, 3},
            new object[] {4, false, true, true, false, 4},
            new object[] {4, false, true, false, true, 3},
            new object[] {4, false, false, true, true, 2},
            new object[] {4, true, true, true, false, 4},
            new object[] {4, true, true, false, true, 3},
            new object[] {4, true, false, true, true, 2},
            new object[] {4, false, true, true, true, 1},
            new object[] {4, true, true, true, true, 0},
        };
        [TestCaseSource(nameof(SearchUseNumberVariableForRightSideMaxTestCaseSource))]
        public static void SearchUseNumberVariableForRightSideMaxTest(int conditionValue, bool is1Str, bool is2Str,
            bool is3Str, bool is4Str, int result)
        {
            var instance = new ConditionStringList
            {
                // いったんすべての条件を上書きできるように
                ConditionValue = 4
            };
            instance.Set(0, new ConditionStringDesc
            {
                Condition = StringConditionalOperator.Equal,
                LeftSide = 0,
                RightSide = is1Str ? (IntOrStr)"a" : 0,
                IsUseNumberVariable = !is1Str
            });
            instance.Set(1, new ConditionStringDesc
            {
                Condition = StringConditionalOperator.Equal,
                LeftSide = 0,
                RightSide = is2Str ? (IntOrStr)"a" : 0,
                IsUseNumberVariable = !is2Str
            });
            instance.Set(2, new ConditionStringDesc
            {
                Condition = StringConditionalOperator.Equal,
                LeftSide = 0,
                RightSide = is3Str ? (IntOrStr)"a" : 0,
                IsUseNumberVariable = !is3Str
            });
            instance.Set(3, new ConditionStringDesc
            {
                Condition = StringConditionalOperator.Equal,
                LeftSide = 0,
                RightSide = is4Str ? (IntOrStr)"a" : 0,
                IsUseNumberVariable = !is4Str
            });
            // 条件数を正しく
            instance.ConditionValue = conditionValue;

            var max = instance.SearchUseNumberVariableForRightSideMax();
            Assert.AreEqual(max, result);
        }
    }
}