var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var counter = require('./pointCounter.js');
var fs = require('fs');

var clients = {};
var cards = {};
var turns = 0;
var theme = 0;
var themes = ['nature','horror','history','folklore'];
shuffleArray(themes);
var charactersAvailable = ['MARY_SHELLEY','THE_GRIM_BROTHERS','WILLIAM_WORDSWORTH','BETTINA_VON_ARMIN'];
shuffleArray(charactersAvailable);
var tmp ;

var available_cards;
var discard_cards=[];


console.log(themes);
console.log(charactersAvailable)

function loadDB(){

    
    tmp = JSON.parse(fs.readFileSync('./card_text.json','utf-8'));
    tmp.forEach(function(line){
        cards[line.id] = JSON.parse(JSON.stringify(line));
    });

     available_cards = JSON.parse(JSON.stringify(cards));
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
    }
}

function reShuffleDeck(){

    discard_cards.forEach(function(card){
            available_cards[card] = JSON.parse(JSON.stringify(cards[card]));
            var indexCard = discard_cards.indexOf(card);
            discard_cards.splice(indexCard, 1);
        });
}


function shuffleArray(a) {
    for (let i = a.length; i; i--) {
        let j = Math.floor(Math.random() * i);
        [a[i - 1], a[j]] = [a[j], a[i - 1]];
    }
}


function sendDrawCard (socket, playerId) {

        var rdmID;

        if(Object.keys(available_cards).length == 1){
            rdmID = Object.keys(available_cards).length - 1;

           reShuffleDeck();

        }else{

            rdmID = Math.floor(Math.random() * (Object.keys(available_cards).length-1));
        }
        var keyId = Object.keys(available_cards)[rdmID];
        var tosend = {id : Number(available_cards[keyId].id), playerId : playerId};
        

        console.log(tosend);


        for (var id in clients){
            if(clients[id].id == playerId){
                clients[id].cards.push(Number(available_cards[keyId].id));
            }
        }

        socket.emit("DRAW_CARD", tosend);
        socket.broadcast.emit("DRAW_CARD", tosend);

       /* console.log(rdmID);
        console.log(cards_id.length);
        console.log(tosend);*/
        delete available_cards[keyId];
}

function Power_Mary(socket, playerId)
{
    var pointsEarned = 0;

    for(var id in clients)
    {
        console.log("current player checked " + clients[id].id)

        var indexToRemove = [];

        if(clients[id].id != playerId)
        {
            for (var i = 0; i < 4; i++) {

                for (var y = cards[clients[id].cards[i]].theme.length - 1; y >= 0; y--) {

                    var thToCheck = cards[clients[id].cards[i]].theme[y];

                    if(thToCheck == "horror")
                    {

                        socket.emit("DISCARD_CARD", {playerID: clients[id].id, cardID: clients[id].cards[i]})
                        socket.broadcast.emit("DISCARD_CARD", {playerID: clients[id].id, cardID: clients[id].cards[i]})
                        pointsEarned++;

                        var indexCard = clients[id].cards.indexOf(Number(clients[id].cards[i]));
                        indexToRemove.push(indexCard);
                    }
                };
            };
        }

        for (var idToRmv = indexToRemove.length - 1; idToRmv >= 0; idToRmv--) {
            clients[id].cards.splice(indexToRemove[idToRmv], 1)
        };

    }
    
    UpdateScore(socket, playerId, pointsEarned);

    socket.emit("REFILL_HAND");
    socket.broadcast.emit("REFILL_HAND");

}

