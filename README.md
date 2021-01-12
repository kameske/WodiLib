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
        - 各種 ValueObject を record 化。（struct 廃止）
    - Target Framework を ```.NET Standard 2.1``` から ```.NET Standard 2.0``` に変更
        - ```Ver 2.X``` -> ```Ver 3.X``` 移行の場合のみ影響。```Ver 1.X``` は現状 ```.NET Standard 2.0``` を対象としているため。
        - これにより ```Ver 3.0``` での Target Framework は ```.NET Framework 4.6.1``` および ```.NET Standard 2.0``` となる。
    - ```WodiLib.Database``` 名前空間の各種クラス見直し。
        - ```Ver 1.X```, ```2.X``` から破壊的変更あり。
    - Jsonシリアライズ/デシリアライズ対応。
    - BinaryFormatter 対応終了。
        - 具体的には
            - ISerializable インタフェース実装を解除
            - SerializableAttribute 付与を取りやめ
        - ```.NET 5``` にて ```BinaryFormatter``` が非推奨となったため。
    - インタフェース整理。
    - その他破壊的変更にならない範囲での機能改修。

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
