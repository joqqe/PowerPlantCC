# PowerPlantCC
A solution of the coding challenge provide by [Engie](https://github.com/gem-spaas/powerplant-coding-challenge)

## Local setup
1. Open powershell and navigate to root folder.
2. Restore dependencies
    ```
    dotnet restore
    ```
3. Build solution
    ```
    dotnet build
    ```
4. Run tests
    ```
    dotnet test
    ```
5. Run api
    ```
    dotnet run --project .\src\PowerplantCC.Api\PowerplantCC.Api.csproj
    ```
6. Open a browser and navigate to [http://localhost:8888/swagger/index.html](http://localhost:8888/swagger/index.html)

## Docker
1. Open powershell and navigate to root folder.
2. Create a docker image.
    ```
    docker build -f docker/Dockerfile.Api .
    ```
3. Run the docker image.
    ```
    docker run -p 8888:8080 <image-id>
    ```
4. No swagger available in a production image! Please use Postman or an other tool to call the productionplan endpoint.