var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var counter = require('./pointCounter.js');
var fs = require('fs');

var clients = {};
var cards = {};
var turns = 0;
var theme = 1;
var tmp ;

var cards_id = [];


for (var i = 0; i < 40; i++) {
    cards_id.push(i);
};

var available_cards = JSON.parse(JSON.stringify(cards));
var discard_cars = {};

function loadDB(){

    
    tmp = JSON.parse(fs.readFileSync('./deck.json','utf-8'));
    tmp.forEach(function(line){
        //console.log(line);
        cards[line.id] = JSON.parse(JSON.stringify(line));
    });

}

function findCustomRooms() {
    var availableRooms = [];
    var rooms = io.sockets.adapter.rooms;
    if (rooms) {
        for (var room in rooms) {
            if (!rooms[room].sockets.hasOwnProperty(room)) {
                availableRooms.push(room);
            }
        }
    }
    return availableRooms;
}

function drawInitialCards (socket, number) {
    for (var i = 0; i < number; i++) {
        sendDrawCard(socket, i%4);
    };
}



function sendDrawCard (socket, playerId) {

        var rdmID = Math.floor(Math.random() * (Object.keys(cards).length-1));
        var keyId = Object.keys(cards)[rdmID];
        var tosend = {id : Number(cards[keyId].id), playerId : playerId};

        console.log(tosend);


        for (var id in clients){
            if(clients[id].id == playerId){
                clients[id].cards.push(Number(cards[keyId].id));
            }
        }

        socket.emit("DRAW_CARD", tosend);
        socket.broadcast.emit("DRAW_CARD", tosend);

       /* console.log(rdmID);
        console.log(cards_id.length);
        console.log(tosend);*/
        delete cards[keyId];
}


