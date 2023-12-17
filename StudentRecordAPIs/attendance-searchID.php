<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: GET");

// Check if the id parameter is provided in the URL
if (isset($_GET["id"])) {
    $id = $_GET["id"];

    include('servercon.php');

    // Use prepared statements to prevent SQL injection
    $query = "SELECT * FROM Attendance WHERE id = ?";
    $stmt = mysqli_prepare($dbconnect, $query);
    mysqli_stmt_bind_param($stmt, "i", $id);
    mysqli_stmt_execute($stmt);

    $result = mysqli_stmt_get_result($stmt);

    // Fetch data as an associative array
    $attendanceData = mysqli_fetch_assoc($result);

    if (!empty($attendanceData)) {
        echo json_encode(array("message" => "Attendance Data Retrieved Successfully", "status" => true, "data" => $attendanceData));
    } else {
        echo json_encode(array("message" => "No Attendance Data Found for the Specified ID", "status" => false));
    }

    mysqli_stmt_close($stmt);
    mysqli_close($dbconnect);
} else {
    echo json_encode(array("message" => "Invalid request. Missing required parameters.", "status" => false));
}
?>