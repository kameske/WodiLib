// ========================================
// Project Name : WodiLib
// File Name    : MoveRoute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WodiLib.Cmn;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンド・動作指定
    /// </summary>
    [Serializable]
    public class MoveRoute : EventCommandBase, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■動作指定：{2}{0} / {1}";

        private const string EventCommandSentenceTargetThisEvent = "このイベント";
        private const string EventCommandSentenceTargetHero = "主人公";
        private const string EventCommandSentenceTargetMember = "仲間{0}";
        private const string EventCommandSentenceTargetMapEvent = "Ev{0}";
        private const string EventCommandSentenceTargetVarAddress = "ｷｬﾗ[{0}]";

        private const string EventCommandSentenceWait = "[ｳｪｲﾄ] ";
        private const string EventCommandSentenceNonWait = "";

        private const int TargetHero = -2;
        private const int TargetThisEvent = -1;

        private const int EventCommandSentenceMaxLength = 200;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.MoveRoute;

        /// <inheritdoc />
        public override bool HasActionEntry => true;

        private ActionEntry actionEntry = new ActionEntry();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>[NotNull] 動作指定</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public override ActionEntry ActionEntry
        {
            get => actionEntry;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ActionEntry)));
                actionEntry = value;
                actionEntry.Owner = Owner;
                NotifyPropertyChanged();
            }
        }

        private int target;

        /// <summary>対象イベントID</summary>
        public int Target
        {
            get => target;
            set
            {
                target = value;
                NotifyPropertyChanged();
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
                actionEntry.Owner = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x02;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

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
        public MoveRoute()
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
                    return Target;

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
                Target = value;
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
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetSafetyStringVariable(int index)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void SetSafetyStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var targetName = GetTargetEventName(resolver, type, desc);
            var moveRouteStr = ActionEntry.GetEventCommandSentence(resolver, type, desc);
            var waitStr = ActionEntry.IsWaitForComplete
                ? EventCommandSentenceWait
                : EventCommandSentenceNonWait;

            var result = string.Format(EventCommandSentenceFormat,
                targetName, moveRouteStr, waitStr);

            var encode = Encoding.Default;

            var resultBytes = encode.GetBytes(result);

            if (resultBytes.Length <= EventCommandSentenceMaxLength) return result;

            // 文字列長が指定長を超過する場合、指定長まで切り詰めて末尾に "..." / ".." を付与する
            var chars = encode.GetChars(resultBytes.Take(EventCommandSentenceMaxLength).ToArray());
            var charList = chars.ToList();
            chars = charList.ToArray();

            var manufacturedStr = new string(chars);

            var sb = new StringBuilder();
            sb.Append(manufacturedStr);

            var appendStr = manufacturedStr.EndsWith("・")
                ? ".."
                : "...";

            sb.Append(appendStr);

            return sb.ToString();
        }

        private string GetTargetEventName(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (Target.IsVariableAddressSimpleCheck())
            {
                var targetName = resolver.GetNumericVariableAddressStringIfVariableAddress(Target, type, desc);
                return string.Format(EventCommandSentenceTargetVarAddress, targetName);
            }

            if (Target == TargetThisEvent) return EventCommandSentenceTargetThisEvent;
            if (Target == TargetHero) return EventCommandSentenceTargetHero;

            if (Target < -1)
            {
                var memberNumber = (Target + 2) * -1;
                return string.Format(EventCommandSentenceTargetMember, memberNumber);
            }

            return string.Format(EventCommandSentenceTargetMapEvent, Target);
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
            info.AddValue(nameof(actionEntry), actionEntry);
            info.AddValue(nameof(Target), Target);
            info.AddValue(nameof(HasOwner), HasOwner);
            if (HasOwner) info.AddValue(nameof(owner), owner.Id);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected MoveRoute(SerializationInfo info, StreamingContext context)
        {
            actionEntry = info.GetValue<ActionEntry>(nameof(actionEntry));
            Target = info.GetInt32(nameof(Target));
            var savedOwner = info.GetBoolean(nameof(HasOwner));
            if (savedOwner) owner = TargetAddressOwner.FromId(info.GetValue<string>(nameof(owner)));
        }
    }
}