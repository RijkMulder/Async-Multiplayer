<?php
$email = $request->email;
$username = $request->username;
$password = password_hash($request->password, PASSWORD_DEFAULT);

// check if email is valid
if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
    $response->status = "invalidEmail";
    $response->customMessage = "email is not valid";
    die (json_encode($response));
}

// check if email exists
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE email = :email");
$stmt->bindparam(":email", $email);
$stmt->execute();

$results = $stmt->fetchAll(PDO::FETCH_ASSOC);
if (count($results) > 0) {
    $response->status = "emailAlreadyExists";
    $response->customMessage = "Email already exists";
    die (json_encode($response));
}

// check if username exists
$stmt = $connectionResult->prepare("SELECT * FROM users WHERE username = :username");
$stmt->bindparam(":username", $username);
$stmt->execute();

$results = $stmt->fetchAll(PDO::FETCH_ASSOC);
if (count($results) > 0) {
    $response->status = "usernameAlreadyExists";
    $response->customMessage = "Username already exists";
    die (json_encode($response));
}

// check username length
if (strlen($username) < 5 || strlen($username) > 20) {
    $msg = strlen($username) > 20 ? "long" : "short";
    $response->status = "usernameLength";
    $response->customMessage = "The username is too $msg";
    die (json_encode($response));
}

// insert new account into database
$stmt = $connectionResult->prepare("INSERT INTO users (email, username, password_hash) VALUES (:email, :username, :password_hash)");
$stmt->bindParam(":email", $email);
$stmt->bindParam(":username", $username);
$stmt->bindParam(":password_hash", $password);
$stmt->execute();

// account made succesfully
$token = generateToken();
include 'login.php';

