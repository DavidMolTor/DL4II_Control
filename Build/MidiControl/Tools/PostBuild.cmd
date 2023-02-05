rem ----------------------------------------------------------------------------
rem 
rem Project: MIDI CONTROL
rem
rem Usage: Postbuild [ProjectRoot] [OutputDir] [Configuration] [Platform]
rem
rem The first parameter indicates the project configuration
rem The first parameter indicates the project platform
rem The second parameter indicates the project root directory
rem The third parameter indicates the output directory
rem
rem ----------------------------------------------------------------------------

rem ----------------------------------------------------------------------------
rem ---- DIRECTORIES
rem ----------------------------------------------------------------------------

set PROJECT_ROOT=%1
set OUT_FOLDER=%2

rem ----------------------------------------------------------------------------
rem ---- ARGUMENTS CHECKING
rem ----------------------------------------------------------------------------

if %3==Debug   	goto DEBUG
if %3==Release 	goto RELEASE

echo Error during PostBuild step: Invalid configuration - %3
goto END

:RELEASE

rem ----------------------------------------------------------------------------
rem ---- REMOVE FILES
rem ----------------------------------------------------------------------------

del "%OUT_FOLDER%*.pdb" /S /Q >NUL
del "%OUT_FOLDER%*.metagen" /S /Q >NUL

rem ----------------------------------------------------------------------------
rem ---- MOVE LIBRARIES AND CONFIGURATIONS
rem ----------------------------------------------------------------------------

mkdir "%OUT_FOLDER%dll"

move "%OUT_FOLDER%*.dll" 		"%OUT_FOLDER%\dll" >NUL
move "%OUT_FOLDER%*.xml" 		"%OUT_FOLDER%\dll" >NUL
move "%OUT_FOLDER%*.dylib" 	"%OUT_FOLDER%\dll" >NUL

rem ----------------------------------------------------------------------------
rem ---- COPY GENERATED FILES TO TARGET DIRECTORY
rem ----------------------------------------------------------------------------

rmdir "%PROJECT_ROOT%..\..\Install\DL4MkII Control" /S /Q >NUL
mkdir "%PROJECT_ROOT%..\..\Install\DL4MkII Control"
xcopy "%OUT_FOLDER%" "%PROJECT_ROOT%..\..\Install\DL4MkII Control\" /S /E /Y >NUL

goto END

:DEBUG

goto END

rem ----------------------------------------------------------------------------
rem ---- PROCESS END
rem ----------------------------------------------------------------------------

:END
echo Postbuild done!
exit /b 0
