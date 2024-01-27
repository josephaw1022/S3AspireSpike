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

### Running the Project

To run the project:

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/your-repo-name.git
   cd your-repo-name
   ```

2. Start the application:

   ```bash
   dotnet run
   ```

   This will start the MinIO container and the Aspire application.

### Setting Up MinIO Access Keys

To set up MinIO access keys, follow these steps:

1. **Access MinIO Console**:
   Navigate to `http://localhost:9001` and log in using the root user credentials.

2. **Create a New User**:
   - Navigate to the 'Identity' section.
   - Create a new user with the desired access and secret keys.

   Alternatively, you can use the MinIO Client (`mc`) to set up the user:

   ```bash
   mc alias set myminio http://localhost:9000 minio <minio_root_password>
   mc admin user add myminio <access_token> <secret_token>
   mc admin policy attach myminio readwrite --user <access_token>
   ```

### Automating User and Policy Setup

An entrypoint script is being developed to automate the user and policy setup in MinIO. This script will:

- Set the MinIO alias.
- Add a new user with specified access and secret tokens.
- Attach the `readwrite` policy to the user.

This will allow users to start the application without manual configuration steps.

