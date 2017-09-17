<?php
    $host    = "10.0.0.61";
    $port    = 33123;
    $messageToServer = "5ebe2294ecd0e0f08eab7690d2a6ee69";

    $res = array("successful" => 1);

    if($_POST["token"]=="5ebe2294ecd0e0f08eab7690d2a6ee69")
    {
        try {
            $socket = socket_create(AF_INET, SOCK_STREAM, 0);
            $result = socket_connect($socket, $host, $port);
            socket_write($socket, $messageToServer, strlen($messageToServer));
            socket_close($socket);
        }
        catch (Exception $e) {
            $res["successful"] = 0;
        }
    }
    else
    {
        $res["successful"] = 0;
    }
    echo json_encode($res);
?>
