

docker-compose down
docker system prune -f
rmdir /s /q mssql


docker-compose up -d mssql
docker-compose down --remove-orphans
docker-compose up -d


# vectorsearchapp:
  #   image: ${DOCKER_REGISTRY-}vectorsearchapp
  #   build:
  #     context: .
  #     dockerfile: VectorSearchApp/Dockerfile
  #   depends_on:
  #     - mssql
  #   networks:
  #     - app-network

### Sql Server Database
![alt text](image.png)

## Endpoints

### Ask Endpoint:
![alt text](image-1.png)
