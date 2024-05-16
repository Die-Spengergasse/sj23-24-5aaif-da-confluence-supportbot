#!/bin/bash
cd frontend
npm install
npm run build
cd ../backend/Supportbot.Webapi
dotnet restore --no-cache
dotnet watch run

