<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

include('servercon.php');

$searchId = isset($_GET['id']) ? $_GET['id'] : '';

$query = "SELECT * FROM classes WHERE id = '$searchId'";

$result = mysqli_query($dbconnect, $query);

if ($result) {
    $class = mysqli_fetch_assoc($result);

    if ($class) {
        echo json_encode(array("message" => "Search Successful", "status" => true, "class" => $class));
    } else {
        echo json_encode(array("message" => "Class not found", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Failed to perform search", "status" => false));
}

?>