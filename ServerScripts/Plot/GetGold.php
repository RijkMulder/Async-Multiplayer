<?php
// set constants
define("INTERVAL", 10);
define("QUANTITY", 2);
define("MAX", 10);

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
$secondsToAdd = $fullIntervals * INTERVAL;
$newTime = $lastUpdateTime + $secondsToAdd;

// update user data
$beetToAdd = max(0, min(MAX, $fullIntervals * QUANTITY));
$newLastUpdated = date("Y-m-d H:i:s", $newTime);

// add beet in db
$stmt = $connectionResult->prepare("UPDATE user_data SET beet = beet + :beet WHERE user_id = :userid");
$stmt->execute([':beet' => $beetToAdd, ':userid' => $userid]);

// upate time
$stmt = $connectionResult->prepare("UPDATE user_tiles SET last_updated = :new_last WHERE tile_id = :tile_id");
$stmt->execute([':new_last' => $newLastUpdated, ':tile_id' => $tileResult['tile_id']]);

// response
$stmt = $connectionResult->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
$stmt->execute([':user_id' => $userid]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
$userData = new stdClass;
if ($result === false) {
    $response->status = "noUserFound";
} else {
    $response->status = "beetAdded";
    $response->customMessage = "$beetToAdd beet added";
    $response->userData = MakeUserData($result, $userData);
}

function MakeUserData($result, $userData) {
    foreach ($result as $key => $value) {
        if ($key != 'user_id') {
            $userData->$key = $value;
        }
    }
    return $userData;
}
die(json_encode($response));
