// ========================================
// Project Name : WodiLib
// File Name    : ChangeMapChipSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・マップチップ通行設定
    /// </summary>
    public class ChangeMapChipSetting : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.ChangeMapChipSetting;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 2)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode;

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
        public override void SetNumberVariable(int index, int value)
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
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetStringVariable(int index)
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
        public override void SetStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>全初期化</summary>
        public bool IsReset { get; set; }

        /// <summary>変更チップID</summary>
        public int ChangeChipId { get; set; }

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
    }
}