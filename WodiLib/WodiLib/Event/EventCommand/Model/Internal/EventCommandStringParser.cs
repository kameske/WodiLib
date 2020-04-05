// ========================================
// Project Name : WodiLib
// File Name    : EventCommandStringParser.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコードをパースするクラス
    /// </summary>
    internal class EventCommandStringParser
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コマンドコードの正規表現
        /// </summary>
        public readonly string regex = @"^\[(.*)\]\[(.*)\]<(.*)>\((.*)\)\((.*)\)(.*)$";

        /// <summary>
        /// 項目分割文字
        /// </summary>
        public readonly char itemSplitter = ',';

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 内部使用の分割文字列
        /// </summary>
        private const string splitter = "__";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコード
        /// </summary>
        public string EventCodeString { get; }

        /// <summary>
        /// インデント<br/>
        /// <see cref="Parse"/>実行完了までは値が正しく取得できない。
        /// </summary>
        public Indent Indent { get; private set; }

        /// <summary>
        /// 数値引数リスト<br/>
        /// <see cref="Parse"/>実行完了までは空リストが返却される。
        /// </summary>
        public List<int> NumberArgList { get; private set; } = new List<int>();

        /// <summary>
        /// 文字列引数リスト<br/>
        /// <see cref="Parse"/>実行完了までは空リストが返却される。
        /// </summary>
        public List<string> StrArgList { get; private set; } = new List<string>();

        /// <summary>
        /// 拡張文字列<br/>
        /// <see cref="Parse"/>実行完了までは空文字が返却される。
        /// </summary>
        public string ExpansionString { get; private set; } = "";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// パース済みフラグ
        /// </summary>
        private bool IsParsed { get; set; }

        /// <summary>
        /// ロガー
        /// </summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventCodeString">[NotNullOrEmpty] コマンドコード文字列</param>
        /// <exception cref="ArgumentNullException">eventCodeString が null の場合</exception>
        /// <exception cref="ArgumentException">eventCodeString が空文字の場合</exception>
        public EventCommandStringParser(string eventCodeString)
        {
            if (eventCodeString is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(eventCodeString)));
            if (eventCodeString.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(eventCodeString)));

            EventCodeString = eventCodeString;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     すでにパース実行済みの場合、
        ///     または EventCodeString がコマンドコードとして不適切な場合
        /// </exception>
        /// <exception cref="InvalidCastException">EventCodeString に含まれる文字列が適切に変換できなかった場合</exception>
        public void Parse()
        {
            if (IsParsed)
                throw new InvalidOperationException(
                    "すでにパース済みのため、実行できません。");

            Logger.Debug("イベントコードのパースを開始します。");

            IsParsed = true;

            var splitText = GetSplitText();
            Logger.Debug($"コマンドコード文字列の分割に成功しました。{splitText}");

            var code = ConvertStringToEventCode(splitText[0]);
            Logger.Debug($"    イベントコード：{code}");

            var (intArgLength, strArgLength) = ConvertStringToArgLengths(splitText[1]);
            Logger.Debug($"    数値引数の長さ：{intArgLength}");
            Logger.Debug($"    文字列引数の長さ：{strArgLength}");

            Indent = ConvertStringToIndent(splitText[2]);
            Logger.Debug($"    インデント：{Indent}");

            var numArgList = ConvertStringToNumberList(splitText[3], intArgLength);

            // 数値引数リストの0番目はイベントコードである必要がある
            numArgList.Insert(0, code);

            NumberArgList = numArgList;
            Logger.Debug($"    数値引数リスト：{NumberArgList}");

            StrArgList = ConvertStringToStringList(splitText[4], strArgLength);
            Logger.Debug($"    文字列引数リスト：{StrArgList}");

            Logger.Debug($"    拡張文字列：{splitText[5]}");
            ExpansionString = splitText[5];

            Logger.Debug("イベントコードのパースが完了しました。");
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// CommandCodeStringを正規表現に従い分割した配列を返す。
        /// </summary>
        /// <returns>分割後の文字列配列</returns>
        /// <exception cref="InvalidOperationException">EventCodeString がコマンドコードとして不適切な場合</exception>
        private string[] GetSplitText()
        {
            var replaceDst = $"$1{splitter}$2{splitter}$3{splitter}$4{splitter}$5{splitter}$6";

            var replaced = Regex.Replace(EventCodeString, regex, replaceDst);
            var split = Regex.Split(replaced, splitter);

            if (split.Length != 6)
                throw new InvalidOperationException(
                    ErrorMessage.Unsuitable(nameof(EventCodeString), EventCodeString));

            return split;
        }

        /// <summary>
        /// コマンドコード値を文字列から数値に変換する。
        /// </summary>
        /// <param name="codeStr">コマンドコード値文字列</param>
        /// <returns>コマンドコード数値</returns>
        /// <exception cref="InvalidCastException">変換できない場合</exception>
        private int ConvertStringToEventCode(string codeStr)
        {
            if (!int.TryParse(codeStr, out var eventCode))
                throw new InvalidCastException(
                    GetParseErrorMessage("イベントコード"));
            return eventCode;
        }

        /// <summary>
        /// 引数の数の組を文字列から数値に変換する。
        /// </summary>
        /// <param name="argStr">引数の数文字列</param>
        /// <returns>引数の数</returns>
        /// <exception cref="InvalidOperationException">
        ///     argStr が正しく分割できない場合、
        ///     または引数の数として不適切な値が指定されていた場合
        /// </exception>
        /// <exception cref="InvalidCastException">変換できない場合</exception>
        private (int, int) ConvertStringToArgLengths(string argStr)
        {
            var args = argStr.Split(itemSplitter);
            if (args.Length != 2)
                throw new InvalidOperationException(
                    ErrorMessage.Unsuitable(nameof(EventCodeString), EventCodeString));
            if (!int.TryParse(args[0].Trim(), out var numArgs))
                throw new InvalidCastException(
                    GetParseErrorMessage("数値引数の数"));
            if (numArgs < EventCommandBase.NumberArgsLengthMin || EventCommandBase.NumberArgsLengthMax < numArgs)
                throw new InvalidOperationException(
                    ErrorMessage.OutOfRange("数値引数の数", EventCommandBase.NumberArgsLengthMin,
                        EventCommandBase.NumberArgsLengthMax, numArgs));

            if (!int.TryParse(args[1].Trim(), out var strArgs))
                throw new InvalidCastException(
                    GetParseErrorMessage("文字列引数の数"));
            if (strArgs < EventCommandBase.StringArgsLengthMin || EventCommandBase.StringArgsLengthMax < strArgs)
                throw new InvalidOperationException(
                    ErrorMessage.OutOfRange("文字列引数の数", EventCommandBase.StringArgsLengthMin,
                        EventCommandBase.StringArgsLengthMax, numArgs));

            return (numArgs, strArgs);
        }

        /// <summary>
        /// インデントを文字列から数値に変換する。
        /// </summary>
        /// <param name="indentStr">インデント文字列</param>
        /// <returns>インデント値</returns>
        /// <exception cref="InvalidCastException">変換できない場合</exception>
        /// <exception cref="InvalidOperationException">インデントの値として不適切な値が設定されていた場合</exception>
        private Indent ConvertStringToIndent(string indentStr)
        {
            if (!sbyte.TryParse(indentStr, out var indent))
                throw new InvalidCastException(
                    GetParseErrorMessage("インデント"));
            return indent;
        }

        /// <summary>
        /// 数値引数文字列を数値引数リストに変換する。
        /// </summary>
        /// <param name="numberArgsStr">数値引数文字列</param>
        /// <param name="hopeLength">希望する長さ</param>
        /// <returns>数値引数リスト</returns>
        /// <exception cref="InvalidCastException">数値が正しく変化できなかった場合</exception>
        private List<int> ConvertStringToNumberList(string numberArgsStr, int hopeLength)
        {
            var numArgList = numberArgsStr.IsEmpty()
                ? new List<int>()
                : numberArgsStr.Split(itemSplitter).Select(s =>
                {
                    // ほしいのはint配列なので変換
                    if (!int.TryParse(s.Trim(), out var result))
                        throw new InvalidCastException(
                            GetParseErrorMessage("数値引数"));
                    return result;
                }).ToList();

            // 希望する引数の数と実際の数が異なる場合、希望する数を優先する
            if (numArgList.Count != hopeLength)
            {
                Logger.Warning("指定された数値引数の数と実際の数が異なるため、自動補正します。");
                numArgList.AdjustLength(hopeLength, i => default);
            }

            return numArgList;
        }

        /// <summary>
        /// 数値引数文字列を数値引数リストに変換する。
        /// </summary>
        /// <param name="stringArgsStr">文字列引数文字列</param>
        /// <param name="hopeLength">希望する長さ</param>
        /// <returns>文字列引数リスト</returns>
        /// <exception cref="InvalidCastException"></exception>
        private List<string> ConvertStringToStringList(string stringArgsStr, int hopeLength)
        {
            const string encloseRegex = "^\"(.*)\"$";
            var strArgList = stringArgsStr.IsEmpty()
                ? new List<string>()
                : stringArgsStr.Split(itemSplitter)
                    // 前後の "" をトリム
                    .Select(x => Regex.Replace(x.Trim(), encloseRegex, "$1"))
                    .ToList();

            // 希望する引数の数と実際の数が異なる場合、希望する数を優先する
            if (strArgList.Count != hopeLength)
            {
                Logger.Warning("指定された文字列引数の数と実際の数が異なるため、自動補正します。");
                strArgList.AdjustLength(hopeLength, i => "");
            }

            return strArgList;
        }


        /// <summary>
        /// エラーメッセージを取得する。
        /// </summary>
        /// <param name="itemName">エラー項目</param>
        /// <returns>メッセージ</returns>
        private string GetParseErrorMessage(string itemName)
            => $"{itemName}の取得に失敗しました。{{ {EventCodeString} }}";
    }
}