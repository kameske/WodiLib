// ========================================
// Project Name : WodiLib
// File Name    : FileIOMessage.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// ファイル読み込み/書き出し汎用メッセージ
    /// </summary>
    internal static class FileIOMessage
    {
        /// <summary>
        /// 読み込み開始メッセージ
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <returns></returns>
        public static string StartFileRead(Type readerClassType)
        {
            return $"{readerClassType.Name} ファイル読み込みを開始します。";
        }

        /// <summary>
        /// 読み込み終了メッセージ
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <returns></returns>
        public static string EndFileRead(Type readerClassType)
        {
            return $"{readerClassType.Name} ファイル読み込みが完了しました。";
        }

        /// <summary>
        /// 書き出し開始メッセージ
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <returns></returns>
        public static string StartFileWrite(Type readerClassType)
        {
            return $"{readerClassType.Name} ファイル書き出しを開始します。";
        }

        /// <summary>
        /// 書き出し終了メッセージ
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <returns></returns>
        public static string EndFileWrite(Type readerClassType)
        {
            return $"{readerClassType.Name} ファイル書き出しが完了しました。";
        }

        /// <summary>
        /// 読み込み開始メッセージ（汎用）
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <param name="description">読み込み内容</param>
        /// <returns></returns>
        public static string StartCommonRead(Type readerClassType, string description)
        {
            return $"{readerClassType.Name} ***** {description}読み込み開始 *****";
        }

        /// <summary>
        /// 読み込み終了メッセージ（汎用）
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <param name="description">読み込み内容</param>
        /// <returns></returns>
        public static string EndCommonRead(Type readerClassType, string description)
        {
            return $"{readerClassType.Name} ***** {description}読み込み完了 *****";
        }

        /// <summary>
        /// チェックOK時のメッセージ
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <param name="itemName">対象項目名</param>
        /// <param name="option">メッセージに付与する文字列</param>
        /// <returns></returns>
        public static string CheckOk(Type readerClassType, string itemName, string option = null)
        {
            return $"{readerClassType} {itemName}チェックOK {option ?? ""}";
        }

        /// <summary>
        /// 項目読み込み時のメッセージ
        /// </summary>
        /// <param name="readerClassType">対象クラス</param>
        /// <param name="itemName">対象項目名</param>
        /// <param name="item">取得値</param>
        /// <returns></returns>
        public static string SuccessRead(Type readerClassType, string itemName, object item)
        {
            return $"{readerClassType} {itemName}: {item}";
        }
    }
}