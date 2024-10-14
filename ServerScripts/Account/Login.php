<?php
 
// check if email exists
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE email = :email");
$stmt->bindparam(":email", $request->email);
$stmt->execute();

$result = $stmt->fetch(PDO::FETCH_ASSOC);
if ($result == false) {
    $response->status = "emailNotInDatabase";
    $response->customMessage = "The given email: $request->email is not in the database";
    die (json_encode($response));
}

// check password
$hash = $result['password_hash'];
if (password_verify($request->password, $hash)) {
    // update token in database
    $stmt = $connectionResult->prepare("UPDATE users SET token = :token WHERE user_id = :id");
    $stmt->bindparam(":token", $token);
    $stmt->bindparam(":id", $result['user_id']);
    $stmt->execute();
    
    // send succes response to unity
    $response->status = "loginSuccesfull";
    $response->customMessage = "succesfully logged in";
    $response->token = $token;
    die(json_encode($response));
}
// incorrect password
else {
    $response->status = "invalidPassword";
    $response->customMessage = "Given password is incorrect for this account";
    die(json_encode($response));
}

