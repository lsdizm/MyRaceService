sudo git pull -C MyRaceService
sudo systemctl stop myrace.service
sudo dotnet publish MyRaceService/my.race.api -c Release -o ../publish
sudo cp MyRaceService/myrace.service /etc/systemd/system/myrace.service
sudo systemctl daemon-reload
sudo systemctl enable myrace.service
sudo systemctl start myrace.service
sudo systemctl status myrace.service 