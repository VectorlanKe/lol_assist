@echo off
dotnet publish -r win-x86 ^
	-c Release ^
	-p:PublishSingleFile=true ^
	-p:IncludeNativeLibrariesForSelfExtract=true ^
	-o ./bin/Publish