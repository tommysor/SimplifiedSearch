#!/bin/bash

dotnet restore
dotnet build --no-restore
dotnet tool restore
