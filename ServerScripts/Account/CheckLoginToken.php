<?php
$token = $request->token;

// try get user with this token
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE token = :token");
$stmt->bindparam(":token", $token);
$stmt->execute();

// check if user exists
$result = $stmt->fetch(PDO::FETCH_ASSOC);
if ($result == false) {
    $response->status = "wrongToken";
    $response->customMessage = "this token is not in database";
    die (json_encode($response));
}

// token found, login
$response->status = "tokenFound";
$response->customMessage = "login token is found on device";
die (json_encode($response));


