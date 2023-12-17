<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

// Check if the academic history ID is provided in the request
if (isset($data["id"])) {
    $academic_history_id = $data["id"];

    include('servercon.php');

    // Delete the academic history record
    $query = "DELETE FROM AcademicHistory WHERE id = $academic_history_id";
    $deleteResult = mysqli_query($dbconnect, $query);

    if ($deleteResult) {
        echo json_encode(array("message" => "Academic history deleted successfully", "status" => true));
    } else {
        echo json_encode(array("message" => "Error deleting academic history", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Invalid request. Missing 'id' parameter.", "status" => false));
}

?>