<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: DELETE");

$data = json_decode(file_get_contents("php://input"), true);

// Check if the ID parameter is provided in the request
if (isset($data["id"])) {
    $attendanceId = $data["id"];

    include('servercon.php');

    // Use prepared statements to prevent SQL injection
    $query = "DELETE FROM Attendance WHERE id = ?";
    $stmt = mysqli_prepare($dbconnect, $query);
    mysqli_stmt_bind_param($stmt, "i", $attendanceId);
    $success = mysqli_stmt_execute($stmt);

    if ($success) {
        echo json_encode(array("message" => "Attendance Data Deleted Successfully", "status" => true));
    } else {
        echo json_encode(array("message" => "Failed to Delete Attendance Data", "status" => false));
    }

    mysqli_stmt_close($stmt);
    mysqli_close($dbconnect);
} else {
    echo json_encode(array("message" => "Invalid request. Missing 'id' parameter.", "status" => false));
}
?>