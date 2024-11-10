<?php
// get user
$userid = getUser($connectionResult, $request, "user_id");

// get all tiles fomr user
$stmt = $connectionResult->prepare("SELECT * FROM user_tiles WHERE user_id = :user_id");
$stmt->execute([':user_id' => $userid]);
$tiles = $stmt->fetchAll(PDO::FETCH_ASSOC);

// send plot
$response->plotSize = "10,10";
$response->status = "plot";
$response->customMessage = "empty plot sent";

// get tile data
$responseTiles = [];
for ($i = 0; $i < count($tiles); $i++) {
    $tileData = new stdClass();
    $tileData->posX = $tiles[$i]['tile_pos_x'];
    $tileData->posY = $tiles[$i]['tile_pos_y'];
    $tileData->tileType = $tiles[$i]['tile_type'];
    $tileData->lastUpdate = $tiles[$i]['last_updated'];
    $responseTiles[$i] = $tileData;
}
$response->tiles = $responseTiles;

// get user data or make new
$userData = new stdClass();
$stmt = $connectionResult->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
$stmt->execute([':user_id' => $userid]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);

// data doesnt exist, make new
if ($result == false) {
    $userData->gold = 50;
    $stmt = $connectionResult->prepare("INSERT INTO user_data (user_id) VALUES (:user_id)");
    $stmt->execute([':user_id' => $userid]);
}
else {
    foreach ($result as $key => $value) {
        if ($key != 'user_id') {
            $userData->$key = $value;
        }
    }
}
$response->userData = $userData;
die(json_encode($response));
