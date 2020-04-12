// ========================================
// Project Name : WodiLib
// File Name    : ICharaMoveCommand.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    /// キャラ動作指定コマンドインタフェース
    /// </summary>
    public interface ICharaMoveCommand : IEquatable<ICharaMoveCommand>
    {
        /// <summary>
        /// 動作コマンド種別
        /// </summary>
        CharaMoveCommandCode CommandCode { get; }

        /// <summary>変数の数（Byte）</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        byte ValueLengthByte { get; }

        /// <summary>変数の数</summary>
        int ValueLength { get; }

        /// <summary>インデックスを指定して数値を取得する。</summary>
        /// <param name="index">インデックス</param>
        /// <returns>インデックスに対応した数値</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        CharaMoveCommandValue GetNumberValue(int index);

        /// <summary>インデックスを指定して数値を設定する。</summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定する値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetNumberValue(int index, CharaMoveCommandValue value);

        /// <summary>
        /// イベントコマンド文字列を取得する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="type">イベント種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string GetEventCommandSentence(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc);

        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        void OutputVersionWarningLogIfNeed();

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        byte[] ToBinary();
    }
}