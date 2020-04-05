// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    /// <para>動作指定コマンド_共通動作クラス</para>
    /// <para>（隠蔽クラス）</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public abstract class CharaMoveCommandBase : ModelBase<CharaMoveCommandBase>, ICharaMoveCommand
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>動作指定コマンド種別</summary>
        public abstract CharaMoveCommandCode CommandCode { get; }

        /// <summary>変数の数</summary>
        public abstract byte ValueLengthByte { get; }

        private int valueLength = -1;

        /// <summary>変数の数</summary>
        public int ValueLength
        {
            get
            {
                if (valueLength != -1) return valueLength;
                var bytes = new byte[] {ValueLengthByte, 0x00, 0x00, 0x00};
                valueLength = bytes.ToInt32(Endian.Little);
                return valueLength;
            }
        }

        /// <summary>変数リスト</summary>
        protected CharaMoveCommandValue[] NumberValues { get; set; }

        /// <summary>
        /// 数値変数の値を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 変数の数-1)] 変数インデックス</param>
        /// <returns>取得した数値</returns>
        /// <exception cref="ArgumentOutOfRangeException">インデックスが指定範囲以外の場合</exception>
        public CharaMoveCommandValue GetNumberValue(int index)
        {
            if (index < 0 || ValueLength <= index)
                throw new ArgumentOutOfRangeException($"存在しない変数を取得しようとしました。index: {index} maxLength: {ValueLength} ");
            return NumberValues[index];
        }

        /// <summary>
        /// 数値変数の値を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 変数の数-1)] 変数インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">インデックスが指定範囲以外の場合</exception>
        public void SetNumberValue(int index, CharaMoveCommandValue value)
        {
            if (index < 0 || ValueLength <= index)
                throw new ArgumentOutOfRangeException($"存在しない変数を取得しようとしました。index: {index} maxLength: {ValueLength} ");
            NumberValues[index] = value;
        }

        /// <summary>
        /// イベントコマンド文字列を取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public abstract string GetEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc);

        /// <summary>
        /// 終了バイトコード
        /// </summary>
        public static byte[] EndBlockCode => new byte[] {0x01, 0x00};

        /// <summary>ロガー</summary>
        protected static readonly WodiLibLogger Logger = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CharaMoveCommandBase()
        {
            NumberValues = new CharaMoveCommandValue[ValueLength];
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ICharaMoveCommand other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            if (CommandCode != other.CommandCode) return false;
            if (ValueLength != other.ValueLength) return false;

            for (var i = 0; i < ValueLength; i++)
            {
                if (GetNumberValue(i) != other.GetNumberValue(i)) return false;
            }

            return true;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(CharaMoveCommandBase other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return Equals(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public byte[] ToBinary()
        {
            var result = new List<byte> {CommandCode.Code, ValueLengthByte};
            // 動作コマンドコード
            // 変数の数
            // 変数
            foreach (var value in NumberValues)
            {
                result.AddRange(value.ToBytes(Endian.Woditor));
            }

            // 終端コード
            result.AddRange(EndBlockCode);
            return result.ToArray();
        }

        /// <inheritdoc />
        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public virtual void OutputVersionWarningLogIfNeed()
        {
        }
    }
}