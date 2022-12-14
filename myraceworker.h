sudo git -C /home/opc/MyRaceService pull
sudo dotnet publish /home/opc/MyRaceService/my.race.worker -c Release -o /home/opc/MyRaceService/publish/
sudo systemctl stop myraceworker.service
sudo systemctl start myraceworker.service
sudo systemctl status myraceworker.service 