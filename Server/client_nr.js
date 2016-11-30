var io_client = require('socket.io-client');
var ipAddress = "localhost";
var port = "3000";

var socket = io_client.connect('http://' + ipAddress + ':' + port);
socket.emit('USER_CONNECT');
socket.emit('PLAY',{"name" : "NODE no room"});