// ========================================
// Project Name : WodiLib
// File Name    : MapEffectShake.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・マップエフェクト（シェイク）
    /// </summary>
    [Serializable]
    public class MapEffectShake : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormatShake
            = "■マップエフェクト：[シェイク] {0} {1} {2} ({3})ﾌﾚｰﾑ";

        private const string EventCommandSentenceFormatStop
            = "■マップエフェクト：[シェイク]シェイク停止";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.MapEffectShake;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Gold;

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
                {
                    var byte0 = (byte) (Speed.Code + Power.Code);
                    var byte1 = ShakeType.Code;
                    return new byte[] {byte0, byte1, 0x00, 0x00}.ToInt32(Endian.Environment);
                }

                case 2:
                    return Duration;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 2, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 3)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    var byte0upper = (byte) (bytes[0] & 0xF0);
                    Speed = MapEffectShakeSpeed.FromByte(byte0upper);
                    var byte0lower = (byte) (bytes[0] & 0x0F);
                    Power = MapEffectShakePower.FromByte(byte0lower);
                    ShakeType = MapEffectShakeType.FromByte(bytes[1]);
                    return;
                }

                case 2:
                    Duration = value;
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
            if (ShakeType == MapEffectShakeType.Stop)
            {
                return EventCommandSentenceFormatStop;
            }

            var varName = resolver.GetNumericVariableAddressStringIfVariableAddress(Duration, type, desc);

            return string.Format(EventCommandSentenceFormatShake,
                Power.EventCommandSentence, Speed.EventCommandSentence,
                ShakeType.EventCommandSentence, varName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private MapEffectShakeType shakeType = MapEffectShakeType.Horizontal;
        private MapEffectShakePower power = MapEffectShakePower.Power1;
        private MapEffectShakeSpeed speed = MapEffectShakeSpeed.Speed1;

        /// <summary>[NotNull] 揺れ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapEffectShakeType ShakeType
        {
            get => shakeType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ShakeType)));
                shakeType = value;
            }
        }

        /// <summary>[NotNull] 強さ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapEffectShakePower Power
        {
            get => power;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Power)));
                power = value;
            }
        }

        /// <summary>[NotNull] 拡大率</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapEffectShakeSpeed Speed
        {
            get => speed;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Speed)));
                speed = value;
            }
        }

        /// <summary>処理時間</summary>
        public int Duration { get; set; }
    }
}