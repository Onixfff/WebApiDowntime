# Проверяем права администратора
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Warning "Требуются права администратора. Запустите скрипт от имени администратора."
    Break
}

# Останавливаем службу
Stop-Service -Name "WebApiDowntime"

# Удаляем службу
Remove-Service -Name "WebApiDowntime"

Write-Host "Служба успешно удалена" 