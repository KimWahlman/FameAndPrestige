var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);

var clients = {};
var cards = {};

cards["0"]= {
    name: "0",
    description: "1",
    type: "word"
}

cards["1"]= {
	name: "1",
	description: "1",
	type: "word"
}
cards["2"]= {
	name: "2",
	description: "2",
	type: "word"
}
cards["3"]= {
	name: "3",
	description: "3",
	type: "word"
}
cards["4"]= {
	name: "4",
	description: "4",
	type: "action"
}
cards["5"]= {
	name: "5",
	description: "5",
	type: "event"
}

var cards_id = [];


for (var i = 0; i < 40; i++) {
    cards_id.push(i);
};

var available_cards = JSON.parse(JSON.stringify(cards));
var discard_cars = {};

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

        var rdmID = Math.floor(Math.random() * (cards_id.length-1));
        var tosend = {id : cards_id[rdmID], playerId : playerId};


        for (var id in clients){
            if(clients[id].id == playerId){
                clients[id].cards.push(cards_id[rdmID]);
            }
        }

        socket.emit("DRAW_CARD", tosend);
        socket.broadcast.emit("DRAW_CARD", tosend);

       /* console.log(rdmID);
        console.log(cards_id.length);
        console.log(tosend);*/
        cards_id.splice(rdmID, 1);
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
    	card_id = data.cardID;

        console.log("Played card id : " + card_id);

        for (var id in clients){
            if(clients[id].id == data.playerID){

                var indexCard = clients[id].cards.indexOf(Number(card_id));

                console.log("Player id : " + clients[id].id);
                console.log("Player cards : " + clients[id].cards)
                console.log("indexCard : " + indexCard);


                if(indexCard != -1)
                {
                    socket.emit("PLAY_CARD", data);
                    socket.broadcast.emit("PLAY_CARD", data);
                    clients[id].cards.splice(card_id, 1);
                }
                else
                {
                    socket.emit("INVALID_PLAY_CARD", data);
                    socket.broadcast.emit("INVALID_PLAY_CARD", data);
                }
            }
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
        if(clients[id].cards.length<4){
            /*Draw Cads function*/
            console.log("less then 4 cards")
        }
        console.log("NEW TURN");
        currentTurn = (currentTurn+1)%4;
        socket.emit("PLAYER_TURN", {playerId: currentTurn});
        socket.broadcast.emit("PLAYER_TURN", {playerId: currentTurn});

    });


});
server.listen(3000, function(){
	console.log('listening on *:3000');
});