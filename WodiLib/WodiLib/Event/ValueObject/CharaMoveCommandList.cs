// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    /// キャラ動作指定コマンドリスト
    /// </summary>
    [Serializable]
    public class CharaMoveCommandList : RestrictedCapacityList<ICharaMoveCommand>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary> // 300k行までは動作確認
        public static int MaxCapacity => int.MaxValue;

        /// <summary>最小容量</summary>
        public static int MinCapacity => 0;

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
                Items.OfType<AddValue>().ToList()
                    .ForEach(x => x.Owner = value);
                Items.OfType<AssignValue>().ToList()
                    .ForEach(x => x.Owner = value);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>所有イベント保持フラグ</summary>
        private bool HasOwner => !(owner is null);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharaMoveCommandList()
        {
            StartObserveListEvent();
        }

        /// <summary>
        /// コンストラクタ（初期値指定）
        /// </summary>
        /// <param name="items">初期要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が不適切な場合</exception>
        public CharaMoveCommandList(IEnumerable<ICharaMoveCommand> items) : base(items)
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
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override ICharaMoveCommand MakeDefaultItem(int index) => new MoveUp();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Event Handler
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region CollectionChanged

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ExecuteByAction<ICharaMoveCommand>(
                addAction: PostInsertItem
            );
        }

        /// <summary>
        /// 要素追加後に呼び出される処理
        /// </summary>
        /// <param name="index">追加するインデックス</param>
        /// <param name="items">追加要素</param>
        private void PostInsertItem(int index, IEnumerable<ICharaMoveCommand> items)
        {
            items.ForEach(item =>
            {
                // AddValue, AssignValueの値を保有イベントによって変化させるための設定
                switch (item)
                {
                    case AddValue addValue:
                        addValue.Owner = Owner;
                        break;
                    case AssignValue assignValue:
                        assignValue.Owner = Owner;
                        break;
                }
            });
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(HasOwner), HasOwner);
            if (HasOwner) info.AddValue(nameof(owner), owner!.Id);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected CharaMoveCommandList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var savedOwner = info.GetBoolean(nameof(HasOwner));
            if (savedOwner) owner = TargetAddressOwner.FromId(info.GetValue<string>(nameof(owner)));
        }
    }
}
