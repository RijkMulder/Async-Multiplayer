<?php
// get user
$userid = getUser($connectionResult, $request);

// try find tile in db
$tile = $request->tile;
$tolerance = 0.01;
$stmt = $connectionResult->prepare(
    "SELECT * FROM user_tiles WHERE user_id = :user_id 
    AND ABS(tile_pos_x - :posX) < :tolerance 
    AND ABS(tile_pos_y - :posY) < :tolerance"
);
$stmt->execute([
    ':user_id' => $userid, 
    ':posX' => $tile->posX, 
    ':posY' => $tile->posY,
    ':tolerance' => $tolerance
]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);

// tile doesnt exist
if ($result == false) {
    $response->status = "doesntmatter";
    $response->customMessage = "posx: $tile->posX, posy: $tile->posY";
    die(json_encode($response));
}

// tile already exists
$response->status = "tileExists";
$response->customMessage = "This tile already has an occupent";
die(json_encode($response));