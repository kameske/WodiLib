// ========================================
// Project Name : WodiLib
// File Name    : EventCommandShortCutKeyList.cs
//
// MIT License Copyright(c) 23019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// イベントコマンドショートカットキーリスト
    /// </summary>
    [Serializable]
    public class EventCommandShortCutKeyList : RestrictedCapacityCollection<EventCommandShortCutKey>,
        IFixedLengthEventCommandShortCutKeyList, IEquatable<EventCommandShortCutKeyList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => 30;

        /// <summary>最小容量</summary>
        public static int MinCapacity => 30;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static class ValidationErrorMessage
        {
            public static string Duplicate(EventCommandShortCutKey command) =>
                $"同じキーが複数登録されています。({command})";

            public static string Invalid() =>
                "現在使用されていないコマンド（インデックス19~29）のショートカットキーには" +
                $"${nameof(EventCommandShortCutKey)}.{nameof(EventCommandShortCutKey.None)}が" +
                "設定されている必要があります。";
        }

        /// <summary>使用中のインデックス最大値</summary>
        private const int UseMaxIndex = 18;

        // 各項目リストインデックス

        private const int IndexMessage = 0;
        private const int IndexChoice = 1;
        private const int IndexSetVariable = 2;
        private const int IndexDbManagement = 3;
        private const int IndexSetString = 4;
        private const int IndexSetVariablePlus = 5;
        private const int IndexConditionNumber = 6;
        private const int IndexConditionString = 7;
        private const int IndexKeyInput = 8;
        private const int IndexPicture = 9;
        private const int IndexEffect = 10;
        private const int IndexSound = 11;
        private const int IndexSaveAndLoad = 12;
        private const int IndexPartyGraphic = 13;
        private const int IndexMapChip = 14;
        private const int IndexTransfer = 15;
        private const int IndexEventControl = 16;
        private const int IndexCommonEvent = 17;
        private const int IndexDownload = 18;

        private int[] NotUseIndexes { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// [NotNull] 「文章の表示」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey Message
        {
            get => this[IndexMessage];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Message)));
                this[IndexMessage] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「選択肢」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey Choice
        {
            get => this[IndexChoice];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Choice)));
                this[IndexChoice] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「変数操作」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey SetVariable
        {
            get => this[IndexSetVariable];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SetVariable)));
                this[IndexSetVariable] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「DB操作」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey DBManagement
        {
            get => this[IndexDbManagement];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBManagement)));
                this[IndexDbManagement] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「文字列操作」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey SetString
        {
            get => this[IndexSetString];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SetString)));
                this[IndexSetString] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「変数操作＋」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey SetVariablePlus
        {
            get => this[IndexSetVariablePlus];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SetVariablePlus)));
                this[IndexSetVariablePlus] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「条件（変数）」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey ConditionNumber
        {
            get => this[IndexConditionNumber];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ConditionNumber)));
                this[IndexConditionNumber] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「条件（文字列）」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey ConditionString
        {
            get => this[IndexConditionString];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ConditionString)));
                this[IndexConditionString] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「キー入力」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey KeyInput
        {
            get => this[IndexKeyInput];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(KeyInput)));
                this[IndexKeyInput] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「ピクチャ」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey Picture
        {
            get => this[IndexPicture];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Picture)));
                this[IndexPicture] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「エフェクト」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey Effect
        {
            get => this[IndexEffect];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Effect)));
                this[IndexEffect] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「サウンド」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey Sound
        {
            get => this[IndexSound];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Sound)));
                this[IndexSound] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「セーブ・ロード操作」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey SaveAndLoad
        {
            get => this[IndexSaveAndLoad];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SaveAndLoad)));
                this[IndexSaveAndLoad] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「パーティ画像」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey PartyGraphic
        {
            get => this[IndexPartyGraphic];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(PartyGraphic)));
                this[IndexPartyGraphic] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「チップ処理」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey MapChip
        {
            get => this[IndexMapChip];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MapChip)));
                this[IndexMapChip] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「場所移動」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey Transfer
        {
            get => this[IndexTransfer];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Transfer)));
                this[IndexTransfer] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「イベント制御」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey EventControl
        {
            get => this[IndexEventControl];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventControl)));
                this[IndexEventControl] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「コモンイベント」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey CommonEvent
        {
            get => this[IndexCommonEvent];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CommonEvent)));
                this[IndexCommonEvent] = value;
            }
        }

        /// <summary>
        /// [NotNull] 「ダウンロード処理」コマンドのショートカットキー
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandShortCutKey Download
        {
            get => this[IndexDownload];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Download)));
                this[IndexDownload] = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventCommandShortCutKeyList() : this(new[]
        {
            EventCommandShortCutKey.One,
            EventCommandShortCutKey.Two,
            EventCommandShortCutKey.Three,
            EventCommandShortCutKey.Four,
            EventCommandShortCutKey.Five,
            EventCommandShortCutKey.Six,
            EventCommandShortCutKey.Seven,
            EventCommandShortCutKey.Eight,
            EventCommandShortCutKey.Nine,
            EventCommandShortCutKey.A,
            EventCommandShortCutKey.B,
            EventCommandShortCutKey.C,
            EventCommandShortCutKey.D,
            EventCommandShortCutKey.E,
            EventCommandShortCutKey.F,
            EventCommandShortCutKey.G,
            EventCommandShortCutKey.H,
            EventCommandShortCutKey.I,
            EventCommandShortCutKey.J,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
            EventCommandShortCutKey.None,
        })
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">初期DB項目設定リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public EventCommandShortCutKeyList(IReadOnlyCollection<EventCommandShortCutKey> list) : base(list)
        {
            var notUseIndexes = new List<int>();
            for (var i = UseMaxIndex + 1; i < MaxCapacity; i++)
            {
                notUseIndexes.Add(i);
            }

            NotUseIndexes = notUseIndexes.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// リストの各項目が適切な設定であることを検証する。
        /// </summary>
        /// <remarks>
        /// 以下の点をチェックする。
        /// <pre>「文章の表示」～「ダウンロード処理」でキーが被っていないか</pre>
        /// <pre>使用していない項目に未使用時の値が設定されているか</pre>
        /// </remarks>
        /// <param name="errorMsg">
        ///     返戻エラーメッセージ。
        ///     設定値が適切である場合、null。
        /// </param>
        /// <returns>設定値が適切である場合、true</returns>
        public bool Validate(out string errorMsg)
        {
            if (!ValidateDuplicateKey(out errorMsg)) return false;

            if (!ValidateNotUseKeys())
            {
                errorMsg = ValidationErrorMessage.Invalid();
                return false;
            }

            errorMsg = null;
            return true;
        }

        /// <summary>
        /// 使用しているキーが重複していないことを検証する。
        /// </summary>
        /// <returns>キー重複が存在しない場合、true</returns>
        public bool ValidateDuplicateKey(out string errorMsg)
        {
            var cloneList = Items.Where(x => !x.Equals(EventCommandShortCutKey.None)).ToList();
            cloneList.Sort((left, right) =>
                String.Compare(left.Code, right.Code, StringComparison.CurrentCultureIgnoreCase));

            for (var i = 1; i < cloneList.Count; i++)
            {
                if (cloneList[i - 1].Equals(cloneList[i]))
                {
                    errorMsg = ValidationErrorMessage.Duplicate(cloneList[i]);
                    return false;
                }
            }

            errorMsg = null;
            return true;
        }

        /// <summary>
        /// 使用していないキー設定の設定値が適切であることを検証する。
        /// <remarks>
        /// 使用していない設定にはすべてデフォルト値が設定されている必要がある。
        /// </remarks>
        /// </summary>
        /// <returns>キー設定が適切な場合、true</returns>
        public bool ValidateNotUseKeys()
        {
            return NotUseIndexes.All(idx => this[idx] == EventCommandShortCutKey.None);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(EventCommandShortCutKeyList other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return Equals((RestrictedCapacityCollection<EventCommandShortCutKey>) other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override EventCommandShortCutKey MakeDefaultItem(int index) => EventCommandShortCutKey.One;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(makeSerializationItemsKeyName(-1), Items.Count);
            for (var i = 0; i < Items.Count; i++)
            {
                info.AddValue(makeSerializationItemsKeyName(i), Items[i].Code);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected EventCommandShortCutKeyList(SerializationInfo info, StreamingContext context) : base(ReadList(info))
        {
        }

        /// <summary>
        /// SerializationInfoから要素情報を取り出す。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <returns>取り出したリスト</returns>
        private static IReadOnlyList<EventCommandShortCutKey> ReadList(SerializationInfo info)
        {
            var result = new List<EventCommandShortCutKey>();

            var count = info.GetInt32(makeSerializationItemsKeyName(-1));
            for (var i = 0; i < count; i++)
            {
                result.Add(
                    EventCommandShortCutKey.FromCode(info.GetValue<string>(makeSerializationItemsKeyName(i))));
            }

            return result;
        }

        /// <summary>
        /// シリアライズ時の要素のキー名を生成する。
        /// </summary>
        /// <param name="index">要素インデックス</param>
        /// <returns>キー名</returns>
        private static string makeSerializationItemsKeyName(int index) => $"key_{index}";
    }
}