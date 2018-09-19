FROM microsoft/dotnet:2.1.4-aspnetcore-runtime
ARG source
WORKDIR /app
EXPOSE 80
EXPOSE 5000
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "MyVendor.MyService.dll"]
