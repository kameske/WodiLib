// ========================================
// Project Name : WodiLib
// File Name    : ErrorMessage.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// エラーメッセージ生成クラス
    /// </summary>
    internal static class ErrorMessage
    {
        /// <summary>
        /// NotNullエラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotNull(string itemName)
        {
            return $"{itemName}にnullを設定できません。";
        }

        /// <summary>
        /// NotEmptyエラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotEmpty(string itemName)
        {
            return $"{itemName}にEmptyを設定できません。";
        }

        /// <summary>
        /// 範囲エラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <param name="setValue">設定値</param>
        /// <returns>エラーメッセージ</returns>
        public static string OutOfRange(string itemName, IntOrStr min, IntOrStr max, int setValue)
        {
            return $"{itemName}は{min.ToValueString()}～{max.ToValueString()}のみ設定できます。(設定値：{setValue})";
        }

        /// <summary>
        /// 項目数エラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <param name="size">項目数</param>
        /// <returns>エラーメッセージ</returns>
        public static string LengthRange(string itemName, IntOrStr min, IntOrStr max, int size)
        {
            return $"{itemName}のサイズは{min.ToValueString()}～{max.ToValueString()}にする必要があります。(サイズ：{size})";
        }

        /// <summary>
        /// オブジェクト不適切エラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <param name="item">エラーオブジェクト</param>
        /// <returns>エラーメッセージ</returns>
        public static string Unsuitable(string itemName, object item)
        {
            return $"{itemName}が不適切です。{{ {item} }}";
        }

        /// <summary>
        /// 改行を含む場合のエラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <param name="value">エラー文字列</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotNewLine(string itemName, string value)
        {
            return $"{itemName}に改行を含むことはできません。（設定値：{value}）";
        }

        /// <summary>
        /// いずれの型にもキャスト不可能な場合のエラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <param name="needCastClassNameList">キャスト可能であるべき型名リスト</param>
        /// <returns></returns>
        public static string InvalidAnyCast(string itemName, params string[] needCastClassNameList)
        {
            return $"{itemName}は{string.Join(", ", needCastClassNameList)}のいずれかに" +
                   $"キャスト可能である必要があります。";
        }
    }
}