<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

// Check if required parameters are provided
if (
    isset($data["first_name"]) &&
    isset($data["last_name"]) &&
    isset($data["date_of_birth"]) &&
    isset($data["address"]) &&
    isset($data["email"]) &&
    isset($data["phone"]) &&
    isset($data["class_id"]) &&
    isset($data["grade_level"])
) {
    $firstName = $data["first_name"];
    $lastName = $data["last_name"];
    $dateOfBirth = $data["date_of_birth"];
    $address = $data["address"];
    $email = $data["email"];
    $phone = $data["phone"];
    $classId = $data["class_id"];
    $gradeLevel = $data["grade_level"];

    include('servercon.php');

    // Use prepared statements to prevent SQL injection
    $query = "INSERT INTO Students (first_name, last_name, date_of_birth, address, email, phone, class_id, grade_level) 
              VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

    $stmt = mysqli_prepare($dbconnect, $query);

    mysqli_stmt_bind_param($stmt, "ssssssii", $firstName, $lastName, $dateOfBirth, $address, $email, $phone, $classId, $gradeLevel);

    if(mysqli_stmt_execute($stmt)) {
        echo json_encode(array("message" => "Inserted Successfully", "status" => true));
    } else {
        echo json_encode(array("message" => "Not Inserted", "status" => false));
    }

    mysqli_stmt_close($stmt);
    mysqli_close($dbconnect);
} else {
    echo json_encode(array("message" => "Invalid request. Missing required parameters.", "status" => false));
}
?>