<?php
// get user
$userid = getUser($connectionResult, $request, "user_id");

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
$tileResult = $stmt->fetch(PDO::FETCH_ASSOC);

// tile doesnt exist
if ($tileResult == false) {
    // check if request info is empty
    if ($request->tile->tileType == null) {
        $response->status = "emptyTileData";
        $response->customMessage = "tile data sent was empty.";
        $response->userData = GetUserData($connectionResult, $userid);
        die(json_encode($response));
    }
    // check price
    if (CheckPrice($connectionResult, $userid, $tile) == true) {
        // update gold
        UpdateGold($connectionResult, $userid, $tile);

        // send succes response
        $response->userData = GetUserData($connectionResult, $userid);
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

else {
    // tile already exists
    include 'GetGold.php';
}

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
    $stmt = $connectionResult->prepare("SELECT * FROM prices WHERE building_name = :building_name");
    $stmt->execute([':building_name' => $tile->tileType]);
    $result = $stmt->fetch(PDO::FETCH_ASSOC);
    $p = $result['price'];
    return $p;
}