# GPS Data Insertion Service

## Overview

This service is designed to continuously simulate and insert GPS data into an Oracle database. The service simulates the generation of GPS coordinates within predefined bounds and inserts this data at regular intervals, making it useful for testing and development purposes where mock GPS data is required.

## Features

- **Continuous Data Simulation**: Generates and inserts 65 GPS data points in each iteration, simulating real-time data.
- **Database Integration**: Utilizes Oracle's managed data access to interact with an Oracle database.
- **Performance Metrics**: Measures and logs the time taken for each insertion operation.
- **Configurable Delay**: Inserts data with a 5-second pause between iterations to simulate real-time data feed.
- **Error Handling**: Catches and logs database operation exceptions to ensure service continuity.

## Technical Details

- **.NET Version**: The application is built using .NET 7.0
- **Database**: Oracle Database. 
- **ORM**: Direct SQL commands with parameters to prevent SQL injection.

## Background Task

The service runs a background task that performs the following operations in a loop:
1. Opens a connection to the Oracle database.
2. Fetches the current maximum `OBJECT_ID` from the `GPS_DATA` table to maintain data consistency.
3. Generates random GPS coordinates within the specified latitude and longitude bounds.
4. Inserts the new data points into the `GPS_DATA` table with the incremented `OBJECT_ID`.
5. Logs the operation time and sleeps for 5 seconds before the next iteration.

## Contributing

We welcome contributions and suggestions! Please fork the repository and create a new pull request for any features or bug fixes.

