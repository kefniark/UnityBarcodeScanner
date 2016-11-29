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

RESULT=$?
if [ "$RESULT" -ne 0 ]; then
  echo "Build Failed"
  cat $(pwd)/unity.log
  cat $(pwd)/EditorTestResults.xml
fi

exit $RESULT