// ========================================
// Project Name : WodiLib.Test
// File Name    : TestTemplateWithMock.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using NUnit.Framework;

namespace WodiLib.Test.Tools
{
    /// <summary>
    /// モックを使ったテスト用テンプレート
    /// </summary>
    internal static class TestTemplateWithMock
    {
        public static void AssertEqualsCalledMemberHistory<T>(
            MockBase<T> mock,
            params string[] expectedValidationCalled
        )
            where T : class
        {
            Assert.AreEqual(expectedValidationCalled.Length, mock.CalledMemberHistory.Count);
            Assert.IsTrue(expectedValidationCalled.SequenceEqual(mock.CalledMemberHistory));
        }

        public static void AssertContainsCalledMemberHistory<T>(
            MockBase<T> mock,
            params string[] expectedValidationCalled
        )
            where T : class
        {
            Assert.IsTrue(expectedValidationCalled.All(called => mock.CalledMemberHistory.Contains(called)));
        }
    }
}
