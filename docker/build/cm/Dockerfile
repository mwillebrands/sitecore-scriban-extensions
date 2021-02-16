# escape=`

ARG BASE_IMAGE
ARG SXA_IMAGE
ARG SPE_IMAGE
ARG TOOLING_IMAGE
ARG SOLUTION_IMAGE
ARG MANAGEMENT_SERVICES_IMAGE

FROM ${SOLUTION_IMAGE} as solution
FROM ${TOOLING_IMAGE} as tooling
FROM ${SPE_IMAGE} as spe
FROM ${SXA_IMAGE} as sxa
FROM ${MANAGEMENT_SERVICES_IMAGE} as managementservices
FROM ${BASE_IMAGE}


SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Copy development tools and entrypoint
COPY --from=tooling \tools\ \tools\

WORKDIR C:\inetpub\wwwroot

# Add SPE module
COPY --from=spe \module\cm\content .\

# Add SXA module
COPY --from=sxa \module\cm\content .\
COPY --from=sxa \module\tools \module\tools
RUN C:\module\tools\Initialize-Content.ps1 -TargetPath .\; `
    Remove-Item -Path C:\module -Recurse -Force;

# Add Sitecore Management Services
COPY --from=managementservices C:\module\cm\content C:\inetpub\wwwroot

# Copy solution website files
COPY --from=solution \artifacts\website\ .\

