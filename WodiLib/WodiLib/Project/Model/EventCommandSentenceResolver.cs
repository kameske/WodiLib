// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolver.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Text;
using WodiLib.Cmn;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Event;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文文字列解決クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EventCommandSentenceResolver
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string IndentStr = " |";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>コモンイベントリスト</summary>
        public CommonEventList CommonEventList { get; }

        /// <summary>可変DB</summary>
        public DatabaseMergedData ChangeableDatabase { get; }

        /// <summary>ユーザDB</summary>
        public DatabaseMergedData UserDatabase { get; }

        /// <summary>システムDB</summary>
        public DatabaseMergedData SystemDatabase { get; }

        /// <summary>読み込んだマップデータ</summary>
        public MapData? MapData { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private EventCommandSentenceResolver_CommonEvent CommonEvent { get; }
        private EventCommandSentenceResolver_Database_Basic DatabaseBasic { get; }
        private EventCommandSentenceResolver_Database_Special DatabaseSpecial { get; }
        private EventCommandSentenceResolver_MapEvent MapEvent { get; }
        private EventCommandSentenceResolver_Variable Variable { get; }
        private EventCommandSentenceResolver_VariableAddress VariableAddress { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// インデント用文字列を生成する。
        /// </summary>
        /// <param name="indent">インデント</param>
        /// <returns>インデント用文字列</returns>
        public static string MakeIndentString(Indent indent)
        {
            int repeat = indent.ToSByte();

            var builder = new StringBuilder();
            for (var i = 0; i < repeat; i++)
            {
                builder.Append(IndentStr);
            }

            return builder.ToString();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commonEventList">イベントコマンドリスト</param>
        /// <param name="changeableDatabase">可変DB</param>
        /// <param name="userDatabase">ユーザDB</param>
        /// <param name="systemDatabase">システムDB</param>
        /// <param name="mapData">マップデータ</param>
        public EventCommandSentenceResolver(CommonEventList commonEventList,
            DatabaseMergedData changeableDatabase,
            DatabaseMergedData userDatabase, DatabaseMergedData systemDatabase,
            MapData? mapData)
        {
            CommonEventList = commonEventList ??
                              throw new ArgumentNullException(ErrorMessage.NotNull(nameof(commonEventList)));
            ChangeableDatabase = changeableDatabase ??
                                 throw new ArgumentNullException(ErrorMessage.NotNull(nameof(changeableDatabase)));
            UserDatabase = userDatabase ??
                           throw new ArgumentNullException(ErrorMessage.NotNull(nameof(userDatabase)));
            SystemDatabase = systemDatabase ??
                             throw new ArgumentNullException(ErrorMessage.NotNull(nameof(systemDatabase)));
            MapData = mapData;

            CommonEvent = new EventCommandSentenceResolver_CommonEvent(this);
            DatabaseBasic = new EventCommandSentenceResolver_Database_Basic(this);
            DatabaseSpecial = new EventCommandSentenceResolver_Database_Special(this);
            MapEvent = new EventCommandSentenceResolver_MapEvent(this);
            Variable = new EventCommandSentenceResolver_Variable(this);
            VariableAddress = new EventCommandSentenceResolver_VariableAddress(this);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region CommonEvent

        /// <summary>
        /// コモンイベントIDを返す。
        /// </summary>
        /// <param name="name">コモンイベント名</param>
        /// <returns>コモンイベントID（存在しない場合null）</returns>
        public CommonEventId? GetCommonEventId(CommonEventName name)
            => CommonEvent.GetCommonEventId(name);

        /// <summary>
        /// コモンイベントID（文字列）を返す。
        /// </summary>
        /// <param name="name">コモンイベント名</param>
        /// <returns>コモンイベントIDの文字列（存在しない場合専用の文字列）</returns>
        public string GetCommonEventIdString(CommonEventName name)
            => CommonEvent.GetCommonEventIdString(name);

        /// <summary>
        /// コモンイベント名を返す。
        /// </summary>
        /// <param name="id">コモンイベントID</param>
        /// <returns>コモンイベント名（存在しない場合専用の文字列）</returns>
        public (bool, CommonEventName) GetCommonEventName(CommonEventId id)
            => CommonEvent.GetCommonEventName(id);

        /// <summary>
        /// コモンイベントIDとインデックスからコモンイベントセルフ変数名を取得する。
        /// </summary>
        /// <param name="commonEventId">コモンイベントID</param>
        /// <param name="index">インデックス</param>
        /// <returns>コモンイベントセルフ変数名（存在しないコモンイベントの場合空文字）</returns>
        public CommonEventSelfVariableName GetCommonEventSelfVariableName(
            CommonEventId commonEventId, CommonEventVariableIndex index)
            => CommonEvent.GetCommonEventSelfVariableName(commonEventId, index);

        /// <summary>
        /// コモンイベント数値引数の文字列を取得する。
        /// </summary>
        /// <param name="id">コモンイベントID</param>
        /// <param name="intArgIndex">[Range(0, 0～4)]引数インデックス</param>
        /// <param name="value">引数設定値</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>コモンイベント数値引数の文字列</returns>
        public string GetCommonEventIntArgSentence(CommonEventId id,
            CommonEventNumberArgIndex intArgIndex, int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => CommonEvent.GetCommonEventIntArgSentence(id, intArgIndex, value,
                type, desc);


        /// <summary>
        /// 相対指定のコモンイベントIDから絶対指定のコモンイベントIDを取得する。
        /// </summary>
        /// <param name="targetCommonEventId">相対指定のコモンイベントID</param>
        /// <param name="thisCommonEventId">走査対象のコモンイベントID</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <returns>絶対指定したコモンイベントID</returns>
        /// <exception cref="InvalidOperationException">イベントコマンド種別がCommon、かつthisCommonEventIdがnullの場合</exception>
        public CommonEventId GetCorrectEventIdByRelativeId(int targetCommonEventId,
            int? thisCommonEventId, EventCommandSentenceType type)
            => CommonEvent.GetCorrectEventIdByRelativeId(targetCommonEventId, thisCommonEventId, type);

        #endregion

        #region Database

        /// <summary>
        /// タイプIDを指定してDBのタイプ名を取得する。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="id">タイプID</param>
        /// <returns>
        ///     存在フラグとタイプ名。
        ///     存在しない場合、nullであれば専用の文字列。
        ///     id が変数アドレス値の場合、専用の文字列。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (bool, string) GetDatabaseTypeName(DBKind dbKind, int id)
            => DatabaseBasic.GetDatabaseTypeName(dbKind, id);

        /// <summary>
        /// タイプID、データIDを指定してDBのデータ名を取得する。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="dataId">データID</param>
        /// <returns>
        ///     存在フラグとデータ名。
        ///     存在しない場合、専用の文字列。
        ///     dataId が変数アドレス値の場合、専用の文字列。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (bool, string) GetDatabaseDataName(DBKind dbKind, int? typeId,
            int dataId)
            => DatabaseBasic.GetDatabaseDataName(dbKind, typeId, dataId);

        /// <summary>
        /// タイプID、項目IDを指定してDBの項目名を取得する。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="itemId">項目ID</param>
        /// <returns>
        ///     存在フラグと項目名。
        ///     存在しない場合、専用の文字列。
        ///     itemId が変数アドレス値の場合、専用の文字列。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (bool, string) GetDatabaseItemName(DBKind dbKind, int? typeId,
            int itemId)
            => DatabaseBasic.GetDatabaseItemName(dbKind, typeId, itemId);

        /// <summary>
        /// タイプ名を指定してタイプIDを取得する。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="name">タイプ名</param>
        /// <returns>タイプID。見つからない場合null。</returns>
        /// <exception cref="ArgumentNullException">dbKind または name が null の場合</exception>
        public (TypeId?, string) GetDatabaseTypeId(DBKind dbKind, TypeName name)
            => DatabaseBasic.GetDatabaseTypeId(dbKind, name);

        /// <summary>
        /// タイプID、データ名を指定してデータIDを取得する。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="dataName">データ名</param>
        /// <returns>
        ///     データID。
        ///     データが見つからない場合、null。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKind または dataName が null の場合</exception>
        public (DataId?, string) GetDatabaseDataId(DBKind dbKind, int? typeId,
            string dataName)
            => DatabaseBasic.GetDatabaseDataId(dbKind, typeId, dataName);

        /// <summary>
        /// タイプID、項目名を指定して項目IDを取得する。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="itemName">項目名</param>
        /// <returns>
        ///     項目ID。
        ///     項目が見つからない場合、null。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (ItemId?, string) GetDatabaseItemId(DBKind dbKind, int? typeId,
            ItemName itemName)
            => DatabaseBasic.GetDatabaseItemId(dbKind, typeId, itemName);

        /// <summary>
        /// タイプID、項目名を指定して項目IDを取得する。
        /// イベントコマンド「DB書込」専用。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="itemName">項目名</param>
        /// <returns>
        ///     項目ID。
        ///     項目が見つからない場合、null。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (ItemId?, string) GetDatabaseItemIdForInputDatabase(DBKind dbKind, int? typeId,
            ItemName itemName)
            => DatabaseBasic.GetDatabaseItemIdForInputDatabase(dbKind, typeId, itemName);

        /// <summary>
        /// CSV入出力のイベントコマンドDB文字列を取得する。
        /// </summary>
        /// <param name="desc">DBイベントコマンド文字列解決情報</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="commonDesc">付加情報</param>
        /// <returns>
        ///     イベントコマンド文字列
        /// </returns>
        /// <exception cref="ArgumentNullException">desc または desc 内の必要な項目、type、commonDesc が null の場合</exception>
        public string GetDatabaseCommandSentenceForCsvIo(
            CommonEventSentenceResolveDatabaseDesc desc, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc commonDesc)
            => DatabaseSpecial.GetCsvIoEventCommandSentence(desc, type, commonDesc);

        /// <summary>
        /// 変数操作のイベントコマンドDB文字列を取得する。
        /// </summary>
        /// <param name="typeId">タイプID</param>
        /// <param name="dataId">データID</param>
        /// <param name="itemId">項目ID</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        public string GetDatabaseCommandSentenceForSetVariable(
            int typeId, int dataId, int itemId, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc? desc)
            => DatabaseSpecial.GetDatabaseCommandSentenceForSetVariable(
                typeId, dataId, itemId, type, desc);

        #endregion

        #region MapEvent

        /// <summary>
        /// マップイベントID文字列を取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>
        ///     マップイベントID文字列。
        ///     mapEventIdがマップイベントIDとして不適切な場合、専用の文字列。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public (bool, MapEventName) GetMapEventIdStrByNumericEventId(MapEventId mapEventId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => MapEvent.GetMapEventIdStrByNumericEventId(mapEventId, type, desc);

        /// <summary>
        /// マップイベントID文字列を取得する。
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>
        ///     イベントID文字列。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public (bool, MapEventName) GetMapEventIdStr(MapCharacterId characterId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => MapEvent.GetMapEventIdStr(characterId, type, desc);

        /// <summary>
        /// マップイベント名を取得する。
        /// </summary>
        /// <param name="mapId">マップID</param>
        /// <returns>
        ///     マップ名。
        /// </returns>
        public (bool, MapEventName) GetMapName(MapId mapId)
            => MapEvent.GetMapName(mapId);

        /// <summary>
        /// マップイベント名を取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>
        ///     イベント名。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public (bool, MapEventName) GetMapEventName(MapEventId mapEventId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => MapEvent.GetMapEventName(mapEventId, type, desc);

        /// <summary>
        /// キャラクター名を取得する。
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>
        ///     マップイベント名。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public MapEventName GetCharacterName(MapCharacterId characterId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => MapEvent.GetCharacterName(characterId, type, desc);

        /// <summary>
        /// 場所移動対象のイベント名を取得する。
        /// </summary>
        /// <param name="eventId">イベントID</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>
        ///     イベント名。
        /// </returns>
        public MapEventName GetTransferEventName(int eventId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => MapEvent.GetTransferEventName(eventId, type, desc);

        #endregion

        #region Variable

        /// <summary>
        /// 文字列変数名を返す。
        /// </summary>
        /// <param name="index">文字列変数インデックス</param>
        /// <returns>文字列変数名（存在しない場合専用の文字列）</returns>
        public DataName GetStringVariableName(StringVariableIndex index)
            => Variable.GetStringVariableName(index);

        /// <summary>
        /// システム文字列変数名を返す。
        /// </summary>
        /// <param name="index">システム文字列変数インデックス</param>
        /// <returns>システム文字列変数名（存在しない場合専用の文字列）</returns>
        public DataName GetSystemStringVariableName(SystemStringVariableIndex index)
            => Variable.GetSystemStringVariableName(index);

        /// <summary>
        /// システム変数名を返す。
        /// </summary>
        /// <param name="index">システム変数インデックス</param>
        /// <returns>システム変数名（存在しない場合専用の文字列）</returns>
        public DataName GetSystemVariableName(SystemVariableIndex index)
            => Variable.GetSystemVariableName(index);

        /// <summary>
        /// 通常変数名を返す。
        /// </summary>
        /// <param name="index">通常変数インデックス</param>
        /// <returns>通常変数名（存在しない場合専用の文字列）</returns>
        public DataName GetNormalVariableName(NormalNumberVariableIndex index)
            => Variable.GetNormalVariableName(index);

        /// <summary>
        /// 予備変数名を返す。
        /// </summary>
        /// <param name="number">予備変数番号</param>
        /// <param name="index">予備変数インデックス</param>
        /// <returns>予備変数名（存在しない場合専用の文字列）</returns>
        public DataName GetSpareVariableName(SpareNumberVariableNumber number,
            SpareNumberVariableIndex index)
            => Variable.GetSpareVariableName(number, index);

        #endregion

        #region VariableAddress

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（数値変数、文字列変数両用）を取得する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>VariableAddressのイベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetVariableAddressString(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => VariableAddress.GetVariableAddressString(value, type, desc);

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（数値変数、文字列変数両用）を取得する。
        /// 指定した値がVariableAddressの範囲外の場合（999999以下の場合）、int値を文字列に変換して返す。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>
        ///     VariableAddressのイベントコマンド文字列。
        ///     変数アドレス値でない場合、value（を文字列化した値）。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetVariableAddressStringIfVariableAddress(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => VariableAddress.GetVariableAddressStringIfVariableAddress(value,
                type, desc);

        /// <summary>
        /// VariableAddress値からイベントコマンド文字列（数値変数）を取得する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>VariableAddressのイベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetNumericVariableAddressString(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => VariableAddress.GetNumericVariableAddressString(value, type, desc);

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（数値変数）を取得する。
        /// 指定した値がVariableAddressの範囲外の場合（999999以下の場合）、int値を文字列に変換して返す。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>
        ///     VariableAddressのイベントコマンド文字列
        ///     変数アドレス値でない場合、value（を文字列化した値）。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetNumericVariableAddressStringIfVariableAddress(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => VariableAddress.GetNumericVariableAddressStringIfVariableAddress(
                value, type, desc);

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（文字列変数）を取得する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>VariableAddressのイベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetStringVariableAddressString(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => VariableAddress.GetStringVariableAddressString(value, type, desc);

        #endregion
    }
}