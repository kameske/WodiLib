// ========================================
// Project Name : WodiLib
// File Name    : CsvIO.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・CSV入出力
    /// </summary>
    [Serializable]
    public class CsvIO : DBManagementBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormatRead
            = "■CSVﾌｧｲﾙからDBに読込:  ﾌｧｲﾙ \"{0}\"  →  {1} から [{2} データ]";

        private const string EventCommandSentenceFormatWrite
            = "■DBからCSVﾌｧｲﾙに保存:  {1} から [{2} データ]  →  ﾌｧｲﾙ \"{0}\"";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x06;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x04;

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.CsvIO;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));

            var dbDesc = MakeCommonEventSentenceResolveDatabaseDesc();
            var dbStr = resolver.GetDatabaseCommandSentenceForCsvIo(dbDesc, type, desc);

            if (Mode == CsvIOMode.Input)
            {
                return string.Format(EventCommandSentenceFormatRead,
                    FileName, dbStr, ItemLength);
            }

            if (Mode == CsvIOMode.Output)
            {
                return string.Format(EventCommandSentenceFormatWrite,
                    FileName, dbStr, ItemLength);
            }

            // 通常ここへは来ない
            throw new InvalidOperationException();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventSentenceResolveDatabaseDesc MakeCommonEventSentenceResolveDatabaseDesc()
        {
            var dbDesc = new CommonEventSentenceResolveDatabaseDesc()
                .SetDbKind(DBKind);

            if (IsTypeIdUseStr) dbDesc.SetTypeName(DBTypeId.ToStr());
            else dbDesc.SetTypeId(DBTypeId.ToInt());

            if (IsDataIdUseStr) dbDesc.SetDataName(DBDataId.ToStr());
            else dbDesc.SetDataId(DBDataId.ToInt());

            return dbDesc;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[NotNull] DB種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBKind DBKind
        {
            get => _DBKind;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBKind)));
                _DBKind = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>[NotNull] タイプID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBTypeId
        {
            get => IsTypeIdUseStr ? (IntOrStr) _DBTypeId.ToStr() : _DBTypeId.ToInt();
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                _DBTypeId.Merge(value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>[NotNull] データID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBDataId
        {
            get => IsDataIdUseStr ? (IntOrStr) _DBDataId.ToStr() : _DBDataId.ToInt();
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBDataId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBDataId)));
                _DBDataId.Merge(value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// <para>項目ID</para>
        /// <para>通常は設定しないが、ウディタ上で指定した値があれば設定する。</para>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int DBItemIndex
        {
            get => _DBItemId.ToInt();
            set
            {
                _DBItemId = value;
                NotifyPropertyChanged();
            }
        }

        private IntOrStr _dBItemId = 0;

        /// <inheritdoc />
        /// <summary>項目ID</summary>
        protected override IntOrStr _DBItemId
        {
            get => _dBItemId;
            set
            {
                _dBItemId = value;
                NotifyPropertyChanged(nameof(DBItemIndex));
            }
        }

        private CsvIOMode mode = CsvIOMode.Input;

        /// <summary>[NotNull] CSV入出力モード</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CsvIOMode Mode
        {
            get => mode;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Mode)));
                mode = value;
                NotifyPropertyChanged();
            }
        }

        private int itemLength;

        /// <summary>出力/入力データ数</summary>
        public int ItemLength
        {
            get => itemLength;
            set
            {
                itemLength = value;
                NotifyPropertyChanged();
            }
        }

        private string fileName = "";

        /// <summary>[NotNull] 出力/入力ファイル名</summary>
        public string FileName
        {
            get => fileName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(FileName)));
                fileName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>タイプID文字列指定フラグ</summary>
        public bool IsTypeIdUseStr
        {
            get => _IsTypeIdUseStr;
            set
            {
                _IsTypeIdUseStr = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>データID文字列指定フラグ</summary>
        public bool IsDataIdUseStr
        {
            get => _IsDataIdUseStr;
            set
            {
                _IsDataIdUseStr = value;
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>項目ID文字列指定フラグ</summary>
        protected override bool _IsItemIdUseStr
        {
            get => false;
            set { }
        }

        /// <inheritdoc />
        /// <summary>入出力値または代入先</summary>
        protected override int _NumValue
        {
            get => ItemLength;
            set => ItemLength = value;
        }

        /// <inheritdoc />
        /// <summary>代入文字列またはCSVファイル名</summary>
        protected override string _StrValue
        {
            get => FileName;
            set => FileName = value;
        }

        /// <inheritdoc />
        /// <summary>右辺内容コード</summary>
        protected override byte LeftSideCode
        {
            get => 0;
            set { }
        }

        /// <inheritdoc />
        /// <summary>読み書きモード</summary>
        protected override byte ioMode
        {
            get => Mode.Code;
            set => Mode = CsvIOMode.FromByte(value);
        }

        /// <inheritdoc />
        /// <summary>代入演算子コード</summary>
        protected override byte NumberAssignOperationCode
        {
            get => 0;
            set { }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     VersionCheck
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public override void OutputVersionWarningLogIfNeed()
        {
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_00))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_00();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.00未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_00()
        {
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(CsvIO)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_00));
        }
    }
}