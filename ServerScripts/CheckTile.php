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

// tile doesnt exist
if ($results == false) {
    $response->status = "doesntmatter";
    $response->customMessage = "blablabla";
    die(json_encode($response));
}

// tile already exists
$response->status = "tileExists";
$response->customMessage = "This tile already has an occupent";
die(json_encode($response));