WodiLib
Ver 0.5.2
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

- Ver 1.0 2018年10月
  - 正式版公開
  - プロジェクト単位で管理する機能の追加

更新履歴
----------

- 2019/10/06 Ver 0.5.2
  - Extension クラスのインスタンスが null のとき、 string にキャストしようとすると例外が発生する不具合修正。
  - ThisMapEventInfoAddress クラスの設定を修正。
  - 一部のメソッド名を修正。
  - 例外メッセージ修正。
- 2019/08/10 Ver 0.5.1
  - 変数アドレス「このマップイベント情報アドレス」への対応が漏れていたため、対応するクラスを新規作成。
  - 文字列を扱う値オブジェクトについて、null値であるオブジェクトからstringにキャストするとエラーが発生する不具合修正。

作者
----------

カメスケ（HP：[彼岸の亀の停留所](http://kameske027.php.xdomain.jp/)）