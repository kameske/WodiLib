// ========================================
// Project Name : WodiLib
// File Name    : ICharaMoveCommand.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Event
{
    /// <summary>
    /// キャラ動作指定コマンドインタフェース
    /// </summary>
    public interface ICharaMoveCommand
    {
        /// <summary>
        /// 動作コマンドコード
        /// </summary>
        byte CommandCode { get; }

        /// <summary>変数の数（Byte）</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        byte ValueLengthByte { get; }

        /// <summary>変数の数</summary>
        int ValueLength { get; }

        /// <summary>インデックスを指定して数値変数を取得する。</summary>
        /// <param name="index">インデックス</param>
        /// <returns>インデックスに対応した数値</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        int GetNumberValue(int index);

        /// <summary>インデックスを指定して数値変数を設定する。</summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定する値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetNumberValue(int index, int value);

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