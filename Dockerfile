FROM microsoft/dotnet
ADD src/ParralellRequests/ /src
WORKDIR /src
RUN dotnet restore && dotnet build
CMD dotnet run
