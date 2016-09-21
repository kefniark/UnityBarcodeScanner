#! /bin/sh

echo "Run Unity Tests"
/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -runEditorTests -quit -projectPath $(pwd)

echo 'Logs from build'
cat $(pwd)/unity.log