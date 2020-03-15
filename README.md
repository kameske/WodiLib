WodiLib
Ver 1.1.0
====

Description
----------

[Wolf RPG Editor](https://www.silversecond.com/WolfRPGEditor/)で生成されるファイルを外部から扱うためのC#用APIライブラリです。

Target Framework
----------

以下のフレームワークを対象としています。これ以外のフレームワークでの動作確認は行っておりませんので予めご了承ください。

- .NET Framework 4.6.1
- .NET Standard 2.0
- .NET Core 2.1

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

- Ver 1.2 ： 2020/04 ～ 2020/06
  - INotifyPropertyChanged, INotifyCollectionChanged 対応と XXXItemEventHandler クラス廃止
  - ファイルIO関連クラスの見直し
    - Reader および Writer クラス公開

- Ver 2.2 ： Ver1.2と同時
  - 言語バージョンと対象フレームワークの変更
    - 言語バージョン： C# 8.0
    - 対象フレームワーク： .NET Core 3.1
  - 機能面は Ver 1.2 と同等
  - ※Ver 2.2 公開以降のブランチについては "Branches" を参照。

- 適宜
  - 不具合修正 / 機能追加

History
----------

- 2020/03/15 Ver 1.1.0.7
  - 不具合修正

- 2019/12/01 Ver 1.1.0
  - シリアライズ/デシリアライズ対応
  - イベントコマンド拡張

- 2019/10/27 Ver 1.0.0
  - 正式版初版公開

Author
----------

カメスケ（HP：[彼岸の亀の停留所](http://kameske027.php.xdomain.jp/)）