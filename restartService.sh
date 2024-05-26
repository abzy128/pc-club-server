echo "Restarting service with new version"
sudo systemctl stop pc-club-server.service
git pull
dotnet build
sudo systemctl start pc-club-server.service
