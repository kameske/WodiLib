// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolver.MapEvent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Map;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文文字列解決クラス・マップイベント
    /// </summary>
    public class EventCommandSentenceResolver_MapEvent
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string MapEventNameNotFound = "イベントが存在しません";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>呼び出し元</summary>
        public EventCommandSentenceResolver Master { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="master">呼び出し元</param>
        public EventCommandSentenceResolver_MapEvent(
            EventCommandSentenceResolver master)
        {
            Master = master;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップキャラID文字列を取得する。
        /// </summary>
        /// <param name="characterId">キャラID</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>
        ///     マップイベントID文字列。
        ///     mapEventIdがマップイベントIDとして不適切な場合、専用の文字列。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public (bool, MapEventName) GetMapEventIdStrByNumericEventId(int characterId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            try
            {
                var castMapCharacterId = (MapCharacterId) characterId;
                return GetMapEventIdStr(castMapCharacterId, type, desc);
            }
            catch
            {
                return (false, MapEventNameNotFound);
            }
        }

        /// <summary>
        /// マップキャラID文字列を取得する。
        /// </summary>
        /// <param name="characterId">キャラID</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>
        ///     マップイベントID文字列。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public (bool, MapEventName) GetMapEventIdStr(MapCharacterId characterId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            const string notFound = "";

            if (characterId == -1) return (true, "このﾏｯﾌﾟｲﾍﾞﾝﾄ");
            if (characterId == -2) return (true, "主人公");
            if (characterId < -2)
            {
                var memberId = characterId * -1 - 2;
                return (true, $"仲間{memberId}");
            }

            if (characterId.ToInt().IsNumericVariableAddressSimpleCheck())
            {
                var varName = Master.GetNumericVariableAddressString(characterId, type, desc);
                return (true, $"ｷｬﾗ[{varName}]");
            }

            var targetEvent = Master.MapData?.MapEvents.FirstOrDefault(x => x.MapEventId == characterId);
            if (targetEvent is null) return (false, notFound);

            return (true, $"Ev{characterId}");
        }

        /// <summary>
        /// マップ名を取得する。
        /// </summary>
        /// <param name="mapId">マップID</param>
        /// <returns>
        ///     マップ名。
        /// </returns>
        public (bool, string) GetMapName(MapId mapId)
        {
            return Master.GetDatabaseDataName(DBKind.System, 0, mapId);
        }

        /// <summary>
        /// マップイベント名を取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>
        ///     イベント名。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public (bool, MapEventName) GetMapEventName(MapEventId mapEventId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            const string notFound = "";

            if (mapEventId == -1) return (true, "このﾏｯﾌﾟｲﾍﾞﾝﾄ");

            var targetEvent = Master.MapData?.MapEvents.FirstOrDefault(x => x.MapEventId == mapEventId);
            if (targetEvent is null) return (false, notFound);

            return (true, targetEvent.EventName);
        }

        /// <summary>
        /// キャラクター名を取得する。
        /// </summary>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>
        ///     マップイベント名。
        ///     該当マップがプロジェクト内に読み込まれていない場合、空文字。
        /// </returns>
        public MapEventName GetCharacterName(MapCharacterId characterId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (characterId == -1) return "このイベント";
            if (characterId == -2) return "主人公";
            if (characterId < -2) return $"仲間{(characterId + 2) * -1}";

            if (!characterId.ToInt().IsVariableAddressSimpleCheck()) return $"Ev{characterId}";

            var varAddressSentence = Master.GetNumericVariableAddressString(characterId, type, desc);
            return $"ｷｬﾗ[{varAddressSentence}]";
        }

        /// <summary>
        /// 場所移動対象のイベント名を取得する。
        /// </summary>
        /// <param name="eventId">イベントID</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>
        ///     イベント名。
        /// </returns>
        public MapEventName GetTransferEventName(int eventId,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (eventId == -1 || eventId == -10001) return "主人公";
            if (eventId == -2) return "このEv";

            if (!eventId.IsVariableAddressSimpleCheck())
            {
                var mapEventName = GetMapEventName(eventId, type, desc).Item2;
                return $"Ev{eventId}[{mapEventName}]";
            }

            var varAddressSentence = Master.GetNumericVariableAddressString(eventId, type, desc);
            return $"Ev[{varAddressSentence}]";
        }
    }
}