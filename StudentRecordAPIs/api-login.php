<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

include('servercon.php');

// Assuming you are expecting JSON data in the request
$data = json_decode(file_get_contents("php://input"), true);

$username = isset($data['username']) ? $data['username'] : '';
$password = isset($data['password']) ? $data['password'] : '';

// Check if the required parameters are present
if ($username !== '' && $password !== '') {
    // Check the login credentials against the Users table
    $query = "SELECT * FROM Users WHERE username = '$username' AND password = '$password'";
    $result = mysqli_query($dbconnect, $query);

    if ($result) {
        // Check if any row is returned
        if (mysqli_num_rows($result) > 0) {
            echo json_encode(array("message" => "Login successful", "status" => true));
        } else {
            echo json_encode(array("message" => "Invalid username or password", "status" => false));
        }
    } else {
        echo json_encode(array("message" => "Error executing query", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Missing parameters", "status" => false));
}

?>