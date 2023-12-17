<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

// Check if required parameters are provided
if (
    isset($data["student_id"]) &&
    isset($data["class_id"]) &&
    isset($data["attendance_date"]) &&
    isset($data["status"])
) {
    $studentId = $data["student_id"];
    $classId = $data["class_id"];
    $attendanceDate = $data["attendance_date"];
    $status = $data["status"];

    include('servercon.php');

    // Use prepared statements to prevent SQL injection
    $query = "INSERT INTO Attendance (student_id, class_id, attendance_date, status) 
              VALUES (?, ?, ?, ?)";

    $stmt = mysqli_prepare($dbconnect, $query);

    mysqli_stmt_bind_param($stmt, "iiss", $studentId, $classId, $attendanceDate, $status);

    if(mysqli_stmt_execute($stmt)) {
        echo json_encode(array("message" => "Attendance Recorded Successfully", "status" => true));
    } else {
        echo json_encode(array("message" => "Failed to Record Attendance", "status" => false));
    }

    mysqli_stmt_close($stmt);
    mysqli_close($dbconnect);
} else {
    echo json_encode(array("message" => "Invalid request. Missing required parameters.", "status" => false));
}
?>