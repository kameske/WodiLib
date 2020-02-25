// ========================================
// Project Name : WodiLib
// File Name    : ActionEntry.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    /// 動作指定
    /// </summary>
    [Serializable]
    public class ActionEntry : IEquatable<ActionEntry>, ISerializable
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

        private const string EventCommandSentenceMoveRouteJoinStr = " / ";

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

        private CharaMoveCommandList commandList = new CharaMoveCommandList();

        /// <summary>[NotNull] 動作指定コマンドリスト</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CharaMoveCommandList CommandList
        {
            get => commandList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CommandList)));

                commandList = value;
                commandList.Owner = Owner;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private TargetAddressOwner owner;

        /// <summary>[Nullable] 所有イベント種別</summary>
        internal TargetAddressOwner Owner
        {
            get => owner;
            set
            {
                owner = value;
                CommandList.Owner = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>所有イベント保持フラグ</summary>
        private bool HasOwner => !(owner is null);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commands">動作指定コマンド</param>
        public ActionEntry(IEnumerable<ICharaMoveCommand> commands = null)
        {
            if (commands is null) return;

            var charaMoveCommands = commands as ICharaMoveCommand[] ?? commands.ToArray();
            CommandList = new CharaMoveCommandList(charaMoveCommands);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
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

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ActionEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsWaitForComplete == other.IsWaitForComplete
                   && IsRepeatAction == other.IsRepeatAction
                   && IsSkipIfCannotMove == other.IsSkipIfCannotMove
                   && commandList.Equals(other.commandList);
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


        /// <summary>
        /// イベントコマンド文字列を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列のメイン部分</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object GetEventCommandSentence(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            return string.Join(EventCommandSentenceMoveRouteJoinStr,
                CommandList.Select(x => x.GetEventCommandSentence(resolver, type, desc)));
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
            info.AddValue(nameof(IsWaitForComplete), IsWaitForComplete);
            info.AddValue(nameof(IsRepeatAction), IsRepeatAction);
            info.AddValue(nameof(IsSkipIfCannotMove), IsSkipIfCannotMove);
            info.AddValue(nameof(commandList), commandList);
            info.AddValue(nameof(HasOwner), HasOwner);
            if (HasOwner) info.AddValue(nameof(owner), owner.Id);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ActionEntry(SerializationInfo info, StreamingContext context)
        {
            IsWaitForComplete = info.GetBoolean(nameof(IsWaitForComplete));
            IsRepeatAction = info.GetBoolean(nameof(IsRepeatAction));
            IsSkipIfCannotMove = info.GetBoolean(nameof(IsSkipIfCannotMove));
            commandList = info.GetValue<CharaMoveCommandList>(nameof(commandList));
            var savedOwner = info.GetBoolean(nameof(HasOwner));
            if (savedOwner) owner = TargetAddressOwner.FromId(info.GetValue<string>(nameof(owner)));
        }
    }
}