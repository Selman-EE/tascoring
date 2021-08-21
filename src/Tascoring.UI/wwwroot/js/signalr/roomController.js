'use strict';

const url = `/scoring-hub?roomId=${$('input#roomId').val()}&user=${$('input#userid').val()}&displayName=${$('input#displayName').val()}&avatarUrl=${$('input#avatarUrl').val()}`;
const connection = new signalR.HubConnectionBuilder()
	.withUrl(url, { transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling })
	.withAutomaticReconnect([0, 1000, 3000, 5000])
	.build();

connection.serverTimeoutInMilliseconds = 60 * 1000; // 60 second
// SignalR 1.1: interval at which the client will send pings to the server to keep the server from closing the connection
connection.keepAliveIntervalInMilliseconds = 10 * 1000; // 10 seconds

// other users in room
addOtherUsersInRoom(connection);
// remove user from room when user disconnected
removeUserFromRoom(connection);
//    
//listen test
read(connection);

//listen to New Person joined to room
newPersonJoinedToRoom(connection);

//
//hub start
connection
	.start()
	.then(() => {
		//console.log(connection);
		//console.log(connection.connectionId); //"BzwXBavk6fFvsTN0csH8Sg"		
		//console.log(connection.connectionStarted); //true
		console.log('Connection started');
		invoke(connection);
	})
	.catch(err => console.log('Error while starting connection: ' + err))

// When connection crush
function restart() {
	try {
		setTimeout(() => connection.start(), 500);
		console.log("Connected after close / First try");
	} catch (err) {
		console.log(err);
		setTimeout(() => connection.start(), 1000);
		console.log("Connected after close  / Second try");
	}
};
//
connection.onclose(restart);



function addOtherUsersInRoom(connection) {
	connection.on('AddOtherUsersInRoom', function (response) {
		console.log(response);
		for (var i = 0; i < response.length; i++) {
			var user = response[i];
			$users.append(addUserJoinedToRoom(user));
		}
	});
}

function removeUserFromRoom(connection) {
	connection.on('RemoveUsersInRoom', function (userId) {
		console.log("user removed to room.");
		$getUser(userId).remove();
	});
}


const $users = $('div#users div.player-list');
function newPersonJoinedToRoom(connection) {
	connection.on('newPersonJoinedToRoom', function (response) {
		console.log("New Person joined to room.");
		if (isTheUserInTheRoom(response.userId) === false) {
			var addUser = addUserJoinedToRoom(response);
			$users.append(addUser);
		}
		//	avatarUrl: "/avatar/72675.jpg"
		//displayName: "See"
		//roomId: "b48511f42d5740fe9bcc5e98048a3d1e"
		//userId: 72675
		//username: "See"		
	});
}

const $getUser = function (userId) {
	return $('div#users div[user-id*="' + userId + '"]');
}

const isTheUserInTheRoom = function (userId) {
	return $getUser(userId).length > 0;
}

const addUserJoinedToRoom = function (user) {
	return `<div class="player" user-id="${user.userId}"><div class="player-image"><img src="${user.avatarUrl}" /></div><div class="player-name">${user.displayName}</div></div>`;
}

const checkSquare = '<div class="score-checked"><i class="fa fa-check-square-o" aria-hidden="true"></i></div>';
const addCheckSquareToUser = function (userId) {
	var $userDiv = $(`'div#users div[user-id*="${userId}"]'`);
	var userName = $userDiv.text();
	userName = cleanUserName(userName);
	$userDiv.append(userName + checkSquare);
}

const addScore = function (score) {
	return `<div class="player-score">${score}</div>`;
}


// TODO: add new person to room user list (Done)
// TODO: odaya girdiginde odada bulunan diger kisileri gormus olacak (Done)
// TODO: oy verdikten sonra oyu verenin verdigi puan ayni grouptakilere gitmeli isterse degistirebilmeli.
// TODO: oy veren diger herkesin ve kendi ekraninda checked durumunda olmali
// TODO: oy verdikten sonra degistirirse yine herkese bildirim gitmeli.
// TODO: 30 sn icinde oy kullanilmali. Sure bitince otomatik bitmeli ve verilen oylarindan en yuksek olan hesaplanmali
// TODO: oylamayi bitir butonu olmali. Oylari temizle butonu olmali herkeste temizlemeli
// TODO: Herkeste ortak calisan bir timer olmali (Version:2)
// TODO: Bildirimler icin js function ile tetiklenmeli (Version:2)
// TODO: Kullanicilar bir yerde sabit tutalabilir. (Version:2)
// TODO: Puanlama esnasinda O anki islemler json file yazilip en son silenebilir. 


// Alt siradaki 2 method test icindir. En son silenebilir.
function invoke(connection) {
	//connection.invoke('SendMessage', "Selman", "SA");
}

function read(connection) {
	connection.on('Whatever', function (response, name, message) {
		console.log(response + ' ' + name + ' ' + message);
	});
}