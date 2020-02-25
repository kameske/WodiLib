// ========================================
// Project Name : WodiLib
// File Name    : DatabaseItemDescList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目設定と設定値リスト
    /// </summary>
    [Serializable]
    public class DatabaseItemDescList : RestrictedCapacityCollection<DatabaseItemDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => DBItemSettingList.MaxCapacity;

        /// <summary>最小容量</summary>
        public static int MinCapacity => DBItemSettingList.MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseItemDescList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">初期DB項目設定リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public DatabaseItemDescList(IReadOnlyCollection<DatabaseItemDesc> list) : base(list)
        {
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="itemSettingList">[NotNull] 項目設定リスト</param>
        /// <exception cref="ArgumentNullException">itemSettingList が null の場合</exception>
        internal DatabaseItemDescList(DBItemSettingList itemSettingList)
            : base(new Func<IReadOnlyCollection<DatabaseItemDesc>>(() =>
            {
                if (itemSettingList is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(itemSettingList)));

                var list = new List<DatabaseItemDesc>();

                foreach (var setting in itemSettingList)
                {
                    list.Add(new DatabaseItemDesc
                    {
                        ItemType = setting.ItemType,
                        ItemName = setting.ItemName,
                        SpecialSettingDesc = setting.SpecialSettingDesc
                    });
                }

                return list;
            })())
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
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override DatabaseItemDesc MakeDefaultItem(int index) => new DatabaseItemDesc();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DatabaseItemDescList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}