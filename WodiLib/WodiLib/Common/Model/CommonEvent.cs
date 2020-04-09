// ========================================
// Project Name : WodiLib
// File Name    : CommonEvent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Event;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Event.EventCommand;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベントクラス
    /// </summary>
    [Serializable]
    public class CommonEvent : ModelBase<CommonEvent>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダバイト
        /// </summary>
        internal static readonly byte[] HeaderBytes =
        {
            0x8E
        };

        /// <summary>
        /// 引数初期値の後のチェックディジット
        /// </summary>
        internal static readonly byte[] AfterInitValueBytes =
        {
            0x90
        };

        /// <summary>
        /// セルフ変数名の後のチェックディジット
        /// </summary>
        internal static readonly byte[] AfterMemoBytesSelfVariableNamesBytes =
        {
            0x91
        };

        /// <summary>
        /// 返戻値の意味の前のチェックディジット
        /// </summary>
        internal static readonly byte[] BeforeReturnValueSummaryBytesBefore =
        {
            0x92
        };

        /// <summary>
        /// コモンイベント末尾のチェックディジット（Ver2.00以前）
        /// </summary>
        internal static readonly byte[] FooterBytesBeforeVer2_00 =
        {
            0x91
        };

        /// <summary>
        /// コモンイベント末尾のチェックディジット（Ver2.00以前）
        /// </summary>
        internal static readonly byte[] FooterBytesAfterVer2_00 =
        {
            0x92
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventId id;

        /// <summary>コモンイベントID</summary>
        public CommonEventId Id
        {
            get => id;
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        private CommonEventBootCondition condition = new CommonEventBootCondition();

        /// <summary>
        /// [NotNull] 起動条件
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventBootCondition BootCondition
        {
            get => condition;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CriteriaOperator)));
                condition = value;
                NotifyPropertyChanged();
            }
        }

        private int numberArgsLength;

        /// <summary>
        /// [Range(0, 4)] 数値引数の数
        /// </summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外の値がセットされた場合</exception>
        public int NumberArgsLength
        {
            get => numberArgsLength;
            set
            {
                if (value < CommonEventNumberArgIndex.MinValue || CommonEventNumberArgIndex.MaxValue < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(NumberArgsLength), CommonEventNumberArgIndex.MinValue,
                            CommonEventNumberArgIndex.MaxValue, value));
                numberArgsLength = value;
                NotifyPropertyChanged();
            }
        }

        private int strArgsLength;

        /// <summary>
        /// [Range(0, 4)] 文字列引数の数
        /// </summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外の値がセットされた場合</exception>
        public int StrArgsLength
        {
            get => strArgsLength;
            set
            {
                if (value < CommonEventStringArgIndex.MinValue || CommonEventStringArgIndex.MaxValue < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(StrArgsLength), CommonEventStringArgIndex.MinValue,
                            CommonEventStringArgIndex.MaxValue, value));
                strArgsLength = value;
                NotifyPropertyChanged();
            }
        }

        private CommonEventName name = "";

        /// <summary>
        /// [NotNull] コモンイベント名
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventName Name
        {
            get => name;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Name)));
                name = value;
                NotifyPropertyChanged();
            }
        }

        private EventCommandList eventCommands = new EventCommandList(new[] {new Blank()})
        {
            Owner = TargetAddressOwner.CommonEvent
        };

        /// <summary>
        /// [NotNull] イベントコマンド
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandList EventCommands
        {
            get => eventCommands;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventCommands)));
                value.Owner = TargetAddressOwner.CommonEvent;
                eventCommands = value;
                NotifyPropertyChanged();
            }
        }

        private CommonEventDescription description = "";

        /// <summary>
        /// [NotNull] 説明文
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public CommonEventDescription Description
        {
            get => description;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Description)));
                description = value;
                NotifyPropertyChanged();
            }
        }

        private CommonEventMemo memo = "";

        /// <summary>
        /// [NotNull] メモ
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventMemo Memo
        {
            get => memo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Memo)));
                memo = value;
                NotifyPropertyChanged();
            }
        }

        private CommonEventLabelColor labelColor = CommonEventLabelColor.Black;

        /// <summary>[NotNull] ラベル色</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventLabelColor LabelColor
        {
            get => labelColor;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(LabelColor)));
                labelColor = value;
                NotifyPropertyChanged();
            }
        }

        private CommonEventFooterString footerString = "";

        /// <summary>
        /// [NotNull] フッタ文字列
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public CommonEventFooterString FooterString
        {
            get => footerString;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(FooterString)));
                footerString = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>返戻アドレス情報（Ver2.00～）</summary>
        private readonly CommonEventReturnValue returnValueInfo = new CommonEventReturnValue();

        /// <summary>
        /// [NotNull] 返戻値の意味（Ver2.00～）
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public CommonEventResultDescription ReturnValueDescription
        {
            get => returnValueInfo.Description;
            set => returnValueInfo.Description = value;
        }

        /// <summary>
        /// 値を返すフラグ（Ver2.00～）
        /// </summary>
        public bool IsReturnValue => returnValueInfo.IsReturnValue;

        /// <summary>
        /// セルフ変数インデックス（値を返さない場合-1）（Ver2.00～）
        /// </summary>
        public CommonEventReturnVariableIndex ReturnVariableIndex => returnValueInfo.ReturnVariableIndex;

        private CommonEventSelfVariableNameList selfVariableNameList = new CommonEventSelfVariableNameList();

        /// <summary>
        /// [NotNull] 変数名リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventSelfVariableNameList SelfVariableNameList
        {
            get => selfVariableNameList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SelfVariableNameList)));

                selfVariableNameList = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 引数特殊指定情報リスト
        /// </summary>
        private CommonEventSpecialArgDescList CommonEventSpecialArgDescList { get; } =
            new CommonEventSpecialArgDescList();

        /// <summary>
        /// 数値引数特殊指定情報リスト
        /// </summary>
        public CommonEventSpecialNumberArgDescList NumberArgDescList
            => CommonEventSpecialArgDescList.NumberArgDescList;

        /// <summary>
        /// 文字列引数特殊指定情報リスト
        /// </summary>
        public CommonEventSpecialStringArgDescList StringArgDescList
            => CommonEventSpecialArgDescList.StringArgDescList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベント返戻値プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnReturnValueInfoPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(CommonEventReturnValue.IsReturnValue):
                case nameof(CommonEventReturnValue.ReturnVariableIndex):
                    NotifyPropertyChanged(args.PropertyName);
                    break;

                case nameof(CommonEventReturnValue.Description):
                    NotifyPropertyChanged(nameof(ReturnValueDescription));
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEvent()
        {
            returnValueInfo.PropertyChanged += OnReturnValueInfoPropertyChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 数値引数の情報を更新する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="desc">[NotNull] 情報</param>
        /// <exception cref="ArgumentNullException">descがnullの場合</exception>
        [Obsolete("NumberArgDescList を直接更新してください。Ver1.4で削除します。")]
        public void UpdateSpecialNumberArgDesc(CommonEventNumberArgIndex index,
            CommonEventSpecialNumberArgDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));

            CommonEventSpecialArgDescList.UpdateSpecialNumberArgDesc(index, desc);
        }

        /// <summary>
        /// 数値引数の情報を取得する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>情報インスタンス</returns>
        [Obsolete("NumberArgDescList を参照更新してください。Ver1.4で削除します。")]
        public CommonEventSpecialNumberArgDesc GetSpecialNumberArgDesc(CommonEventNumberArgIndex index)
        {
            return CommonEventSpecialArgDescList.GetSpecialNumberArgDesc(index);
        }

        /// <summary>
        /// 文字列引数の情報を更新する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="desc">[NotNull] 情報</param>
        /// <exception cref="ArgumentNullException">descがnullの場合</exception>
        [Obsolete("StringArgDescList を直接更新してください。Ver1.4で削除します。")]
        public void UpdateSpecialStringArgDesc(CommonEventStringArgIndex index,
            CommonEventSpecialStringArgDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));
            CommonEventSpecialArgDescList.UpdateSpecialStringArgDesc(index, desc);
        }

        /// <summary>
        /// 文字列引数の情報を取得する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>情報インスタンス</returns>
        [Obsolete("StringArgDescList を直接参照してください。Ver1.4で削除します。")]
        public CommonEventSpecialStringArgDesc GetSpecialStringArgDesc(CommonEventStringArgIndex index)
        {
            return CommonEventSpecialArgDescList.GetSpecialStringArgDesc(index);
        }

        /// <summary>
        /// 返戻セルフ変数インデックスをセットする。
        /// </summary>
        /// <param name="commonVarAddress">[Range(-1, 99)] 返戻アドレス</param>
        /// <exception cref="ArgumentOutOfRangeException">commonVarAddressが指定範囲外の場合</exception>
        public void SetReturnVariableIndex(CommonEventReturnVariableIndex commonVarAddress)
        {
            try
            {
                returnValueInfo.SetReturnVariableIndex(commonVarAddress);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException(ex.Message);
            }
        }

        /// <summary>
        /// 返戻フラグをOffにする。
        /// </summary>
        public void SetReturnValueNone()
        {
            returnValueInfo.SetReturnValueNone();
        }

        /// <summary>
        /// イベントコードリストを取得する。
        /// </summary>
        /// <returns>イベントコードリスト</returns>
        public IReadOnlyList<string> GetEventCodeStringList()
            => EventCommands.GetEventCodeStringList();

        /// <summary>
        /// イベントコマンド文字列情報リストを取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyList<EventCommandSentenceInfo> MakeEventCommandSentenceInfoList(
            EventCommandSentenceResolver resolver,
            EventCommandSentenceResolveDesc desc)
        {
            var sentenceType = EventCommandSentenceType.Common;

            return EventCommands.MakeEventCommandSentenceInfoList(resolver, sentenceType, desc);
        }


        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(CommonEvent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id
                   && name == other.name
                   && numberArgsLength == other.numberArgsLength
                   && strArgsLength == other.strArgsLength
                   && description == other.description
                   && memo == other.memo
                   && labelColor == other.labelColor
                   && FooterString == other.FooterString
                   && condition.Equals(other.condition)
                   && returnValueInfo.Equals(other.returnValueInfo)
                   && eventCommands.Equals(other.eventCommands)
                   && selfVariableNameList.Equals(other.selfVariableNameList)
                   && CommonEventSpecialArgDescList.Equals(other.CommonEventSpecialArgDescList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(HeaderBytes);

            // コモンイベントID
            result.AddRange(Id.ToBytes(Endian.Woditor));

            // 起動条件
            result.AddRange(BootCondition.ToBinary());

            // 数値引数の数
            result.Add((byte) NumberArgsLength);

            // 文字列引数の数
            result.Add((byte) StrArgsLength);

            // コモンイベント名
            result.AddRange(Name.ToWoditorStringBytes());

            // イベントコマンド行数
            result.AddRange(EventCommands.Count.ToBytes(Endian.Woditor));

            // イベントコマンド
            foreach (var command in EventCommands.ToList())
            {
                result.AddRange(command.ToBinary());
            }

            // コモンイベント説明
            result.AddRange(Description.ToWoditorStringBytes());

            // メモ
            result.AddRange(Memo.ToWoditorStringBytes());

            // 引数特殊指定
            result.AddRange(CommonEventSpecialArgDescList.ToBinary());

            // 引数初期値後のチェックディジット
            result.AddRange(AfterInitValueBytes);

            // コモンイベントラベル色
            result.AddRange(LabelColor.Code.ToBytes(Endian.Woditor));

            // 変数名
            result.AddRange(SelfVariableNameList.ToBinary());

            // 変数名後のチェックディジット
            result.AddRange(AfterMemoBytesSelfVariableNamesBytes);

            // フッタ文字列
            result.AddRange(FooterString.ToWoditorStringBytes());

            // コモンイベント末尾
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_00))
            {
                // ver2.00 未満の場合はここまで
                result.AddRange(FooterBytesBeforeVer2_00);
                return result.ToArray();
            }

            // 返戻値前のチェックディジット
            result.AddRange(BeforeReturnValueSummaryBytesBefore);

            // 返戻値
            result.AddRange(returnValueInfo.ToBinary());

            // フッタ
            result.AddRange(FooterBytesAfterVer2_00);

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Id), Id);
            info.AddValue(nameof(condition), condition);
            info.AddValue(nameof(numberArgsLength), numberArgsLength);
            info.AddValue(nameof(strArgsLength), strArgsLength);
            info.AddValue(nameof(name), name);
            info.AddValue(nameof(eventCommands), eventCommands);
            info.AddValue(nameof(description), description);
            info.AddValue(nameof(memo), memo);
            info.AddValue(nameof(labelColor), labelColor.Code);
            info.AddValue(nameof(footerString), footerString);
            info.AddValue(nameof(returnValueInfo), returnValueInfo);
            info.AddValue(nameof(selfVariableNameList), selfVariableNameList);
            info.AddValue(nameof(CommonEventSpecialArgDescList), CommonEventSpecialArgDescList);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected CommonEvent(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt32(nameof(Id));
            condition = info.GetValue<CommonEventBootCondition>(nameof(condition));
            numberArgsLength = info.GetInt32(nameof(numberArgsLength));
            strArgsLength = info.GetInt32(nameof(strArgsLength));
            name = info.GetValue<CommonEventName>(nameof(name));
            eventCommands = info.GetValue<EventCommandList>(nameof(eventCommands));
            description = info.GetValue<CommonEventDescription>(nameof(description));
            memo = info.GetValue<CommonEventMemo>(nameof(memo));
            labelColor = CommonEventLabelColor.FromInt(info.GetInt32(nameof(labelColor)));
            footerString = info.GetValue<CommonEventFooterString>(nameof(footerString));
            returnValueInfo = info.GetValue<CommonEventReturnValue>(nameof(returnValueInfo));
            selfVariableNameList = info.GetValue<CommonEventSelfVariableNameList>(nameof(selfVariableNameList));
            CommonEventSpecialArgDescList =
                info.GetValue<CommonEventSpecialArgDescList>(nameof(CommonEventSpecialArgDescList));
        }
    }
}