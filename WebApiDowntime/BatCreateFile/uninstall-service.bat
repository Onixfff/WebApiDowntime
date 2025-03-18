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

echo Uninstalling WebApiDowntime Service...

REM Проверяем, существует ли служба
sc query "WebApiDowntime" >nul 2>&1
if %errorLevel% neq 0 (
    echo Service is not installed.
    pause
    exit /b 0
)

REM Останавливаем службу
echo Stopping service...
sc stop "WebApiDowntime"
if %errorLevel% neq 0 (
    echo Warning: Failed to stop service
    echo Attempting to delete service anyway...
)

REM Удаляем службу
echo Removing service...
sc delete "WebApiDowntime"
if %errorLevel% neq 0 (
    echo Error: Failed to delete service
    echo Please try again or remove the service manually through Services (services.msc)
    pause
    exit /b 1
)

echo Service uninstalled successfully!
pause 