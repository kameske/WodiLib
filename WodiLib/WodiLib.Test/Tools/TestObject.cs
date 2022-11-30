// ========================================
// Project Name : WodiLib.Test
// File Name    : TestObject.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Test.Tools
{
    /// <summary>
    /// テスト用のObject
    /// </summary>
    public class TestObject
    {
        public string StringValue { get; set; }

        public TestObject(string str = "")
        {
            StringValue = str;
        }
    }
}
