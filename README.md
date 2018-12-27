# LinuxService

paste file myservice.service into /etc/systemd/system  
  
register service:
$ systemctl daemon-reload  
$ systemctl enable myservice.service
$ systemctl start myservice.service
$ systemctl status myservice.service

$ systemctl stop myservice.service
