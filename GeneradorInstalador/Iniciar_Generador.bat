@echo off
chcp 65001 > nul
title Generador R Alpha

echo ==========================================================
echo        GENERADOR R ALPHA - Desarrollado por Codigo Mecha
echo ==========================================================
echo.
echo Iniciando interfaz grafica del generador...
echo.

cd /d "%~dp0"

if exist "Lanzador\GeneradorInstalador.exe" (
    start "" "Lanzador\GeneradorInstalador.exe"
) else (
    echo [ERROR] No se encontró Lanzador\GeneradorInstalador.exe.
    pause
)
