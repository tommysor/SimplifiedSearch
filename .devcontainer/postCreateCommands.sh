#!/bin/bash

sudo apt update
sudo apt-get install dotnet-sdk-3.1 -y

dotnet restore
dotnet build --no-restore
