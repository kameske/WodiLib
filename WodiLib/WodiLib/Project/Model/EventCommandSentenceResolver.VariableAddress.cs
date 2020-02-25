// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolver.VariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文文字列解決クラス・変数アドレス
    /// </summary>
    internal class EventCommandSentenceResolver_VariableAddress
    {
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
        public EventCommandSentenceResolver_VariableAddress(
            EventCommandSentenceResolver master)
        {
            Master = master;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（数値変数、文字列変数両用）を取得する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>VariableAddressのイベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetVariableAddressString(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            if (value.TryToVariableAddress(out var variableAddress))
            {
                return variableAddress.MakeEventCommandString(Master, type,
                    VariableAddressValueType.Both, desc);
            }

            return string.Format(VariableAddressValueType.Both.EventCommandStringErrorFormat,
                value);
        }

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（数値変数、文字列変数両用）を取得する。
        /// 指定した値がVariableAddressの範囲外の場合（999999以下の場合）、int値を文字列に変換して返す。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>
        ///     VariableAddressのイベントコマンド文字列。
        ///     変数アドレス値でない場合、value（を文字列化した値）。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetVariableAddressStringIfVariableAddress(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            if (!value.IsVariableAddressSimpleCheck()) return value.ToString();

            return GetVariableAddressString(value, type, desc);
        }

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（数値変数）を取得する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>VariableAddressのイベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetNumericVariableAddressString(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            if (value.TryToVariableAddress(out var variableAddress))
            {
                return variableAddress.MakeEventCommandString(Master, type,
                    VariableAddressValueType.Numeric, desc);
            }

            return string.Format(VariableAddressValueType.Numeric.EventCommandStringErrorFormat,
                value);
        }

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（数値変数）を取得する。
        /// 指定した値がVariableAddressの範囲外の場合（999999以下の場合）、int値を文字列に変換して返す。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>
        ///     VariableAddressのイベントコマンド文字列
        ///     変数アドレス値でない場合、value（を文字列化した値）。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetNumericVariableAddressStringIfVariableAddress(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            if (!value.IsVariableAddressSimpleCheck()) return value.ToString();

            return GetNumericVariableAddressString(value, type, desc);
        }

        /// <summary>
        /// int値からVariableAddressのイベントコマンド文字列（文字列変数）を取得する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="type">[NotNull] イベントコマンド種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>VariableAddressのイベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     typeがnullの場合、
        ///     または必要なときにdescがnullの場合
        /// </exception>
        public string GetStringVariableAddressString(int value,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            if (value.TryToVariableAddress(out var variableAddress))
            {
                return variableAddress.MakeEventCommandString(Master, type,
                    VariableAddressValueType.String, desc);
            }

            return string.Format(VariableAddressValueType.String.EventCommandStringErrorFormat,
                value);
        }
    }
}