
//---------------------------------------Variables---------------------------------------//

//-----------------------Const-----------------------//

//Table body element
const listTable = $("#listTable")[0];

//Connection settings + build
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7247/ConnectionHub")
    .build();

//---------------------------------------------------//

//---------------------------------------------------------------------------------------//



//---------------------------------------Functions---------------------------------------//

//------------------Arrow functions------------------//

//add a row to the table
const row = async (airplane) =>
    `<tr name="row${airplane.airplaneId}">
        <td>${time()}</td>
        <td>${airplane.number}</td>
        <td>${airplane.serialNumber}</td>
        <td>${airplane.companyName}</td>
        <td>${airplane.origin}</td>
        <td>${airplane.destenation}</td>
        <td>${airplane.currentLeg.number}</td>
    </tr>`;

const time = () => {
    const date = new Date();
    let hours = date.getHours();
    let minutes = date.getMinutes();
    let seconds = date.getSeconds();
    if (hours < 10) hours = '0' + hours;
    if (minutes < 10) minutes = '0' + minutes;
    if (seconds < 10) seconds = '0' + seconds;
    return `${hours}:${minutes}:${seconds}`
}

//Print some text to console
const print = (msg) => console.log(time() + ' ' + msg);

//Add the aiplane to the table
const AddToTable = async (airplane) => {
    print(`Add airplane ${airplane.airplaneId} to table`);
    listTable.innerHTML += await row(airplane);
}

//Function to start the connection
const Start = async () => {
    await connection.start()
        .then(function () {
            print('SignalR Connected');
            connection.invoke("SendFirstAirplane").catch(function (err) {
                print("Cant send first airplane...")
            });
        }).catch(function (err) {
            print('Connected failed, try again in half second');
            setTimeout(Start, 500);
        });
}

//Remove the airplane rows from the table
const RemoveFromTable = async (airplane) => {
    print(`Remove airplane ${airplane.airplaneId} from table`);
    const id = airplane.airplaneId;
    const children = listTable.children;
    //Find all rows contains the given airplane
    for (var i = children.length - 1; i >= 0; --i) {
        if (children[i].attributes[0].nodeValue.startsWith(`row${id}`)) {
            children[i].remove();
        }
    }
    listTable.children = children;
}


//---------------------------------------------------//


//------------------Connection functions-------------//

//Reciving airplane-> update table
connection.on("ReceiveAirplane", async function (airplane) {
    await AddToTable(airplane);
});

//Reciving airplane that was deleted from airport-> update table by remove all rows contains the given airplane
connection.on("ReceiveDeleteAirplane", async function (airplane) {
    await RemoveFromTable(airplane);
});

//---------------------------------------------------//

//---------------------------------------------------------------------------------------//




//-----------------------------------Called at loading-----------------------------------//

//Start when window got loaded, add airplane to the airport from air
addEventListener('load', async () => {
    print('Page loaded');
    //Start the connection
    await Start();
})

//---------------------------------------------------------------------------------------//