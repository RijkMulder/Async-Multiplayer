<?php
// get user
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE token = :token");
$stmt->execute([':token' => $request->token]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);

// give new plot
$userid = $result['user_id'];
$stmt = $connectionResult->prepare("SELECT * FROM user_plots WHERE user_id = :id");
$stmt->execute([':id' => $userid]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
if ($result == null) {
    // insert new plot
    $stmt = $connectionResult->prepare("INSERT INTO user_plots (user_id, plot_layout) VALUES (:user_id, :plot_layout)");
    $stmt->execute([':user_id' => $userid, ':plot_layout' => json_encode($request->plot)]);
    $response->status = "plotSaved";
    $response->customMessage = "plot has been saved.";
    die(json_encode($response));
}

// save plot and tiles
else {
    // save tiles
    $stmt = $connectionResult->prepare("UPDATE user_plots SET plot_layout = :plot_layout WHERE user_id = :user_id");
    $stmt->execute([':user_id' => $userid, ':plot_layout' => json_encode($request->plot)]);

    $response->status = "plotUpdated";
    $response->customMessage = "user plot updated";
    die(json_encode($response));
}