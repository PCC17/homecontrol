<?php
/**
 * Created by PhpStorm.
 * User: paulc
 * Date: 27.06.2017
 * Time: 15:03
 */

require ("../dbaccess.php");

$res =array("success" => 0);
if(isset($_POST["modi"]) && $_POST["modi"] == 1)
{
echo $_POST["name"];
echo $_POST["ip_address"];

    if(addLight($_POST["name"], $_POST["ip_address"]));
        $res["success"] = 1;
    json_encode($res);
}

if(isset($_POST["modi"]) && $_POST["modi"] == 2)
{
    if(removeLight($_POST["ip_address"]));
        $res["success"] = 1;
    json_encode($res);
}

if(isset($_POST["modi"]) && $_POST["modi"] == 3)
{
    if(removeAllLights());
        $res["success"] = 1;
    json_encode($res);
}

function addLight($name, $ip){
    global $db;
    $sql = "Insert into lights (name, ip_address) Values('" . $name . "', '".$ip."')";
    echo $sql;
    $stmt = $db->prepare($sql);
    $res = $stmt->execute();

    echo $res;
    return $res;
}

function removeLight($ip){
    global $db;
    $sql = "Delete lights Where ip_address = '".$ip."'";
    $stmt = $db->prepare($sql);
    $res = $stmt->execute();
    return $res;
}

function removeAllLights(){
    global $db;
    $sql = "TRUNCATE TABLE lights;";
    $stmt = $db->prepare($sql);
    $res = $stmt->execute();
    return $res;
}