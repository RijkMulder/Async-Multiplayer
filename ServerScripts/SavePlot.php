<?php
// get user
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE token = :token");
$stmt->execute([':token' => $request->token]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);

// give new plot
$userid = $result['user_id'];
$plot = $request->plot;
$stmt = $connectionResult->prepare("SELECT * FROM user_plots WHERE user_id = :id");
$stmt->execute([':id' => $userid]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
if ($result == null) {
    // insert new plot
    $stmt = $connectionResult->prepare("INSERT INTO user_plots (user_id, plot_layout) VALUES (:user_id, :plot_layout)");
    $stmt->execute([':user_id' => $userid, ':plot_layout' => json_encode($plot)]);

    // insert all tiles
    $plotobj = json_decode($plot);
    foreach ($plotobj->tiles as $tile) {
        $stmt = $connectionResult->prepare("INSERT INTO user_tiles (user_id, tile_position, tile_coord, tile_occupent) VALUES (:user_id, :tile_position, :tile_coord, :tile_occupent)");
        $stmt->execute([':user_id' => $userid, ':tile_position' => json_encode($tile->position), ':tile_coord' => json_encode($tile->coord), ':tile_occupent' => json_encode($tile->tileOccupent)]);
    }

    $response->status = "plotSaved";
    $response->customMessage = "plot has been saved.";
    die(json_encode($response));
}

// save plot and tiles
else {
    // save plot
    $stmt = $connectionResult->prepare("UPDATE user_plots SET plot_layout = :plot_layout WHERE user_id = :user_id");
    $stmt->execute([':user_id' => $userid, ':plot_layout' => json_encode($request->plot)]);

    // save tiles
    $plotobj = json_decode($plot);
    $stmt = $connectionResult->prepare("SELECT * FROM user_tiles WHERE user_ide = :user_id");
    $stmt->execute([':user_id' => $userid]);
    $results = $stmt->fetchAll(PDO::FETCH_ASSOC);
    for ($i = 0; $i < $results.count; $i++) {
        $tile = $plotobj->tiles[$i];
        $stmt = $connectionResult->prepare("UPDATE user_tiles SET tile_occupent = :tile_occupent WHERE tile_coord = :tile_coord");
        $stmt->execute([':tile_occupent' => json_encode($tile->occupent)]);
    }

    $response->status = "plotUpdated";
    $response->customMessage = "user plot updated";
    die(json_encode($response));
}