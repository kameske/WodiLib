// ========================================
// Project Name : WodiLib
// File Name    : EventCommandList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドリスト
    /// </summary>
    public class EventCommandList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベント行数最大数</summary>
        public static readonly int LineMax = 999999;

        /// <summary>イベント行数最小数</summary>
        public static readonly int LineMin = 0;

        /// <summary>イベントコマンド終端</summary>
        public static readonly byte[] EndEventCommand = {0x03, 0x00, 0x00, 0x00};

        /// <summary>行数</summary>
        public int Count => commandList.Count;

        /// <summary>コマンドリスト</summary>
        private readonly List<IEventCommand> commandList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commands">[NotNull][LengthRange(0, 999999)]  イベントコマンド</param>
        /// <exception cref="ArgumentNullException">pagesがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">pagesの長さが指定範囲以外の場合</exception>
        public EventCommandList(IEnumerable<IEventCommand> commands)
        {
            if (commands == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(commands)));
            var mapEventOnePages = commands.ToList();
            if (!ValidateCommandListLength(mapEventOnePages))
                throw new ArgumentException(
                    ErrorMessage.LengthRange(nameof(commands), LineMin, LineMax, mapEventOnePages.Count));
            commandList = mapEventOnePages;
        }

        /// <summary>
        /// イベントコマンドを追加する。
        /// </summary>
        /// <param name="eventCommand">イベントコマンド</param>
        /// <exception cref="ArgumentNullException">mapEventがnullの場合</exception>
        /// <exception cref="InvalidOperationException">行数が10を超える場合</exception>
        public void Add(IEventCommand eventCommand)
        {
            if (eventCommand == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(eventCommand)));
            if (!ValidateCommandListLength(commandList.Count + 1))
                throw new InvalidOperationException(
                    $"コマンド数が{LineMax}を超えるため、これ以上追加できません。");
            commandList.Add(eventCommand);
        }

        /// <summary>
        /// イベントコマンドを追加する。
        /// </summary>
        /// <param name="eventCommands">[NotNull] イベントコマンドリスト</param>
        /// <exception cref="ArgumentNullException">eventCommandsがnullの場合</exception>
        /// <exception cref="InvalidOperationException">行数が999999を超える場合</exception>
        public void AddRange(IEnumerable<IEventCommand> eventCommands)
        {
            if (eventCommands == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(eventCommands)));

            var argCommands = eventCommands.ToList();

            if (!ValidateCommandListLength(commandList.Count + argCommands.Count))
                throw new InvalidOperationException(
                    $"行数が{LineMax}を超えるため、これ以上追加できません。");
            commandList.AddRange(argCommands);
        }

        /// <summary>
        /// イベントコマンドを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 行数-1)] インデックス</param>
        /// <param name="item">[NotNull] 挿入する項目</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">行数が999999を超える場合</exception>
        public void Insert(int index, IEventCommand item)
        {
            if (index < 0 || commandList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"行数-1(={commandList.Count - 1})", index));
            if (item == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));
            if (!ValidateCommandListLength(commandList.Count + 1))
                throw new InvalidOperationException(
                    $"行数が{LineMax}を超えるため、これ以上追加できません。");
            commandList.Insert(index, item);
        }

        /// <summary>
        /// イベントコマンドを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, 行数-1)] インデックス</param>
        /// <param name="collection">挿入するイベントコマンド</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">行数が999999を超える場合</exception>
        public void InsertRange(int index, IEnumerable<IEventCommand> collection)
        {
            if (index < 0 || commandList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"行数-1(={commandList.Count - 1})", index));
            if (collection == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(collection)));
            var collectionList = collection.ToList();
            if (!ValidateCommandListLength(commandList.Count + collectionList.Count))
                throw new InvalidOperationException(
                    $"行数が{LineMax}を超えるため、これ以上追加できません。");

            commandList.InsertRange(index, collectionList);
        }

        /// <summary>
        /// リストからアイテムを除去する。
        /// </summary>
        /// <param name="item">[NotNull] 削除アイテム</param>
        /// <returns>削除成否</returns>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果行数が0以下になる場合</exception>
        public bool Remove(IEventCommand item)
        {
            if (item == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));

            // 削除する要素が見つからない場合は何もしない
            var delItem = commandList.SingleOrDefault(x => x == item);
            if (delItem == default(IEventCommand)) return false;

            // 削除した結果最小行数を下回る場合はエラー
            if (commandList.Count - 1 < LineMin)
                throw new InvalidOperationException(
                    $"行数が{LineMin}を下回るため、これ以上削除できません。");

            return commandList.Remove(item);
        }

        /// <summary>
        /// インデックスを指定してイベントコマンドを削除する。
        /// </summary>
        /// <param name="index">[Range(0, 行数-1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">index が指定範囲以外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果行数が0以下になる場合</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || commandList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"行数-1(={commandList.Count - 1})", index));
            if (commandList.Count - 1 < LineMin)
                throw new InvalidOperationException(
                    $"行数が{LineMin}を下回るため、これ以上削除できません。");

            commandList.RemoveAt(index);
        }

        /// <summary>
        /// イベントコマンドを範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, 行数-1)] 開始index</param>
        /// <param name="count">[Range(0, 行数-1)] 削除数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, count が指定範囲以外</exception>
        /// <exception cref="InvalidOperationException">削除した結果行数が0以下になる</exception>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || commandList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"行数-1(={commandList.Count - 1})", index));
            if (count < 0 || commandList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), 0, $"行数-1(={commandList.Count}-1)", count));
            if (count <= 0) return;

            var removeItemCount = count - index;
            if (!ValidateCommandListLength(commandList.Count - removeItemCount))
                throw new InvalidOperationException(
                    $"行数が{LineMin}を下回るため、これ以上削除できません。");
            commandList.RemoveRange(index, count);
        }

        /// <summary>
        /// イベントコマンドを取得する。
        /// </summary>
        /// <param name="index">[Range(0, 行数-1)] インデックス</param>
        /// <returns>イベントコマンド</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public IEventCommand Get(int index)
        {
            if (index < 0 || commandList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"行数-1(={commandList.Count - 1})", index));

            return commandList[index];
        }

        /// <summary>
        /// イベントコマンドを範囲取得する。
        /// </summary>
        /// <param name="index">[Range(0, 行数-1)] インデックス</param>
        /// <param name="count">[Range(0, 行数)] 取得数</param>
        /// <returns>イベントコマンドリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">取得範囲が行数を超える場合</exception>
        public IEnumerable<IEventCommand> GetRange(int index, int count)
        {
            if (index < 0 || commandList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"行数-1(={commandList.Count - 1})", index));
            if (count < 0 || commandList.Count < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), 0, $"行数(={commandList.Count})", count));
            if (index + count > commandList.Count)
                throw new ArgumentException(
                    $"index + count <= 行数である必要があります。" +
                    $"（index: {index}, count:{count}, 行数：{commandList.Count}");

            return commandList.GetRange(index, count);
        }

        /// <summary>
        /// すべてのイベントコマンドを返す。
        /// </summary>
        /// <returns>イベントコマンドリスト</returns>
        public IEnumerable<IEventCommand> GetAll()
        {
            return commandList;
        }

        /// <summary>
        /// マップイベントページリストの長さが適切かどうか検証する。
        /// </summary>
        /// <param name="pages">[NotNull] マップイベントページリスト</param>
        /// <returns>適切な場合true</returns>
        /// <exception cref="ArgumentNullException">pagesがnullの場合</exception>
        public static bool ValidateCommandListLength(IEnumerable<IEventCommand> pages)
        {
            if (pages == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(pages)));
            var mapEventOnePages = pages as IEventCommand[] ?? pages.ToArray();
            return ValidateCommandListLength(mapEventOnePages.Length);
        }

        private static bool ValidateCommandListLength(int length)
        {
            return (LineMin <= length && length <= LineMax);
        }

        /// <summary>
        /// イベントコマンドリストとして適切かどうかを返す。
        /// </summary>
        /// <returns>適切ではないデータの場合、false</returns>
        public bool Validate()
        {
            if (!CheckLastCommand(commandList)) return false;

            return true;
        }

        /// <summary>
        /// コマンド末尾の適正チェック
        /// </summary>
        /// <param name="commands">コマンドリスト</param>
        /// <returns>コマンド末尾が「空白行（Blankクラス）」ではない場合、false</returns>
        private static bool CheckLastCommand(IEnumerable<IEventCommand> commands)
        {
            var lastCommand = commands.LastOrDefault();
            if (lastCommand == null) return false;
            return lastCommand is Blank;
        }

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            if (!CheckLastCommand(commandList))
            {
                throw new InvalidOperationException(
                    "コマンド末尾は「空白行」である必要があります。");
            }
            var result = new List<byte>();

            // イベント行数
            result.AddRange(Count.ToWoditorIntBytes());

            // イベントコマンド
            foreach (var command in commandList)
            {
                result.AddRange(command.ToBinary());
            }

            // イベントコマンド終端
            result.AddRange(EndEventCommand);

            return result.ToArray();
        }
    }
}