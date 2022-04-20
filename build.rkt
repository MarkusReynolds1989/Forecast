; @echo off
; Racket.exe "%~f0" %*
; exit /b
#lang racket/base
(require racket/system)

(thread (lambda () (system "dotnet publish -r win-x64 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r win-x86 -c release --self-contained true")))

#|
    Cross platform compilation not yet supported.
    Could easily compile the linux ones over wsl, however.
(thread (lambda () (system "dotnet publish -r linux-x64 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r linux-x86 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r osx-x64 -c release --self-contained true")))
(thread (lambda () (system "dotnet publish -r osx-x86 -c release --self-contained true")))
|#