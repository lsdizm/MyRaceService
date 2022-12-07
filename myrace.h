git pull
cp myrace.service /etc/systemd/system/myrace.service
systemctl daemon-reload
systemctl enable myrace.service
systemctl stop myrace.service
dotnet publish my.race.api -c Release -o ../publish
systemctl start myrace.service
systemctl status myrace.service 