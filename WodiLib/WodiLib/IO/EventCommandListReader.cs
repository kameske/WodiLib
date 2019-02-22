// ========================================
// Project Name : WodiLib
// File Name    : EventCommandListReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Event;
using WodiLib.Event.EventCommand;

namespace WodiLib.IO
{
    /// <summary>
    /// イベントコマンド読み込みクラス
    /// </summary>
    internal class EventCommandListReader
    {
        /// <summary>読み込み経過状態</summary>
        private FileReadStatus Status { get; }

        /// <summary>イベントコマンド行数</summary>
        private int Length { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">イベントコマンド数</param>
        public EventCommandListReader(FileReadStatus status, int length)
        {
            Status = status;
            Length = length;
        }

        /// <summary>
        /// イベントコマンドリストを読み込み、返す。
        /// </summary>
        /// <returns>イベントコマンドリスト</returns>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        public EventCommandList Read()
        {
            var eventCommandList = new List<IEventCommand>();
            for (var i = 0; i < Length; i++)
            {
                ReadEventCommand(Status, eventCommandList);
            }

            return new EventCommandList(eventCommandList);
        }

        /// <summary>
        /// イベントコマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commandList">データ格納先</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private static void ReadEventCommand(FileReadStatus status, ICollection<IEventCommand> commandList)
        {
            // 数値変数の数
            var numVarLength = status.ReadByte();
            status.IncreaseByteOffset();

            // 数値変数
            var numVarList = new List<int>();
            for (var i = 0; i < numVarLength; i++)
            {
                var numVar = status.ReadInt();
                numVarList.Add(numVar);
                status.IncreaseIntOffset();
            }

            // インデント
            var indent = status.ReadByte();
            status.IncreaseByteOffset();

            // 文字データ数
            var strVarLength = status.ReadByte();
            status.IncreaseByteOffset();

            // 文字列変数
            var strVarList = new List<string>();
            for (var i = 0; i < strVarLength; i++)
            {
                var woditorString = status.ReadString();
                strVarList.Add(woditorString.String);
                status.AddOffset(woditorString.ByteLength);
            }

            // 動作指定フラグ
            var hasMoveCommand = status.ReadByte() != 0;
            status.IncreaseByteOffset();

            // 動作指定コマンド
            ActionEntry actionEntry = null;
            if (hasMoveCommand)
            {
                actionEntry = new ActionEntry();
                ReadEventActionEntry(status, actionEntry);
            }

            // 結果
            var eventCommand = EventCommandFactory.CreateRaw(
                numVarLength, numVarList,
                indent,
                strVarLength, strVarList,
                actionEntry);
            commandList.Add(eventCommand);
        }

        /// <summary>
        /// イベントコマンドの動作指定コマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="actionEntry">データ格納先</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private static void ReadEventActionEntry(FileReadStatus status, ActionEntry actionEntry)
        {
            // ヘッダチェック
            foreach (var b in ActionEntry.HeaderBytes)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"イベントコマンド中の動作指定ヘッダの値が異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            // 動作フラグ
            actionEntry.SetOptionFlag(status.ReadByte());
            status.IncreaseByteOffset();

            // 動作コマンド数
            var commandLength = status.ReadInt();
            status.IncreaseIntOffset();

            // 動作指定コマンド
            var charaMoveCommandList = new List<ICharaMoveCommand>();
            for (var i = 0; i < commandLength; i++)
            {
                ReadCharaMoveCommand(status, charaMoveCommandList);
            }

            actionEntry.CommandList = charaMoveCommandList;
        }

        /// <summary>
        /// 動作指定コマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commandList">データ格納先</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private static void ReadCharaMoveCommand(FileReadStatus status, ICollection<ICharaMoveCommand> commandList)
        {
            // 動作指定コード
            var charaMoveCode = status.ReadByte();
            var charaMoveCommand = CharaMoveCommandFactory.CreateRaw(charaMoveCode);
            status.IncreaseByteOffset();

            // 変数の数
            var varLength = status.ReadByte();
            status.IncreaseByteOffset();

            // 変数
            for (var i = 0; i < varLength; i++)
            {
                var variable = status.ReadInt();
                charaMoveCommand.SetNumberValue(i, variable);
                status.IncreaseIntOffset();
            }

            // 終端コードチェック
            foreach (var b in Event.CharaMoveCommand.CharaMoveCommandBase.EndBlockCode)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"動作指定コマンド末尾の値が異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            // 結果
            commandList.Add(charaMoveCommand);
        }
    }
}