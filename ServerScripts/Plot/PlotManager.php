<?php
// include connection check
include '../ConnectionCheck.php';

// check action type
switch ($request->action) {
    case "getPlot":
        include 'GetPlot.php';
        break;
    case "savePlot":
        include 'SavePlot.php';
        break;
    case "checkTile":
        include 'CheckTile.php';
        break;
    case "sell":
        include 'Sell.php';
        break;
    default:
    $response->status = "noValidAction";
    $response->customMessage = "no valid action was given.";
    die (json_encode($response));
}
function GetUserData($connectionResult, $userid) {
    // get userdata from db
    $stmt = $connectionResult->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
    $stmt->execute([':user_id' => $userid]);
    $result = $stmt->fetch(PDO::FETCH_ASSOC);

    // make userdata into class
    $userData = new stdClass();
    foreach ($result as $key => $value) {
        if ($key != 'user_id') {
            $userData->$key = $value;
        }
    }
    return $userData;
}
