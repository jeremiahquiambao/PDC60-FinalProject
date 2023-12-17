<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

include('servercon.php');

// Check if the class_id parameter is present in the URL
if (isset($_GET['class_id'])) {
    $classId = $_GET['class_id'];

    // Fetch all students with the given class_id
    $query = "SELECT * FROM students WHERE class_id = '$classId'";
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