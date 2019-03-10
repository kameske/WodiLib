WodiLib
Ver 0.2.0
====

概要
----------

[Wolf RPG Editor](https://www.silversecond.com/WolfRPGEditor/)で生成されるファイルを外部から扱うためのC#用APIライブラリです。

ターゲットフレームワーク
----------

以下のフレームワークを対象としています。これ以外のフレームワークでの動作確認は行っていないのでご了承ください。

- .NET Framework 4.6.1
- .NET Standard 2.0
- .NET Core 2.1

使用方法
----------

WodiLib.dllを使用したいプロジェクトの参照に追加してください。
使用するにあたり、明示的に初期化を行う必要はありません。

各API詳細は <https://kameske.github.io/WodiLib/> 、または gh-pages ブランチにある Help ディレクトリ配下の WodiLibAPI.chm または index.html を参照してください。

ライセンス
----------

MIT License Copyright (c) 2019 kameske

更新予定
----------

- Ver 0.3 2019/04以降
  - データベース関連ファイル対応

更新履歴
----------

- 2019/03/10 Ver 0.2.0
  - 機能追加
    - コモンイベントデータファイル、コモンファイルに対応。
    - ログ出力機能を追加。
    - ウディタバージョン差分に対応。
      - ファイル出力時、設定されたバージョンで対応していないイベントコマンドが使用されている場合に警告ログを出力する機能を追加。

  - 改修/変更
    - DBKindクラスをWodiLib.Databaseクラス名前空間に移動。
    - IWodiLibObjectインタフェース廃止。
    - NumberPlusPictureInfoType に「拡大率」を追加。
      - Ver2.00より前で使用されていた設定。

  - 修正
    - StringExtension.IsEmpty が正しく動作しなかったバグ。

  - その他
    - ソースコードのクリーンアップを実施。

作者
----------

[カメスケ](http://kameske027.php.xdomain.jp/)