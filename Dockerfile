# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BUILD_IMAGE} AS prep

# Gather only artifacts necessary for NuGet restore, retaining directory structure
COPY *.sln nuget.config \nuget\
COPY src\ \temp\
RUN Invoke-Expression 'robocopy C:\temp C:\nuget\src /s /ndl /njh /njs *.csproj *.scproj packages.config'

FROM ${BUILD_IMAGE} AS builder

ARG BUILD_CONFIGURATION

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Create an empty working directory
WORKDIR C:\build

# Copy prepped NuGet artifacts, and restore as distinct layer to take better advantage of caching
COPY --from=prep .\nuget .\
RUN nuget restore

# Copy remaining source code
COPY src\ .\src\

# Copy transforms, retaining directory structure
RUN Invoke-Expression 'robocopy C:\build\src C:\out\transforms /s /ndl /njh /njs *.xdt'

RUN mkdir 'C:\Program Files\dotnet\sdk\NuGetFallbackFolder'
RUN nuget restore

# Build website with file publish
RUN msbuild .\src\Feature\Custom.XA.Feature.ScribanExtensions\code\Custom.XA.Feature.ScribanExtensions.csproj /p:Configuration=$env:BUILD_CONFIGURATION /p:DeployOnBuild=True /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:PublishUrl=C:\out\website

RUN mkdir 'C:\out\xconnect'
RUN mkdir 'C:\out\xconnect\bin'
RUN mkdir 'C:\out\xconnect\App_Data\Models'
RUN mkdir 'c:\out\identityserver'

FROM ${BASE_IMAGE}

WORKDIR C:\artifacts

# Copy final build artifacts
COPY --from=builder C:\out\website .\website\
COPY --from=builder C:\out\identityserver .\identityserver\
COPY --from=builder C:\out\xconnect .\xconnect\