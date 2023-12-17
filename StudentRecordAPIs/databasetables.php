<?php

$hostname = "localhost";
$username = "root";
$password = "";
$database = "student_records_db";

// Create connection
$dbconnect = mysqli_connect($hostname, $username, $password, $database);

// Check connection
if (mysqli_connect_errno()) {
    echo "Failed to connect to MySQL: " . mysqli_connect_error();
    die();
}

// Table 2: Classes
$query = "CREATE TABLE IF NOT EXISTS Classes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    class_name VARCHAR(200)
)";

if (mysqli_query($dbconnect, $query)) {
    echo "Table 'Classes' created successfully<br>";
} else {
    echo "Error creating table 'Classes': " . mysqli_error($dbconnect) . "<br>";
}

// Table 1: Students
$query = "CREATE TABLE IF NOT EXISTS Students (
    id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    date_of_birth DATE,
    address VARCHAR(255),
    email VARCHAR(50),
    phone VARCHAR(15),
    grade_level INT,
    class_id INT,
    FOREIGN KEY (class_id) REFERENCES Classes(id) ON DELETE CASCADE
)";

if (mysqli_query($dbconnect, $query)) {
    echo "Table 'Students' created successfully<br>";
} else {
    echo "Error creating table 'Students': " . mysqli_error($dbconnect) . "<br>";
}

// Table 3: AcademicHistory
$query = "CREATE TABLE IF NOT EXISTS AcademicHistory (
    id INT PRIMARY KEY AUTO_INCREMENT,
    student_id INT,
    school VARCHAR(50),
    academic_year VARCHAR(10),
    year_level VARCHAR(10),
    FOREIGN KEY (student_id) REFERENCES Students(id) ON DELETE CASCADE
)";

if (mysqli_query($dbconnect, $query)) {
    echo "Table 'AcademicHistory' created successfully<br>";
} else {
    echo "Error creating table 'AcademicHistory': " . mysqli_error($dbconnect) . "<br>";
}

// Table 4: Attendance
$query = "CREATE TABLE IF NOT EXISTS Attendance (
    id INT PRIMARY KEY AUTO_INCREMENT,
    student_id INT,
    class_id INT,
    attendance_date DATE,
    status VARCHAR(10),
    FOREIGN KEY (student_id) REFERENCES Students(id) ON DELETE CASCADE,
    FOREIGN KEY (class_id) REFERENCES Classes(id) ON DELETE CASCADE
)";

if (mysqli_query($dbconnect, $query)) {
    echo "Table 'Attendance' created successfully<br>";
} else {
    echo "Error creating table 'Attendance': " . mysqli_error($dbconnect) . "<br>";
}

// Table 5: Users
$query = "CREATE TABLE IF NOT EXISTS Users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50),
    password VARCHAR(255)
)";

if (mysqli_query($dbconnect, $query)) {
    echo "Table 'Users' created successfully<br>";

    // Insert default user
    $username = "admin";
    $password = "password123";

    $insertQuery = "INSERT INTO Users (username, password) VALUES ('$username', '$password')";

    if (mysqli_query($dbconnect, $insertQuery)) {
        echo "Default user inserted successfully<br>";
    } else {
        echo "Error inserting default user: " . mysqli_error($dbconnect) . "<br>";
    }
} else {
    echo "Error creating table 'Users': " . mysqli_error($dbconnect) . "<br>";
}


// Close the connection
mysqli_close($dbconnect);
?>