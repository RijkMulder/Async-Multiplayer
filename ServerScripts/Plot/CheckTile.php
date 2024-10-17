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
    // check price
    if (CheckPrice($connectionResult, $userid, $tile) == true) {
        // update gold
        UpdateGold($connectionResult, $userid, $tile);

        // get user data
        $userdata = new stdClass();
        $userdata->gold = GetUserData($connectionResult, $userid)['gold'];

        // send succes response
        $response->userData = $userdata;
        $response->status = "tileFree";
        $response->customMessage = "succesfully placed building";
        die(json_encode($response));
    }
    else {
        $response->status = "notEnoughGold";
        $response->customMessage = "user doesn't have enough gold to buy this building.";
        die(json_encode($response));
    }
}

// tile already exists
$response->status = "tileExists";
$response->customMessage = "This tile already has an occupent";
die(json_encode($response));

function CheckPrice($connectionResult, $userid, $tile) {
    // get current gold amount
    $gold = GetGoldAmnt($connectionResult, $userid);

    // get price
    $price = GetPriceAmnt($connectionResult, $tile);
    
    // check if enough gold
    return $gold >= $price ? true : false;
}

function UpdateGold($connectionResult, $userid, $tile) {
    // calculate new gold
    $newGold = GetGoldAmnt($connectionResult, $userid) - GetPriceAmnt($connectionResult, $tile);
    
    // update gold
    $stmt = $connectionResult->prepare("UPDATE user_data SET gold = :new_gold WHERE user_id = :user_id");
    $stmt->execute([':new_gold' => $newGold, ':user_id' => $userid]);
}
function GetGoldAmnt($connectionResult, $userid) {
    $stmt = $connectionResult->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
    $stmt->execute([':user_id' => $userid]);
    $result = $stmt->fetch(PDO::FETCH_ASSOC);
    $g = $result['gold'];
    return $g;
}
function GetPriceAmnt($connectionResult, $tile) {
    $stmt = $connectionResult->prepare("SELECT * FROM building_prices WHERE building_name = :building_name");
    $stmt->execute([':building_name' => $tile->tileType]);
    $result = $stmt->fetch(PDO::FETCH_ASSOC);
    $p = $result['price'];
    return $p;
}
function GetUserData($connectionResult, $userid) {
    $stmt = $connectionResult->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
    $stmt->execute([':user_id' => $userid]);
    $result = $stmt->fetch(PDO::FETCH_ASSOC);
    return $result;
}