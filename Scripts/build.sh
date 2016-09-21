#! /bin/sh

echo "Run Unity Tests"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -runEditorTests \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -quit

exit $?