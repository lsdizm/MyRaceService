sudo git -C /home/opc/MyRaceService pull
sudo cp /home/opc/MyRaceService/myrace.service /etc/systemd/system/myrace.service
sudo cp /home/opc/MyRaceService/myraceworker.service /etc/systemd/system/myraceworker.service
sudo systemctl daemon-reload
sudo systemctl enable myrace.service
sudo systemctl enable myraceworker.service
sudo systemctl stop myrace.service
sudo systemctl stop myraceworker.service
sudo dotnet publish /home/opc/MyRaceService/my.race.api -c Release -o /home/opc/MyRaceService/publish/
sudo dotnet publish /home/opc/MyRaceService/my.race.worker -c Release -o /home/opc/MyRaceService/publish/
sudo systemctl start myrace.service
sudo systemctl start myraceworker.service
sudo systemctl status myrace.service 
sudo systemctl status myraceworker.service 