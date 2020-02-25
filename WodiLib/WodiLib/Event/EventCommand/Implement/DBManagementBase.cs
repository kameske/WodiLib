// ========================================
// Project Name : WodiLib
// File Name    : DBManagementBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作ベースクラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class DBManagementBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>標準数値変数個数</summary>
        private const byte StandardNumberVariableCount = 0x06;

        /// <summary>標準文字列変数個数</summary>
        private const byte StandardStringVariableCount = 0x05;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary> 返戻値セットフラグ </summary>
        private bool IsSetReturnValue { get; set; }

        /// <summary> 文字列引数セットフラグ </summary>
        private bool IsSetStringArg { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount =>
            IsSetReturnValue ? (byte) 0x06 : (byte) 0x05;

        /// <inheritdoc />
        public override byte StringVariableCount =>
            IsSetStringArg ? (byte) 0x04 : (byte) 0x00;

        /// <inheritdoc />
        public override byte NumberVariableCountMin => 0x05;

        /// <inheritdoc />
        public override byte StringVariableCountMin => 0x00;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 5)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public sealed override int GetSafetyNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1: // DBタイプID
                    if (_DBTypeId.HasInt) return _DBTypeId.ToInt();
                    return 0;

                case 2: // DBデータID
                    if (_DBDataId.HasInt) return _DBDataId.ToInt();
                    return 0;

                case 3: // DB項目ID
                    if (_DBItemId.HasInt) return _DBItemId.ToInt();
                    return 0;

                case 4: // 各種フラグ
                {
                    var operationCode = (byte) (NumberAssignOperationCode + LeftSideCode);
                    var mode = (byte) (ioMode + _DBKind.Code);
                    var ustFlg = new NameUseStrFlag
                    {
                        TypeIdFlag = _IsTypeIdUseStr,
                        DataIdFlag = _IsDataIdUseStr,
                        ItemIdFlag = _IsItemIdUseStr
                    };
                    byte[] bytes = {operationCode, mode, ustFlg.ToByte(), 0};
                    return bytes.ToInt32(Endian.Environment);
                }

                case 5: // 数値または代入先
                    return NumValue;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 5, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 5)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public sealed override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1: // DBタイプID
                    _DBTypeId.Merge(value);
                    return;

                case 2: // DBデータID
                    _DBDataId.Merge(value);
                    return;

                case 3: // DB項目ID
                    _DBItemId.Merge(value);
                    return;

                case 4: // 各種フラグ
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    NumberAssignOperationCode = (byte) (bytes[0] & 0xF0);
                    LeftSideCode = (byte) (bytes[0] & 0x0F);

                    ioMode = (byte) (bytes[1] & 0xF0);
                    _DBKind = DBKind.FromCode((byte) (bytes[1] & 0x0F));

                    var useStrFlag = new NameUseStrFlag(bytes[2]);
                    _IsTypeIdUseStr = useStrFlag.TypeIdFlag;
                    _IsDataIdUseStr = useStrFlag.DataIdFlag;
                    _IsItemIdUseStr = useStrFlag.ItemIdFlag;

                    return;
                }

                case 5: // 数値または代入先
                    NumValue = value;
                    IsSetReturnValue = true;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 5, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 3)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public sealed override string GetSafetyStringVariable(int index)
        {
            switch (index)
            {
                case 0: // 代入文字列またはCSVファイル名
                    return StrValue;

                case 1: // DBタイプ名（文字列指定でない場合は空文字）
                    if (_IsTypeIdUseStr) return _DBTypeId.ToStr();
                    return "";

                case 2: // DBデータ名（文字列指定でない場合は空文字）
                    if (_IsDataIdUseStr) return _DBDataId.ToStr();
                    return "";

                case 3: // DB項目名（文字列指定でない場合は空文字）
                    if (_IsItemIdUseStr) return _DBItemId.ToStr();
                    return "";

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 3, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 3)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public sealed override void SetSafetyStringVariable(int index, string value)
        {
            if (value is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
            switch (index)
            {
                case 0: // 代入文字列またはCSVファイル名
                    StrValue = value;
                    return;

                case 1: // DBタイプ名（文字列指定でない場合は空文字）
                    _DBTypeId.Merge(value);
                    return;

                case 2: // DBデータ名（文字列指定でない場合は空文字）
                    _DBDataId.Merge(value);
                    return;

                case 3: // DB項目名（文字列指定でない場合は空文字）
                    _DBItemId.Merge(value);
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 3, index));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        internal override bool IsNormalNumberArgIndex(int index) => index < StandardNumberVariableCount;

        /// <inheritdoc />
        internal override bool IsNormalStringArgIndex(int index) => index < StandardStringVariableCount;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DBKind dbKind = DBKind.Changeable;
        private readonly IntOrStr dbTypeId = 0;
        private readonly IntOrStr dbDataId = 0;
        private readonly IntOrStr dbItemId = 0;

        /// <summary>[NotNull] DB種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        protected virtual DBKind _DBKind
        {
            get => dbKind;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_DBKind)));
                dbKind = value;
            }
        }

        /// <summary>[NotNull] タイプID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        protected virtual IntOrStr _DBTypeId
        {
            get => dbTypeId;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_DBTypeId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_DBTypeId)));
                dbTypeId.Merge(value);
            }
        }

        /// <summary>[NotNull] データID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        protected virtual IntOrStr _DBDataId
        {
            get => dbDataId;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_DBDataId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_DBDataId)));
                dbDataId.Merge(value);
            }
        }

        /// <summary>[NotNull] 項目ID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        protected virtual IntOrStr _DBItemId
        {
            get => dbItemId;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_DBItemId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_DBItemId)));
                dbItemId.Merge(value);
            }
        }

        /// <summary>タイプID文字列指定フラグ</summary>
        protected virtual bool _IsTypeIdUseStr { get; set; }

        /// <summary>データID文字列指定フラグ</summary>
        protected virtual bool _IsDataIdUseStr { get; set; }

        /// <summary>項目ID文字列指定フラグ</summary>
        protected virtual bool _IsItemIdUseStr { get; set; }

        /// <summary>入出力値または代入先</summary>
        protected int NumValue
        {
            get => _NumValue;
            set
            {
                _NumValue = value;
                IsSetReturnValue = true;
            }
        }

        /// <summary>入出力値または代入先</summary>
        protected abstract int _NumValue { get; set; }

        /// <summary>代入文字列またはCSVファイル名</summary>
        protected string StrValue
        {
            get => _StrValue;
            set
            {
                if (value is null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(StrValue)));
                _StrValue = value;
                IsSetStringArg = true;
            }
        }

        /// <summary>代入文字列またはCSVファイル名</summary>
        protected abstract string _StrValue { get; set; }

        /// <summary>右辺内容コード</summary>
        protected abstract byte LeftSideCode { get; set; }

        /// <summary>読み書きモード</summary>
        protected abstract byte ioMode { get; set; }

        /// <summary>代入演算子コード</summary>
        protected abstract byte NumberAssignOperationCode { get; set; }

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.DeepRed;

        /// <summary>
        /// 名前に文字列を使用する。ラグ
        /// </summary>
        private class NameUseStrFlag
        {
            /// <summary>タイプIDビットフラグ</summary>
            private static readonly byte TypeIdFlagBit
                = EventCommandConstant.DBManagement.UseStringFlg.TypeIdFlagBit;

            /// <summary>データIDビットフラグ</summary>
            private static readonly byte DataIdFlagBit
                = EventCommandConstant.DBManagement.UseStringFlg.DataIdFlagBit;

            /// <summary>項目IDビットフラグ</summary>
            private static readonly byte ItemIdFlagBit
                = EventCommandConstant.DBManagement.UseStringFlg.ItemIdFlagBit;

            /// <summary>タイプID</summary>
            public bool TypeIdFlag { get; set; }

            /// <summary>データID</summary>
            public bool DataIdFlag { get; set; }

            /// <summary>項目ID</summary>
            public bool ItemIdFlag { get; set; }

            public NameUseStrFlag()
            {
            }

            public NameUseStrFlag(byte flag)
            {
                TypeIdFlag = (flag & TypeIdFlagBit) != 0;
                DataIdFlag = (flag & DataIdFlagBit) != 0;
                ItemIdFlag = (flag & ItemIdFlagBit) != 0;
            }

            public byte ToByte()
            {
                byte result = 0x00;
                if (TypeIdFlag) result += TypeIdFlagBit;
                if (DataIdFlag) result += DataIdFlagBit;
                if (ItemIdFlag) result += ItemIdFlagBit;
                return result;
            }
        }
    }
}