<?php
// get user
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE token = :token");
$stmt->execute([':token' => $request->token]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);

// check if user has a plot
$userid = $result['user_id'];
$stmt = $connectionResult->prepare("SELECT * FROM user_plots WHERE user_id = :id");
$stmt->execute([':id' => $userid]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
if ($result == null) {
    $response->status = "userHasNoPlot";
    $response->customMessage = "this user does not have a plot yet";
    die (json_encode($response));   
}

