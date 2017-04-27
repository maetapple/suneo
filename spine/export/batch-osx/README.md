# ファイルセット
- `export.sh`   バッチファイル
- `config.txt`  各種設定ファイル
- `list.txt`    処理対象の.spineリスト (実行時に生成されます)


# 準備
- `config.txt`に必要事項を入力してください
- Spineは終了しておいてください **(バッチ処理中は使用しないでください)**


# 実行手順
- `ターミナル`を起動してください
- `export.sh`をドラッグ&ドロップします
- `xxx:xxx$ /aaa/bbb/ccc/..../export.sh` みたいに表示されたら`Enter`で実行されます
- 設定などに問題がなければ、順に出力されていきます
- 内容によりますが1ファイルにつき10秒程度かかります


# 途中で停止する場合
- `control`+`C` で強制終了できます
- 反応しにくいタイミングがあるので、終了するまでゆっくり確実に何度か入力してください
- `強制終了しました` というメッセージが表示されます


# Output 形式
- inport対象の.spineファイル名を、outputするディレクトリ名とします.
- アニメーション、Atlas、テクスチャはskeleton名で出力されます.
- 出力形式がBinary(.skel)の場合のみ、.skel ==> .skel.bytes にリネームされます

```
inport/
    001/
        AAA.spine
    002/
        BBB.spine

output/
    AAA/
        AAA-skeleton-name.json
        AAA-skeleton-name.atlas
        AAA-skeleton-name.png
    BBB/
        BBB-skeleton-name.json
        BBB-skeleton-name.atlas
        BBB-skeleton-name.png
```


