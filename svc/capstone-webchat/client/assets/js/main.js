//var connection = $.connection('https://localhost:44399/chatter');
var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost/api/chatter?Group=1234&UserId=4321",
        {transport: signalR.HttpTransportType.WebSockets})
    .build();
var username = "";
var userBannedUsers = [];
var sessionId = "";
var storage = window.localStorage;
var messageList = new Array();
var testing = false;

connection.start().then(function() {
    console.log("connected.");
});


//receive message
connection.on("ReceiveMessage", (user, message, time) => {

    //declare selectList of messages and create option to add message
    var selectList = document.getElementById("selectMessages");
    var receiveMessage = document.createElement("option");

    var userBanned = false;
    //check if message is sent by a user on the banned list
    for (var i = 0; i < userBannedUsers.length+1; i++) {
        //user is on banned list
        if(user === userBannedUsers[i]){
            userBanned = true;
            break;
        }
    }
    if(userBanned == true){
        selectList = document.getElementById("selectMessages");
        receiveMessage = document.createElement("option");
        receiveMessage.text = user + ": Message Blocked";
        selectList.add(receiveMessage);
        if (messageList.length >=100) {
            messageList.shift();
        }
        messageList.push(new Array(user, "Message Blocked", time));
    }else{
        //user is not on banned list
        selectList = document.getElementById("selectMessages");
        receiveMessage = document.createElement("option");
        receiveMessage.text = user + ": " + message;
        selectList.add(receiveMessage);
        if (messageList.length >=100) {
            messageList.shift();
        }
        messageList.push(new Array(user, message, time));

    }

});

function sendMessage() {
    var messageBody = document.getElementById("txtMessageSubmitBox").value;
    connection.invoke("SendMessage", username, messageBody);
    console.log("Sent Message");
}

//set identity
function setIdentity() {
    console.log("messages found.");
    var usernameInput = document.getElementById("username").value;
    var sessionIdInput = document.getElementById("session").value;
    if (usernameInput != "" && sessionIdInput != "") {
        sessionId = sessionIdInput;
        connection.invoke("GetUserSessionInfo", usernameInput, sessionIdInput);
        username = usernameInput;
        if (storage.getItem(sessionId) != null) {
            console.log("messages found.");
            var messages = storage.getItem(sessionId);
            do {
                var message = messages.substring(0, messages.indexOf("!@#") + 3);
                var name = message.substring(0, message.indexOf("$%$"));
                var messageBody = message.substring(message.indexOf("$%$") + 3, message.lastIndexOf("$%$"));
                var dateFound = new Date(message.substring(message.lastIndexOf("$%$") + 3, message.indexOf("!@#")));

                messageList.push(new Array(name, messageBody, dateFound));
                messageList = messageList.sort((a, b) => a[2] - b[2]);
                messages = messages.replace(message, "");
            } while (messages != "");
            messageList.forEach(appendMessage);
        } else {
            console.log("messages not found");
        }
    } else {
        window.alert("Make sure both username and session ID are set before setting identity.")
    }
}

//Append message function
function appendMessage(item, index) {
    selectList = document.getElementById("selectMessages");
    receiveMessage = document.createElement("option");
    receiveMessage.text = item[0] + ": " + item[1];
    selectList.add(receiveMessage);
}

window.addEventListener('beforeunload', (event) => {
    var saveMessage = "";
    for (i = 0; i < messageList.length; i++) {
        saveMessage += messageList[i][0] + "$%$" + messageList[i][1] + "$%$" + messageList[i][2] + "!@#";
    }
    storage.setItem(sessionId, saveMessage);
});

//user locally clears chat
function userClearChat(){

    var selectList = document.getElementById("selectMessages");
    var length = selectList.options.length;

    for (var i = length-1; i >= 0; i--) {
        selectList.options[i] = null;
    }

}

//user locally blocks user
function userBlocksUser(){

    var selectList = document.getElementById("selectMessages");
    var selectedMessage = selectList.options[selectList.selectedIndex].text;

    var banUser = selectedMessage.substr(0, selectedMessage.indexOf(':'));

    if(banUser === username){

        //if user tries to ban themselves
        var message = document.createElement("option");
        message.text = "Error: Cannot ban user";
        selectList.add(message);
    }else{

        //add or remove user from banned list
        if(userBannedUsers.includes(banUser)){
            var index = userBannedUsers.indexOf(banUser);
            userBannedUsers.splice(index, 1);
        }else{
            userBannedUsers.push(banUser);
        }

    }
}


