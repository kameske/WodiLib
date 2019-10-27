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
    /// <summary>イベントコマンド分情報</summary>
    public class EventCommandSentenceInfo
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベントコマンドカラーセット</summary>
        public EventCommandColorSet ColorSet { get; }

        /// <summary>イベントコマンド文</summary>
        public EventCommandSentence Sentence { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="colorSet">[NotNull] カラーセット</param>
        /// <param name="sentence">[NotNull] コマンド文</param>
        /// <exception cref="ArgumentNullException">colorSet, sentence が null の場合</exception>
        public EventCommandSentenceInfo(EventCommandColorSet colorSet,
            EventCommandSentence sentence)
        {
            if (colorSet == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(colorSet)));
            if (sentence == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(sentence)));

            ColorSet = colorSet;
            Sentence = sentence;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コマンドカラー種別からコマンドカラーを取得する。
        /// </summary>
        /// <param name="type">[NotNull] コマンドカラー種別</param>
        /// <returns>コマンドカラー</returns>
        /// <exception cref="ArgumentNullException">type が null の場合</exception>
        public Color GetCommandColor(CommandColorType type)
            => ColorSet.GetCommandColor(type);
    }
}