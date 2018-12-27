# LinuxService

Paste file myservice.service into /etc/systemd/system  
  
Register the service:
$ systemctl daemon-reload
$ systemctl enable myservice.service
$ systemctl start myservice.service
$ systemctl status myservice.service


Stop the service
$ systemctl stop myservice.service
