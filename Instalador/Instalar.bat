@echo off
chcp 65001 > nul
title Instalador de NutriEvolve

echo ==========================================================
echo        INSTALADOR DE NUTRIEVOLVE - SISTEMA CLÍNICO
echo ==========================================================
echo.
echo Iniciando asistente gráfico de instalación...
echo.

cd /d "%~dp0"

if exist "Setup.exe" (
    start "" "Setup.exe"
) else (
    echo [ERROR] No se encontró Setup.exe en este directorio.
    pause
)
