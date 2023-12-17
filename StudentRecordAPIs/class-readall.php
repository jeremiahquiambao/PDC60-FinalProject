<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

include('servercon.php');

$query = "SELECT * FROM classes";

$result = mysqli_query($dbconnect, $query);

if ($result) {
    $classes = array();

    while ($row = mysqli_fetch_assoc($result)) {
        $classes[] = $row;
    }

    echo json_encode(array("message" => "Retrieved Successfully", "status" => true, "classes" => $classes));
} else {
    echo json_encode(array("message" => "Failed to retrieve classes", "status" => false));
}

?>