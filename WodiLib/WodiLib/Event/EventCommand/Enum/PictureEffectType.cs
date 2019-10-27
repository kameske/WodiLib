// ========================================
// Project Name : WodiLib
// File Name    : PictureEffectType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// ピクチャエフェクト種別
    /// </summary>
    public class PictureEffectType : TypeSafeEnum<PictureEffectType>
    {
        /// <summary>フラッシュ</summary>
        public static readonly PictureEffectType Flush;

        /// <summary>色調整</summary>
        public static readonly PictureEffectType ColorCorrect;

        /// <summary>描画座標シフト</summary>
        public static readonly PictureEffectType DrawPositionShift;

        /// <summary>シェイク</summary>
        public static readonly PictureEffectType Shake;

        /// <summary>ズーム</summary>
        public static readonly PictureEffectType Zoom;

        /// <summary>点滅（明滅）</summary>
        public static readonly PictureEffectType SwitchFlicker;

        /// <summary>点滅（自動フラッシュ）</summary>
        public static readonly PictureEffectType SwitchAutoFlush;

        /// <summary>自動拡大縮小</summary>
        public static readonly PictureEffectType AutoEnlarge;

        /// <summary>パターン切り替え（1回）</summary>
        public static readonly PictureEffectType AutoPatternSwitchOnce;

        /// <summary>パターン切り替え（ループ）</summary>
        public static readonly PictureEffectType AutoPatternSwitchLoop;

        /// <summary>パターン切り替え（往復）</summary>
        public static readonly PictureEffectType AutoPatternSwitchRoundTrip;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文フォーマット</summary>
        private string EventCommandSentenceFormat { get; }

        /// <summary>イベントコマンド文処理時間名</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentenceProcessTimeName { get; }

        static PictureEffectType()
        {
            Flush = new PictureEffectType(nameof(Flush), 0x00,
                "[フラッシュ] R{0}/G{1}/B{2} ", "");
            ColorCorrect = new PictureEffectType(nameof(ColorCorrect), 0x10,
                "[色調整] R{0}/G{1}/B{2} ", "");
            DrawPositionShift = new PictureEffectType(nameof(DrawPositionShift), 0x20,
                "[描画座標ｼﾌﾄ] Xｼﾌﾄ {0} / Yｼﾌﾄ {1}", "");
            Shake = new PictureEffectType(nameof(Shake), 0x30,
                "[シェイク] Xｼｪｲｸ {0} / Yｼｪｲｸ {1} / {2}回 ", "");
            Zoom = new PictureEffectType(nameof(Zoom), 0x40,
                "[ズーム] 中心X {0} / 中心Y {1} / {2}％ ", "");
            SwitchFlicker = new PictureEffectType(nameof(SwitchFlicker), 0x50,
                "[点滅A(明滅)] R{0}/G{1}/B{2} ", "明滅間隔");
            SwitchAutoFlush = new PictureEffectType(nameof(SwitchAutoFlush), 0x60,
                "[点滅B(自動ﾌﾗｯｼｭ)] R{0}/G{1}/B{2} ", "点滅間隔");
            AutoEnlarge = new PictureEffectType(nameof(AutoEnlarge), 0x70,
                "[自動拡大縮小] 横{0}％/縦{1}％/ 休{2}ﾌﾚｰﾑ ", "拡縮時間");
            AutoPatternSwitchOnce = new PictureEffectType(nameof(AutoPatternSwitchOnce), 0x80,
                "[自動ﾊﾟﾀｰﾝ切替(1回)] 開始ﾊﾟﾀｰﾝ[{0}] → 終了ﾊﾟﾀｰﾝ[{1}] ", "間隔");
            AutoPatternSwitchLoop = new PictureEffectType(nameof(AutoPatternSwitchLoop), 0x90,
                "[自動ﾊﾟﾀｰﾝ切替(ﾙｰﾌﾟ)] 開始ﾊﾟﾀｰﾝ[{0}] → 終了ﾊﾟﾀｰﾝ[{1}] ", "間隔");
            AutoPatternSwitchRoundTrip = new PictureEffectType(nameof(AutoPatternSwitchRoundTrip), 0xA0,
                "[自動ﾊﾟﾀｰﾝ切替(往復)] 開始ﾊﾟﾀｰﾝ[{0}] ←→ 終了ﾊﾟﾀｰﾝ[{1}] ", "間隔");
        }

        private PictureEffectType(string id, byte code,
            string eventCommandSentenceFormat,
            string eventCommandSentenceProcessTimeName) : base(id)
        {
            Code = code;
            EventCommandSentenceFormat = eventCommandSentenceFormat;
            EventCommandSentenceProcessTimeName = eventCommandSentenceProcessTimeName;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static PictureEffectType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }

        /// <summary>
        /// イベントコマンド文字列を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <param name="value1">値1</param>
        /// <param name="value2">値2</param>
        /// <param name="value3">値3</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MakeEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc, int value1, int value2, int value3)
        {
            var value1Name = resolver.GetNumericVariableAddressStringIfVariableAddress(value1, type, desc);
            var value2Name = resolver.GetNumericVariableAddressStringIfVariableAddress(value2, type, desc);
            var value3Name = resolver.GetNumericVariableAddressStringIfVariableAddress(value3, type, desc);
            return string.Format(EventCommandSentenceFormat,
                value1Name, value2Name, value3Name);
        }
    }
}