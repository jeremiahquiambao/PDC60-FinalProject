<?php
$hostname = "localhost";
$username = "root";
$password = "";
$database = "pdc60final";

// Create connection
$dbconnect = mysqli_connect($hostname, $username, $password);

// Check connection
if (mysqli_connect_errno()) {
    echo "Failed to connect to MySQL: " . mysqli_connect_error();
    die();
}

// Check if the database exists
$checkDbQuery = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '$database'";
$result = mysqli_query($dbconnect, $checkDbQuery);

if (mysqli_num_rows($result) == 0) {
    // If the database doesn't exist, create it
    $createDbQuery = "CREATE DATABASE $database";
    
    if (mysqli_query($dbconnect, $createDbQuery)) {
        echo "Database created successfully";
    } else {
        echo "Error creating database: " . mysqli_error($dbconnect);
        die();
    }
}

// Select the database
if (!mysqli_select_db($dbconnect, $database)) {
    echo "Error selecting database: " . mysqli_error($dbconnect);
    die();
}


?>