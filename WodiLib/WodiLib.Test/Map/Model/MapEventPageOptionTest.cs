using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageOptionTest
    {
        private static readonly object[] TestCaseSource =
        {
            new object[] {false, false, false, false, false, false, false, (byte) 0},
            new object[] {true, false, false, false, false, false, false, (byte) 1},
            new object[] {false, true, false, false, false, false, false, (byte) 2},
            new object[] {true, true, false, false, false, false, false, (byte) 3},
            new object[] {false, false, true, false, false, false, false, (byte) 4},
            new object[] {true, false, true, false, false, false, false, (byte) 5},
            new object[] {false, true, true, false, false, false, false, (byte) 6},
            new object[] {true, true, true, false, false, false, false, (byte) 7},
            new object[] {false, false, false, true, false, false, false, (byte) 8},
            new object[] {true, false, false, true, false, false, false, (byte) 9},
            new object[] {false, true, false, true, false, false, false, (byte) 10},
            new object[] {true, true, false, true, false, false, false, (byte) 11},
            new object[] {false, false, true, true, false, false, false, (byte) 12},
            new object[] {true, false, true, true, false, false, false, (byte) 13},
            new object[] {false, true, true, true, false, false, false, (byte) 14},
            new object[] {true, true, true, true, false, false, false, (byte) 15},
            new object[] {false, false, false, false, true, false, false, (byte) 16},
            new object[] {true, false, false, false, true, false, false, (byte) 17},
            new object[] {false, true, false, false, true, false, false, (byte) 18},
            new object[] {true, true, false, false, true, false, false, (byte) 19},
            new object[] {false, false, true, false, true, false, false, (byte) 20},
            new object[] {true, false, true, false, true, false, false, (byte) 21},
            new object[] {false, true, true, false, true, false, false, (byte) 22},
            new object[] {true, true, true, false, true, false, false, (byte) 23},
            new object[] {false, false, false, true, true, false, false, (byte) 24},
            new object[] {true, false, false, true, true, false, false, (byte) 25},
            new object[] {false, true, false, true, true, false, false, (byte) 26},
            new object[] {true, true, false, true, true, false, false, (byte) 27},
            new object[] {false, false, true, true, true, false, false, (byte) 28},
            new object[] {true, false, true, true, true, false, false, (byte) 29},
            new object[] {false, true, true, true, true, false, false, (byte) 30},
            new object[] {true, true, true, true, true, false, false, (byte) 31},
            new object[] {false, false, false, false, false, true, false, (byte) 32},
            new object[] {true, false, false, false, false, true, false, (byte) 33},
            new object[] {false, true, false, false, false, true, false, (byte) 34},
            new object[] {true, true, false, false, false, true, false, (byte) 35},
            new object[] {false, false, true, false, false, true, false, (byte) 36},
            new object[] {true, false, true, false, false, true, false, (byte) 37},
            new object[] {false, true, true, false, false, true, false, (byte) 38},
            new object[] {true, true, true, false, false, true, false, (byte) 39},
            new object[] {false, false, false, true, false, true, false, (byte) 40},
            new object[] {true, false, false, true, false, true, false, (byte) 41},
            new object[] {false, true, false, true, false, true, false, (byte) 42},
            new object[] {true, true, false, true, false, true, false, (byte) 43},
            new object[] {false, false, true, true, false, true, false, (byte) 44},
            new object[] {true, false, true, true, false, true, false, (byte) 45},
            new object[] {false, true, true, true, false, true, false, (byte) 46},
            new object[] {true, true, true, true, false, true, false, (byte) 47},
            new object[] {false, false, false, false, true, true, false, (byte) 48},
            new object[] {true, false, false, false, true, true, false, (byte) 49},
            new object[] {false, true, false, false, true, true, false, (byte) 50},
            new object[] {true, true, false, false, true, true, false, (byte) 51},
            new object[] {false, false, true, false, true, true, false, (byte) 52},
            new object[] {true, false, true, false, true, true, false, (byte) 53},
            new object[] {false, true, true, false, true, true, false, (byte) 54},
            new object[] {true, true, true, false, true, true, false, (byte) 55},
            new object[] {false, false, false, true, true, true, false, (byte) 56},
            new object[] {true, false, false, true, true, true, false, (byte) 57},
            new object[] {false, true, false, true, true, true, false, (byte) 58},
            new object[] {true, true, false, true, true, true, false, (byte) 59},
            new object[] {false, false, true, true, true, true, false, (byte) 60},
            new object[] {true, false, true, true, true, true, false, (byte) 61},
            new object[] {false, true, true, true, true, true, false, (byte) 62},
            new object[] {true, true, true, true, true, true, false, (byte) 63},
            new object[] {false, false, false, false, false, false, true, (byte) 64},
            new object[] {true, false, false, false, false, false, true, (byte) 65},
            new object[] {false, true, false, false, false, false, true, (byte) 66},
            new object[] {true, true, false, false, false, false, true, (byte) 67},
            new object[] {false, false, true, false, false, false, true, (byte) 68},
            new object[] {true, false, true, false, false, false, true, (byte) 69},
            new object[] {false, true, true, false, false, false, true, (byte) 70},
            new object[] {true, true, true, false, false, false, true, (byte) 71},
            new object[] {false, false, false, true, false, false, true, (byte) 72},
            new object[] {true, false, false, true, false, false, true, (byte) 73},
            new object[] {false, true, false, true, false, false, true, (byte) 74},
            new object[] {true, true, false, true, false, false, true, (byte) 75},
            new object[] {false, false, true, true, false, false, true, (byte) 76},
            new object[] {true, false, true, true, false, false, true, (byte) 77},
            new object[] {false, true, true, true, false, false, true, (byte) 78},
            new object[] {true, true, true, true, false, false, true, (byte) 79},
            new object[] {false, false, false, false, true, false, true, (byte) 80},
            new object[] {true, false, false, false, true, false, true, (byte) 81},
            new object[] {false, true, false, false, true, false, true, (byte) 82},
            new object[] {true, true, false, false, true, false, true, (byte) 83},
            new object[] {false, false, true, false, true, false, true, (byte) 84},
            new object[] {true, false, true, false, true, false, true, (byte) 85},
            new object[] {false, true, true, false, true, false, true, (byte) 86},
            new object[] {true, true, true, false, true, false, true, (byte) 87},
            new object[] {false, false, false, true, true, false, true, (byte) 88},
            new object[] {true, false, false, true, true, false, true, (byte) 89},
            new object[] {false, true, false, true, true, false, true, (byte) 90},
            new object[] {true, true, false, true, true, false, true, (byte) 91},
            new object[] {false, false, true, true, true, false, true, (byte) 92},
            new object[] {true, false, true, true, true, false, true, (byte) 93},
            new object[] {false, true, true, true, true, false, true, (byte) 94},
            new object[] {true, true, true, true, true, false, true, (byte) 95},
            new object[] {false, false, false, false, false, true, true, (byte) 96},
            new object[] {true, false, false, false, false, true, true, (byte) 97},
            new object[] {false, true, false, false, false, true, true, (byte) 98},
            new object[] {true, true, false, false, false, true, true, (byte) 99},
            new object[] {false, false, true, false, false, true, true, (byte) 100},
            new object[] {true, false, true, false, false, true, true, (byte) 101},
            new object[] {false, true, true, false, false, true, true, (byte) 102},
            new object[] {true, true, true, false, false, true, true, (byte) 103},
            new object[] {false, false, false, true, false, true, true, (byte) 104},
            new object[] {true, false, false, true, false, true, true, (byte) 105},
            new object[] {false, true, false, true, false, true, true, (byte) 106},
            new object[] {true, true, false, true, false, true, true, (byte) 107},
            new object[] {false, false, true, true, false, true, true, (byte) 108},
            new object[] {true, false, true, true, false, true, true, (byte) 109},
            new object[] {false, true, true, true, false, true, true, (byte) 110},
            new object[] {true, true, true, true, false, true, true, (byte) 111},
            new object[] {false, false, false, false, true, true, true, (byte) 112},
            new object[] {true, false, false, false, true, true, true, (byte) 113},
            new object[] {false, true, false, false, true, true, true, (byte) 114},
            new object[] {true, true, false, false, true, true, true, (byte) 115},
            new object[] {false, false, true, false, true, true, true, (byte) 116},
            new object[] {true, false, true, false, true, true, true, (byte) 117},
            new object[] {false, true, true, false, true, true, true, (byte) 118},
            new object[] {true, true, true, false, true, true, true, (byte) 119},
            new object[] {false, false, false, true, true, true, true, (byte) 120},
            new object[] {true, false, false, true, true, true, true, (byte) 121},
            new object[] {false, true, false, true, true, true, true, (byte) 122},
            new object[] {true, true, false, true, true, true, true, (byte) 123},
            new object[] {false, false, true, true, true, true, true, (byte) 124},
            new object[] {true, false, true, true, true, true, true, (byte) 125},
            new object[] {false, true, true, true, true, true, true, (byte) 126},
            new object[] {true, true, true, true, true, true, true, (byte) 127},
        };

        [TestCaseSource(nameof(TestCaseSource))]
        public static void SetOptionFlagTest(
            bool isWaitAnimationOn, bool isMoveAnimationOn, bool isFixedDirection,
            bool isSkipThrough, bool isAboveHero, bool isHitBox,
            bool isPlaceHalfStepUp, byte optionCode)
        {
            var instance = new MapEventPageOption();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            instance.SetOptionFlag(optionCode);

            // 各フラグが一致すること
            Assert.AreEqual(instance.IsWaitAnimationOn, isWaitAnimationOn);
            Assert.AreEqual(instance.IsMoveAnimationOn, isMoveAnimationOn);
            Assert.AreEqual(instance.IsFixedDirection, isFixedDirection);
            Assert.AreEqual(instance.IsSkipThrough, isSkipThrough);
            Assert.AreEqual(instance.IsAboveHero, isAboveHero);
            Assert.AreEqual(instance.IsHitBox, isHitBox);
            Assert.AreEqual(instance.IsPlaceHalfStepUp, isPlaceHalfStepUp);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 7);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageOption.IsWaitAnimationOn)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapEventPageOption.IsMoveAnimationOn)));
            Assert.IsTrue(changedPropertyList[2].Equals(nameof(MapEventPageOption.IsFixedDirection)));
            Assert.IsTrue(changedPropertyList[3].Equals(nameof(MapEventPageOption.IsSkipThrough)));
            Assert.IsTrue(changedPropertyList[4].Equals(nameof(MapEventPageOption.IsAboveHero)));
            Assert.IsTrue(changedPropertyList[5].Equals(nameof(MapEventPageOption.IsHitBox)));
            Assert.IsTrue(changedPropertyList[6].Equals(nameof(MapEventPageOption.IsPlaceHalfStepUp)));
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapEventPageOption
            {
                IsAboveHero = true,
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