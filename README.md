# MinIO / S3 Spike Repository for Aspire

This repository contains the spike project for integrating MinIO, an S3 compatible object storage, into the Aspire framework. The primary goal is to provide a seamless local development experience using MinIO, which mirrors the S3 API. 
This is mainly to be a sandbox repo for this pr https://github.com/dotnet/aspire/pull/1800

## Overview

The project sets up a MinIO container, simulating the S3 API environment on `localhost:9000` with a web console available on `localhost:9001`. 
The main challenge addressed in this spike is the setup and configuration of access keys for MinIO, which is essential for the proper functioning of the S3 SDK within the Aspire framework.

## Getting Started


### Configuration

The MinIO container is configured with the following default credentials and ports:

- **Root User**: `minio`
- **Root Password**: `RunningZebraMan32332#`
- **API Address**: `localhost:9000`
- **Console Address**: `localhost:9001`


