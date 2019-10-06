// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthEventCommandShortCutKeyList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// 【長さ固定】イベントコマンドショートカットキーリスト
    /// </summary>
    public interface IFixedLengthEventCommandShortCutKeyList : IFixedLengthCollection<EventCommandShortCutKey>
    {
        /// <summary>
        /// リストの各項目が適切な設定であることを検証する。
        /// </summary>
        /// <remarks>
        /// 以下の点をチェックする。
        /// <pre>「文章の表示」～「ダウンロード処理」でキーが被っていないか</pre>
        /// <pre>使用していない項目に未使用時の値が設定されているか</pre>
        /// </remarks>
        /// <param name="errorMsg">
        ///     返戻エラーメッセージ。
        ///     設定値が適切である場合、null。
        /// </param>
        /// <returns>設定値が適切である場合、true</returns>
        bool Validate(out string errorMsg);
    }
}