WodiLib
Ver 0.4.0
====

概要
----------

[Wolf RPG Editor](https://www.silversecond.com/WolfRPGEditor/)で生成されるファイルを外部から扱うためのC#用APIライブラリです。

ターゲットフレームワーク
----------

以下のフレームワークを対象としています。これ以外のフレームワークでの動作確認は行っておりませんので予めご了承ください。

- .NET Framework 4.6.1
- .NET Standard 2.0
- .NET Core 2.1

使用方法
----------

WodiLib.dllを使用したいプロジェクトの参照に追加してください。
使用するにあたり、ウディタの最新バージョンに適用する場合は明示的に初期化を行う必要はありません。最新バージョン以外に対応する場合、ウディタバージョンの明示的な指定が必要になります。詳細は[Wikiページ](https://github.com/kameske/WodiLib/wiki/WoditorVersion)を御覧ください。

各API詳細は <https://kameske.github.io/WodiLib/> 、または releaseページにあるZIPファイルを解凍して得られるchmファイルを参照してください。

ライセンス
----------

MIT License Copyright (c) 2019 kameske

更新予定
----------

- Ver 0.5 2019/07 予定
  - Iniファイル対応

- Ver 0.6 未定
  - マップファイル処理見直し

更新履歴
----------

- 2019/06/30 Ver 0.4.0
  - タイルセットデータに関する処理実装

作者
----------

カメスケ（HP：[彼岸の亀の停留所](http://kameske027.php.xdomain.jp/)）