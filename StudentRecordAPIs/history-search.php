<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

// Check if the student ID is provided in the URL
if (isset($_GET["student_id"])) {
    $student_id = $_GET["student_id"];

    include('servercon.php');

    // Fetch all academic history records for a specific student_id, sorted by academic_year
    $query = "SELECT * FROM AcademicHistory WHERE student_id = $student_id ORDER BY academic_year";
    $result = mysqli_query($dbconnect, $query);

    if ($result) {
        // Check if academic history records were found
        if (mysqli_num_rows($result) > 0) {
            // Fetch all academic history records
            $academicHistory = array();
            while ($row = mysqli_fetch_assoc($result)) {
                $academicHistory[] = $row;
            }

            // Return the academic history data as JSON
            echo json_encode(array("message" => "Academic history found", "status" => true, "academic_history" => $academicHistory));
        } else {
            echo json_encode(array("message" => "Academic history not found", "status" => false));
        }
    } else {
        echo json_encode(array("message" => "Query error", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Invalid request. Missing 'student_id' parameter.", "status" => false));
}

?>