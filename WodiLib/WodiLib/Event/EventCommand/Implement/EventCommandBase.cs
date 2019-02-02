// ========================================
// Project Name : WodiLib
// File Name    : EventCommandBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドのすべて共通処理を定義する。ラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class EventCommandBase : IEventCommand
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>数値変数個数</summary>
        public abstract byte NumberVariableCount { get; }

        /// <inheritdoc />
        /// <summary>イベントコマンドコード</summary>
        public abstract int EventCommandCode { get; }

        /// <inheritdoc />
        /// <summary>インデントの深さ</summary>
        public byte Indent { get; set; }

        /// <inheritdoc />
        /// <summary>文字列変数個数</summary>
        public abstract byte StringVariableCount { get; }

        /// <inheritdoc />
        /// <summary>動作指定ありフラグ</summary>
        public virtual bool HasActionEntry => false;

        /// <inheritdoc />
        /// <summary>キャラ動作指定
        /// <para>動作指定を持たないコマンドの場合、null</para></summary>
        public virtual ActionEntry ActionEntry
        {
            get => null;
            set { }
        }

        /// <inheritdoc />
        public abstract int GetNumberVariable(int index);

        /// <inheritdoc />
        public abstract void SetNumberVariable(int index, int value);

        /// <inheritdoc />
        public abstract string GetStringVariable(int index);

        /// <inheritdoc />
        public abstract void SetStringVariable(int index, string value);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private readonly byte FlgNotHasActionEntry = 0x00;
        private readonly byte FlgHasActionEntry = 0x01;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public virtual byte[] ToBinary()
        {
            var result = new List<byte>();

            // 数値変数の数
            result.AddRange(MakeNumberVariableCountBytes());
            // 数値変数
            result.AddRange(MakeNumberVariableBytes());
            // インデントの深さ
            result.AddRange(MakeIndentBytes());
            // 文字データ数
            result.AddRange(MakeStringVariableCountBytes());
            // 文字列変数
            result.AddRange(MakeStringVariableBytes());
            // 動作指定コマンド
            result.AddRange(MakeActionEntryBytes());

            return result.ToArray();
        }

        private byte[] MakeNumberVariableCountBytes()
        {
            return new[] {NumberVariableCount};
        }

        private byte[] MakeNumberVariableBytes()
        {
            var result = new List<byte>();
            for (var i = 0; i < NumberVariableCount; i++)
            {
                var numBytes = GetNumberVariable(i).ToBytes(Endian.Woditor);
                result.AddRange(numBytes);
            }

            return result.ToArray();
        }

        private byte[] MakeIndentBytes()
        {
            return new[] {Indent};
        }

        private byte[] MakeStringVariableCountBytes()
        {
            return new[] {StringVariableCount};
        }

        private byte[] MakeStringVariableBytes()
        {
            var result = new List<byte>();
            for (var i = 0; i < StringVariableCount; i++)
            {
                var strBytes = new WoditorString(GetStringVariable(i)).StringByte;
                result.AddRange(strBytes);
            }

            return result.ToArray();
        }

        private byte[] MakeActionEntryBytes()
        {
            var result = new List<byte>();
            if (ActionEntry == null)
            {
                result.Add(FlgNotHasActionEntry);
                return result.ToArray();
            }

            result.Add(FlgHasActionEntry);
            result.AddRange(ActionEntry.ToBinary());
            return result.ToArray();
        }
    }
}