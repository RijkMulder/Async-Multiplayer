<?php
// include connection check
include '../ConnectionCheck.php';

// check action type
switch ($request->action) {
    case "createAccount":
        include 'CreateAccount.php';  
        break;
    case "loginAccount":
        $token = generateToken();
        include 'Login.php';
        break;
    case "checkToken":
        include 'CheckLoginToken.php';
        break;
    case "logout":
        include 'Logout.php';
        break;
    case "resetPassword":
        include 'ResetPassword.php';
        break;
    default:
    $response->status = "noValidAction";
    $response->customMessage = "no valid action was given.";
    die (json_encode($response));
}

// generate login token
function generateToken($length = 64) {
    return bin2hex(random_bytes($length / 2));
}