function showAdminOptions() {

    //declare testing div
    var divAdminOptions = document.getElementById("divAdminOptions");

    //hide or show testing div, hidden is default
    if (divAdminOptions.style.display === "none") {
        divAdminOptions.style.display = "block";
    } else {
        divAdminOptions.style.display = "none";
    }

}

//admin clears chat
function adminClearChat(){

    //admin clears chat
    connection.invoke("AdminClearsChat");

}

connection.on("ClearChat", () => {
    var selectList = document.getElementById("selectMessages");
    var length = selectList.options.length;

    for (var i = length-1; i >= 0; i--) {
        selectList.options[i] = null;
    }
});

//admin deletes message from chat
function adminDeleteMessage(){

    //get selectList
    var selectList = document.getElementById("selectMessages");

    if (testing == false) {
        //get the selected message
        selectedMessage = selectList.options[selectList.selectedIndex].text;
    } else {
        selectedMessage = selectList.options[selectList.length - 1].text;
    }

    //split message into username and message
    var username = selectedMessage.substr(0, selectedMessage.indexOf(':'));
    var message = selectedMessage.substr(selectedMessage.indexOf(':')+1, selectedMessage.length);

    //invoke admindeletesmessage function
    connection.invoke("AdminDeletesMessage", username, message);

}

connection.on("DeleteMessage", (username, message) => {

    //get selectList
    var selectList = document.getElementById("selectMessages");
    var length = selectList.options.length;

    var totalMessage = username + ":" + message;

    for (var i = length-1; i >= 0; i--) {
        if(selectList.options[i].value === totalMessage){
            selectList.options[i] = null;
        }

    }

});

function adminToggleChat(){
    // will automatically send a message in chat with the new state of the chat
    connection.invoke("ToggleLockChat");

}

function adminBlockUser(){
    if (testing == false) {
        //get selectList
        var selectList = document.getElementById("selectMessages");

        //get the selected message
        var selectedMessage = selectList.options[selectList.selectedIndex].text;

        //split message into username and message
        var username = selectedMessage.substr(0, selectedMessage.indexOf(':'));

        connection.invoke("AdminBlocksUser", (username));
    } else {
        connection.invoke("AdminBlocksUser", "test");
    }
}

connection.on("ReceiveUserInfo", (username, isAdmin, isModerator, isBlocked) => {

    var selectList = document.getElementById("selectMessages");
    var infoMessage = document.createElement("option");
    infoMessage.text = "User Info: Username: " + username + ", Admin: " + isAdmin + ", Moderator: " + isModerator
        + ", Blocked: " + isBlocked;

    selectList.add(infoMessage);

});

function adminViewUserInfo(){
    if (testing == false) {
        //get selectList
        var selectList = document.getElementById("selectMessages");

        //get the selected message
        var selectedMessage = selectList.options[selectList.selectedIndex].text;

        //split message into username and message
        var username = selectedMessage.substr(0, selectedMessage.indexOf(':'));
        connection.invoke("ViewUserInfo", username);	// Username of the user whose info is being searched
    } else {
        connection.invoke("ViewUserInfo", "test")
    }
}

connection.on("ReceiveAllUsers", (users) => {


    //get selectList
    var selectList = document.getElementById("selectMessages");
    usersList = document.createElement("option");
    usersList.text = "Connected Users: " + users;

    selectList.add(usersList);

});


function userShowConnectedUsers(){
    //user show connected users
    connection.invoke("GetAllUsers");
}

function adminToggleMod(){
    if (testing == false) {
        //get selectList
        var selectList = document.getElementById("selectMessages");

        //get the selected message
        var selectedMessage = selectList.options[selectList.selectedIndex].text;

        //split message into username and message
        var username = selectedMessage.substr(0, selectedMessage.indexOf(':'));

        connection.invoke("AdminModsUser", username); // Username of the user to be modded
    } else {
        connection.invoke("AdminModsUser", "test");
    }
}

function addWordFilter(){
    //add word to word filter
    var filter = document.getElementById("txtAddRemoveWord").value;
    connection.invoke("AddFilter", filter); // For adding words to be filtered
    // Expected input is single filter value, or comma seperated list
}


