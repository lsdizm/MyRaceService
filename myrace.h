git -C MyRaceService pull
dotnet publish MyRaceService/my.race.api -c Release -o ../publish
cp MyRaceService/myrace.service /etc/systemd/system/myrace.service
systemctl daemon-reload
systemctl stop myrace.service
systemctl enable myrace.service
systemctl start myrace.service
systemctl status myrace.service 