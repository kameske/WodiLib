// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBタイプ情報クラス
    /// </summary>
    [Serializable]
    public partial class DatabaseTypeDesc : ModelBase<DatabaseTypeDesc>, ISerializable, IDeserializationCallback
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// [NotNull] DBタイプ名
        /// </summary>
        /// <exception cref="PropertyNullException">nullがセットされた場合</exception>
        public TypeName TypeName
        {
            get => TypeSetting.TypeName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(TypeName)));

                TypeSetting.TypeName = value;
            }
        }

        /// <summary>
        /// [NotNull] メモ
        /// </summary>
        /// <exception cref="PropertyNullException">nullがセットされた場合</exception>
        public DatabaseMemo Memo
        {
            get => TypeSetting.Memo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Memo)));

                TypeSetting.Memo = value;
            }
        }

        /// <summary>
        /// データの設定方法
        /// </summary>
        public DBDataSettingType DataSettingType => DataSetting.DataSettingType;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DB種別
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public DBKind DBKind => DataSetting.DBKind;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DBタイプID
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public TypeId TypeId => DataSetting.TypeId;

        /// <summary>
        /// DB項目設定と設定値リスト
        /// </summary>
        public DatabaseItemDescList ItemDescList { get; } = new DatabaseItemDescList();

        /// <summary>
        /// DBデータ設定と設定値リスト
        /// </summary>
        public DatabaseDataDescList DataDescList { get; } = new DatabaseDataDescList();

        /// <summary>（読み取り専用）データ名リスト</summary>
        public IReadOnlyDataNameList DataNameList => TypeSetting.DataNameList;

        /// <summary>
        /// 項目設定リスト（読み取り専用）
        /// </summary>
        public IReadOnlyDBItemSettingList DBItemSettingList => TypeSetting.ItemSettingList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目設定リスト
        /// </summary>
        internal DBItemSettingList WritableItemSettingList => TypeSetting.ItemSettingList;

        /// <summary>
        /// 項目設定値リスト
        /// </summary>
        internal DBItemValuesList WritableItemValuesList => DataSetting.SettingValuesList;

        /// <summary>
        /// データ名リスト
        /// </summary>
        internal DataNameList WritableDataNameList => TypeSetting.DataNameList;

        /// <summary>
        /// タイプ設定
        /// </summary>
        internal DBTypeSetting TypeSetting { get; } = new DBTypeSetting();

        /// <summary>
        /// データ設定
        /// </summary>
        internal DBDataSetting DataSetting { get; } = new DBDataSetting();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイプ設定プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnTypeSettingPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(DBTypeSetting.TypeName):
                case nameof(DBTypeSetting.Memo):
                case nameof(DBTypeSetting.DataNameList):
                case nameof(DBTypeSetting.ItemSettingList):
                    NotifyPropertyChanged(args.PropertyName);
                    break;
            }
        }

        /// <summary>
        /// データ設定プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnDataSettingPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(DBDataSetting.DataSettingType):
                case nameof(DBDataSetting.DBKind):
                case nameof(DBDataSetting.TypeId):
                    NotifyPropertyChanged(args.PropertyName);
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseTypeDesc() : this(BaseListType.Public)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="typeSetting">[NotNull] タイプ設定</param>
        /// <param name="dataSetting">[NotNull] データ設定</param>
        /// <exception cref="ArgumentNullException">typeSetting, dataSetting が null の場合</exception>
        internal DatabaseTypeDesc(DBTypeSetting typeSetting,
            DBDataSetting dataSetting)
        {
            if (typeSetting is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(typeSetting)));
            if (dataSetting is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dataSetting)));

            UpdateItemType(typeSetting, dataSetting);

            TypeSetting = typeSetting;
            DataSetting = dataSetting;

            var itemDescList = new DatabaseItemDescList(typeSetting.ItemSettingList);
            ItemDescList.Overwrite(0, itemDescList);

            var dataDescList = new DatabaseDataDescList(typeSetting.DataNameList, dataSetting.SettingValuesList);
            DataDescList.Overwrite(0, dataDescList);

            TypeSetting.PropertyChanged += OnTypeSettingPropertyChanged;
            DataSetting.PropertyChanged += OnDataSettingPropertyChanged;
            RegisterDataDescListHandlerDataDescList();
            RegisterItemDescListHandlerItemDescList();
        }

        private void UpdateItemType(DBTypeSetting typeSetting,
            DBDataSetting dataSetting)
        {
            // タイプ設定の項目数とデータ設定の項目数=0が一致しない場合がある。データ設定の項目数が真
            var itemCount = dataSetting.SettingValuesList[0].Count;
            typeSetting.ItemSettingList.AdjustLength(itemCount);

            for (var i = 0; i < itemCount; i++)
            {
                typeSetting.ItemSettingList[i].ItemType = dataSetting.SettingValuesList[0][i].Type;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="baseListType">初期化種別</param>
        private DatabaseTypeDesc(BaseListType baseListType)
        {
            TypeSetting.PropertyChanged += OnTypeSettingPropertyChanged;
            DataSetting.PropertyChanged += OnDataSettingPropertyChanged;
            /*
             * 使用する場所によって操作するリストが異なるので、
             * 操作対象のリストへの変更が操作対象ではないリストへ反映されるように
             * イベントハンドラの登録が必要
             */
            switch (baseListType)
            {
                case BaseListType.DBType:
                case BaseListType.Public:
                    RegisterDataDescListHandlerDataDescList();
                    RegisterItemDescListHandlerItemDescList();
                    break;
                case BaseListType.DBData:
                    RegisterDataDescListHandlerDataNameList();
                    RegisterItemDescListHandlerItemValuesList();
                    break;
                case BaseListType.DBTypeSet:
                    RegisterDataDescListHandlerItemValuesList();
                    RegisterItemDescListHandlerItemSettingList();
                    break;
                default:
                    // 通常来ない
                    throw new ArgumentException();
            }
        }

#pragma warning disable 618
        /// <summary>
        /// DataDescList のイベントハンドラ登録を行う。
        /// </summary>
        private void RegisterDataDescListHandlerDataDescList()
        {
            DataDescList.SetItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataDesc.SetItemHandler(this));
            DataDescList.InsertItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataDesc.InsertItemHandler(this));
            DataDescList.RemoveItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataDesc.RemoveItemHandler(this));
            DataDescList.ClearItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataDesc.ClearItemHandler(this));
        }

        /// <summary>
        /// DataNameList のイベントハンドラ登録を行う
        /// </summary>
        private void RegisterDataDescListHandlerDataNameList()
        {
            WritableDataNameList.SetItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataName.SetItemHandler(this));
            WritableDataNameList.InsertItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataName.InsertItemHandler(this));
            WritableDataNameList.RemoveItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataName.RemoveItemHandler(this));
            WritableDataNameList.ClearItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.DataName.ClearItemHandler(this));
        }

        /// <summary>
        /// ItemValuesList のイベントハンドラ登録を行う。
        /// </summary>
        private void RegisterDataDescListHandlerItemValuesList()
        {
            WritableItemValuesList.SetItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.ItemValues.SetItemHandler(this));
            WritableItemValuesList.InsertItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.ItemValues.InsertItemHandler(this));
            WritableItemValuesList.RemoveItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.ItemValues.RemoveItemHandler(this));
            WritableItemValuesList.ClearItemHandlerList.Add(
                new DatabaseTypeDescHandler.DataDescList.ItemValues.ClearItemHandler(this));
        }

        /// <summary>
        /// ItemDescList のイベントハンドラ登録を行う。
        /// </summary>
        private void RegisterItemDescListHandlerItemDescList()
        {
            ItemDescList.SetItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemDesc.SetItemHandler(this));
            ItemDescList.InsertItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemDesc.InsertItemHandler(this));
            ItemDescList.RemoveItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemDesc.RemoveItemHandler(this));
            ItemDescList.ClearItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemDesc.ClearItemHandler(this));
        }

        /// <summary>
        /// ItemSettingList のイベントハンドラ登録を行う
        /// </summary>
        private void RegisterItemDescListHandlerItemSettingList()
        {
            WritableItemSettingList.SetItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemSetting.SetItemHandler(this));
            WritableItemSettingList.InsertItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemSetting.InsertItemHandler(this));
            WritableItemSettingList.RemoveItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemSetting.RemoveItemHandler(this));
            WritableItemSettingList.ClearItemHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemSetting.ClearItemHandler(this));
        }

        /// <summary>
        /// ItemValuesList のイベントハンドラ登録を行う。
        /// </summary>
        private void RegisterItemDescListHandlerItemValuesList()
        {
            WritableItemValuesList.SetFieldHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemValues.SetItemHandler(this));
            WritableItemValuesList.InsertFieldHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemValues.InsertItemHandler(this));
            WritableItemValuesList.RemoveFieldHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemValues.RemoveItemHandler(this));
            WritableItemValuesList.ClearFieldHandlerList.Add(
                new DatabaseTypeDescHandler.ItemDescList.ItemValues.ClearItemHandler(this));
        }
