// ========================================
// Project Name : WodiLib
// File Name    : SpecialArgDescReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Common;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// 特殊引数読み込みクラス
    /// </summary>
    internal class SpecialArgDescReader
    {
        /// <summary>数値引数名リスト</summary>
        public List<string> NumberArgNameList { get; private set; } = new List<string>();

        /// <summary>文字列引数名リスト</summary>
        public List<string> StringArgNameList { get; private set; } = new List<string>();

        /// <summary>数値引数種別リスト</summary>
        public List<CommonEventArgType> NumberArgTypeList { get; private set; } = new List<CommonEventArgType>();

        /// <summary>文字列引数種別リスト</summary>
        public List<CommonEventArgType> StringArgTypeList { get; private set; } = new List<CommonEventArgType>();

        /// <summary>数値引数特殊指定文字列パラメータリスト</summary>
        public List<List<string>> NumberArgStringParamsList { get; private set; } = new List<List<string>>();

        /// <summary>文字列引数特殊指定文字列パラメータリスト</summary>
        public List<List<string>> StringArgStringParamsList { get; private set; } = new List<List<string>>();

        /// <summary>数値引数特殊指定数値パラメータリスト</summary>
        public List<List<int>> NumberArgNumberParamsList { get; private set; } = new List<List<int>>();

        /// <summary>文字列引数特殊指定数値パラメータリスト</summary>
        public List<List<int>> StringArgNumberParamsList { get; private set; } = new List<List<int>>();

        /// <summary>数値特殊指定数値初期値リスト</summary>
        public List<int> NumberArgInitValueList { get; private set; } = new List<int>();

        /// <summary>読み込み経過状態</summary>
        private FileReadStatus Status { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        public SpecialArgDescReader(FileReadStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 特殊引数リストを読み込み、返す。
        /// </summary>
        /// <returns>特殊引数リスト</returns>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        public void Read()
        {
            try
            {
                Logger.Debug(FileIOMessage.StartCommonRead(typeof(SpecialArgDescReader),
                    "特殊引数リスト"));

                // ヘッダ
                foreach (var b in CommonEventSpecialArgDescList.Header)
                {
                    if (Status.ReadByte() != b)
                        throw new InvalidOperationException(
                            $"ファイルデータが仕様と異なります。（offset:{Status.Offset}）");
                    Status.IncreaseByteOffset();
                }

                Logger.Debug(FileIOMessage.CheckOk(typeof(SpecialArgDescReader),
                    "ヘッダ"));

                // 引数名
                var argNameList = ReadArgNames(Status);
                var argNameListCount = argNameList.Count;
                NumberArgNameList = argNameList.Take(argNameListCount / 2).ToList();
                StringArgNameList = argNameList.Skip(argNameListCount / 2).ToList();
                // 引数特殊指定
                var argTypeList = ReadSpecialArgType(Status);
                var argTypeListCount = argTypeList.Count;
                NumberArgTypeList = argTypeList.Take(argTypeListCount / 2).ToList();
                StringArgTypeList = argTypeList.Skip(argTypeListCount / 2).ToList();
                // 数値特殊指定文字列パラメータ
                var stringArgLists = ReadSpecialStringArgList(Status);
                var stringArgListsCount = stringArgLists.Count;
                NumberArgStringParamsList = stringArgLists.Take(stringArgListsCount / 2).ToList();
                StringArgStringParamsList = stringArgLists.Skip(stringArgListsCount / 2).ToList();
                // 数値特殊指定数値パラメータ
                var numberArgLists = ReadSpecialNumberArgList(Status);
                var numberArgListsCount = numberArgLists.Count;
                NumberArgNumberParamsList = numberArgLists.Take(numberArgListsCount / 2).ToList();
                StringArgNumberParamsList = numberArgLists.Skip(numberArgListsCount / 2).ToList();
                // 数値特殊指定数値初期値
                NumberArgInitValueList = ReadInitValue(Status);

                Logger.Debug(FileIOMessage.EndCommonRead(typeof(SpecialArgDescReader),
                    "特殊引数リスト"));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"引数特殊指定データが仕様と異なります。（offset:{Status.Offset}）", ex);
            }
        }

        /// <summary>
        /// 引数名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>引数名リスト</returns>
        private List<string> ReadArgNames(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(SpecialArgDescReader),
                "引数名"));

            var result = new List<string>();

            var length = status.ReadInt();
            status.IncreaseIntOffset();

            for (var i = 0; i < length; i++)
            {
                var argName = status.ReadString();
                status.AddOffset(argName.ByteLength);
                result.Add(argName.String);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(SpecialArgDescReader),
                "引数名"));

            return result;
        }

        /// <summary>
        /// 引数特殊指定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>引数特殊指定リスト</returns>
        private List<CommonEventArgType> ReadSpecialArgType(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(SpecialArgDescReader),
                "引数特殊指定"));

            var result = new List<CommonEventArgType>();

            var length = status.ReadInt();
            status.IncreaseIntOffset();

            for (var i = 0; i < length; i++)
            {
                var b = status.ReadByte();
                status.IncreaseByteOffset();
                result.Add(CommonEventArgType.FromByte(b));
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(SpecialArgDescReader),
                "引数特殊指定"));

            return result;
        }

        /// <summary>
        /// 数値特殊指定文字列パラメータ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>数値特殊指定文字列パラメータリスト</returns>
        private List<List<string>> ReadSpecialStringArgList(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(SpecialArgDescReader),
                "数値特殊指定文字列パラメータ"));

            var result = new List<List<string>>();

            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                "特殊文字列引数の数", length));

            for (var i = 0; i < length; i++)
            {
                var caseLength = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                    $"引数{i}の文字列選択可能数", caseLength));

                var caseDescriptionList = new List<string>();

                for (var j = 0; j < caseLength; j++)
                {
                    var caseDescription = status.ReadString();
                    status.AddOffset(caseDescription.ByteLength);
                    caseDescriptionList.Add(caseDescription.String);

                    Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                        $"引数{i} {j}番目の文字列", caseDescription.String));
                }

                result.Add(caseDescriptionList);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(SpecialArgDescReader),
                "数値特殊指定文字列パラメータ"));

            return result;
        }

        /// <summary>
        /// 数値特殊指定数値パラメータ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>数値特殊指定数値パラメータリスト</returns>
        private List<List<int>> ReadSpecialNumberArgList(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(SpecialArgDescReader),
                "数値特殊指定数値パラメータ"));

            var result = new List<List<int>>();

            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                "特殊数値引数の数", length));

            for (var i = 0; i < length; i++)
            {
                var caseLength = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                    $"引数{i}の数値選択可能数", caseLength));

                var caseNumberList = new List<int>();

                for (var j = 0; j < caseLength; j++)
                {
                    var caseNumber = status.ReadInt();
                    status.IncreaseIntOffset();
                    caseNumberList.Add(caseNumber);

                    Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                        $"引数{i} {j}番目の数値", caseNumber));
                }

                result.Add(caseNumberList);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(SpecialArgDescReader),
                "数値特殊指定数値パラメータ"));

            return result;
        }

        /// <summary>
        /// 数値特殊指定数値初期値
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>数値特殊指定数値初期値リスト</returns>
        private List<int> ReadInitValue(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(SpecialArgDescReader),
                "数値特殊指定数値初期値"));

            var result = new List<int>();

            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                "数値特殊指定数値初期値の数", length));

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadInt();
                status.IncreaseIntOffset();

                result.Add(value);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(SpecialArgDescReader),
                    $"数値特殊指定数値{i}の初期値", value));
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(SpecialArgDescReader),
                "数値特殊指定数値初期値"));

            return result;
        }
    }
}