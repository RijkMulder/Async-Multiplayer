<?php
// set constants
define("INTERVAL", 10);
define("QUANTITY", 2);

// convert last update to timestamp
$lastUpdated = $tileResult['last_updated'];
$lastUpdateTime = strtotime($lastUpdated);

// current time
$currentTime = time();

// calculate elapsed time
$elapsedTime = $currentTime - $lastUpdateTime;

// calculate if crop grew
$fullIntervals = floor($elapsedTime / INTERVAL);

// calculate new timestamp
$secondsToAdd = $fullIntervals / INTERVAL;
$newTime = $lastUpdateTime + $secondsToAdd;

// update user data
$beetToAdd = $fullIntervals * QUANTITY;
$newLastUpdated = date("Y-m-d H:i:s", $newTime);

// add beet in db
$stmt = $connectionResult->prepare("UPDATE user_data SET Beet = Beet + :beet WHERE user_id = :userid");
$stmt->execute([':beet' => $beetToAdd, ':userid' => $userid]);

// response
$stmt = $connectionResult->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
$stmt->execute([':user_id' => $userid]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
$userData = new stdClass;
$response->userData = MakeUserData($result, $userData);
$response->status = "beetAdded";
$response->customMessage = "$userid";

function MakeUserData($result, $userData) {
    foreach ($result as $key => $value) {
        if ($key != 'user_id') {
            $userData->$key = $value;
        }
    }
}
die(json_encode($response));
