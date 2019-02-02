// ========================================
// Project Name : WodiLib
// File Name    : AudioType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

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

        static AudioType()
        {
            Bgm = new AudioType(nameof(Bgm), 0x00);
            Bgs = new AudioType(nameof(Bgs), 0x10);
            Se = new AudioType(nameof(Se), 0x20);
        }

        private AudioType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static AudioType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}