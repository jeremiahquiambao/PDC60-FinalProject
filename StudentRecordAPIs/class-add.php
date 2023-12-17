<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

$className = $data["class_name"];

include('servercon.php');

$query = "INSERT INTO classes(class_name) 
          VALUES ('".$className."')";

if(mysqli_query($dbconnect, $query)) {
    echo json_encode(array("message" => "Inserted Successfully", "status" => true));
} else {
    echo json_encode(array("message" => "Not Inserted", "status" => false));
}

?>