io.on('connection', function(socket){ 

	var currentUser;

	socket.on("HI ROOM",function(){

		socket.broadcast.to("pippo").emit("HI");


	});

	socket.on("USER_CONNECT", function (data){
		console.log("-----On USER_CONNECT-----");
		console.log("User Connected ");
		console.log(socket.client.id);


        clients[socket.client.id] = {};
        clients[socket.client.id].name = data.name;
        clients[socket.client.id].id = Object.keys(clients).indexOf(socket.client.id);
        clients[socket.client.id].cards = [];

        socket.emit("ASSIGN_ID", {id : Object.keys(clients).indexOf(socket.client.id)});

        if(Object.keys(clients).length == 4)
        {
            console.log("4 connections");
            socket.emit("INIT_GAME");
            socket.broadcast.emit("INIT_GAME");
            drawInitialCards(socket, 16);

            currentTurn = Math.floor(Math.random() * 4)
            socket.emit("CHANGE_TURN", {playerId: currentTurn});
            socket.broadcast.emit("CHANGE_TURN", { playerId: currentTurn });

        }
	});

    socket.on('EVERYONE_READY', function()
    {
        //initGame for everyone with their ID
        //give everyone cards (loop with draw function)
    });

  	/*socket.on('PLAY', function (data) {
  		console.log("-----On PLAY-----");
        console.log(data);
  		currentUser = {
  			name: data.name
  		};
  		clients[socket.client.id]= currentUser;
    	console.log("his name is " + currentUser.name);
    	socket.emit("USER_CONNECTED", currentUser);
    	socket.broadcast.emit("USER_CONNECTED", currentUser);

    	socket.emit("ROLL_DICE",{roll: Math.floor(Math.random() * 20)+""})
  	});*/



  	/*
	socket.on('disconnect', function(){
		console.log("-----On USER_DISCONNECTED-----");
		socket.broadcast.emit("USER_DISCONNECTED", currentUser);
		for (var i = 0; i < clients.length; i++) {
			if(clients[i].name === currentUser.name){
				console.log("User " + currentUser.name +  " disconnect");
				clients.splice(i,1);
			}
		}
    });
	*/
	
    socket.on("DRAW_CARDS", function(data){
    	console.log("-----On DRAW_CARDS-----");
    	console.log(data);
    	card_number = Number(data.number);
    	var send_card = {cards:[]};
    	for (var i = 0; i < card_number; i++) {
    		keys = Object.keys(available_cards);
    		do 
    			rand = keys[Math.floor(Math.random() * keys.length)];
    		while(available_cards[rand].type !== data.type)

    		console.log("Sending");
    		console.log(available_cards[rand])
    		send_card.cards.push(available_cards[rand]);
    		delete available_cards[rand];
    	}
    	console.log("--------SEND_CARDS-------");
    	console.log("Sending");
    	console.log(JSON.stringify(send_card));
    	socket.emit("SEND_CARDS", send_card);
    	socket.broadcast.to(clients[socket.id]).emit("SEND_CARDS", send_card);
    	console.log("--------SEND_CARDS_DONE-------");
    });


    socket.on("PLAY_CARD", function(data){
    	console.log("-----On PLAY_CARDS-----");
    	var card_ids = data.cardID.split(",");

        var playerId; 
        console.log("PlayedCards " + card_ids);

        var cards_toCheck = [];

        card_ids.forEach(function(card_id){

            for (var id in clients){
                if(clients[id].id == data.playerID){

                    playerId = data.playerID;

                    console.log("Played card id : " + card_id);

                    var indexCard = clients[id].cards.indexOf(Number(card_id));

                    console.log("Player id : " + clients[id].id);
                    console.log("Player cards : " + clients[id].cards)
                    console.log("indexCard : " + indexCard);


                    if(indexCard == -1){

                        socket.emit("INVALID_PLAY_CARD", {error: "invalid_card"});
                        socket.broadcast.emit("INVALID_PLAY_CARD", {error: "invalid_card"});
                        return;
                    }

                    cards_toCheck.push(cards[card_id]);

                }
            }
        });

        console.log(counter.checkLinks(cards_toCheck));
        console.log(cards_toCheck);

        if(counter.checkLinks(cards_toCheck)){
            var point = counter.countPoints(cards_toCheck, "horror");
            console.log(point);

            socket.emit("PLAY_CARD", point);
            socket.broadcast.emit("PLAY_CARD", point);
            card_ids.forEach(function(c){
                var indexCard = clients[playerId].cards.indexOf(Number(c));
                clients[id].cards.splice(indexCard, 1)
            })
        }
        else{
            socket.emit("INVALID_PLAY_CARD", {error: "invalid_link"});
            socket.broadcast.emit("INVALID_PLAY_CARD", {error: "invalid_card"});
        }

       
    });

    socket.on("LIST_ROOMS", function(){
    	console.log("LIST ROOM");
    	//console.log(JSON.stringify(io.sockets.adapter.rooms));
    	socket.emit("ROOMS",{rooms: findCustomRooms()})

    });

    socket.on("CREATE_ROOM", function(data){
    	console.log("CREATE ROOM");
    	//console.log(data);
    	socket.join(data.name);
    	clients[socket.id].room = data.name;
    	console.log(clients[socket.id]);
    	//console.log(JSON.stringify(io.sockets.adapter.rooms));
    	//console.log(findCustomRooms());

    });

    socket.on("JOIN_ROOM", function(data){
    	console.log("JOIN ROOM");
    	socket.join(data.name);
    	clients[socket.id].room = data.name;
    	console.log(clients[socket.id]);

    });

    socket.on("END_TURN", function(){

        var id = socket.client.id;
        var playerId = Object.keys(clients).indexOf(socket.client.id);

        console.log("player cards : " + clients[id].cards + "  amount : " + clients[id].cards.length);

        for (var i = clients[id].cards.length; i < 4; i++) {
            console.log("player id : " + playerId + "  card number : " + i);
            sendDrawCard(socket, playerId)
        };

        currentTurn = (currentTurn+1)%4;
        socket.emit("CHANGE_TURN", {playerId: currentTurn});
        socket.broadcast.emit("CHANGE_TURN", { playerId: currentTurn });

        turns += 1;
        if (turns == 8) {
            socket.emit("CHANGE_THEME", { theme: theme % 4 });            
            socket.broadcast.emit("CHANGE_THEME", { theme: theme % 4 });
            theme++;
            turns = 0;
        }

    });


});

loadDB();
//console.log(cards);

var port = process.env.port || 3000
server.listen(port, function(){
	console.log('listening on *:' + port);
});