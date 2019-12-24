#Get base SDK image from Microsoft
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

#Copy the csproj file 
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY AccountsMicroservice.sln ./
COPY ["DatabaseAccessLayer/DatabaseAccessLayer.csproj", "./DatabaseAccessLayer/"]
COPY ["ProductManagementMicroservice/ProductManagementMicroservice.csproj", "./ProductManagementMicroservice/"]

#restore any dependecies (via nuget)
RUN dotnet restore "ProductManagementMicroservice/ProductManagementMicroservice.csproj"
RUN dotnet restore "DatabaseAccessLayer/DatabaseAccessLayer.csproj"

#Copy the project files 
COPY . .

#Build out release project one - as referenced
WORKDIR "/src/DatabaseAccessLayer"
RUN dotnet build "DatabaseAccessLayer.csproj" -c Release -o /app

#Build out release hosting App
WORKDIR "/src/ProductManagementMicroservice"
RUN dotnet build "ProductManagementMicroservice.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ProductManagementMicroservice.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ProductManagementMicroservice.dll"]