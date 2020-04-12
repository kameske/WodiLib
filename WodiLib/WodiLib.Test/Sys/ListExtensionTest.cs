using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class ListExtensionTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(3, 1, -1, true)]
        [TestCase(4, 4, -1, true)]
        [TestCase(2, 5, -1, true)]
        [TestCase(3, 1, 10, false)]
        [TestCase(4, 4, 10, false)]
        [TestCase(2, 5, 10, false)]
        public static void AdjustLengthTest(int initLength, int adjustLength, int defaultValue, bool isError)
        {
            var list = new List<int>();
            for (var i = 0; i < initLength; i++) list.Add(i);


            var makeDefaultValueFunc = defaultValue == -1
                ? null
                : new Func<int, int>(i => defaultValue);

            var errorOccured = false;
            try
            {
                list.AdjustLength(adjustLength, makeDefaultValueFunc);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 長さが意図した値であること
            Assert.AreEqual(list.Count, adjustLength);

            // 手を加えなかった部分の値がそのままであること
            var checkLength = initLength < adjustLength ? initLength : adjustLength;
            for (var i = 0; i < checkLength; i++)
            {
                Assert.AreEqual(list[i], i);
            }

            // 手を加えた部分の値が defaultValue と一致すること
            var addLength = adjustLength - initLength;
            if (addLength < 0) addLength = 0;
            for (var i = 0; i < addLength; i++)
            {
                Assert.AreEqual(list[i + checkLength], defaultValue);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public static void GetHashCodeForEqualityComparerTest(int listLength)
        {
            const int prime = 31;
            IEqualityComparer<int> testComparer = new GetHashCodeTestEqualityComparer();

            var list = new List<int>();
            // 要素が0のとき、ハッシュ値0
            // 要素が1以上のとき、1を起点に 要素の分だけ
            //     hashCode = (hashCode * 31) ^ IEqualityComparer<T>.GetHashCode(list[i])
            // を実行する
            var answer = listLength == 0 ? 0 : 1;

            for (var i = 0; i < listLength; i++)
            {
                list.Add(i);
                answer = (answer * prime) ^ testComparer.GetHashCode(i);
            }

            // 値が意図した値と一致すること
            Assert.AreEqual(list.GetHashCode(testComparer), answer);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public static void GetHashCodeForFuncTest(int listLength)
        {
            const int prime = 31;
            Func<int, int> testFunc = new GetHashCodeTestEqualityComparer().GetHashCode;

            var list = new List<int>();
            // 要素が0のとき、ハッシュ値0
            // 要素が1以上のとき、1を起点に 要素の分だけ
            //     hashCode = (hashCode * 31) ^ Func(list[i])
            // を実行する
            var answer = listLength == 0 ? 0 : 1;

            for (var i = 0; i < listLength; i++)
            {
                list.Add(i);
                answer = (answer * prime) ^ testFunc(i);
            }

            // 値が意図した値と一致すること
            Assert.AreEqual(list.GetHashCode(testFunc), answer);
        }

        private class GetHashCodeTestEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}