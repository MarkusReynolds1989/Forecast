; @echo off
; Racket.exe "%~f0" %*
; exit /b
#lang racket/base
(require racket/system)

(thread (lambda () (system "dotnet publish -r win-x64 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r win-x32 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r linux-x64 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r linux-x32 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r osx-x64 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r osx-x32 -c release --self-contained true")))