// ========================================
// Project Name : WodiLib
// File Name    : ActionEntry.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定
    /// </summary>
    public class ActionEntry : IWodiLibObject
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ヘッダバイト</summary>
        public static readonly byte[] HeaderBytes = {0x03, 0x03, 0x03, 0x00, 0x03};

        /// <summary>動作完了までウェイトフラグ</summary>
        public static readonly byte FlgWaitForCompleteOn = 0x04;

        /// <summary>動作を繰り返すフラグ</summary>
        public static readonly byte FlgRepeatAction = 0x01;

        /// <summary>移動できない場合は飛ばすフラグ</summary>
        public static readonly byte FlgSkipIfCannotMove = 0x02;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>動作完了まで待機フラグ</summary>
        public bool IsWaitForComplete { get; set; }

        /// <summary>動作繰り返しフラグ</summary>
        public bool IsRepeatAction { get; set; }

        /// <summary>移動できないときは飛ばすフラグ</summary>
        public bool IsSkipIfCannotMove { get; set; } = true;

        /// <summary>
        /// オプションフラグをセットする。
        /// </summary>
        /// <param name="flag">オプションフラグ</param>
        public void SetOptionFlag(byte flag)
        {
            // 動作完了までウェイト
            IsWaitForComplete = (flag & FlgWaitForCompleteOn) != 0;
            // 動作を繰り返す
            IsRepeatAction = (flag & FlgRepeatAction) != 0;
            // 移動できない場合は飛ばす
            IsSkipIfCannotMove = (flag & FlgSkipIfCannotMove) != 0;
        }

        /// <summary>動作指定コマンドリスト</summary>
        public List<ICharaMoveCommand> CommandList { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commands">動作指定コマンド</param>
        public ActionEntry(IEnumerable<ICharaMoveCommand> commands = null)
        {
            CommandList = commands == null ? new List<ICharaMoveCommand>() : commands.ToList();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オプション用byteを生成する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte MakeOptionByte()
        {
            byte result = 0x00;
            // 動作完了までウェイト
            if (IsWaitForComplete) result += FlgWaitForCompleteOn;
            // 動作を繰り返す
            if (IsRepeatAction) result += FlgRepeatAction;
            // 移動できない場合は飛ばす
            if (IsSkipIfCannotMove) result += FlgSkipIfCannotMove;
            return result;
        }

        /// <summary>
        /// 移動ルート用byte配列を生成する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public IEnumerable<byte> MakeMoveRouteByte()
        {
            var result = new List<byte>();
            foreach (var command in CommandList)
            {
                result.AddRange(command.ToBinary());
            }

            return result;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();
            // ヘッダ
            result.AddRange(HeaderBytes);
            // イベントフラグ
            result.Add(MakeOptionByte());
            // 動作コマンド数
            {
                var length = CommandList.Count;
                var lengthBytes = length.ToBytes(Endian.Woditor);
                result.AddRange(lengthBytes);
            }
            // 動作コマンド
            foreach (var command in CommandList)
            {
                result.AddRange(command.ToBinary());
            }

            return result.ToArray();
        }
    }
}