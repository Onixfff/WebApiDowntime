@echo off
echo Checking administrator privileges...

REM Проверка прав администратора
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Error: This script requires administrator privileges!
    echo Please run this script as administrator.
    pause
    exit /b 1
)

echo Installing WebApiDowntime Service...

REM Поиск исполняемого файла
set FOUND=0
for %%v in (8.0 7.0 6.0) do (
    if exist "%~dp0..\bin\Release\net%%v\win-x64\WebApiDowntime.exe" (
        set SERVICE_PATH=%~dp0..\bin\Release\net%%v\win-x64\WebApiDowntime.exe
        set FOUND=1
        goto :found
    )
)

:found
if %FOUND% equ 0 (
    echo Error: Service executable not found!
    echo Please make sure that:
    echo 1. The application is built in Release mode
    echo 2. The executable is in bin\Release\net8.0\win-x64\WebApiDowntime.exe
    echo.
    echo Current directory: %~dp0
    echo Expected path: %~dp0..\bin\Release\net8.0\win-x64\WebApiDowntime.exe
    pause
    exit /b 1
)

echo Found service executable at: %SERVICE_PATH%

REM Проверяем, существует ли уже служба
sc query "WebApiDowntime" >nul 2>&1
if %errorLevel% equ 0 (
    echo Service already exists. Updating configuration...
    sc stop "WebApiDowntime"
    sc delete "WebApiDowntime"
)

REM Устанавливаем службу
sc create "WebApiDowntime" binPath= "%SERVICE_PATH%" start= auto
if %errorLevel% neq 0 (
    echo Error: Failed to create service
    pause
    exit /b 1
)

sc description "WebApiDowntime" "WebApiDowntime Service for monitoring system downtime"
sc config "WebApiDowntime" obj= LocalSystem

REM Запускаем службу
sc start "WebApiDowntime"
if %errorLevel% neq 0 (
    echo Warning: Service created but failed to start
    echo Please check the service status in Services (services.msc)
)

echo Service installed successfully!
echo The service will start automatically on system startup.
pause 