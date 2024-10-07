<?php
// send plot
$response->plot = "10,10";
$response->status = "plot";
$response->customMessage = "empty plot sent";
die(json_encode($response));

