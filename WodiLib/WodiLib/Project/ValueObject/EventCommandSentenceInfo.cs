// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Drawing;
using WodiLib.Ini;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>イベントコマンド文情報</summary>
    [CommonMultiValueObject]
    public partial record EventCommandSentenceInfo
    {
        /// <summary>イベントコマンドカラーセット</summary>
        public EventCommandColorSet ColorSet { get; }

        /// <summary>イベントコマンド文</summary>
        public EventCommandSentence Sentence { get; }

        /// <summary>
        ///     コマンドカラー種別からコマンドカラーを取得する。
        /// </summary>
        /// <param name="type">コマンドカラー種別</param>
        /// <returns>コマンドカラー</returns>
        /// <exception cref="ArgumentNullException">type が null の場合</exception>
        public Color GetCommandColor(CommandColorType type)
            => ColorSet.GetCommandColor(type);
    }
}
