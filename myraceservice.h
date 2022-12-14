sudo git -C /home/opc/MyRaceService pull
sudo dotnet publish /home/opc/MyRaceService/my.race.api -c Release -o /home/opc/MyRaceService/publish/
sudo systemctl stop myrace.service
sudo systemctl start myrace.service
sudo systemctl status myrace.service 