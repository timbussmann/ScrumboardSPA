@echo off
set resultDir=..\results
if not exist %resultDir% mkdir %resultDir%

..\tools\NAnt\NAnt -buildfile:nant.build -l:%resultDir%\TDDJavascript-Nant-jscript-tests.log integration-build 
IF ERRORLEVEL 1 GOTO Failed
..\tools\NAnt\NAnt -buildfile:nant.build -l:%resultDir%\TDDJavascript-Nant-jscript-tests.log unit-tests 
IF ERRORLEVEL 1 GOTO Failed
..\tools\NAnt\NAnt -buildfile:nant.build -l:%resultDir%\TDDJavascript-Nant-jscript-tests.log javascript-unit-tests 
IF ERRORLEVEL 1 GOTO Failed
..\tools\NAnt\NAnt -buildfile:nant.build -l:%resultDir%\TDDJavascript-Nant-jscript-tests.log javascript-code-coverage 
IF ERRORLEVEL 1 GOTO Failed

echo "compilation and unit testing completed. Log file and unit-tests results are stored in %resultDir%"
GOTO End


:Failed
echo "Failed"

:End
pause
