#!/bin/bash

#---------------------------------------------------------------------------------------------------
# Usage
#---------------------------------------------------------------------------------------------------
function usage() {
cat <<_EOS_
    # Usage
    $0 [-h]

    # Description
    spine export processing.

    # Options
    -h    display help

    # Reference:
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
  while getopts abf:h OPT
  do
    case $OPT in
      h)
        usage
        ;;
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

# Read Config
readonly CONFIG_FILE=$SCRIPT_DIR'/config.txt'
test -e $CONFIG_FILE
if [ $? = 1 ]; then "configファイルが存在しません >> $CONFIG_FILE"; exit; fi

# config file の CONF_KEY に設定された値を読み込みます
readonly SPINE_CONF_KEY="<F:spine>"
readonly INPORT_CONF_KEY="<D:inport>"
readonly OUTPUT_CONF_KEY="<D:output>"
readonly EXPORT_CONF_KEY="<F:export.json>"

readonly SPINE=`grep "$SPINE_CONF_KEY" "$CONFIG_FILE" | sed -e "s/$SPINE_CONF_KEY//"`
readonly INPORT_DIR=`grep "$INPORT_CONF_KEY" "$CONFIG_FILE" | sed -e "s/$INPORT_CONF_KEY//"`
readonly OUTPUT_DIR=`grep "$OUTPUT_CONF_KEY" "$CONFIG_FILE" | sed -e "s/$OUTPUT_CONF_KEY//"`
readonly EXPORT_JSON=`grep "$EXPORT_CONF_KEY" "$CONFIG_FILE" | sed -e "s/$EXPORT_CONF_KEY//"`

# 対象のspineファイルリスト
readonly TARGET_LIST=$SCRIPT_DIR'/list.txt'

echo "# Show settings"
echo '- SCRIPT_DIR     ' $SCRIPT_DIR
echo '- CONFIG_FILE    ' $CONFIG_FILE
echo '- TARGET_LIST    ' $TARGET_LIST
echo '- '$SPINE_CONF_KEY '     ' $SPINE
echo '- '$EXPORT_CONF_KEY $EXPORT_JSON
echo '- '$INPORT_CONF_KEY '    '  $INPORT_DIR
echo '- '$OUTPUT_CONF_KEY '    '  $OUTPUT_DIR

# 文字列長が0ならエラー
if [ -z "$SPINE" ];       then error_exit; fi
if [ -z "$EXPORT_JSON" ]; then error_exit; fi
if [ -z "$INPORT_DIR" ];  then error_exit; fi
if [ -z "$OUTPUT_DIR" ];  then error_exit; fi


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
echo -e "\n//===== Export"
#---------------------------------------------------------------------------------------------------
echo "# Make project file list."

find $INPORT_DIR -name *.spine -maxdepth 3 > $TARGET_LIST


echo "# Export with list."

index=0

cat $TARGET_LIST | while read _path;
do
    # xxx/uuu/zzz.spine => zzz.spine
    _filename=`basename $_path`

    # xxx/yyy/zzz.spine => zzz
    _dir_name=`basename $_path .spine`

    # setup parameter
    _spine=$SPINE
    _input_file=$_path
    _output_dir=$OUTPUT_DIR/$_dir_name
    _export_json_file=$EXPORT_JSON

    echo -e ""
    echo -e "//-------------------------------------------------"
    echo -e "  EXPORT [$index] ~ $_filename"
    echo -e "//-------------------------------------------------"

    # create output directory
    mkdir $_output_dir

    # export command
    $_spine -i $_input_file -o $_output_dir -e $_export_json_file

    echo -e "export ==> $_export_json_file"
    echo -e "input  ==> $_path"
    echo -e "output ==> $_output_dir"


    # rename binary file for unity ( xxx.skel => xxx.skel.bytes )
    _original=`find $_output_dir -name *.skel`

    if [ -n "$_original" ]; then

        _copy=`echo $_original | sed -e 's/.skel/.skel.bytes/'` 

        if [ -z "$_copy" ]; then
            _copy=$_original
        fi

        mv -f $_original $_copy

        echo -e "rename ==> $_original ...> $_copy"
    fi

    # rename atlas file for unity ( xxx.atlas => xxx.atlas.txt )
    _original=`find $_output_dir -name *.skel`

    if [ -n "$_original" ]; then

        _copy=`echo $_original | sed -e 's/.atlas/.atlas.txt/'` 

        if [ -z "$_copy" ]; then
            _copy=$_original
        fi

        mv -f $_original $_copy

        echo -e "rename ==> $_original ...> $_copy"
    fi



    index=$(( index + 1 ))
done


#---------------------------------------------------------------------------------------------------
echo -e "\n//===== Done!"
#---------------------------------------------------------------------------------------------------
