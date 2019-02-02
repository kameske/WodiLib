using NUnit.Framework;
using WodiLib.Event;

namespace WodiLib.Test.Event
{
    [TestFixture]
    public class EventIdHelperTest
    {
        private static readonly object[] IsMapEventIdTestCaseSource =
        {
            new object[] {-2, false},
            new object[] {-1, true},
            new object[] {0, true},
            new object[] {9999, true},
            new object[] {10000, false},
        };
        [TestCaseSource(nameof(IsMapEventIdTestCaseSource))]
        public static void IsMapEventIdTest(int eventId, bool isMapEventId)
        {
            var result = EventIdHelper.IsMapEventId(eventId);
            // 結果が一致すること
            Assert.AreEqual(result, isMapEventId);
        }
        
        private static readonly object[] IsCommonEventIdTestCaseSource =
        {
            new object[] {-2, false},
            new object[] {-1, true},
            new object[] {0, true},
            new object[] {9999, true},
            new object[] {10000, false},
            new object[] {499999, false},
            new object[] {500000, true},
            new object[] {509999, true},
            new object[] {510000, false},
            new object[] {600049, false},
            new object[] {600050, true},
            new object[] {600150, true},
            new object[] {600151, false},
        };
        [TestCaseSource(nameof(IsCommonEventIdTestCaseSource))]
        public static void IsCommonEventIdTest(int eventId, bool isCommonEventId)
        {
            var result = EventIdHelper.IsCommonEventId(eventId);
            // 結果が一致すること
            Assert.AreEqual(result, isCommonEventId);
        }

        private static readonly object[] IsNormalCommonEventIdTestCaseSource =
        {
            new object[] {-1, false},
            new object[] {0, true},
            new object[] {9999, true},
            new object[] {10000, false},
        };
        [TestCaseSource(nameof(IsNormalCommonEventIdTestCaseSource))]
        public static void IsNormalCommonEventIdTest(int eventId, bool isCommonEventId)
        {
            var result = EventIdHelper.IsNormalCommonEventId(eventId);
            // 結果が一致すること
            Assert.AreEqual(result, isCommonEventId);
        }

        private static readonly object[] IsAbsoluteCommonIdTestCaseSource =
        {
            new object[] {-1, false},
            new object[] {0, false},
            new object[] {499999, false},
            new object[] {500000, true},
            new object[] {509999, true},
            new object[] {510000, false},
        };
        [TestCaseSource(nameof(IsAbsoluteCommonIdTestCaseSource))]
        public static void IsAbsoluteCommonIdTest(int eventId, bool isCommonEventId)
        {
            var result = EventIdHelper.IsAbsoluteCommonId(eventId);
            // 結果が一致すること
            Assert.AreEqual(result, isCommonEventId);
        }
        
        private static readonly object[] IsRelativeCommonIdTestCaseSource =
        {
            new object[] {-1, false},
            new object[] {0, false},
            new object[] {600049, false},
            new object[] {600050, true},
            new object[] {600100, true},
            new object[] {600150, true},
            new object[] {600151, false},
        };
        [TestCaseSource(nameof(IsRelativeCommonIdTestCaseSource))]
        public static void IsRelativeCommonIdTest(int eventId, bool isCommonEventId)
        {
            var result = EventIdHelper.IsRelativeCommonId(eventId);
            // 結果が一致すること
            Assert.AreEqual(result, isCommonEventId);
        }
    }
}