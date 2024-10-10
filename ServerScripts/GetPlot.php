<?php
// get user
$userid = getUser($connectionResult, $request);

// get all tiles fomr user
$stmt = $connectionResult->prepare("SELECT * FROM user_tiles WHERE user_id = :user_id");
$stmt->execute([':user_id' => $userid]);
$tiles = $stmt->fetchAll(PDO::FETCH_ASSOC);

// send plot
$response->plotSize = "10,10";
$response->status = "plot";
$response->customMessage = "empty plot sent";

$responseTiles = [];
for ($i = 0; $i < count($tiles); $i++) {
    $tileData = new stdClass();
    $tileData->posX = $tiles[$i]['tile_pos_x'];
    $tileData->posY = $tiles[$i]['tile_pos_y'];
    $tileData->tileType = $tiles[$i]['tile_type'];
    $responseTiles[$i] = $tileData;
}
$response->tiles = $responseTiles;
die(json_encode($response));
