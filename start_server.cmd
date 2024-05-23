docker run -d --name elasticsearch -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "xpack.security.enabled=false" -e "xpack.security.enrollment.enabled=false" elasticsearch:8.13.4

cd bot
dotnet restore --no-cache
start dotnet run

cd ../frontend
cmd /C npm install
cmd /C npm run build
cd ..\backend\Supportbot.Webapi
dotnet restore --no-cache
:start
dotnet watch run
goto start
