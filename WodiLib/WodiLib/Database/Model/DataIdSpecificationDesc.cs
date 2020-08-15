// ========================================
// Project Name : WodiLib
// File Name    : DataIdSpecificationDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// データの設定方法＝指定DBの場合の追加設定情報
    /// </summary>
    public class DataIdSpecificationDesc : ModelBase<DataIdSpecificationDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定DB種別
        /// </summary>
        public DBKind DBKind { get; }

        /// <summary>
        /// 指定DBタイプID
        /// </summary>
        public TypeId TypeId { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbKind">参照DB種別</param>
        /// <param name="typeId">参照タイプID</param>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        public DataIdSpecificationDesc(DBKind dbKind, TypeId typeId)
        {
            if (dbKind == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));

            DBKind = dbKind;
            TypeId = typeId;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override bool Equals(DataIdSpecificationDesc? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DBKind == other.DBKind
                   && TypeId == other.TypeId;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイプコード値（DBタイプによる値 + タイプID）を返却する。
        /// </summary>
        /// <returns>DBタイプによる値 + タイプID</returns>
        internal int ToTypeCode()
            => DBKind.DBDataSettingTypeCode * 10000 + TypeId;
    }
}
