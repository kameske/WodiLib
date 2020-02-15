// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandListReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// キャラ動作指定コマンドリスト読み込みクラス
    /// </summary>
    internal class CharaMoveCommandListReader
    {
        /// <summary>読み込み経過状態</summary>
        private FileReadStatus Status { get; }

        /// <summary>動作指定コマンド行数</summary>
        private int Length { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">動作指定コマンド数</param>
        public CharaMoveCommandListReader(FileReadStatus status, int length)
        {
            Status = status;
            Length = length;
        }

        /// <summary>
        /// 動作指定コマンドリストを読み込み、返す。
        /// </summary>
        /// <returns>イベントコマンドリスト</returns>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        public List<ICharaMoveCommand> Read()
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(EventCommandListReader),
                "イベントコマンドリスト"));

            var charaMoveCommandList = new List<ICharaMoveCommand>();
            for (var i = 0; i < Length; i++)
            {
                ReadCharaMoveCommand(Status, charaMoveCommandList);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(EventCommandListReader),
                "イベントコマンドリスト"));

            return charaMoveCommandList;
        }

        /// <summary>
        /// 動作指定コマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commandList">データ格納先</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private void ReadCharaMoveCommand(FileReadStatus status, ICollection<ICharaMoveCommand> commandList)
        {
            // 動作指定コード
            var charaMoveCode = status.ReadByte();
            CharaMoveCommandCode commandCode;
            try
            {
                commandCode = CharaMoveCommandCode.FromByte(charaMoveCode);
            }
            catch
            {
                throw new InvalidOperationException(
                    $"存在しない動作指定コマンドコードが読み込まれました。" +
                    $"（コマンドコード値：{charaMoveCode}, offset：{status.Offset}");
            }

            var charaMoveCommand = CharaMoveCommandFactory.CreateRaw(commandCode);
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "動作指定コード", charaMoveCode));

            // 変数の数
            var varLength = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "変数の数", charaMoveCode));

            // 変数
            for (var i = 0; i < varLength; i++)
            {
                var value = status.ReadInt();
                charaMoveCommand.SetNumberValue(i, value);
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                    $"変数{i}", value));
            }

            // 終端コードチェック
            foreach (var b in CharaMoveCommandBase.EndBlockCode)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"動作指定コマンド末尾の値が異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(EventCommandListReader),
                $"終端コード"));

            // 結果
            commandList.Add(charaMoveCommand);
        }
    }
}