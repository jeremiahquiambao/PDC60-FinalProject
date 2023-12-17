<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

// Check if the ID and other data are provided in the request
if (isset($_POST["id"]) && isset($_POST["school"]) && isset($_POST["academic_year"]) && isset($_POST["year_level"])) {
    $id = $_POST["id"];
    $school = $_POST["school"];
    $academic_year = $_POST["academic_year"];
    $year_level = $_POST["year_level"];

    include('servercon.php');

    // Update the academic history record
    $query = "UPDATE AcademicHistory SET school='$school', academic_year='$academic_year', year_level='$year_level' WHERE id='$id'";
    $updateResult = mysqli_query($dbconnect, $query);

    if ($updateResult) {
        echo json_encode(array("message" => "Academic history updated successfully", "status" => true));
    } else {
        echo json_encode(array("message" => "Error updating academic history", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Invalid request. Missing required data (id, school, academic_year, or year_level)", "status" => false));
}
?>