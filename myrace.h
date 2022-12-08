git pull
cp myrace.service /etc/systemd/system/myrace.service
cp myraceworker.service /etc/systemd/system/myraceworker.service
systemctl daemon-reload
systemctl enable myrace.service
systemctl enable myraceworker.service
systemctl stop myrace.service
systemctl stop myraceworker.service
dotnet publish my.race.api -c Release -o ../publish
dotnet publish my.race.myraceworker -c Release -o ../publish
systemctl start myrace.service
systemctl start myraceworker.service
systemctl status myrace.service 
systemctl status myraceworker.service 