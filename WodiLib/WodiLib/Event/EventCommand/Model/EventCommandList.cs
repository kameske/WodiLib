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
    public class EventCommandList : RestrictedCapacityCollection<IEventCommand>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => 999999;

        /// <summary>最小容量</summary>
        public static int MinCapacity => 0;

        /// <summary>イベントコマンド終端</summary>
        public static readonly byte[] EndEventCommand = {0x03, 0x00, 0x00, 0x00};

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventCommandList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">初期イベントコマンドリスト</param>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public EventCommandList(IReadOnlyCollection<IEventCommand> list) : base(list)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンドリストとして適切かどうかを返す。
        /// </summary>
        /// <returns>適切ではないデータの場合、false</returns>
        public bool Validate()
        {
            if (!CheckLastCommand(Items)) return false;

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override IEventCommand MakeDefaultItem(int index) => new Blank();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            if (!CheckLastCommand(Items))
            {
                throw new InvalidOperationException(
                    "コマンド末尾は「空白行」である必要があります。");
            }

            var result = new List<byte>();

            // イベント行数
            result.AddRange(Count.ToWoditorIntBytes());

            // イベントコマンド
            foreach (var command in Items)
            {
                result.AddRange(command.ToBinary());
            }

            // イベントコマンド終端
            result.AddRange(EndEventCommand);

            return result.ToArray();
        }
    }
}