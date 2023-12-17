<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

include('servercon.php');

// Check if the class_id parameter is present in the URL
if (isset($_GET['class_id'])) {
    $classId = $_GET['class_id'];

    // Additional parameters for searching by name and grade_level
    $name = isset($_GET['name']) ? $_GET['name'] : null;
    $gradeLevel = isset($_GET['grade_level']) ? $_GET['grade_level'] : null;

    // Build the SQL query based on provided parameters
    $query = "SELECT * FROM students WHERE class_id = '$classId'";

    // Append conditions for name and grade_level if provided
    if ($name !== null && $name !== '') {
        $query .= " AND (first_name LIKE '%$name%' OR last_name LIKE '%$name%')";
    }

    if ($gradeLevel !== null && $gradeLevel !== '0') {
        $query .= " AND grade_level = '$gradeLevel'";
    }

    $result = mysqli_query($dbconnect, $query);

    if ($result) {
        $students = array();

        // Fetch each student's details
        while ($row = mysqli_fetch_assoc($result)) {
            $students[] = $row;
        }

        echo json_encode(array("status" => true, "students" => $students));
    } else {
        echo json_encode(array("message" => "Failed to fetch students", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Missing class_id parameter", "status" => false));
}
?>