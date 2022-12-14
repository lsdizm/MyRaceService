git -C /home/opc/MyRaceService pull
cp /home/opc/MyRaceService/myrace.service /etc/systemd/system/myrace.service
cp /home/opc/MyRaceService/myraceworker.service /etc/systemd/system/myraceworker.service
systemctl daemon-reload
systemctl enable myrace.service
systemctl enable myraceworker.service
systemctl stop myrace.service
systemctl stop myraceworker.service
dotnet publish /home/opc/MyRaceService/my.race.api -c Release -o /home/opc/MyRaceService/publish/
dotnet publish /home/opc/MyRaceService/my.race.worker -c Release -o /home/opc/MyRaceService/publish/
systemctl start myrace.service
systemctl start myraceworker.service
systemctl status myrace.service 
systemctl status myraceworker.service 