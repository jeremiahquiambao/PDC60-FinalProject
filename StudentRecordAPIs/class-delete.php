<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: DELETE");

$data = json_decode(file_get_contents("php://input"), true);

$classId = $data["class_id"];
//$classId= 4;

include('servercon.php');

$query = "DELETE FROM classes WHERE id = '".$classId."'";

if(mysqli_query($dbconnect, $query)) {
    echo json_encode(array("message" => "Deleted Successfully", "status" => true));
} else {
    echo json_encode(array("message" => "Not Deleted", "status" => false));
}

?>