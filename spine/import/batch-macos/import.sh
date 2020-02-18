#!/bin/bash

#---------------------------------------------------------------------------------------------------
# Usage
#---------------------------------------------------------------------------------------------------
function usage() {
cat <<_EOS_
     Usage
    -----------------
    $ ./import.sh [-a file] [-i dir] [-e str] [-o dir] [-s num]

     Description
    -----------------
    スケルトンデータファイル (json/spine) から .spine ファイルとして出力します.
    出力するファイル名とスケルトン名はインポートするファイル名と同一です.

        input/
            abcde.json
            12345.spine
        
        output/
            abcde.spine { skeleton name: abcde }
            12345.spine { skeleton name: 12345 }

     Options
    -----------------
    -a    Spineコマンドツール. 基本的にSpine.appの中にあります. '/Applications/Spine/Spine.app/Contents/MacOS/Spine'
    -i    インポートするファイルが含まれているディレクトリ. [-e]で指定した拡張子のファイルを対象とします.
    -e    インポート対象とするファイルの拡張子. (.json / .spine / .skel)
    -o    .spineが出力されるディレクトリ.
    -s    インポート設定 スケール値.
    -h    Help!

     Reference
    -----------------
    http://ja.esotericsoftware.com/spine-export

_EOS_
    exit 1
}


# 強制終了検出
function force_finished () {
    status=$?
    echo '強制終了しました'
    echo "ステータス: $status"
    exit $status
}

trap 'force_finished'  {1,2,3,15}

# エラー終了
function error_exit () {
    echo 'エラーのため終了しました'
    exit 1
}


#---------------------------------------------------------------------------------------------------
# Validation option and argment
#---------------------------------------------------------------------------------------------------
if [ "$OPTIND" = 1 ]; then
  while getopts a:i:e:o:s:h OPT
  do
    case $OPT in
      a) OPT_A_ENABLED=1 ; SPINE_EXE=$OPTARG ;;
      i) OPT_I_ENABLED=1 ; INPUT_DIR=$OPTARG ;;
      e) OPT_E_ENABLED=1 ; INPUT_FILE_EXTENSION=$OPTARG ;;
      o) OPT_O_ENABLED=1 ; OUTPUT_DIR=$OPTARG ;;
      s) OPT_S_ENABLED=1 ; SCALE=$OPTARG ;;
      h) usage ;;
      \?)
        echo "Try to enter the h option." 1>&2
        ;;
    esac
  done
else
  echo "No installed getopts-command." 1>&2
  exit 1
fi



#---------------------------------------------------------------------------------------------------
echo -e "\n//===== Initialization"
#---------------------------------------------------------------------------------------------------

# script のフォルダへ移動しておきます
readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)
cd $SCRIPT_DIR

# input 対象ファイルリスト
readonly TARGET_LIST=$SCRIPT_DIR'/import-file-list.log'

echo "# Show settings"
echo '- SCRIPT_DIR           ' $SCRIPT_DIR
echo '- TARGET_LIST          ' $TARGET_LIST
echo '- SPINE_EXE            ' $SPINE_EXE
echo '- INPUT_DIR            ' $INPUT_DIR
echo '- INPUT_FILE_EXTENSION ' $INPUT_FILE_EXTENSION
echo '- OUTPUT_DIR           ' $OUTPUT_DIR
echo '- SCALE                ' $SCALE


# 文字列長が0ならエラー
if [ -z "$SPINE_EXE" ];            then error_exit; fi
if [ -z "$INPUT_DIR" ];            then error_exit; fi
if [ -z "$INPUT_FILE_EXTENSION" ]; then error_exit; fi
if [ -z "$OUTPUT_DIR" ];           then error_exit; fi
if [ -z "$SCALE" ];                then error_exit; fi


#---------------------------------------------------------------------------------------------------
echo -e "\n//===== Setup"
#---------------------------------------------------------------------------------------------------
echo "# Remake output directry."

if [ -d $OUTPUT_DIR ]; then
    echo "# Remove all contents."
    rm -r $OUTPUT_DIR/*
else
    echo "# Make new."
    mkdir $OUTPUT_DIR
fi


#---------------------------------------------------------------------------------------------------
echo -e "\n//===== Import"
#---------------------------------------------------------------------------------------------------
echo "# Make input file list."
echo "find $INPUT_DIR -name *$INPUT_FILE_EXTENSION -maxdepth 3 > $TARGET_LIST"

find $INPUT_DIR -name *$INPUT_FILE_EXTENSION -maxdepth 3 | sort > $TARGET_LIST

echo ""
echo "# Import with list."

index=0
total_count=`cat $TARGET_LIST | wc -l`

cat $TARGET_LIST | while read _input_file;
do
    # xxx/yyy/zzz.spine => zzz
    _filename=`basename $_input_file $INPUT_FILE_EXTENSION`

    # setup parameter
    _spine=$SPINE_EXE
    _output_file=$OUTPUT_DIR/$_filename.spine

    echo -e ""
    echo -e "//-------------------------------------------------"
    echo -e "  Import ["$index" /"$total_count"] ~ "$_filename
    echo -e "//-------------------------------------------------"

    # execute command
    echo "$_spine --input $_input_file --output $_output_file --scale $SCALE --import $_filename"
    $_spine --input $_input_file --output $_output_file --scale $SCALE --import $_filename

    index=$(( index + 1 ))
done


#---------------------------------------------------------------------------------------------------
echo -e "\n//===== "
#---------------------------------------------------------------------------------------------------
