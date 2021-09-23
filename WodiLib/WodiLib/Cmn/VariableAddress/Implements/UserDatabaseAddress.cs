// ========================================
// Project Name : WodiLib
// File Name    : UserDatabaseAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1000000000, 1099999999)] ユーザDBアドレス値
    /// </summary>
    [VariableAddress(MinValue = 1000000000, MaxValue = 1099999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(UserDatabaseAddress), typeof(VariableAddress) }
    )]
    public partial class UserDatabaseAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ﾕｰｻﾞDB({0},{1},{2})[{3} {4} ]";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>タイプID</summary>
        public TypeId TypeId => RawValue.SubInt(6, 2);

        /// <summary>データID</summary>
        public DataId DataId => RawValue.SubInt(2, 4);

        /// <summary>項目ID</summary>
        public ItemId ItemId => RawValue.SubInt(0, 2);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string ResolveEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
        {
            var dataName = resolver.GetDatabaseDataName(DBKind.User, TypeId, DataId).Item2;
            var itemName = resolver.GetDatabaseItemName(DBKind.User, TypeId, ItemId).Item2;

            return string.Format(EventCommandSentenceFormat,
                TypeId, DataId, ItemId, dataName, itemName);
        }
    }
}
