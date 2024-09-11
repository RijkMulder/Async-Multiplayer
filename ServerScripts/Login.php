<?php
$response = new stdClass();
$request = json_decode($_POST['newEntry']);

// check if incoming packet is valid
if (!isset($_POST['newEntry'])) {
    $response->status = "invalidPostData";
    $response->customMessage = "no valid data given in post array.";
    die(json_encode($response));
}

// if json has errors
if (json_last_error() != JSON_ERROR_NONE) {
    $response->status = "invalidJson";
    $response->customMessage = "provided json data could not be decoded: " . json_last_error_msg();
    die (json_encode($response));
}

$connectionResult = getDatabaseConnection();

// connection error
if (is_array($connectionResult) && isset($connectionResult['error']) && $connectionResult['error']) {
    $response->status = "dataBaseConnectionError";
    $response->customMessage = $connectionResult['message'];
    die(json_encode($response));
}

// invalid action
if (!isset($request->action) || empty($request->action)) {
    $response->status = "invalidAction";
    $response->customMessage = "The field 'action' is missing or empty in the request.";
    die(json_encode($response));
}

// request action switch
switch ($request->action) {
    case "createAccount":
        createAccount($request, $response);
        break;
    case "loginAccount":
        loginAccount($request, $response);
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

// createAccount
function createAccount($request, $response) {
    // default response
    $response->status = "no status yet";
    $response->customMessage = "no message yet";
    die(json_encode($response));

    // add new account to database
    $email = $request['email'];
    $username = $request['username'];
    $password = "";
}

// loginAccount
function loginAccount($request, $response) {

}
function getDatabaseConnection() {
    $dbHost = 'localhost';
    $dbName = 'example';
    $dbUser = 'root';
    $dbPass = '';

    try {
        // try to make connection with database
        $pdo = new PDO("mysql:host=$dbHost;dbname=$dbName", $dbUser, $dbPass);
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        return $pdo;
    } catch (PDOException $e) {
        // return errors if connection failed
        return [
            'error' => true,
            'message' => $e->getMessage()
        ];
    }
}
