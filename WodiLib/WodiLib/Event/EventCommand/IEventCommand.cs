// ========================================
// Project Name : WodiLib
// File Name    : IEventCommand.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドインタフェース
    /// </summary>
    public interface IEventCommand : IEquatable<IEventCommand>
    {
        /// <summary>数値変数個数</summary>
        byte NumberVariableCount { get; }

        /// <summary>数値変数最小個数</summary>
        byte NumberVariableCountMin { get; }

        /// <summary>イベントコマンドコード</summary>
        EventCommandCode EventCommandCode { get; }

        /// <summary>実際のイベントコマンドコード</summary>
        /// <remarks>
        /// コマンドコード種別が「未定義」の場合、実際に指定された値が格納される。
        /// </remarks>
        int RawEventCommandCode { get; }

        /// <summary>インデントの深さ</summary>
        Indent Indent { get; set; }

        /// <summary>文字列変数個数</summary>
        byte StringVariableCount { get; }

        /// <summary>文字列変数最小個数</summary>
        byte StringVariableCountMin { get; }

        /// <summary>動作指定ありフラグ</summary>
        bool HasActionEntry { get; }

        /// <summary>拡張数値引数リスト</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        EventCommandExpansionNumberArgList ExpansionNumberArgList { get; }

        /// <summary>拡張文字列引数リスト</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        EventCommandExpansionStringArgList ExpansionStringArgList { get; }

        /// <summary>すべての数値引数リスト</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IReadOnlyList<int> AllNumberArgList { get; }

        /// <summary>すべての文字列引数リスト</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IReadOnlyList<string> AllStringArgList { get; }

        /// <summary>キャラ動作指定
        /// <para>動作指定を持たないコマンドの場合、null</para></summary>
        ActionEntry ActionEntry { get; set; }

        /// <summary>拡張文字列</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        string ExpansionString { get; set; }

        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// 拡張引数にも対応する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>数値変数</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        int GetNumberVariable(int index);

        /// <summary>
        /// インデックスを指定して数値変数を設定する。
        /// 拡張引数にも対応する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetNumberVariable(int index, int value);

        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// 拡張引数にも対応する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>設定値</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        string GetStringVariable(int index);

        /// <summary>
        /// インデックスを指定して数値変数を設定する。
        /// 拡張引数にも対応する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetStringVariable(int index, string value);

        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>数値変数</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        int GetSafetyNumberVariable(int index);

        /// <summary>
        /// インデックスを指定して数値変数を設定する。
        /// ウディタ標準仕様でサポートしているインデックスのみ設定可能。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetSafetyNumberVariable(int index, int value);

        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>設定値</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        string GetSafetyStringVariable(int index);

        /// <summary>
        /// インデックスを指定して文字列変数を設定する。
        /// ウディタ標準仕様でサポートしているインデックスのみ設置可能。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定値</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void SetSafetyStringVariable(int index, string value);

        /// <summary>
        /// イベントコードに変換する。
        /// </summary>
        /// <returns>イベントコード文字列</returns>
        string ToEventCodeString();

        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        void OutputVersionWarningLogIfNeed();

        /// <summary>
        /// イベントコマンド文字列情報を取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        EventCommandSentenceInfo GetEventCommandSentenceInfo(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc);

        /// <summary>バイナリデータに変換する。</summary>
        /// <returns>バイナリデータ</returns>
        byte[] ToBinary();
    }
}