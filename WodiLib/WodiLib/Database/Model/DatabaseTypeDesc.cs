// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeDesc.cs
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
        [Obsolete("このプロパティは Ver 1.6 で廃止します。代わりに ReferDatabaseDesc プロパティを参照してください。")]
        public DBKind DBKind => DataSetting.DBKind;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DBタイプID
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        [Obsolete("このプロパティは Ver 1.6 で廃止します。代わりに ReferDatabaseDesc プロパティを参照してください。")]
        public TypeId TypeId => DataSetting.TypeId;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DB情報
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public DataIdSpecificationDesc ReferDatabaseDesc => DataSetting.ReferDatabaseDesc;

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
#pragma warning disable 618 // TODO: Ver 1.6 まで
                case nameof(DBDataSetting.DBKind):
                case nameof(DBDataSetting.TypeId):
#pragma warning restore 618
                case nameof(DBDataSetting.ReferDatabaseDesc):
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
            DataDescList.CollectionChanged += DataDescListCollectionChanged;
            ItemDescList.CollectionChanged += ItemDescListCollectionChanged;
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
                    DataDescList.CollectionChanged += DataDescListCollectionChanged;
                    ItemDescList.CollectionChanged += ItemDescListCollectionChanged;
                    break;
                case BaseListType.DBData:
                    WritableDataNameList.CollectionChanged += WritableDataNameListChanged;
                    WritableItemValuesList.FieldCollectionChanged += WritableItemValuesListChanged_DBData;
                    break;
                case BaseListType.DBTypeSet:
                    WritableItemValuesList.CollectionChanged += WritableItemValuesListChanged_DBTypeSet;
                    WritableItemSettingList.CollectionChanged += WritableItemSettingListChanged;
                    break;
                default:
                    // 通常来ない
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// DataDescList のイベントハンドラ。
        /// </summary>
        private void DataDescListCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.Cast<DatabaseDataDesc>().ToArray();
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    WritableItemValuesList.InsertRange(args.NewStartingIndex,
                        newItems.Select(item => item.ItemValueList));
                    WritableDataNameList.InsertRange(args.NewStartingIndex, newItems.Select(item => item.DataName));
                    break;

                case NotifyCollectionChangedAction.Move:
                    var newItemsCount = args.NewItems.Count;
                    WritableItemValuesList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    WritableDataNameList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldItemsCount = args.OldItems.Count;
                    WritableItemValuesList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    WritableDataNameList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var i = 0;
                    foreach (var item in newItems)
                    {
                        WritableItemValuesList[args.NewStartingIndex + i] = new DBItemValueList(item.ItemValueList);
                        WritableDataNameList[args.NewStartingIndex + i++] = item.DataName;
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    WritableItemValuesList.Clear();
                    WritableDataNameList.Clear();
                    break;

                default:
                    // 通常ここには来ない
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// WritableDataNameList のイベントハンドラ。
        /// </summary>
        private void WritableDataNameListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.Cast<DataName>().ToArray();
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var valuesListAndNameList = newItems.Select(item =>
                            (item, WritableItemValuesList.CreateValueListInstance()))
                        .ToList();

                    DataDescList.InsertRange(args.NewStartingIndex,
                        valuesListAndNameList.Select(valuesListAndName =>
                            new DatabaseDataDesc(valuesListAndName.item, valuesListAndName.Item2)));
                    WritableItemValuesList.InsertRange(args.NewStartingIndex,
                        valuesListAndNameList.Select(item => item.Item2));
                    break;

                case NotifyCollectionChangedAction.Move:
                    var newItemsCount = args.NewItems.Count;
                    DataDescList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    WritableItemValuesList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldItemsCount = args.OldItems.Count;
                    DataDescList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    WritableItemValuesList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var i = 0;
                    foreach (var item in newItems)
                    {
                        var valuesList = WritableItemValuesList.CreateValueListInstance();
                        DataDescList[args.NewStartingIndex + i] =
                            new DatabaseDataDesc(item, valuesList);
                        WritableItemValuesList[args.NewStartingIndex + i++] = valuesList;
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    DataDescList.Clear();
                    WritableItemValuesList.Clear();
                    break;

                default:
                    // 通常ここには来ない
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// WritableItemValuesList のイベントハンドラ。
        /// </summary>
        private void WritableItemValuesListChanged_DBTypeSet(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.Cast<IFixedLengthDBItemValueList>().ToArray();
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var valuesListAndNameList = newItems.Select(item =>
                            (new DataName(""), (DBItemValueList) item))
                        .ToList();

                    DataDescList.InsertRange(args.NewStartingIndex,
                        valuesListAndNameList.Select(valuesListAndName =>
                            new DatabaseDataDesc(valuesListAndName.Item1, valuesListAndName.Item2)));
                    WritableDataNameList.InsertRange(args.NewStartingIndex,
                        valuesListAndNameList.Select(item => item.Item1));
                    break;

                case NotifyCollectionChangedAction.Move:
                    var newItemsCount = args.NewItems.Count;
                    DataDescList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    WritableDataNameList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldItemsCount = args.OldItems.Count;
                    DataDescList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    WritableDataNameList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var i = 0;
                    foreach (var item in newItems)
                    {
                        var name = new DataName("");
                        DataDescList[args.NewStartingIndex + i] =
                            new DatabaseDataDesc(name, (DBItemValueList) item);
                        WritableDataNameList[args.NewStartingIndex + i++] = name;
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    DataDescList.Clear();
                    WritableDataNameList.Clear();
                    break;

                default:
                    // 通常ここには来ない
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// ItemDescList のイベントハンドラ。
        /// </summary>
        private void ItemDescListCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.Cast<DatabaseItemDesc>().ToArray();
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    WritableItemSettingList.InsertRange(args.NewStartingIndex,
                        newItems.Select(item => item.ToDBItemSetting()));
                    WritableItemValuesList.InsertFieldRange(args.NewStartingIndex,
                        newItems.Select(item => item.ItemType.DBItemDefaultValue));
                    break;

                case NotifyCollectionChangedAction.Move:
                    var newItemsCount = args.NewItems.Count;
                    WritableItemSettingList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    WritableItemValuesList.MoveFieldRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldItemsCount = args.OldItems.Count;
                    WritableItemSettingList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    WritableItemValuesList.RemoveFieldRange(args.OldStartingIndex, oldItemsCount);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var i = 0;
                    foreach (var item in newItems)
                    {
                        WritableItemSettingList[args.NewStartingIndex + i] = item.ToDBItemSetting();
                        WritableItemValuesList.SetField(args.NewStartingIndex + i++, item.ItemType.DBItemDefaultValue);
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    WritableItemSettingList.Clear();
                    WritableItemValuesList.ClearField();
                    break;

                default:
                    // 通常ここには来ない
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// WritableItemSettingList のイベントハンドラ。
        /// </summary>
        private void WritableItemSettingListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.Cast<DBItemSetting>().ToArray();
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var descList = newItems.Select(item => new DatabaseItemDesc
                    {
                        ItemName = item.ItemName,
                        SpecialSettingDesc = item.SpecialSettingDesc,
                        ItemType = item.ItemType,
                    }).ToList();

                    ItemDescList.InsertRange(args.NewStartingIndex, descList);
                    WritableItemValuesList.InsertFieldRange(args.NewStartingIndex,
                        descList.Select(item => item.ItemType.DBItemDefaultValue));
                    break;

                case NotifyCollectionChangedAction.Move:
                    var newItemsCount = args.NewItems.Count;
                    ItemDescList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    WritableItemValuesList.MoveFieldRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldItemsCount = args.OldItems.Count;
                    ItemDescList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    WritableItemValuesList.RemoveFieldRange(args.OldStartingIndex, oldItemsCount);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var i = 0;
                    foreach (var item in newItems)
                    {
                        var desc = new DatabaseItemDesc
                        {
                            ItemName = item.ItemName,
                            SpecialSettingDesc = item.SpecialSettingDesc,
                            ItemType = item.ItemType,
                        };
                        ItemDescList[args.NewStartingIndex + i] = desc;
                        WritableItemValuesList.SetField(args.NewStartingIndex + i++, item.ItemType.DBItemDefaultValue);
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    ItemDescList.Clear();
                    WritableItemValuesList.ClearField();
                    break;

                default:
                    // 通常ここには来ない
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// WritableItemValuesList のイベントハンドラ。
        /// </summary>
        private void WritableItemValuesListChanged_DBData(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.Cast<DBItemValue>().ToArray();
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var descList = newItems.Select(item => new DatabaseItemDesc
                    {
                        ItemName = "",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(),
                        ItemType = item.Type,
                    }).ToList();
                    ItemDescList.InsertRange(args.NewStartingIndex, descList);
                    WritableItemSettingList.InsertRange(args.NewStartingIndex,
                        descList.Select(item => item.ToDBItemSetting()));
                    break;

                case NotifyCollectionChangedAction.Move:
                    var newItemsCount = args.NewItems.Count;
                    ItemDescList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    WritableItemSettingList.MoveRange(args.OldStartingIndex, args.NewStartingIndex, newItemsCount);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldItemsCount = args.OldItems.Count;
                    ItemDescList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    WritableItemSettingList.RemoveRange(args.OldStartingIndex, oldItemsCount);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var i = 0;
                    foreach (var item in newItems)
                    {
                        var desc = new DatabaseItemDesc
                        {
                            ItemName = "",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(),
                            ItemType = item.Type,
                        };
                        ItemDescList[args.NewStartingIndex + i] = desc;
                        WritableItemSettingList[args.NewStartingIndex + i++] = desc.ToDBItemSetting();
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    ItemDescList.Clear();
                    WritableItemSettingList.Clear();
                    break;

                default:
                    // 通常ここには来ない
                    throw new InvalidOperationException();
            }
        }

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
        [Obsolete("このメソッドは Ver 1.6 で廃止します。" +
                  "代わりに SetDataSettingType(DBDataSettingType, ReferDatabaseDesc) メソッドを使用してください。" +
                  "第2,第3引数を省略している場合はこの警告を無視して構いません。（Ver 1.6で警告が消えます）")]
        public void SetDataSettingType(DBDataSettingType settingType,
            DBKind dbKind = null, TypeId? typeId = null)
        {
            DataSetting.SetDataSettingType(settingType, dbKind, typeId);
        }

        /// <summary>
        /// データの設定方法をセットする。
        /// </summary>
        /// <param name="settingType">[NotNull] データの設定方法種別</param>
        /// <param name="referDatabaseDesc">[Nullable] 種別が「データベース参照」の場合の参照DB情報</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ referDatabaseDescがnullの場合
        /// </exception>
        public void SetDataSettingType(DBDataSettingType settingType,
            DataIdSpecificationDesc referDatabaseDesc /* = null */) // TODO: Ver 1.6 以降 referDatabaseDesc のデフォルト値を null とする
        {
            DataSetting.SetDataSettingType(settingType, referDatabaseDesc);
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
            DataDescList.CollectionChanged += DataDescListCollectionChanged;
            ItemDescList.CollectionChanged += ItemDescListCollectionChanged;
        }
    }
}
