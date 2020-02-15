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
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

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

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

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
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(EventCommandListReader),
                "イベントコマンドリスト"));

            var eventCommandList = new List<IEventCommand>();
            for (var i = 0; i < Length; i++)
            {
                ReadEventCommand(Status, eventCommandList);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(EventCommandListReader),
                "イベントコマンドリスト"));

            return new EventCommandList(eventCommandList);
        }

        /// <summary>
        /// イベントコマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commandList">データ格納先</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private void ReadEventCommand(FileReadStatus status, ICollection<IEventCommand> commandList)
        {
            // 数値変数の数
            var numVarLength = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "数値変数の数", numVarLength));

            // 数値変数
            var numVarList = new List<int>();
            for (var i = 0; i < numVarLength; i++)
            {
                var numVar = status.ReadInt();
                numVarList.Add(numVar);
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                    $"数値変数{i}", numVar));
            }

            // インデント
            var indent = (sbyte) status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "インデント", indent));

            // 文字データ数
            var strVarLength = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "文字列変数の数", strVarLength));

            // 文字列変数
            var strVarList = new List<string>();
            for (var i = 0; i < strVarLength; i++)
            {
                var woditorString = status.ReadString();
                strVarList.Add(woditorString.String);
                status.AddOffset(woditorString.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                    $"文字列変数{i}", woditorString.String));
            }

            // 動作指定フラグ
            var hasMoveCommand = status.ReadByte() != 0;
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "動作指定フラグ", hasMoveCommand));

            // 動作指定コマンド
            ActionEntry actionEntry = null;
            if (hasMoveCommand)
            {
                actionEntry = new ActionEntry();
                ReadEventActionEntry(status, actionEntry);
            }

            // 引数の数チェック
            if (numVarLength != numVarList.Count)
                throw new InvalidOperationException(
                    "指定された数値引数の数と実際の数値引数の数が一致しません。");
            if (strVarLength != strVarList.Count)
                throw new InvalidOperationException(
                    "指定された文字列引数の数と実際の文字列引数の数が一致しません。");

            // 結果
            var eventCommand = EventCommandFactory.CreateRaw(
                numVarList,
                indent,
                strVarList,
                actionEntry);

            Logger.Debug("イベントコマンド生成成功");

            commandList.Add(eventCommand);
        }

        /// <summary>
        /// イベントコマンドの動作指定コマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="actionEntry">データ格納先</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private void ReadEventActionEntry(FileReadStatus status, ActionEntry actionEntry)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(EventCommandListReader),
                "動作指定コマンド"));

            // ヘッダチェック
            foreach (var b in ActionEntry.HeaderBytes)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"イベントコマンド中のイベントコマンドヘッダの値が異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(EventCommandListReader),
                "動作指定コマンドヘッダ"));

            // 動作フラグ
            var optionFlag = status.ReadByte();
            actionEntry.SetOptionFlag(optionFlag);
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "数値変数の数", optionFlag));

            // 動作コマンドリスト
            actionEntry.CommandList = ReadCharaMoveCommand(status);

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(EventCommandListReader),
                "動作指定コマンド"));
        }

        /// <summary>
        /// 動作コマンドリスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private CharaMoveCommandList ReadCharaMoveCommand(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(EventCommandListReader),
                "動作コマンドリスト"));

            // 動作コマンド数
            var commandLength = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "動作コマンド数", commandLength));

            // 動作指定コマンド
            var reader = new CharaMoveCommandListReader(status, commandLength);
            var result = reader.Read();

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(EventCommandListReader),
                "動作コマンドリスト"));

            return new CharaMoveCommandList(result);
        }
    }
}