function UpdateScore(socket, playerID, points)
{
        clients[socket.client.id].score = clients[socket.client.id].score + points;
        clients[socket.client.id].ink = clients[socket.client.id].ink + points;

        if(clients[socket.client.id].ink > 16)
        {
           clients[socket.client.id].ink = 16; 
        }

        socket.emit("UPDATE_SCORE", {playerID : clients[socket.client.id].id, totalPoints : clients[socket.client.id].score, ink : clients[socket.client.id].ink });
        socket.broadcast.emit("UPDATE_SCORE", {playerID : clients[socket.client.id].id, totalPoints : clients[socket.client.id].score, ink : clients[socket.client.id].ink });

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
        clients[socket.client.id].score = 0;
        clients[socket.client.id].ink = 0;
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

            socket.emit("CHANGE_THEME", { theme: themes[0] });            
            socket.broadcast.emit("CHANGE_THEME", { theme: themes[0]});

            socket.emit("ASSIGN_CHARACTER", {chars: charactersAvailable})
            socket.broadcast.emit("ASSIGN_CHARACTER", {chars: charactersAvailable})

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

                    playerId = id;

                    console.log("Played card id : " + card_id);

                    var indexCard = clients[id].cards.indexOf(Number(card_id));

                    console.log("Player id : " + clients[id].id);
                    console.log("Player cards : " + clients[id].cards)
                    console.log("indexCard : " + indexCard);


                    if(indexCard == -1){

                        socket.emit("INVALID_PLAY_CARD", {error: "invalid_card", playerID : clients[playerId].id});
                        socket.broadcast.emit("INVALID_PLAY_CARD", {error: "invalid_card", playerID : clients[playerId].id});
                        return;
                    }

                    console.log("CARD TO CHECK");
                    console.log(card_id);
                    //console.log(cards);
                    console.log(cards[card_id]);
                    cards_toCheck.push(cards[card_id]);

                }
            }
        });
        
        console.log("cards_toCheck");
        console.log(cards_toCheck); 
        console.log("themes " + themes);
        console.log("theme " + theme);
        console.log("Current Theme " + themes[theme]);
        console.log(counter.checkLinks(cards_toCheck));

        if(counter.checkLinks(cards_toCheck)){
            console.log("CHECK POINT WITH " + themes[theme%4]);
            var point = counter.countPoints(cards_toCheck, themes[theme%4]);

            socket.emit("PLAY_CARD", {cards : card_ids.join(), playerID : clients[playerId].id});
            socket.broadcast.emit("PLAY_CARD", {cards : card_ids.join(), playerID : clients[playerId].id});

            UpdateScore(socket, clients[playerId].id, point);

            console.log("clients[playerId].cards");
            console.log(clients[playerId].cards);

            card_ids.forEach(function(c){
                var indexCard = clients[playerId].cards.indexOf(Number(c));
                clients[playerId].cards.splice(indexCard, 1)
            });

            console.log("clients[playerId].cards");
            console.log(clients[playerId].cards);

            console.log("discard_cards");
            console.log(discard_cards);

            card_ids.forEach(function(card_id){
                discard_cards.push(card_id);
            });

            console.log("discard_cards");
            console.log(discard_cards);
        }
        else{
            socket.emit("INVALID_PLAY_CARD", {error: "invalid_link", cards: card_ids, playerID : clients[playerId].id});
            socket.broadcast.emit("INVALID_PLAY_CARD", {error: "invalid_card", cards: card_ids, playerID : clients[playerId].id});
        }

       
    });

    socket.on("USE_POWER", function(data){

        console.log("power received");
        console.log(data);

        switch(data["Power_Name"]) {
    case "BETT":
        break;
    case "WILL":
        break;
    case "GRIM":
        break;
    case "MARY":
        Power_Mary(socket, data["player_ID"]);
        break;
    default:
        break;
    } 

    });

    socket.on("SHAME", function(data)
    {
        var player_ID = data["Player_ID"];
        var Opponent_ID = data["Opponent_ID"];
        var Theme = data["Theme"];
        var earnedPoints = data["EarnedPoints"];
        var cost = data["Cost"];

        var ptHorr = 0, ptFolk = 0, ptHist = 0, ptNat = 0;

        console.log ("SHAME  >>> PlayerID : " + player_ID + "  OpponentID : " + Opponent_ID + "  Theme : "  + Theme);

        for(var id in clients)
        {
            if(clients[id].id == Opponent_ID)
            {
                var savedClientID = id;

                for (var i = 0; i < 4; i++) {

                    for (var y = cards[clients[id].cards[i]].theme.length - 1; y >= 0; y--) {

                        var thToCheck = cards[clients[id].cards[i]].theme[y];
                        var cardPoint = parseInt(cards[clients[id].cards[i]].point);

                        switch(thToCheck)
                        {
                            case "horror":
                                ptHorr += cardPoint;
                                break;
                            case "folklore":
                                ptFolk += cardPoint;
                                break;
                            case "history":
                                ptHist += cardPoint;
                                break;  
                            case "nature":
                                ptNat += cardPoint;
                                break;
                        }
                    };
                };
            }
        }

        console.log ("horror = " + ptHorr+ " folklore = " + ptFolk+  " history = " +ptHist + " nature = " + ptNat);

       // var dictPoints = {parseInt(ptHorr): "horror", parseInt(ptFolk): "folklore", parseInt(ptHist): "history", parseInt(ptNat): "nature"};
        var highestThemes = [];
        var dictPoints = {"horror" : ptHorr, "folklore" : ptFolk, "history" : ptHist, "nature" : ptNat};
        var bestTheme = Object.keys(dictPoints).reduce(function(a, b){ return dictPoints[a] > dictPoints[b] ? a : b });
        var bestResult = dictPoints[bestTheme];
        
        highestThemes.push(bestTheme);
        delete dictPoints[bestTheme];

        console.log("bestTheme = " +bestTheme);

        console.log("____");


        for(var y = Object.keys(dictPoints).length - 1; y >= 0; y--)
        {
            var tmpBestTheme = Object.keys(dictPoints).reduce(function(a, b){ return dictPoints[a] > dictPoints[b] ? a : b }); 
            var tmpBestResult = dictPoints[tmpBestTheme];
            console.log("tmpBest = " +tmpBestTheme);
            if(bestResult == tmpBestResult)
            {
                highestThemes.push(tmpBestTheme);
                delete dictPoints[tmpBestTheme];
            } 
        }

        console.log("____");


        for (var i = highestThemes.length - 1; i >= 0; i--) {
            if(Theme == highestThemes[i])
            {
                console.log(highestThemes[i]);

                var indexToRemove = [];

                for (var i = clients[savedClientID].cards.length - 1; i >= 0; i--) {
                    socket.emit("DISCARD_CARD", {playerID: clients[savedClientID].id, cardID: clients[savedClientID].cards[i]})
                    socket.broadcast.emit("DISCARD_CARD", {playerID: clients[savedClientID].id, cardID: clients[savedClientID].cards[i]})

                    //var indexCard = clients[savedClientID].cards.indexOf(Number(clients[savedClientID].cards[i]));
                    //indexToRemove.push(indexCard);

                    //console.log(indexToRemove);
                };

               /* for (var idToRmv = indexToRemove.length - 1; idToRmv >= 0; idToRmv--) {
                    clients[savedClientID].cards.splice(indexToRemove[idToRmv], 1)
                    console.log( clients[savedClientID].cards);
                };*/
                clients[savedClientID].cards = [];
                clients[socket.client.id].score = clients[socket.client.id].score + parseInt(earnedPoints);

                break;
            }
        };

        clients[socket.client.id].ink = clients[socket.client.id].ink - parseInt(cost);
        UpdateScore(socket, player_ID, 0);

        socket.emit("REFILL_HAND");
        socket.broadcast.emit("REFILL_HAND");


        //var thPoints = { ptHorr : "horror", ptFolk: "folklore", ptHist: "history", ptNat: "nature"};
        


                        /*if(thToCheck == "horror")
                        {

                            socket.emit("DISCARD_CARD", {playerID: clients[id].id, cardID: clients[id].cards[i]})
                            socket.broadcast.emit("DISCARD_CARD", {playerID: clients[id].id, cardID: clients[id].cards[i]})
                            pointsEarned++;

                            var indexCard = clients[id].cards.indexOf(Number(clients[id].cards[i]));
                            indexToRemove.push(indexCard);
                        }*/



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
            theme++;
            socket.emit("CHANGE_THEME", { theme: themes[theme % 4] });            
            socket.broadcast.emit("CHANGE_THEME", { theme: themes[theme %4]});
            turns = 0;
        }

    });

    socket.on("ASK_FOR_CARDS", function()
    {
        var id = socket.client.id;
        var playerId = Object.keys(clients).indexOf(socket.client.id);

        for (var i = clients[id].cards.length; i < 4; i++) {
            sendDrawCard(socket, playerId)
        };
    });


});

loadDB();
//console.log(cards);

var port = process.env.port || 3000
server.listen(port, function(){
	console.log('listening on *:' + port);
});