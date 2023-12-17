<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

// Check if required parameters are provided
if (
    isset($data["id"]) &&
    isset($data["first_name"]) &&
    isset($data["last_name"]) &&
    isset($data["date_of_birth"]) &&
    isset($data["address"]) &&
    isset($data["email"]) &&
    isset($data["phone"]) &&
    isset($data["grade_level"])
) {
    $id = $data["id"];
    $firstName = $data["first_name"];
    $lastName = $data["last_name"];
    $dateOfBirth = $data["date_of_birth"];
    $address = $data["address"];
    $email = $data["email"];
    $phone = $data["phone"];
    $gradeLevel = $data["grade_level"];

    include('servercon.php');

    // Use prepared statements to prevent SQL injection
    $query = "UPDATE Students SET
              first_name = ?,
              last_name = ?,
              date_of_birth = ?,
              address = ?,
              email = ?,
              phone = ?,
              grade_level = ?
              WHERE id = ?";

    $stmt = mysqli_prepare($dbconnect, $query);

    mysqli_stmt_bind_param($stmt, "ssssssii", $firstName, $lastName, $dateOfBirth, $address, $email, $phone, $gradeLevel, $id);

    if (mysqli_stmt_execute($stmt)) {
        echo json_encode(array("message" => "Updated Successfully", "status" => true));
    } else {
        echo json_encode(array("message" => "Not Updated", "status" => false));
    }

    mysqli_stmt_close($stmt);
    mysqli_close($dbconnect);
} else {
    echo json_encode(array("message" => "Invalid request. Missing required parameters.", "status" => false));
}
?>