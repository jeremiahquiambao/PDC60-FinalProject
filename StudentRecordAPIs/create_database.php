<?php

$hostname = "localhost";
$username = "root";
$password = "";

// Create connection
$dbconnect = mysqli_connect($hostname, $username, $password);

// Check connection
if (mysqli_connect_errno()) {
    echo "Failed to connect to MySQL: " . mysqli_connect_error();
    die();
}

// Create database
$database = "student_records_db";
$query = "CREATE DATABASE IF NOT EXISTS $database";

if (mysqli_query($dbconnect, $query)) {
    echo "Database '$database' created successfully<br>";
} else {
    echo "Error creating database '$database': " . mysqli_error($dbconnect) . "<br>";
}

// Include database tables script
include_once("databasetables.php");

?>