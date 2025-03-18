# Проверяем права администратора
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Warning "Требуются права администратора. Запустите скрипт от имени администратора."
    Break
}

# Создаем источник событий в Windows Event Log
if (-NOT [System.Diagnostics.EventLog]::SourceExists("WebApiDowntime")) {
    New-EventLog -LogName Application -Source "WebApiDowntime"
}

# Путь к исполняемому файлу
$servicePath = Join-Path $PSScriptRoot "bin\Release\net8.0\win-x64\publish\WebApiDowntime.exe"

# Проверяем существование файла
if (-NOT (Test-Path $servicePath)) {
    Write-Warning "Файл службы не найден: $servicePath"
    Write-Host "Сначала выполните публикацию приложения: dotnet publish -c Release -r win-x64"
    Break
}

# Создаем службу
New-Service -Name "WebApiDowntime" `
    -BinaryPathName $servicePath `
    -DisplayName "WebApiDowntime Service" `
    -StartupType Automatic `
    -Description "WebApiDowntime Service for API"

# Запускаем службу
Start-Service -Name "WebApiDowntime"

Write-Host "Служба успешно установлена и запущена"
Write-Host "Для управления службой используйте:"
Write-Host "  - Остановка: Stop-Service WebApiDowntime"
Write-Host "  - Запуск: Start-Service WebApiDowntime"
Write-Host "  - Удаление: Remove-Service WebApiDowntime"
Write-Host ""
Write-Host "Логи доступны в Windows Event Viewer:"
Write-Host "1. Откройте Event Viewer (eventvwr.msc)"
Write-Host "2. Перейдите в Windows Logs -> Application"
Write-Host "3. Фильтруйте по источнику 'WebApiDowntime'" 