<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

// Check if the student ID is provided in the URL
if (isset($_GET["id"])) {
    $id = $_GET["id"];

    include('servercon.php');

    // Fetch the student by ID
    $query = "SELECT * FROM Students WHERE id = $id";
    $result = mysqli_query($dbconnect, $query);

    if ($result) {
        // Check if a student was found
        if (mysqli_num_rows($result) > 0) {
            $student = mysqli_fetch_assoc($result);

            // Return the student data as JSON
            echo json_encode(array("message" => "Student found", "status" => true, "student" => $student));
        } else {
            echo json_encode(array("message" => "Student not found", "status" => false));
        }
    } else {
        echo json_encode(array("message" => "Query error", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Invalid request. Missing 'id' parameter.", "status" => false));
}

?>