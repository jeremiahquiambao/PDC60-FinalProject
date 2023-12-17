<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

include('servercon.php');

$searchTerm = isset($_GET['search']) ? $_GET['search'] : '';

$query = "SELECT * FROM classes WHERE class_name LIKE '%$searchTerm%'";

$result = mysqli_query($dbconnect, $query);

if ($result) {
    $classes = array();

    while ($row = mysqli_fetch_assoc($result)) {
        $classes[] = $row;
    }

    echo json_encode(array("message" => "Search Successful", "status" => true, "classes" => $classes));
} else {
    echo json_encode(array("message" => "Failed to perform search", "status" => false));
}

?>