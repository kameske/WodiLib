// ========================================
// Project Name : WodiLib
// File Name    : IEventCommand.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドインタフェース
    /// </summary>
    public interface IEventCommand
    {
        /// <summary>数値変数個数</summary>
        byte NumberVariableCount { get; }

        /// <summary>イベントコマンドコード</summary>
        EventCommandCode EventCommandCode { get; }

        /// <summary>インデントの深さ</summary>
        byte Indent { get; set; }

        /// <summary>文字列変数個数</summary>
        byte StringVariableCount { get; }

        /// <summary>動作指定ありフラグ</summary>
        bool HasActionEntry { get; }

        /// <summary>キャラ動作指定
        /// <para>動作指定を持たないコマンドの場合、null</para></summary>
        ActionEntry ActionEntry { get; set; }

        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>数値変数</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        int GetNumberVariable(int index);

        /// <summary>
        /// インデックスを指定して数値変数を設定する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetNumberVariable(int index, int value);

        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>設定値</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        string GetStringVariable(int index);

        /// <summary>
        /// インデックスを指定して数値変数を設定する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetStringVariable(int index, string value);

        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        void OutputVersionWarningLogIfNeed();

        /// <summary>バイナリデータに変換する</summary>
        /// <returns>バイナリデータ</returns>
        byte[] ToBinary();
    }
}