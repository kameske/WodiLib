using NUnit.Framework;
using WodiLib.Event.EventCommand;

namespace WodiLib.Test.Event.EventCommand
{
    using Factory = EventCommandFactory;

    /// <summary>
    ///     イベントコマンド・DB操作に関するテスト
    /// </summary>
    [TestFixture]
    public static class DBManagementTest
    {
        /// <summary>
        ///     数値入力に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementInputNumberTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputNumber
                    { DBTypeId = (intSrc, stringSrc), IsTypeIdUseStr = isUseStr };

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }

            /// <summary>
            ///     データIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBDataTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputNumber
                    { DBDataId = (intSrc, stringSrc), IsDataIdUseStr = isUseStr };

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBDataId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBDataId = stringSrc;
                else instance.DBDataId = intSrc;
                typeId = instance.DBDataId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsDataIdUseStr, isUseStr);
            }

            /// <summary>
            ///     項目IDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBItemTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputNumber
                    { DBItemId = (intSrc, stringSrc), IsItemIdUseStr = isUseStr };

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBItemId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBItemId = stringSrc;
                else instance.DBItemId = intSrc;
                typeId = instance.DBItemId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsItemIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     文字列入力に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementInputStringTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputString();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }

            /// <summary>
            ///     データIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBDataTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputString();
                instance.DBDataId = (intSrc, stringSrc);
                instance.IsDataIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBDataId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBDataId = stringSrc;
                else instance.DBDataId = intSrc;
                typeId = instance.DBDataId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsDataIdUseStr, isUseStr);
            }

            /// <summary>
            ///     項目IDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBItemTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputString();
                instance.DBItemId = (intSrc, stringSrc);
                instance.IsItemIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBItemId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBItemId = stringSrc;
                else instance.DBItemId = intSrc;
                typeId = instance.DBItemId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsItemIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     出力に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementOutputTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementOutput();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }

            /// <summary>
            ///     データIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBDataTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementOutput();
                instance.DBDataId = (intSrc, stringSrc);
                instance.IsDataIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBDataId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBDataId = stringSrc;
                else instance.DBDataId = intSrc;
                typeId = instance.DBDataId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsDataIdUseStr, isUseStr);
            }

            /// <summary>
            ///     項目IDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBItemTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementOutput();
                instance.DBItemId = (intSrc, stringSrc);
                instance.IsItemIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBItemId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBItemId = stringSrc;
                else instance.DBItemId = intSrc;
                typeId = instance.DBItemId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsItemIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     全データ初期化に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementClearDataTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementClearData();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     全項目初期化に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementClearFieldTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementClearField();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }

            /// <summary>
            ///     データIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBDataTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementClearField();
                instance.DBDataId = (intSrc, stringSrc);
                instance.IsDataIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBDataId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBDataId = stringSrc;
                else instance.DBDataId = intSrc;
                typeId = instance.DBDataId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsDataIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     データ番号取得に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetDataIdTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementGetDataId();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     データ数取得に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetDataLengthTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementGetDataLength();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     データ名取得に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetDataNameTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputNumber();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     項目番号取得に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetItemIdTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputNumber();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     項目数取得に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetItemLengthTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputNumber();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     項目名取得に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetItemNameTest
        {
            /// <summary>
            ///     タイプIDテスト
            /// </summary>
            /// <param name="intSrc"></param>
            /// <param name="stringSrc"></param>
            /// <param name="isUseStr"></param>
            [TestCase(30, "abc", true)]
            [TestCase(30, "abc", false)]
            public static void DBTypeTest(int intSrc, string stringSrc, bool isUseStr)
            {
                var instance = new DBManagementInputNumber();
                instance.DBTypeId = (intSrc, stringSrc);
                instance.IsTypeIdUseStr = isUseStr;

                // UseStr フラグによってIntかStrどちらかを持つこと
                var typeId = instance.DBTypeId;
                Assert.AreNotEqual(typeId.HasInt, isUseStr);
                Assert.AreEqual(typeId.HasStr, isUseStr);

                // 取得した値が同じであること
                var hasStr = typeId.HasStr;
                var isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // int または string 設定、正しく取得できること
                if (isUseStr) instance.DBTypeId = stringSrc;
                else instance.DBTypeId = intSrc;
                typeId = instance.DBTypeId;
                hasStr = typeId.HasStr;
                isSameValue = hasStr
                    ? typeId.ToStr().Equals(stringSrc)
                    : typeId.ToInt().Equals(intSrc);
                Assert.IsTrue(isSameValue);

                // 文字列使用フラグが正しいこと
                Assert.AreEqual(instance.IsTypeIdUseStr, isUseStr);
            }
        }

        /// <summary>
        ///     タイプ番号取得に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetTypeIdTest
        {
            // 特になし
        }

        /// <summary>
        ///     数値入力に関するテスト
        /// </summary>
        [TestFixture]
        public static class DBManagementGetTypeNameTest
        {
            // 特になし
        }
    }
}
