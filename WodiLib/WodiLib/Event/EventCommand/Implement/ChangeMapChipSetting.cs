// ========================================
// Project Name : WodiLib
// File Name    : ChangeMapChipSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・マップチップ通行設定
    /// </summary>
    [Serializable]
    public class ChangeMapChipSetting : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const int InitializeChipId = -1000000;

        private const string EventCommandSentenceFormatSetting
            = "■ﾏｯﾌﾟﾁｯﾌﾟ通行設定： ﾁｯﾌﾟ{0} = ({1} )";

        private const string EventCommandSentenceFormatInitialize
            = "■ﾏｯﾌﾟﾁｯﾌﾟ通行設定：設定初期化";

        private static class EventCommandSentenceSettingStrings
        {
            public static string NoUp = "↑禁";
            public static string NoLeft = "←禁";
            public static string NoRight = "→禁";
            public static string NoDown = "↓禁";
            public static string AboveHero = "主人公の上";
            public static string HalfTrans = "茂み";
            public static string MatchLowerLayer = "下ﾚｲﾔｰ依存";
            public static string Counter = "ｶｳﾝﾀｰ";
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChangeMapChipSetting;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.YellowGreen;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 2)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                    return ChangeChipId;

                case 2:
                    return settings.ToInt();

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 2, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 2)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    ChangeChipId = value;
                    return;

                case 2:
                    settings = new MapChipSettings(value);
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 2, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetSafetyStringVariable(int index)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void SetSafetyStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (IsReset) return EventCommandSentenceFormatInitialize;

            var settingStrList = new List<string>();
            if (IsNoDown) settingStrList.Add(EventCommandSentenceSettingStrings.NoDown);
            if (IsNoLeft) settingStrList.Add(EventCommandSentenceSettingStrings.NoLeft);
            if (IsNoRight) settingStrList.Add(EventCommandSentenceSettingStrings.NoRight);
            if (IsNoUp) settingStrList.Add(EventCommandSentenceSettingStrings.NoUp);
            if (IsAboveHero) settingStrList.Add(EventCommandSentenceSettingStrings.AboveHero);
            if (IsHalfTrans) settingStrList.Add(EventCommandSentenceSettingStrings.HalfTrans);
            if (IsCounter) settingStrList.Add(EventCommandSentenceSettingStrings.Counter);
            if (IsMatchLowerLayer) settingStrList.Add(EventCommandSentenceSettingStrings.MatchLowerLayer);

            var settingsStr = string.Join(" ", settingStrList);

            var showChipId = MapChipTranslator.Translate(ChangeChipId);

            return string.Format(EventCommandSentenceFormatSetting,
                showChipId, settingsStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private bool isReset;

        /// <summary>全初期化</summary>
        public bool IsReset
        {
            get => isReset;
            set
            {
                isReset = value;
                if (value) changeChipId = InitializeChipId;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ChangeChipId));
            }
        }

        private int changeChipId;

        /// <summary>変更チップID</summary>
        public int ChangeChipId
        {
            get => changeChipId;
            set
            {
                changeChipId = value;
                isReset = changeChipId == InitializeChipId;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsReset));
            }
        }

        private MapChipSettings settings = new MapChipSettings();

        /// <summary>↑不能</summary>
        public bool IsNoUp
        {
            get => settings.IsNoUp;
            set => settings.IsNoUp = value;
        }

        /// <summary>←不能</summary>
        public bool IsNoLeft
        {
            get => settings.IsNoLeft;
            set => settings.IsNoLeft = value;
        }

        /// <summary>→不能</summary>
        public bool IsNoRight
        {
            get => settings.IsNoRight;
            set => settings.IsNoRight = value;
        }

        /// <summary>↓不能</summary>
        public bool IsNoDown
        {
            get => settings.IsNoDown;
            set => settings.IsNoDown = value;
        }

        /// <summary>主人公の上</summary>
        public bool IsAboveHero
        {
            get => settings.IsAboveHero;
            set => settings.IsAboveHero = value;
        }

        /// <summary>下半身透過</summary>
        public bool IsHalfTrans
        {
            get => settings.IsHalfTrans;
            set => settings.IsHalfTrans = value;
        }

        /// <summary>下レイヤー依存</summary>
        public bool IsMatchLowerLayer
        {
            get => settings.IsMatchLowerLayer;
            set => settings.IsMatchLowerLayer = value;
        }

        /// <summary>カウンター</summary>
        public bool IsCounter
        {
            get => settings.IsCounter;
            set => settings.IsCounter = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップチップ通行設定プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(MapChipSettings.IsNoUp):
                case nameof(MapChipSettings.IsNoLeft):
                case nameof(MapChipSettings.IsNoRight):
                case nameof(MapChipSettings.IsNoDown):
                case nameof(MapChipSettings.IsAboveHero):
                case nameof(MapChipSettings.IsHalfTrans):
                case nameof(MapChipSettings.IsMatchLowerLayer):
                case nameof(MapChipSettings.IsCounter):
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
        public ChangeMapChipSetting()
        {
            settings.PropertyChanged += OnSettingsPropertyChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Inner Class
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 内部マップチップIDを表示マップチップIDに変換するためのクラス
        /// </summary>
        private static class MapChipTranslator
        {
            private static IReadOnlyList<MapChipTranslatorMapItem> mapList = new List<MapChipTranslatorMapItem>
            {
                // (targetMin, targetMax, translateFunc)
                (-18, -11, (int target) => target + (11 + target) * -2 + 19),
                (0, 7, (int target) => target),
                (8, int.MaxValue - 8, (int target) => target + 8),
                // いずれにも当てはまらない場合の保険
                (int.MinValue, int.MaxValue, (int target) => int.MinValue)
            };

            /// <summary>
            /// 内部マップチップIDを表示マップチップIDに変換する。
            /// </summary>
            /// <param name="target">内部マップチップID</param>
            /// <returns>表示マップチップID</returns>
            public static int Translate(int target)
            {
                var map = mapList.First(x => x.Min <= target && target <= x.Max);
                return map.TranslateFunc(target);
            }

            /// <summary>
            /// 内部マップチップIDを表示マップチップIDに変換するための情報クラス
            /// </summary>
            private class MapChipTranslatorMapItem
            {
                /// <summary>対象範囲最小値</summary>
                public int Min { get; }

                /// <summary>対象範囲最大値</summary>
                public int Max { get; }

                /// <summary>変換関数</summary>
                public Func<int, int> TranslateFunc { get; }

                public MapChipTranslatorMapItem(int min, int max, Func<int, int> translate)
                {
                    Min = min;
                    Max = max;
                    TranslateFunc = translate;
                }

                public static implicit operator MapChipTranslatorMapItem(
                    (int min, int max, Expression<Func<int, int>> translateFunc) item)
                {
                    return new MapChipTranslatorMapItem(item.min, item.max, item.translateFunc.Compile());
                }
            }
        }
    }
}