// ========================================
// Project Name : WodiLib
// File Name    : EventCommandConstant.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドに関する定数
    /// </summary>
    public static class EventCommandConstant
    {
        /// <summary>選択肢分岐・その他</summary>
        public static class ChoiceStartForkingEtc
        {
            /// <summary>選択肢コード</summary>
            public static class ChoiceCode
            {
                /// <summary>左キー</summary>
                public static readonly byte LeftKey = 0x64;

                /// <summary>右キー</summary>
                public static readonly byte RightKey = 0x65;

                /// <summary>強制中断</summary>
                public static readonly byte ForceExit = 0x66;
            }
        }

        /// <summary>DB操作</summary>
        public static class DBManagement
        {
            /// <summary>読み書きモード</summary>
            public static class IoMode
            {
                /// <summary>Output</summary>
                public static readonly byte Read = 0x10;

                /// <summary>Input</summary>
                public static readonly byte Write = 0x00;
            }

            /// <summary>特殊処理時のデータIDと項目IDのセット</summary>
            public static class IdSet
            {
                /// <summary>タイプ番号またはタイプ名取得</summary>
                public static class GetTypeX
                {
                    /// <summary>データID</summary>
                    public static readonly int DataId = -3;

                    /// <summary>項目ID</summary>
                    public static readonly int ItemId = -3;
                }

                /// <summary>データ番号またはデータ名取得</summary>
                public static class GetDataX
                {
                    /// <summary>項目ID</summary>
                    public static readonly int ItemId = -3;
                }

                /// <summary>項目番号または項目名取得</summary>
                public static class GetItemX
                {
                    /// <summary>データID</summary>
                    public static readonly int DataId = -3;
                }

                /// <summary>データ数取得</summary>
                public static class GetDataLength
                {
                    /// <summary>データID</summary>
                    public static readonly int DataId = -1;

                    /// <summary>項目ID</summary>
                    public static readonly int ItemId = 0;
                }

                /// <summary>項目数取得</summary>
                public static class GetItemLength
                {
                    /// <summary>項目ID</summary>
                    public static readonly int ItemId = -1;
                }

                /// <summary>全データ初期化</summary>
                public static class ClearData
                {
                    /// <summary>データID</summary>
                    public static readonly int DataId = -2;

                    /// <summary>項目ID</summary>
                    public static readonly int ItemId = 0;
                }

                /// <summary>全項目初期化</summary>
                public static class ClearField
                {
                    /// <summary>項目ID</summary>
                    public static readonly int ItemId = -2;
                }
            }

            /// <summary>名前に文字列を使用するフラグ</summary>
            public static class UseStringFlg
            {
                /// <summary>タイプIDビットフラグ</summary>
                public static readonly byte TypeIdFlagBit = 0x01;

                /// <summary>データIDビットフラグ</summary>
                public static readonly byte DataIdFlagBit = 0x02;

                /// <summary>項目IDビットフラグ</summary>
                public static readonly byte ItemIdFlagBit = 0x04;
            }
        }

        /// <summary>文字列操作</summary>
        public static class SetString
        {
            /// <summary>右辺設定コード</summary>
            public static class RightSidePropertyCode
            {
                /// <summary>手動入力</summary>
                public static readonly byte Manual = 0x00;

                /// <summary>文字列変数</summary>
                public static readonly byte StringVar = 0x01;

                /// <summary>変数指定</summary>
                public static readonly byte ReferVar = 0x02;

                /// <summary>キーボード入力</summary>
                public static readonly byte KeyboardInput = 0x03;
            }
        }

        /// <summary>変数操作＋</summary>
        public static class SetVariablePlus
        {
            /// <summary>取得項目コード</summary>
            public static class ExecCode
            {
                /// <summary>キャラ</summary>
                public static readonly byte Chara = 0x10;

                /// <summary>手動入力</summary>
                public static readonly byte Position = 0x20;

                /// <summary>その他</summary>
                public static readonly byte Etc = 0x30;

                /// <summary>手動入力</summary>
                public static readonly byte Picture = 0x40;
            }
        }

        /// <summary>キー入力</summary>
        public static class KeyInput
        {
            /// <summary>処理種別</summary>
            public static class Type
            {
                /// <summary>基本</summary>
                public static readonly byte Basic = 0x00;

                /// <summary>キーボード</summary>
                public static readonly byte Keyboard = 0x01;

                /// <summary>マウス</summary>
                public static readonly byte Mouse = 0x03;

                /// <summary>パッド</summary>
                public static readonly byte Pad = 0x02;
            }
        }

        /// <summary>自動キー入力</summary>
        public static class KeyInputAuto
        {
            /// <summary>処理種別</summary>
            public static class Type
            {
                /// <summary>基本</summary>
                public static readonly byte Basic = 0x00;

                /// <summary>キーボード</summary>
                public static readonly byte Keyboard = 0x10;

                /// <summary>マウス</summary>
                public static readonly byte Mouse = 0x20;
            }
        }

        /// <summary>キー入力禁止</summary>
        public static class KeyInputControl
        {
            /// <summary>対象コード</summary>
            public static class TargetCode
            {
                /// <summary>基本入力</summary>
                public static readonly byte Basic = 0x00;

                /// <summary>デバイス入力</summary>
                public static readonly byte Device = 0x10;
            }
        }

        /// <summary>ピクチャ表示</summary>
        public static class PictureShow
        {
            /// <summary>処理内容コード</summary>
            public static class ExecCode
            {
                /// <summary>表示</summary>
                public static readonly byte Show = 0x00;

                /// <summary>移動</summary>
                public static readonly byte Move = 0x01;

                /// <summary>消去</summary>
                public static readonly byte Erase = 0x02;

                /// <summary>ディレイリセット</summary>
                public static readonly byte DelayReset = 0x03;
            }

            /// <summary>表示タイプコード</summary>
            public static class ShowTypeCode
            {
                /// <summary>ファイル読み込み（直接指定）</summary>
                public static readonly byte LoadFileDirect = 0x00;

                /// <summary>ファイル読み込み（文字列変数指定）</summary>
                public static readonly byte LoadFileVariable = 0x10;

                /// <summary>文字列ピクチャ化</summary>
                public static readonly byte String = 0x20;

                /// <summary>お手軽ウィンドウ（直接指定）</summary>
                public static readonly byte SimpleWindowDirect = 0x30;

                /// <summary>お手軽ウィンドウ（文字列変数指定）</summary>
                public static readonly byte SimpleWindowVariable = 0x40;
            }
        }

        /// <summary>エフェクト</summary>
        public static class Effect
        {
            /// <summary>処理対象</summary>
            public static class TargetCode
            {
                /// <summary>ピクチャ</summary>
                public static readonly byte Picture = 0x00;

                /// <summary>キャラ</summary>
                public static readonly byte Chara = 0x01;

                /// <summary>マップ</summary>
                public static readonly byte Map = 0x02;
            }
        }

        /// <summary>音声再生</summary>
        public static class Sound
        {
            /// <summary>処理内容</summary>
            public static class ExecCode
            {
                /// <summary>再生</summary>
                public static readonly byte Playback = 0x00;

                /// <summary>先読み</summary>
                public static readonly byte Preload = 0x01;

                /// <summary>メモリ手動解放</summary>
                public static readonly byte ReleaseManual = 0x02;

                /// <summary>メモリ全解放</summary>
                public static readonly byte ReleaseAll = 0x03;
            }
        }

        /// <summary>音声再生</summary>
        public static class SaveLoad
        {
            /// <summary>処理内容</summary>
            public static class ExecCode
            {
                /// <summary>再生</summary>
                public static readonly byte Save = 0x00;

                /// <summary>先読み</summary>
                public static readonly byte Load = 0x01;
            }
        }

        /// <summary>パーティ画像</summary>
        public static class PartyGraphic
        {
            /// <summary>処理内容</summary>
            public static class ExecCode
            {
                /// <summary>削除</summary>
                public static readonly byte Remove = 0x00;

                /// <summary>挿入</summary>
                public static readonly byte Insert = 0x01;

                /// <summary>変更</summary>
                public static readonly byte Replace = 0x02;

                /// <summary>画像指定削除</summary>
                public static readonly byte RemoveGraphic = 0x03;

                /// <summary>特殊処理</summary>
                public static readonly byte Special = 0x04;
            }
        }

        /// <summary>場所移動</summary>
        public static class Transfer
        {
            /// <summary>登録位置使用</summary>
            public static readonly int UseSavedPosition = -10001;
        }

        /// <summary>チェックポイント</summary>
        public static class CheckPoint
        {
            /// <summary>種別</summary>
            public static class Type
            {
                /// <summary>通常</summary>
                public static readonly byte Standard = 0x00;

                /// <summary>特殊</summary>
                public static readonly byte Special = 0x01;
            }
        }
    }
}