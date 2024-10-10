<?php
// get user
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE token = :token");
$stmt->execute([':token' => $request->token]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
$userid = $result['user_id'];

// try find tile in db
$tile = $request->tile;
$stmt = $connectionResult->prepare("SELECT * FROM user_tiles WHERE user_id = :user_id AND tile_pos_x = :posX AND tile_pos_y = :posY");
$stmt->execute([':user_id' => $userid, ':posX' => $tile->posX, ':posY' => $tile->posY]);
$results = $stmt->fetchAll(PDO::FETCH_ASSOC);

// create new entry
if ($results == false) {
    $stmt = $connectionResult->prepare("INSERT INTO user_tiles (tile_pos_x, tile_pos_y, tile_type,  user_id) VALUES (:posX, :posY, :tiletype, :user_id)");
    $stmt->execute([
        ':posX' => $tile->posX,
        ':posY' => $tile->posY,
        ':tiletype' => $tile->tileType,
        ':user_id' => $userid
    ]);
    $response->status = "tileInserted";
    $response->customMessage = "new tile inserted in database";
    die(json_encode($response));
}

// tile already exists
$response->status = "tileExists";
$response->customMessage = "This tile already has an occupent";
die(json_encode($response));