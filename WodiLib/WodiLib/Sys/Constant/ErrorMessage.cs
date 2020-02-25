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
        /// List中にnull項目がある場合のエラーメッセージ
        /// </summary>
        /// <param name="listName">エラー項目名</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotNullInList(string listName)
        {
            return $"{listName}中にnull項目が含まれているため、処理できません。";
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
        /// Denyエラーメッセージ
        /// </summary>
        /// <param name="itemName">項目名</param>
        /// <param name="value">設定値</param>
        /// <returns>エラーメッセージ</returns>
        public static string Deny(string itemName, IntOrStr value)
        {
            return $"{itemName}に{value}を設定できません。";
        }

        /// <summary>
        /// NotEqualエラーメッセージ
        /// </summary>
        /// <param name="itemLeft">右辺項目名</param>
        /// <param name="itemRight">左辺項目名</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotEqual(string itemLeft, string itemRight)
        {
            return $"{itemLeft}と{itemRight}が異なるため、処理できません。";
        }

        /// <summary>
        /// GreaterOrEqualエラーメッセージ
        /// </summary>
        /// <param name="itemName">項目名</param>
        /// <param name="limit">上限値</param>
        /// <param name="value">設定値</param>
        /// <returns>エラーメッセージ</returns>
        public static string GreaterOrEqual(string itemName, IntOrStr limit, int value)
        {
            return $"{itemName}は{limit}以上である必要があります。";
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
        /// <param name="allowSize">許容サイズ</param>
        /// <param name="size">項目数</param>
        /// <returns>エラーメッセージ</returns>
        public static string LengthRange(string itemName, IntOrStr allowSize, int size)
        {
            return $"{itemName}のサイズは{allowSize}にする必要があります。(サイズ：{size})";
        }

        /// <summary>
        /// リスト要素数が不足する場合のエラーメッセージ
        /// </summary>
        /// <param name="limit">要素数下限</param>
        /// <returns>エラーメッセージ</returns>
        public static string UnderListLength(int limit)
        {
            return $"要素数が{limit}を下回るため、処理できません。";
        }

        /// <summary>
        /// リスト要素数が不足する場合のエラーメッセージ
        /// </summary>
        /// <param name="itemName">要素名</param>
        /// <param name="limit">要素数下限</param>
        /// <returns>エラーメッセージ</returns>
        public static string UnderListLength(string itemName, int limit)
        {
            return $"{itemName}の要素数が{limit}を下回るため、処理できません。";
        }

        /// <summary>
        /// リスト要素数が超過する場合のエラーメッセージ
        /// </summary>
        /// <param name="limit">要素数上限</param>
        /// <returns>エラーメッセージ</returns>
        public static string OverListLength(int limit)
        {
            return $"要素数が{limit}を上回るため、処理できません。";
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
        /// <returns>エラーメッセージ</returns>
        public static string InvalidAnyCast(string itemName, params string[] needCastClassNameList)
        {
            return $"{itemName}は{string.Join(", ", needCastClassNameList)}のいずれかに" +
                   $"キャスト可能である必要があります。";
        }

        /// <summary>
        /// nullのためキャスト不可能な場合のエラーメッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <param name="className">キャスト先型名</param>
        /// <returns>エラーメッセージ</returns>
        public static string InvalidCastFromNull(string itemName, string className)
        {
            return $"{itemName}はnullのため{className}にキャストできません。";
        }

        /// <summary>
        /// アクセスできない場合のエラーメッセージ
        /// </summary>
        /// <param name="reason">アクセスできない理由</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotAccess(string reason)
        {
            return $"{reason}アクセスできません。";
        }

        /// <summary>
        /// キャスト不可能な場合のエラーメッセージ
        /// </summary>
        /// <param name="reason">キャストできない理由</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotCast(string reason)
        {
            return $"{reason}変換できません。";
        }

        /// <summary>
        /// 処理不可能な場合のエラーメッセージ
        /// </summary>
        /// <param name="reason">処理できない理由</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotExecute(string reason)
        {
            return $"{reason}処理できません。";
        }

        /// <summary>
        /// データが存在しない場合のエラーメッセージ
        /// </summary>
        /// <param name="itemName">項目名</param>
        /// <returns>エラーメッセージ</returns>
        public static string NotFound(string itemName)
        {
            return $"{itemName}が見つかりません。";
        }

        /// <summary>
        /// データサイズが超過する場合のエラーメッセージ
        /// </summary>
        /// <param name="maxByte">データ最大バイト数</param>
        /// <returns>エラーメッセージ</returns>
        public static string OverDataSize(int maxByte)
        {
            return $"{maxByte}byte を超えるため、処理できません。";
        }
    }
}