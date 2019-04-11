FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base 
WORKDIR /app 
EXPOSE 70 
EXPOSE 44370 

FROM microsoft/dotnet:2.2-sdk AS build 
COPY . /src
WORKDIR /src 
COPY ["WebApi1.csproj", "WebApi1/"]
COPY ["Properties/launchSettings.json", "WebApi1/Properties/"]
COPY ["Program.cs", "WebApi1/"] 
COPY ["Startup.cs", "WebApi1/"] 
COPY ["RedirectSection.cs", "WebApi1/"] 
COPY ["appsettings.json", "WebApi1/"]
COPY ["Controllers/ValuesController.cs", "WebApi1/Controllers/"]   
COPY . .
RUN dotnet restore "WebApi1/WebApi1.csproj" 

WORKDIR "/src/WebApi1" 
RUN dotnet build "WebApi1.csproj" -c Release -o /app 

FROM build AS publish 
RUN dotnet publish "WebApi1.csproj" -c Release -o /app 

FROM base AS final 
WORKDIR /app 
COPY --from=publish /app . 
ENTRYPOINT ["dotnet", "WebApi1.dll"]