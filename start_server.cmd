cd frontend
cmd /C npm install
cmd /C npm run build
cd ..\backend\Supportbot.Webapi
dotnet restore --no-cache
:start
dotnet watch run
goto start
