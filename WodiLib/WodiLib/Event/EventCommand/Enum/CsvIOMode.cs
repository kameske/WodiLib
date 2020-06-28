// ========================================
// Project Name : WodiLib
// File Name    : CsvIOMode.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// CSV読み書きモード
    /// </summary>
    public class CsvIOMode : TypeSafeEnum<CsvIOMode>
    {
        /// <summary>読み込み</summary>
        public static readonly CsvIOMode Input;

        /// <summary>書き出し</summary>
        public static readonly CsvIOMode Output;

        /// <summary>値</summary>
        public byte Code { get; }

        static CsvIOMode()
        {
            Input = new CsvIOMode(nameof(Input), 0x00);
            Output = new CsvIOMode(nameof(Output), 0x10);
        }

        private CsvIOMode(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static CsvIOMode FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
