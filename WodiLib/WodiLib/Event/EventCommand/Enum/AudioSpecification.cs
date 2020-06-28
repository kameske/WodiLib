// ========================================
// Project Name : WodiLib
// File Name    : AudioSpecification.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// サウンド・音声指定方法
    /// </summary>
    public class AudioSpecification : TypeSafeEnum<AudioSpecification>
    {
        /// <summary>ファイル名指定</summary>
        public static readonly AudioSpecification FileName;

        /// <summary>SDB直接指定</summary>
        public static readonly AudioSpecification SdbDirect;

        /// <summary>SDB変数指定</summary>
        public static readonly AudioSpecification SdbRefer;

        /// <summary>値</summary>
        public byte Code { get; }

        static AudioSpecification()
        {
            FileName = new AudioSpecification(nameof(FileName), 0x02);
            SdbDirect = new AudioSpecification(nameof(SdbDirect), 0x00);
            SdbRefer = new AudioSpecification(nameof(SdbRefer), 0x01);
        }

        private AudioSpecification(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static AudioSpecification FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
