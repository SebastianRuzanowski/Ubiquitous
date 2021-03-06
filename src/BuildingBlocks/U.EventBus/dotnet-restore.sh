#!/bin/bash
case "$CI_COMMIT_REF_NAME" in
  "master")
  dotnet restore /property:Configuration=Release --configfile nuget.config --no-cache --force
    ;;
  * | "develop")
  dotnet restore /property:Configuration=Release --configfile nuget.develop.config --no-cache --force
    ;;
esac