#pragma warning restore 618

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データの設定方法をセットする。
        /// </summary>
        /// <param name="settingType">[NotNull] データの設定方法種別</param>
        /// <param name="dbKind">[Nullable] 種別が「データベース参照」の場合の参照DB種別</param>
        /// <param name="typeId">[Nullable] 種別が「データベース参照」の場合のタイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ dbKindまたはtypeIdがnullの場合
        /// </exception>
        public void SetDataSettingType(DBDataSettingType settingType,
            DBKind dbKind = null, TypeId? typeId = null)
        {
            DataSetting.SetDataSettingType(settingType, dbKind, typeId);
        }

        /// <summary>
        /// 指定したデータIDの項目値リストを取得する。
        /// </summary>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <returns>DB項目値リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">dataId が指定範囲外の場合</exception>
        public DBItemValueList GetItemValueList(DataId dataId)
            => DataDescList[dataId].ItemValueList;

        /// <summary>
        /// 指定したデータID、項目IDの項目値を取得する。
        /// </summary>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <param name="itemId">[Range(0, {対象DB・タイプ・データの項目数} - 1)] データID</param>
        /// <returns>DB項目値リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">dataId, itemId が指定範囲外の場合</exception>
        public DBItemValue GetItemValue(DataId dataId, ItemId itemId)
            => DataDescList[dataId].ItemValueList[itemId];

        /// <summary>
        /// 自身の情報を元にDBTypeSetインスタンスを生成する。
        /// </summary>
        /// <returns>DBTypeインスタンス</returns>
        public DBTypeSet GenerateDBTypeSet()
        {
            var itemSettingList = new DBItemSettingList(ItemDescList.Select(x => x.ToDBItemSetting()).ToList());

            return new DBTypeSet(itemSettingList)
            {
                TypeName = TypeName,
                Memo = Memo,
            };
        }

        /// <summary>
        /// 自身の情報を元にDBTypeインスタンスを生成する。
        /// </summary>
        /// <returns>DBTypeインスタンス</returns>
        public DBType GenerateDBType()
        {
            var result = new DBType
            {
                TypeName = TypeName,
                Memo = Memo,
            };
            result.ItemDescList.Overwrite(0, ItemDescList);
            result.DataDescList.Overwrite(0, DataDescList);

            return result;
        }

        /// <summary>
        /// 自身の情報を元にDBDataインスタンスを生成する。
        /// </summary>
        /// <param name="start">[Range(0, DataDescList.Count - 1)] 始点データID</param>
        /// <param name="count">[Range(0, DataDescList.Count)] 出力データ数</param>
        /// <returns>DBDataインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">start, countが指定範囲外の場合</exception>
        public DBData GenerateDBData(DataId start, int count)
        {
            var dataDescListCount = DataDescList.Count;
            var startMax = dataDescListCount - 1;
            const int startMin = 0;
            if (start > startMax)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(start), startMin, startMax, start));

            var countMax = dataDescListCount;
            const int countMin = 0;
            if (count < countMin || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), countMin, countMax, count));

            if (dataDescListCount - start < count)
                throw new ArgumentException(
                    $"{nameof(start)}および{nameof(count)}が有効な範囲を示していません。");

            var filteredDataDescList = DataDescList.Skip(start).Take(count).ToList();

            var result = new DBData();
            result.DataDescList.Overwrite(0, filteredDataDescList.ToList());

            return result;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(DatabaseTypeDesc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ItemDescList.Equals(other.ItemDescList)
                   && DataDescList.Equals(other.DataDescList)
                   && TypeSetting.Equals(other.TypeSetting)
                   && DataSetting.Equals(other.DataSetting);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBTypeSet用にバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryForDBTypeSet()
        {
            var result = new List<byte>();

            // 項目数 + 設定種別 & 種別順列 + DBタイプ設定
            result.AddRange(TypeSetting.ToBinaryForDBTypeSet());

            return result.ToArray();
        }

        /// <summary>
        /// DBType用にバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryForDBType()
        {
            var result = new List<byte>();

            // DBタイプ設定
            result.AddRange(TypeSetting.ToBinary());

            // DBデータ設定
            result.AddRange(DataSetting.ToBinary());

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Enum
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 操作基準になるリスト種別
        /// </summary>
        private enum BaseListType
        {
            DBTypeSet,
            DBType,
            DBData,
            Public
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(TypeName), TypeName);
            info.AddValue(nameof(Memo), Memo);
            info.AddValue(nameof(ItemDescList), ItemDescList);
            info.AddValue(nameof(DataDescList), DataDescList);
            info.AddValue(nameof(TypeSetting), TypeSetting);
            info.AddValue(nameof(DataSetting), DataSetting);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DatabaseTypeDesc(SerializationInfo info, StreamingContext context)
        {
            TypeName = info.GetValue<TypeName>(nameof(TypeName));
            Memo = info.GetValue<DatabaseMemo>(nameof(Memo));
            ItemDescList = info.GetValue<DatabaseItemDescList>(nameof(ItemDescList));
            DataDescList = info.GetValue<DatabaseDataDescList>(nameof(DataDescList));
            TypeSetting = info.GetValue<DBTypeSetting>(nameof(TypeSetting));
            DataSetting = info.GetValue<DBDataSetting>(nameof(DataSetting));
        }

        /// <summary>
        /// デシリアライズコールバック
        /// </summary>
        /// <param name="sender">コールバック開始オブジェクト</param>
        public void OnDeserialization(object sender)
        {
            RegisterDataDescListHandlerDataDescList();
            RegisterItemDescListHandlerItemDescList();
        }
    }
}