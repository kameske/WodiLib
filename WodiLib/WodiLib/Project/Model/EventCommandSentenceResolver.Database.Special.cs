// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolver.Database.Special.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文文字列解決クラス・DB（特殊）
    /// </summary>
    internal class EventCommandSentenceResolver_Database_Special
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string CsvIoEventCommandSentenceFormatWithReferX
            = "{0}DB[{1}:{2}]";

        private const string CsvIoEventCommandSentenceFormatWithoutReferX
            = "{0}DB[{1}:{2}]({3} : {4})";

        private const string SetVariableEventCommandSentenceFormatItemReferX
            = "可変DB[{0}:{1}:{2}]";

        private const string SetVariableEventCommandSentenceFormatItemNotReferX
            = "可変DB[{0}:{1}:{2}]({3})";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>利用元</summary>
        public EventCommandSentenceResolver Master { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="master">利用元</param>
        public EventCommandSentenceResolver_Database_Special(
            EventCommandSentenceResolver master)
        {
            Master = master;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public string GetCsvIoEventCommandSentence(
            CommonEventSentenceResolveDatabaseDesc desc, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc commonDesc)
        {
            if (desc == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));
            if (desc.TypeId == null && desc.TypeName == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(
                        $"{nameof(desc)}.{nameof(desc.TypeId)}" +
                        $"または{nameof(desc)}.{nameof(desc.TypeName)}"));
            if (desc.DataId == null && desc.DataName == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(
                        $"{nameof(desc)}.{nameof(desc.DataId)}" +
                        $"または{nameof(desc)}.{nameof(desc.DataName)}"));

            string paramType;
            string typeName;
            TypeId? typeId;
            if (desc.TypeId != null)
            {
                paramType = Master.GetNumericVariableAddressStringIfVariableAddress(desc.TypeId.Value, type,
                    commonDesc);
                typeName = Master.GetDatabaseTypeName(desc.DbKind, desc.TypeId.Value).Item2;
                typeId = desc.TypeId;
            }
            else
            {
                // desc.TypeName != null
                paramType = desc.TypeName;
                var searchTypeId = Master.GetDatabaseTypeId(desc.DbKind, desc.TypeName).Item1;
                typeId = searchTypeId;
                typeName = searchTypeId == null
                    ? EventCommandSentenceResolver_Database_Basic.DatabaseDataNotFound
                    : desc.TypeName.ToString();
            }

            string paramData;
            string dataName;
            if (desc.DataId != null)
            {
                paramData = Master.GetNumericVariableAddressStringIfVariableAddress(desc.DataId.Value, type,
                    commonDesc);
                dataName = Master.GetDatabaseDataName(desc.DbKind, typeId, desc.DataId.Value).Item2;
            }
            else
            {
                // desc.DataName != null
                paramData = desc.DataName;
                dataName = desc.DataName;
            }

            if ((desc.TypeId != null && desc.TypeId.Value.IsVariableAddressSimpleCheck())
                || (desc.DataId != null && desc.DataId.Value.IsVariableAddressSimpleCheck()))
            {
                // 指定されたタイプIDまたはデータIDが変数アドレスの場合は専用フォーマット
                return string.Format(CsvIoEventCommandSentenceFormatWithReferX,
                    desc.DbKind.EventCommandSentence, paramType, paramData);
            }

            return string.Format(CsvIoEventCommandSentenceFormatWithoutReferX,
                desc.DbKind.EventCommandSentence, paramType, paramData,
                typeName, dataName);
        }

        public string GetDatabaseCommandSentenceForSetVariable(
            int typeId, int dataId, int itemId, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var typeStr = Master.GetVariableAddressStringIfVariableAddress(
                typeId, type, desc);
            var dataStr = Master.GetVariableAddressStringIfVariableAddress(
                dataId, type, desc);
            var itemStr = Master.GetVariableAddressStringIfVariableAddress(
                itemId, type, desc);

            if (itemId.IsVariableAddressSimpleCheck())
            {
                return string.Format(SetVariableEventCommandSentenceFormatItemReferX,
                    typeStr, dataStr, itemStr);
            }

            var itemName = Master.GetDatabaseItemName(DBKind.Changeable, typeId, itemId).Item2;

            return string.Format(SetVariableEventCommandSentenceFormatItemNotReferX,
                typeStr, dataStr, itemStr, itemName);
        }
    }
}