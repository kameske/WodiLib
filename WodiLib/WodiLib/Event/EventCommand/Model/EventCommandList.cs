// ========================================
// Project Name : WodiLib
// File Name    : EventCommandList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドリスト
    /// </summary>
    public class EventCommandList : RestrictedCapacityList<IEventCommand, EventCommandList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => 999999;

        /// <summary>最小容量</summary>
        public static int MinCapacity => 0;

        /// <summary>イベントコマンド終端</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] EndEventCommand = {0x03, 0x00, 0x00, 0x00};

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private TargetAddressOwner? owner;

        /// <summary>所有イベント種別</summary>
        internal TargetAddressOwner? Owner
        {
            get => owner;
            set
            {
                owner = value;
                this.OfType<MoveRoute>().ToList()
                    .ForEach(x => x.Owner = value);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventCommandList()
        {
            StartObserveListEvent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">初期イベントコマンド列挙</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が不適切な場合</exception>
        public EventCommandList(IEnumerable<IEventCommand> items) : base(items)
        {
            StartObserveListEvent();
        }

        /// <summary>
        /// 独自リストのイベント購読を開始する。コンストラクタ用。
        /// </summary>
        private void StartObserveListEvent()
        {
            CollectionChanged += OnCollectionChanged;
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
            if (!CheckLastCommand(this)) return false;

            return true;
        }

        /// <summary>
        /// イベントコードリストを取得する。
        /// </summary>
        /// <returns>イベントコードリスト</returns>
        public IReadOnlyList<string> GetEventCodeStringList()
            => this.Select(x => x.ToEventCodeString()).ToList();

        /// <summary>
        /// イベントコマンド文字列情報リストを取得する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="type">イベント種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyList<EventCommandSentenceInfo> MakeEventCommandSentenceInfoList(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc? desc)
            => this.Select(x => x.GetEventCommandSentenceInfo(resolver, type, desc)).ToList();

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
        //     Event Handler
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region CollectionChanged

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ExecuteByAction<IEventCommand>(
                addAction: PostInsertItem
            );
        }


        /// <summary>
        /// 要素追加後に呼び出される処理
        /// </summary>
        /// <param name="index">追加するインデックス</param>
        /// <param name="items">追加要素</param>
        private void PostInsertItem(int index, IEnumerable<IEventCommand> items)
        {
            items.ForEach(item =>
            {
                if (item is MoveRoute moveRoute)
                {
                    moveRoute.Owner = Owner;
                }
            });
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コマンド末尾の適正チェック
        /// </summary>
        /// <param name="commands">コマンドリスト</param>
        /// <returns>コマンド末尾が「空白行（Blankクラス）」ではない場合、false</returns>
        private static bool CheckLastCommand(IEnumerable<IEventCommand> commands)
        {
            var lastCommand = commands.LastOrDefault();
            if (!(lastCommand is Blank castedCommand)) return false;

            if (castedCommand.Indent != 0) return false;

            return true;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            if (!CheckLastCommand(this))
            {
                throw new InvalidOperationException(
                    "コマンド末尾は「空白行」である必要があります。");
            }

            var result = new List<byte>();

            // イベント行数
            result.AddRange(Count.ToWoditorIntBytes());

            // イベントコマンド
            foreach (var command in this)
            {
                result.AddRange(command.ToBinary());
            }

            // イベントコマンド終端
            result.AddRange(EndEventCommand);

            return result.ToArray();
        }

        public override bool ItemEquals(EventCommandList? other)
        {
            throw new NotImplementedException();
        }
    }
}
