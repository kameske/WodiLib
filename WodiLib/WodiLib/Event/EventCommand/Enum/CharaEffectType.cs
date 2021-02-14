// ========================================
// Project Name : WodiLib
// File Name    : CharaEffectType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Map;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     キャラエフェクト種別
    /// </summary>
    public record CharaEffectType : TypeSafeEnum<CharaEffectType>
    {
        /// <summary>フラッシュ</summary>
        public static readonly CharaEffectType Flush;

        /// <summary>シェイク</summary>
        public static readonly CharaEffectType Shake;

        /// <summary>点滅（明滅）</summary>
        public static readonly CharaEffectType Flicker;

        /// <summary>点滅（自動フラッシュ）</summary>
        public static readonly CharaEffectType AutoFlush;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンドフォーマット</summary>
        private string EventCommandSentenceFormat { get; }

        static CharaEffectType()
        {
            Flush = new CharaEffectType(nameof(Flush), 0x00,
                "■キャラエフェクト：{0}[フラッシュ] R{1}/G{2}/B{3}  ({4})ﾌﾚｰﾑ");
            Shake = new CharaEffectType(nameof(Shake), 0x10,
                "■キャラエフェクト：{0}[シェイク] Xｼｪｲｸ {1} / Yｼｪｲｸ {2} / {3}回  ({4})ﾌﾚｰﾑ");
            Flicker = new CharaEffectType(nameof(Flicker), 0x20,
                "■キャラエフェクト：{0}[点滅A(明滅)] R{1}/G{2}/B{3}  ({4})ﾌﾚｰﾑ");
            AutoFlush = new CharaEffectType(nameof(AutoFlush), 0x30,
                "■キャラエフェクト：{0}[点滅B(自動ﾌﾗｯｼｭ)] R{1}/G{2}/B{3}  ({4})ﾌﾚｰﾑ");
        }

        private CharaEffectType(string id, byte code,
            string eventCommandSentenceFormat) : base(id)
        {
            Code = code;
            EventCommandSentenceFormat = eventCommandSentenceFormat;
        }

        /// <summary>
        ///     イベントコマンド文文字列を取得する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="type">イベント種別</param>
        /// <param name="desc">付加情報</param>
        /// <param name="characterId">キャラクターID</param>
        /// <param name="numArg1">引数1（R値またはXシェイク値）</param>
        /// <param name="numArg2">引数2（G値またはYシェイク値）</param>
        /// <param name="numArg3">引数3（B値または回数）</param>
        /// <param name="processTime">フレーム数</param>
        /// <returns>イベントコマンド文字列</returns>
        public string GetEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc? desc,
            MapCharacterId characterId, int numArg1,
            int numArg2, int numArg3, int processTime)
        {
            var eventSentence = resolver.GetCharacterName(characterId, type, desc);
            var numArg1Sentence = resolver.GetNumericVariableAddressStringIfVariableAddress(numArg1, type, desc);
            var numArg2Sentence = resolver.GetNumericVariableAddressStringIfVariableAddress(numArg2, type, desc);
            var numArg3Sentence = resolver.GetNumericVariableAddressStringIfVariableAddress(numArg3, type, desc);
            var processTimeSentence =
                resolver.GetNumericVariableAddressStringIfVariableAddress(processTime, type, desc);

            return string.Format(EventCommandSentenceFormat,
                eventSentence, numArg1Sentence, numArg2Sentence, numArg3Sentence, processTimeSentence);
        }

        /// <summary>
        ///     バイト値から種別インスタンスを取得する。
        /// </summary>
        /// <param name="src">コード値</param>
        /// <returns>インスタンス</returns>
        public static CharaEffectType FromByte(byte src)
        {
            return AllItems.First(x => x.Code == src);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
