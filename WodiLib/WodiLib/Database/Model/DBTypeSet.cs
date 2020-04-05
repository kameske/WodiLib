// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSet.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBタイプセット（XXX.dbtypeset）
    /// </summary>
    [Serializable]
    public class DBTypeSet : ModelBase<DBTypeSet>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルヘッダ
        /// </summary>
        public static readonly byte[] Header =
        {
            0xB9, 0x22, 0x2D, 0x02
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[NotNull] DBタイプ名</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public TypeName TypeName
        {
            get => TypeDesc.TypeName;
            set => TypeDesc.TypeName = value;
        }

        /// <summary>項目設定リスト</summary>
        public DBItemSettingList ItemSettingList => TypeDesc.WritableItemSettingList;

        /// <summary>[NotNull] メモ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DatabaseMemo Memo
        {
            get => TypeDesc.Memo;
            set => TypeDesc.Memo = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DatabaseTypeDesc TypeDesc { get; } = DatabaseTypeDesc.Factory.CreateForDBTypeSet();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイプ情報プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnTypeDescPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(DatabaseTypeDesc.TypeName):
                case nameof(DatabaseTypeDesc.Memo):
                    NotifyPropertyChanged(args.PropertyName);
                    break;

                case nameof(DatabaseTypeDesc.WritableItemSettingList):
                    NotifyPropertyChanged(nameof(ItemSettingList));
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBTypeSet()
        {
            TypeDesc.PropertyChanged += OnTypeDescPropertyChanged;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="itemSettingList">[NotNull] 初期項目設定リスト</param>
        /// <exception cref="ArgumentNullException">dataNameList, itemSettingList が null の場合</exception>
        /// <exception cref="ArgumentException">dataNameList, itemSettingList に null 要素が含まれる場合</exception>
        public DBTypeSet(DBItemSettingList itemSettingList) : this()
        {
            if (itemSettingList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(itemSettingList)));

            TypeDesc = DatabaseTypeDesc.Factory.CreateForDBTypeSet(itemSettingList);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(DBTypeSet other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeDesc.Equals(other.TypeDesc);
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
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(Header);

            // 要素
            result.AddRange(TypeDesc.ToBinaryForDBTypeSet());

            return result.ToArray();
        }
    }
}