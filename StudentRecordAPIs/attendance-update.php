<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

// Check if required parameters are provided
if (
    isset($data["id"]) &&
    isset($data["attendance_date"]) &&
    isset($data["status"])
) {
    $id = $data["id"];
    $attendanceDate = $data["attendance_date"];
    $status = $data["status"];

    include('servercon.php');

    // Use prepared statements to prevent SQL injection
    $query = "UPDATE Attendance SET attendance_date = ?, status = ? WHERE id = ?";
    $stmt = mysqli_prepare($dbconnect, $query);

    mysqli_stmt_bind_param($stmt, "ssi", $attendanceDate, $status, $id);

    if(mysqli_stmt_execute($stmt)) {
        $responseData = array(
            "message" => "Attendance Updated Successfully",
            "status" => true,
            "data" => array(
                "id" => $id,
                "attendance_date" => $attendanceDate,
                "status" => $status
            )
        );
        echo json_encode($responseData);
    } else {
        $responseData = array(
            "message" => "Error updating attendance",
            "status" => false
        );
        echo json_encode($responseData);
    }

    mysqli_stmt_close($stmt);
    mysqli_close($dbconnect);
} else {
    $responseData = array(
        "message" => "Invalid request. Missing required parameters.",
        "status" => false
    );
    echo json_encode($responseData);
}
?>