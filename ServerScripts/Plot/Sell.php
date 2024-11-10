<?php
// get userID
$userid = GetUser($connectionResult, $request, "user_id");

// get type
$type = $request->type;

// get price
$stmt = $connectionResult->prepare("SELECT * FROM prices WHERE building_name = :building_name");
$stmt->execute([':building_name' => $type]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
$price = $result['price'];

// get resource amount
$stmt = $connectionResult->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
$stmt->execute([':user_id' => $userid]);
$result = $stmt->fetch(PDO::FETCH_ASSOC);
$resourceAmount = $result[$type];

// calculate money to get
$moneyToGet = $price * $resourceAmount;

// update money amount
$stmt = $connectionResult->prepare("UPDATE user_data SET $type = 0, gold = gold + :money_to_add WHERE user_id = :user_id");
$stmt->execute([':money_to_add' => $moneyToGet, 'user_id' => $userid]);

// response
$response->status = "moneyAdded";
$response->customMessage = "$moneyToGet has been added to account.";
$response->userData = GetUserData($connectionResult, $userid);
die(json_encode($response));
