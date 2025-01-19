FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Install necessary build dependencies
RUN apt-get update && apt-get install -y \
    libc6-dev \
    libx11-dev \
    xorg-dev \
    libopenal-dev

# Copy source files
COPY src/ ./

# Default target platform
ARG RUNTIME=win-x64

# Build to a temporary location
RUN dotnet restore && \
    dotnet publish -c Release -r ${RUNTIME} --self-contained true -o /build-output

# Create a second stage for copying
FROM alpine:latest
COPY --from=build /build-output /build-output
CMD ["cp", "-r", "/build-output/.", "/app/out/"]