function showWordFilter(){

    //declare word filter div
    var lblAddRemoveWord = document.getElementById("lblAddRemoveWord");
    var txtAddRemoveWord = document.getElementById("txtAddRemoveWord");
    var btnAddWordFilter = document.getElementById("btnAddWordFilter");

    //hide or show testing div, hidden is default
    if(txtAddRemoveWord.style.display === "none"){
        txtAddRemoveWord.style.display = "inline";
        lblAddRemoveWord.style.display = "inline";
        btnAddWordFilter.style.display = "inline";
    } else {
        txtAddRemoveWord.style.display = "none";
        lblAddRemoveWord.style.display = "none";
        btnAddWordFilter.style.display = "none";
    }

}


function showTest(){

    //declare testing div
    var testDiv = document.getElementById("testing");

    //hide or show testing div, hidden is default
    if(testDiv.style.display === "none"){
        testDiv.style.display = "block";
    } else {
        testDiv.style.display = "none";
    }

}

async function runUnitTests()
{
    if (confirm("Make sure a new user named 'test' is in the session, as well as make sure that the current user running the tests is the admin of the session.")) {
        var selectList = document.getElementById("selectMessages");
        var testOutput = "";
        var itemCount = selectList.length;
        testing = true;
        adminViewUserInfo(); // add testing section for function
        await sleep(1000);
        if (itemCount != selectList.length) {
            testOutput = "adminViewUserInfo:Pass";
        } else {
            testOutput = "adminViewUserInfo:Fail";
        }
        console.log("adminViewUserInfo.");
        adminBlockUser(); // add testing section for function
        await sleep(1000);
        adminViewUserInfo();
        await sleep(1000);
        if (selectList.options[selectList.length - 2].text != selectList.options[selectList.length - 1].text) {
            testOutput += "|adminBlockUser:Pass";
        } else {
            testOutput += "|adminBlockUser:Fail";
        }
        console.log("adminBlockUser.");
        adminClearChat();
        await sleep(1000);
        if (selectList.length === 0) {
            testOutput += "|adminClearChat:Pass";
        } else {
            testOutput += "|adminClearChat:Fail";
        }
        console.log("adminClearChat.");
        document.getElementById("txtMessageSubmitBox").value = "This is a test";
        sendMessage();
        await sleep(1000);
        if (selectList.length = 1) {
            testOutput += "|sendMessage:Pass";
        } else {
            testOutput += "|sendMessage:Fail";
        }
        console.log("sendMessage.");
        userClearChat();
        if (selectList.length === 0) {
            testOutput += "|userClearChat:Pass";
        } else {
            testOutput += "|userClearChat:Fail";
        }
        console.log("userClearChat.");
        sendMessage();
        await sleep(1000);
        itemCount = selectList.length;
        adminDeleteMessage(); // add testing to select last message
        await sleep(1000);
        if (itemCount != selectList.length) {
            testOutput += "|adminDeleteMessage:Pass";
        } else {
            testOutput += "|adminDeleteMessage:Fail";
        }
        console.log("adminDeleteMessage.");
        adminToggleMod(); // add testing to set test user to moderator
        await sleep(1000);
        adminViewUserInfo();
        await sleep(1000);
        if (selectList.options[selectList.length - 1].text.includes("Moderator: true")) {
            testOutput += "|adminToggleMod:Pass";
        } else {
            testOutput += "|adminToggleMod:Fail";
        }
        console.log("adminToggleMod.");
        document.getElementById("txtAddRemoveWord").value = "test";
        addWordFilter(); //add test to list of filtered words
        await sleep(1000);
        sendMessage();
        await sleep(1000);
        if (selectList.options[selectList.length - 1].text.includes("test")) {
            testOutput += "|addWordFilter:Fail";
        } else {
            testOutput += "|addWordFilter:Pass";
        }
        console.log("addWordFilter.");
        itemCount = selectList.length;
        userShowConnectedUsers();
        await sleep(1000);
        if (itemCount != selectList.length) {
            testOutput += "|userShowConnectedUsers:Pass";
        } else {
            testOutput += "|userShowConnectedUsers:Fail";
        }
        console.log("userShowConnectedUsers.");
        var file = new Blob([testOutput], {type: "text"});
        if (window.navigator.msSaveOrOpenBlob) // IE10+
            window.navigator.msSaveOrOpenBlob(file, "testOutput.txt");
        else { // Others
            var a = document.createElement("a"),
                    url = URL.createObjectURL(file);
            a.href = url;
            a.download = "testOutput.txt";
            document.body.appendChild(a);
            a.click();
            setTimeout(function() {
                document.body.removeChild(a);
                window.URL.revokeObjectURL(url);  
            }, 0); 
        }
        testing = false;
    }
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
