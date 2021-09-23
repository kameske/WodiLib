// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : Sentence.ErrorDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     文章生成処理
    /// </summary>
    internal static class Sentence
    {
        /// <summary>
        ///     エラーメッセージ
        /// </summary>
        public static class ErrorDesc
        {
            /// <summary>
            ///     Null例外メッセージを取得する。
            /// </summary>
            /// <param name="itemName">項目名</param>
            /// <returns>例外メッセージ</returns>
            public static string Null(string itemName) => $@"{itemName} が {Tag.See.Langword_Null} の場合";

            /// <summary>
            ///     OutOfRange例外メッセージを取得する。
            /// </summary>
            /// <param name="itemName">項目名</param>
            /// <param name="min">最小値</param>
            /// <param name="max">最大値</param>
            /// <returns>例外メッセージ</returns>
            public static string OutOfRange(string itemName, string min, string max) =>
                $@"{itemName} が {min} 未満 または {max} を超える場合";
        }
    }
}
