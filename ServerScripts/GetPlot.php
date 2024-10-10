<?php
// get user
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE token = :token");
$stmt->execute([':token' => $response->token]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
$userid = $result['user_id'];

// get all tiles fomr user
$stmt = $connectionResult->prepare("SELECT * FROM user_tiles WHERE user_id = :user_id");
$stmt->execute([':user_id' => $userid]);


// send plot
$response->plot = "10,10";
$response->status = "plot";
$response->customMessage = "empty plot sent";
die(json_encode($response));

