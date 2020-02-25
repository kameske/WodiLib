// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using WodiLib.Ini;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>イベントコマンド文情報</summary>
    [Serializable]
    public class EventCommandSentenceInfo : IEquatable<EventCommandSentenceInfo>, ISerializable
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
            if (colorSet is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(colorSet)));
            if (sentence is null)
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

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(EventCommandSentenceInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ColorSet.Equals(other.ColorSet)
                   && Sentence.Equals(other.Sentence);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ColorSet), ColorSet.Id);
            info.AddValue(nameof(Sentence), Sentence);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected EventCommandSentenceInfo(SerializationInfo info, StreamingContext context)
        {
            ColorSet = EventCommandColorSet.FromId(info.GetValue<string>(nameof(ColorSet)));
            Sentence = info.GetValue<EventCommandSentence>(nameof(Sentence));
        }
    }
}