

docker-compose down
docker system prune -f
rmdir /s /q mssql


docker-compose up -d mssql
docker-compose down --remove-orphans
docker-compose up -d


### Sql Server Database
![alt text](image.png)

## Endpoints

### Ask Endpoint:
![alt text](image-1.png)

![alt text](image-2.png)

![alt text](image-3.png)

![alt text](image-4.png)