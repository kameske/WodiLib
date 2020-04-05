// ========================================
// Project Name : WodiLib
// File Name    : SaveSpecificSaveData.cs
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
    /// イベントコマンド・内容書込み
    /// </summary>
    [Serializable]
    public class SaveSpecificSaveData : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■ｾｰﾌﾞﾃﾞｰﾀに書き込み：ｾｰﾌﾞﾃﾞｰﾀ[{0}]の {1} =  {2}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.SaveVariable;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x05;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Purple;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 4)] インデックス</param>
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
                    return SaveData;

                case 2:
                    return SaveNumber;

                case 3:
                    return SaveAddress;

                case 4:
                    return IsUseVariableX
                        ? 1
                        : 0;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 4, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 4)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    SaveData = value;
                    return;

                case 2:
                    SaveNumber = value;
                    return;

                case 3:
                    SaveAddress = value;
                    return;

                case 4:
                    IsUseVariableX = value != 0;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 4, index));
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
            var saveNumStr = resolver.GetVariableAddressStringIfVariableAddress(SaveNumber, type, desc);
            var saveAddrStr = resolver.GetVariableAddressStringIfVariableAddress(SaveAddress, type, desc);
            if (IsUseVariableX) saveAddrStr = $"V[{saveAddrStr}]";
            var valueStr = resolver.GetVariableAddressStringIfVariableAddress(SaveData, type, desc);
            return string.Format(EventCommandSentenceFormat,
                saveNumStr, saveAddrStr, valueStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int saveNumber;

        /// <summary>セーブデータ番号</summary>
        public int SaveNumber
        {
            get => saveNumber;
            set
            {
                saveNumber = value;
                NotifyPropertyChanged();
            }
        }

        private int saveAddress;

        /// <summary>読み込み先アドレス</summary>
        public int SaveAddress
        {
            get => saveAddress;
            set
            {
                saveAddress = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUseVariableX;

        /// <summary>X番の変数呼び出し</summary>
        public bool IsUseVariableX
        {
            get => isUseVariableX;
            set
            {
                isUseVariableX = value;
                NotifyPropertyChanged();
            }
        }

        private int saveData;

        /// <summary>書き込み値</summary>
        public int SaveData
        {
            get => saveData;
            set
            {
                saveData = value;
                NotifyPropertyChanged();
            }
        }
    }
}