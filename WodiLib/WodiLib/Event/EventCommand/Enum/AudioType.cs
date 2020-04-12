// ========================================
// Project Name : WodiLib
// File Name    : AudioType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Commons;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 音声タイプ
    /// </summary>
    public class AudioType : TypeSafeEnum<AudioType>
    {
        /// <summary>BGM</summary>
        public static readonly AudioType Bgm;

        /// <summary>BGM</summary>
        public static readonly AudioType Bgs;

        /// <summary>BGM</summary>
        public static readonly AudioType Se;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        internal string EventCommandSentence { get; }

        /// <summary>イベントコマンド・時間文字列</summary>
        internal string EventCommandTimeSentence { get; }

        /// <summary>ループフラグ</summary>
        internal bool IsLoop { get; }

        static AudioType()
        {
            Bgm = new AudioType(nameof(Bgm), 0x00,
                "BGM", "処理時間", true);
            Bgs = new AudioType(nameof(Bgs), 0x10,
                "BGS", "処理時間", true);
            Se = new AudioType(nameof(Se), 0x20,
                "SE", "遅延", false);
        }

        private AudioType(string id, byte code,
            string eventCommandSentence, string eventCommandTimeSentence,
            bool isLoop) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
            EventCommandTimeSentence = eventCommandTimeSentence;
            IsLoop = isLoop;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static AudioType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}