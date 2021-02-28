WodiLib
Ver 2.3.1
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
- develop
  - 機能追加や不具合修正を行った際に反映するブランチ。
- features
  - 機能追加時、developブランチに反映する前に作成されることがあるブランチ。
  - master ブランチに取り込まれたあとは削除される。

Loadmap
----------

- Ver 3.0.0 時期未定
    - ```Ver 1.X```, ```2.X``` 更新終了。
    - 言語バージョン C# 9.0
        - 詳細は [v2.x/feature/Ver3.0変更点.md](https://github.com/kameske/WodiLib/blob/v2.x/feature/Ver3.0%E5%A4%89%E6%9B%B4%E7%82%B9.md) 参照。

- 適宜
  - 不具合修正 / 機能追加

History
----------

- 2021/02/28 Ver 2.3.1, Ver 1.3.1
  - ウディタ Ver 2.255 で追加されたコマンド対応。

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
