<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

if (isset($data["id"])) {
    $id = $data["id"];
    
    include('servercon.php');

    $query = "DELETE FROM Students WHERE id = ".$id;

    if(mysqli_query($dbconnect, $query)) {
        echo json_encode(array("message" => "Deleted Successfully", "status" => true));
    } else {
        echo json_encode(array("message" => "Not Deleted", "status" => false));
    }
} else {
    echo json_encode(array("message" => "Invalid request. Missing 'id' parameter.", "status" => false));
}
?>