<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

// Check if the student ID is provided in the request
if (isset($_POST["student_id"])) {
    $student_id = $_POST["student_id"];

    // Check if the required data (school, academic_year, year_level) is provided
    if (isset($_POST["school"]) && isset($_POST["academic_year"]) && isset($_POST["year_level"])) {
        $school = $_POST["school"];
        $academic_year = $_POST["academic_year"];
        $year_level = $_POST["year_level"];

        include('servercon.php');

        // Insert the new academic history record
        $query = "INSERT INTO AcademicHistory (student_id, school, academic_year, year_level) VALUES ('$student_id', '$school', '$academic_year', '$year_level')";
        $insertResult = mysqli_query($dbconnect, $query);

        if ($insertResult) {
            echo json_encode(array("message" => "Academic history added successfully", "status" => true));
        } else {
            echo json_encode(array("message" => "Error adding academic history", "status" => false));
        }
    } else {
        echo json_encode(array("message" => "Invalid request. Missing required data (school, academic_year, or year_level)", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Invalid request. Missing 'student_id' parameter.", "status" => false));
}

?>