// ========================================
// Project Name : WodiLib
// File Name    : ChoiceStart.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・選択肢・開始
    /// </summary>
    [Serializable]
    public class ChoiceStart : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■文章選択肢:/ {0} ";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChoiceStart;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x02;

        /// <inheritdoc />
        public override byte StringVariableCount => (byte) CaseValue;

        /// <inheritdoc />
        /// <summary>文字列変数最小個数</summary>
        public override byte StringVariableCountMin => 0x01;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 1)] インデックス</param>
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
                    byte[] bytes =
                    {
                        (byte) (CancelForkIndex.Code + CaseValue),
                        forkFlags.ToByte(),
                        0x00, 0x00
                    };
                    return bytes.ToInt32(Endian.Environment);

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 1, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 1)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            if (index == 1)
            {
                var bytes = value.ToBytes(Endian.Environment);
                var cancelFlgByte = (byte) (bytes[0] & 0xF0);
                CancelForkIndex = ChoiceCancelForkType.ForByte(cancelFlgByte);
                CaseValue = bytes[0] & 0x0F;

                forkFlags = new ChoiceForkFlags(bytes[1]);
                return;
            }

            throw new ArgumentOutOfRangeException(
                ErrorMessage.OutOfRange(nameof(index), 1, 1, index));
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, CaseValue - 1)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            return ChoiceCaseList[index];
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, CaseValue - 1)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyStringVariable(int index, string value)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            if (value is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
            ChoiceCaseList[index] = value;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));

            var caseStrList = Enumerable.Range(0, CaseValue)
                .Select(idx => ChoiceCaseList[idx])
                .ToList();
            // 以降の処理のために自身の選択肢をResolveDescに登録
            desc.StartBranch(BranchType.Choice, caseStrList);

            var casesStr = string.Join(" / ",
                caseStrList.Select((s, i) => $"【{i + 1}】{s}"));

            return string.Format(EventCommandSentenceFormat,
                casesStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ChoiceCancelForkType cancelForkIndex = ChoiceCancelForkType.Else;

        /// <summary>[NotNull] キャンセル時分岐番号</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ChoiceCancelForkType CancelForkIndex
        {
            get => cancelForkIndex;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CancelForkIndex)));
                cancelForkIndex = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        public ChoiceCaseList ChoiceCaseList { get; } = new ChoiceCaseList();

        /// <summary>[Range(1, 12)] 選択肢数</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public int CaseValue
        {
            get => ChoiceCaseList.CaseValue;
            set
            {
                if (value < 1 || 12 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseValue), 1, 12, value));
                if (value >= 11)
                {
                    Logger.Warning("選択肢数が11以上のため、ウディタ上で編集すると設定が失われる場合があります。");
                }

                ChoiceCaseList.CaseValue = value;
                NotifyPropertyChanged(nameof(StringVariableCount));
            }
        }

        /// <summary>選択肢その1</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case1
        {
            get => ChoiceCaseList[0];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case1)));
                ChoiceCaseList[0] = value;
            }
        }

        /// <summary>選択肢その2</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case2
        {
            get => ChoiceCaseList[1];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case2)));
                ChoiceCaseList[1] = value;
            }
        }

        /// <summary>選択肢その3</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case3
        {
            get => ChoiceCaseList[2];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case3)));
                ChoiceCaseList[2] = value;
            }
        }

        /// <summary>選択肢その4</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case4
        {
            get => ChoiceCaseList[3];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case4)));
                ChoiceCaseList[3] = value;
            }
        }

        /// <summary>選択肢その5</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case5
        {
            get => ChoiceCaseList[4];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case5)));
                ChoiceCaseList[4] = value;
            }
        }

        /// <summary>選択肢その6</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case6
        {
            get => ChoiceCaseList[5];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case6)));
                ChoiceCaseList[5] = value;
            }
        }

        /// <summary>選択肢その7</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case7
        {
            get => ChoiceCaseList[6];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case7)));
                ChoiceCaseList[6] = value;
            }
        }

        /// <summary>選択肢その8</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case8
        {
            get => ChoiceCaseList[7];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case8)));
                ChoiceCaseList[7] = value;
            }
        }

        /// <summary>選択肢その9</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case9
        {
            get => ChoiceCaseList[8];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case9)));
                ChoiceCaseList[8] = value;
            }
        }

        /// <summary>選択肢その10</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case10
        {
            get => ChoiceCaseList[9];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case10)));
                ChoiceCaseList[9] = value;
            }
        }

        /// <summary>選択肢その11</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string Case11
        {
            get => ChoiceCaseList[10];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case10)));
                ChoiceCaseList[10] = value;
            }
        }

        /// <summary>選択肢その12</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string Case12
        {
            get => ChoiceCaseList[11];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case10)));
                ChoiceCaseList[11] = value;
            }
        }

        private ChoiceForkFlags forkFlags = new ChoiceForkFlags();

        /// <summary>強制中断フラグ</summary>
        public bool IsStopForce
        {
            get => forkFlags.IsStopForce;
            set => forkFlags.IsStopForce = value;
        }

        /// <summary>左キー分岐</summary>
        public bool IsForkLeftKey
        {
            get => forkFlags.IsForkLeftKey;
            set => forkFlags.IsForkLeftKey = value;
        }

        /// <summary>右キー分岐</summary>
        public bool IsForkRightKey
        {
            get => forkFlags.IsForkRightKey;
            set => forkFlags.IsForkRightKey = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 選択肢プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnChoiceCaseListPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(EventCommand.ChoiceCaseList.CaseValue):
                    NotifyPropertyChanged(args.PropertyName);
                    NotifyPropertyChanged(nameof(StringVariableCount));
                    break;
            }
        }

        /// <summary>
        /// 選択肢コレクション変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnChoiceCaseListCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldStartingIndex != -1)
            {
                NotifyPropertyChanged($"Case{args.OldStartingIndex + 1}");
            }

            if (args.NewStartingIndex != -1)
            {
                NotifyPropertyChanged($"Case{args.NewStartingIndex + 1}");
            }
        }

        /// <summary>
        /// 選択肢分岐フラグプロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnForkFlagsPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(ChoiceForkFlags.IsStopForce):
                case nameof(ChoiceForkFlags.IsForkLeftKey):
                case nameof(ChoiceForkFlags.IsForkRightKey):
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
        public ChoiceStart()
        {
            ChoiceCaseList.PropertyChanged += OnChoiceCaseListPropertyChanged;
            ChoiceCaseList.CollectionChanged += OnChoiceCaseListCollectionChanged;
            forkFlags.PropertyChanged += OnForkFlagsPropertyChanged;
        }
    }
}