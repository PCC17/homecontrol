<?php

    require ("../dbaccess.php");

    $turnOnCommand = "#001";
    $turnOffCommand = "#002";

    $response = array("success" => 0);
    if(isset($_POST["token"]) && isset($_POST["name"]) && isset($_POST["status"]))
    {
	
        $token = $_POST["token"];
        $name = $_POST["name"];
        $status = $_POST["status"];

        if($token=="123321")
        {
            $server_port = 51234;
            $message = $turnOnCommand;
            if($status == 0)
                $message     = $turnOffCommand;
            if ($socket = socket_create(AF_INET, SOCK_DGRAM, 0)) {
                socket_sendto($socket, $message, strlen($message), 0, getIpAddressOfStation($name)["ip_address"], $server_port);
                $response["success"] = 1;
            }
        }
	echo json_encode($response);
    }

    if(isset($_GET["get"]) && $_GET["get"] == 1)
    {
        global $db;
        $sql = "Select name, ip_address from lights";
        $stmt = $db->prepare($sql);
        $stmt->execute();
        $result = $stmt->fetch(PDO::FETCH_ASSOC);
        echo json_encode($result);
    }

    function getIpAddressOfStation($name)
    {
        global $db;
        $sql = "Select ip_address from lights Where name = '".$name."'";
        $stmt = $db->prepare($sql);
        $stmt->execute();
        $result = $stmt->fetch(PDO::FETCH_ASSOC);
        return $result;
    }

?>
