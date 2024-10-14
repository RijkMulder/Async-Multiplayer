<?php
$token = $request->token;
$newToken = null;

// destroy token
$stmt = $connectionResult->prepare("UPDATE users SET token = :new_token WHERE token = :old_token");
$stmt->bindparam(":old_token", $token);
$stmt->bindparam(":new_token", $newToken);
$stmt->execute();

$response->status = "loggedOut";
$response->customMessage = "succesfully logged out";
die (json_encode($response));