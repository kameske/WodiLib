// ========================================
// Project Name : WodiLib
// File Name    : EventCommandBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドのすべて共通処理を定義する。
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public abstract class EventCommandBase : ModelBase<EventCommandBase>, IEventCommand
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>数値引数最大数</summary>
        public static int NumberArgsLengthMax => 126;

        /// <summary>数値引数最小数</summary>
        public static int NumberArgsLengthMin => 0;

        /// <summary>文字列引数最大数</summary>
        public static int StringArgsLengthMax => 127;

        /// <summary>文字列引数最小数</summary>
        public static int StringArgsLengthMin => 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>動作指定なしフラグ値</summary>
        private const byte FlgNotHasActionEntry = 0x00;

        /// <summary>動作指定ありフラグ値</summary>
        private const byte FlgHasActionEntry = 0x01;

        /// <summary>コマンドコードフォーマット</summary>
        protected string CommandCodeFormat = "[{0}][{1},{2}]<{3}>({4})({5}){6}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>数値変数個数</summary>
        public abstract byte NumberVariableCount { get; }

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public virtual byte NumberVariableCountMin => NumberVariableCount;

        /// <inheritdoc />
        /// <summary>イベントコマンドコード</summary>
        public abstract EventCommandCode EventCommandCode { get; }

        /// <summary>実際のイベントコマンドコード</summary>
        /// <remarks>
        /// コマンドコード種別が「未定義」の場合、実際に指定された値が格納される。
        /// </remarks>
        public virtual int RawEventCommandCode => EventCommandCode.Code;

        private Indent indent;

        /// <inheritdoc />
        /// <summary>インデントの深さ</summary>
        public Indent Indent
        {
            get => indent;
            set
            {
                indent = value;
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>文字列変数個数</summary>
        public abstract byte StringVariableCount { get; }

        /// <inheritdoc />
        /// <summary>文字列変数最小個数</summary>
        public virtual byte StringVariableCountMin => StringVariableCount;

        /// <inheritdoc />
        /// <summary>動作指定ありフラグ</summary>
        public virtual bool HasActionEntry => false;

        /// <summary>拡張数値引数リスト</summary>
        public EventCommandExpansionNumberArgList ExpansionNumberArgList { get; }
            = new EventCommandExpansionNumberArgList();

        /// <summary>拡張文字列引数リスト</summary>
        public EventCommandExpansionStringArgList ExpansionStringArgList { get; }
            = new EventCommandExpansionStringArgList();

        /// <summary>すべての数値引数リスト</summary>
        public IReadOnlyList<int> AllNumberArgList
        {
            get
            {
                var args = new List<int>();

                // 通常数値引数
                for (var i = 0; i < NumberVariableCount; i++)
                {
                    args.Add(GetSafetyNumberVariable(i));
                }

                // 拡張数値引数
                var maxExpansionLength = NumberArgsLengthMax - NumberVariableCount;
                args.AddRange(ExpansionNumberArgList.Where((_, idx) => idx <= maxExpansionLength));

                return args;
            }
        }

        /// <summary>すべての文字列引数リスト</summary>
        public IReadOnlyList<string> AllStringArgList
        {
            get
            {
                var args = new List<string>();

                // 通常数値引数
                for (var i = 0; i < StringVariableCount; i++)
                {
                    args.Add(GetSafetyStringVariable(i));
                }

                // 拡張数値引数
                var maxExpansionLength = StringArgsLengthMax - StringVariableCount;
                args.AddRange(ExpansionStringArgList.Where((_, idx) => idx <= maxExpansionLength));

                return args;
            }
        }

        /// <inheritdoc />
        /// <summary>キャラ動作指定
        /// <para>動作指定を持たないコマンドの場合、null</para></summary>
        public virtual ActionEntry ActionEntry
        {
            get => null;
            set { }
        }

        /// <summary>イベントコマンドカラーセット</summary>
        protected abstract EventCommandColorSet EventCommandColorSet { get; }

        /// <inheritdoc />
        public virtual int GetNumberVariable(int index)
        {
            var max = AllNumberArgList.Count;
            if (index < 0 || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, max, index));

            if (index < NumberVariableCount) return GetSafetyNumberVariable(index);
            return ExpansionNumberArgList[index - NumberVariableCount];
        }

        private string expansionString = "";

        /// <inheritdoc />
        /// <summary>拡張文字列</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string ExpansionString
        {
            get => expansionString;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ExpansionString)));
                expansionString = value;
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        public void SetNumberVariable(int index, int value)
        {
            var max = AllNumberArgList.Count;
            if (index < 0 || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, max, index));

            if (IsNormalNumberArgIndex(index))
            {
                SetSafetyNumberVariable(index, value);
                return;
            }

            var expansionIndex = index - NumberVariableCount;
            ExpansionNumberArgList.AdjustLength(expansionIndex + 1);
            ExpansionNumberArgList[expansionIndex] = value;
        }

        /// <inheritdoc />
        public string GetStringVariable(int index)
        {
            var max = AllStringArgList.Count;
            if (index < 0 || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, max, index));

            if (index < StringVariableCount) return GetSafetyStringVariable(index);
            return ExpansionStringArgList[index - StringVariableCount];
        }

        /// <inheritdoc />
        public virtual void SetStringVariable(int index, string value)
        {
            var max = AllStringArgList.Count;
            if (index < 0 || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, max, index));

            if (IsNormalStringArgIndex(index))
            {
                SetSafetyStringVariable(index, value);
                return;
            }

            var expansionIndex = index - StringVariableCount;
            ExpansionStringArgList.AdjustLength(expansionIndex + 1);
            ExpansionStringArgList[expansionIndex] = value;
        }

        /// <inheritdoc />
        public abstract int GetSafetyNumberVariable(int index);

        /// <inheritdoc />
        public abstract void SetSafetyNumberVariable(int index, int value);

        /// <inheritdoc />
        public abstract string GetSafetyStringVariable(int index);

        /// <inheritdoc />
        public abstract void SetSafetyStringVariable(int index, string value);

        /// <inheritdoc />
        public bool Equals(IEventCommand other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            if (ActionEntry is null ^ other.ActionEntry is null) return false;
            return RawEventCommandCode == other.RawEventCommandCode
                   && Indent == other.Indent
                   && HasActionEntry == other.HasActionEntry
                   && ExpansionString.Equals(other.ExpansionString)
                   && AllNumberArgList.SequenceEqual(other.AllNumberArgList)
                   && AllStringArgList.SequenceEqual(other.AllStringArgList)
                   && (ActionEntry is null || (ActionEntry.Equals(other.ActionEntry)));
        }

        /// <inheritdoc />
        public override bool Equals(EventCommandBase other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return Equals(other);
        }

        /// <inheritdoc />
        public virtual string ToEventCodeString()
        {
            var numArgs = AllNumberArgList.Where((_, idx) => idx != 0)
                .Select(x => x.ToString()).ToList();
            var strArgs = AllStringArgList.Select(x =>
                    // 改行コードは除去する。除去しない場合は各自このメソッドをオーバーライドして調整
                    $"\"{x}\"".Replace("\r", "").Replace("\n", ""))
                .ToList();

            return string.Format(CommandCodeFormat,
                RawEventCommandCode,
                numArgs.Count, strArgs.Count,
                Indent,
                string.Join(",", numArgs),
                string.Join(",", strArgs),
                ExpansionString);
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCommandSentenceInfo GetEventCommandSentenceInfo(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            if (resolver is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(resolver)));
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            var indentStr = EventCommandSentenceResolver.MakeIndentString(Indent);
            var main = MakeEventCommandMainSentence(resolver, type, desc);

            return new EventCommandSentenceInfo(
                EventCommandColorSet,
                $"{indentStr}{main}");
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ロガー
        /// </summary>
        [field: NonSerialized]
        protected WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public virtual void OutputVersionWarningLogIfNeed()
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     internal Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定した数値引数インデックスが通常使用の範囲であるか（拡張引数でないか）を返す。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>通常使用範囲の引数インデックスの場合true</returns>
        internal virtual bool IsNormalNumberArgIndex(int index) => index < NumberVariableCount;

        /// <summary>
        /// 指定した文字列引数インデックスが通常使用の範囲であるか（拡張引数でないか）を返す。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>通常使用範囲の引数インデックスの場合true</returns>
        internal virtual bool IsNormalStringArgIndex(int index) => index < StringVariableCount;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文字列のメイン部分（インデント以降の部分）を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列のメイン部分</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        protected abstract string MakeEventCommandMainSentence(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public virtual byte[] ToBinary()
        {
            OutputVersionWarningLogIfNeed();

            var result = new List<byte>();

            // 数値変数
            result.AddRange(MakeNumberVariableBytes());
            // インデントの深さ
            result.AddRange(MakeIndentBytes());
            // 文字列変数
            result.AddRange(MakeStringVariableBytes());
            // 動作指定コマンド
            result.AddRange(MakeActionEntryBytes());

            return result.ToArray();
        }

        private byte[] MakeNumberVariableBytes()
        {
            var args = AllNumberArgList;

            var resultSeed = new List<byte>();

            var argsLengthByte = (byte) args.Count;
            resultSeed.Add(argsLengthByte);

            return args.Select(x => x.ToWoditorIntBytes())
                .Aggregate(resultSeed, (n, elem) =>
                {
                    n.AddRange(elem);
                    return n;
                }).ToArray();
        }

        private byte[] MakeIndentBytes()
        {
            return new[] {Indent.ToSbyte().ToByte()};
        }

        private byte[] MakeStringVariableBytes()
        {
            var args = AllStringArgList;

            var resultSeed = new List<byte>();

            var argsLengthByte = (byte) args.Count;
            resultSeed.Add(argsLengthByte);

            return args.Select(x => new WoditorString(x).StringByte)
                .Aggregate(resultSeed, (n, elem) =>
                {
                    n.AddRange(elem);
                    return n;
                }).ToArray();
        }

        private byte[] MakeActionEntryBytes()
        {
            var result = new List<byte>();
            if (ActionEntry is null)
            {
                result.Add(FlgNotHasActionEntry);
                return result.ToArray();
            }

            result.Add(FlgHasActionEntry);
            result.AddRange(ActionEntry.ToBinary());
            return result.ToArray();
        }
    }
}