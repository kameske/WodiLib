// ========================================
// Project Name : WodiLib
// File Name    : ICharaMoveCommand.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <inheritdoc />
    /// <summary>
    /// キャラ動作指定コマンドインタフェース
    /// </summary>
    public interface ICharaMoveCommand : IWodiLibObject
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
    }
}