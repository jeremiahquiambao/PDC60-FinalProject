<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

include('servercon.php');

// Assuming you are expecting JSON data in the request
$data = json_decode(file_get_contents("php://input"), true);

$searchId = isset($data['id']) ? $data['id'] : '';
$newClassName = isset($data['new_class_name']) ? $data['new_class_name'] : '';

// Check if the required parameters are present
if ($searchId !== '' && $newClassName !== '') {
    // Update the class name in the database
    $updateQuery = "UPDATE classes SET class_name = '$newClassName' WHERE id = '$searchId'";
    $updateResult = mysqli_query($dbconnect, $updateQuery);

    if ($updateResult) {
        echo json_encode(array("message" => "Update Successful", "status" => true));
    } else {
        echo json_encode(array("message" => "Failed to update class", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Missing parameters", "status" => false));
}

?>