WodiLib
Ver 2.2.2
====

Description
----------

[Wolf RPG Editor](https://www.silversecond.com/WolfRPGEditor/)で生成されるファイルを外部から扱うためのC#用APIライブラリです。

Target Framework
----------

以下のフレームワークを対象としています。これ以外のフレームワークでの動作確認は行っておりませんので予めご了承ください。

- .NET Standard 2.1

Usage
----------

WodiLib.dllを使用したいプロジェクトの参照に追加してください。
使用するにあたり、ウディタの最新バージョンに適用する場合は明示的に初期化を行う必要はありません。最新バージョン以外に対応する場合、ウディタバージョンの明示的な指定が必要になります。詳細は[Wikiページ](https://github.com/kameske/WodiLib/wiki/WoditorVersion)を御覧ください。

各API詳細は <https://kameske.github.io/WodiLib/> 、または releaseページにあるZIPファイルを解凍して得られるchmファイルを参照してください。

License
----------

[MIT License](https://github.com/kameske/WodiLib/blob/master/LICENSE)

Branches
----------

- master
  - releaseしたDLLの元となったプロジェクトのみをコミットするブランチ。
  - Ver 2.X 公開以降は最もメジャーバージョンが大きなmasterブランチと同期。
- develop
  - Ver 1.2 未満のバージョンで機能追加や不具合修正を行った際に反映するブランチ。
  - Ver 1.2 および Ver 2.2 公開以降は廃止。代わりにメジャーバージョンごとのブランチを作成。
- XXX/master (XXX = Version)
  - Ver XXX 専用のmasterブランチ。メジャーバージョンごとに存在。
  - Ver 1.2 公開以降で使用される。
- XXX/develop (XXX = Version)
  - Ver XXX 専用のdevelopブランチ。メジャーバージョンごとに存在。
  - Ver 1.2 公開以降で使用される。
- features (XXX/features)
  - 機能追加時、developブランチに反映する前に作成されることがあるブランチ。
  - master ブランチに取り込まれたあとは削除される。

Loadmap
----------

- 2020/06
  - Ver 1.3.0, 2.3.0
    - IFixedLengthXXXインタフェースで公開しているリスト型プロパティを実装クラス型プロパティに変更。
    - 一部クラスのEditorBrowsable属性見直し。
    - 予告済みのクラス・プロパティ廃止。
    - （Ver2のみ）外部モジュール化したクラスの再統合。

- 適宜
  - 不具合修正 / 機能追加

History
----------

- 2020/06/07 Ver 2.2.2
  - RestrictedCapacityCollection.Overwrite() で発生する　CollectionChanged イベントの引数を修正(#10)

- 2020/04/25 Ver 2.2.1
  - バイナリデータ出力時、特定のコマンドが正しく出力されない不具合修正(#8)

- 2020/04/12 Ver 2.2.0
  - C# 8.0 対応
  - INotifyPropertyCahgend, INotifyCollectionChanged 対応
  - メソッドやプロパティで使用するListなどの見直し

- 2019/12/01 Ver 1.1.0
  - シリアライズ/デシリアライズ対応
  - イベントコマンド拡張

- 2019/10/27 Ver 1.0.0
  - 正式版初版公開

Author
----------

カメスケ（HP：[彼岸の亀の停留所](http://kameske027.php.xdomain.jp/)）
