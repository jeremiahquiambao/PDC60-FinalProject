<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

// Check if the ID is provided in the URL
if (isset($_GET["id"])) {
    $id = $_GET["id"];

    include('servercon.php');

    // Fetch academic history record for a specific id
    $query = "SELECT * FROM AcademicHistory WHERE id = $id";
    $result = mysqli_query($dbconnect, $query);

    if ($result) {
        // Check if academic history record was found
        if (mysqli_num_rows($result) > 0) {
            // Fetch academic history record
            $academicHistory = mysqli_fetch_assoc($result);

            // Return the academic history data as JSON
            echo json_encode(array("message" => "Academic history found", "status" => true, "academic_history" => $academicHistory));
        } else {
            echo json_encode(array("message" => "Academic history not found", "status" => false));
        }
    } else {
        echo json_encode(array("message" => "Query error", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Invalid request. Missing 'id' parameter.", "status" => false));
}

?>