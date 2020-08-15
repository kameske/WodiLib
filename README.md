WodiLib
Ver 2.3.0
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

- Ver 2.4.0 2020年内を目処（2020年内に更新不可能な可能性あり）
    - （主に ```WodiLib.Database``` 名前空間内の各種クラス）機能の見直し

- 適宜
  - 不具合修正 / 機能追加

History
----------

- 2020/06/28 Ver 2.3.0, Ver 1.3.0
  - IFixedLengthXXXインタフェースで公開しているリスト型プロパティを実装クラス型プロパティに変更。
  - 一部クラスのEditorBrowsable属性見直し。
  - 予告済みのクラス・プロパティ廃止。
  - （Ver2のみ）外部モジュール化したクラスの再統合。

- 2019/10/27 Ver 1.0.0
  - 正式版初版公開

Author
----------

カメスケ（HP：[彼岸の亀の停留所](http://kameske027.php.xdomain.jp/)）
