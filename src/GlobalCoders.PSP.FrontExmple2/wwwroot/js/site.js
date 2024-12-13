// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
const BASE_URL = "http://localhost:9001";
// Write your JavaScript code.

$(document).ready(function () {
    //alert("Hello World");
    $("#loginButton").click(function () {
       //fetch()
        
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: BASE_URL + "/Account/Login",
            contentType: "application/json",
            dataType: "json",
            data:
                JSON.stringify({
                
                    "email": $("#username").val(),
                    "password": $("#password").val()
                
            }),
            success: function (data) {
                console.log(data);
                localStorage.setItem("token", data.accessToken);
                document.getElementById("authorization-token").innerHTML = data.accessToken;
                document.getElementById("authorization-info").innerHTML = "Authorized";
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById("authorization-info").innerHTML = "failed";
                console.log("Error, status = " + textStatus + ", " + "error thrown: " + errorThrown);
            }
        });
    });
});

function getMerchants(){
    $.ajax({
        type: "POST",
        crossDomain: true,
        url: BASE_URL + "/Organization/All",
        contentType: "application/json",
        dataType: "json",
        headers: {
            Authorization: "Bearer " + localStorage.getItem("token")
        },
        data:  JSON.stringify( {
            "page": document.getElementById("page-merchant").value,
            "itemsPerPage": document.getElementById("pageSize-merchant").value,
            "displayName": document.getElementById("filter_of_merchant").value,
        }),
        success: function (data) {
            console.log(data);
            let table = document.getElementById("merchant-table");
            table.innerHTML = "";
            let header = table.createTHead();
            let row = header.insertRow(0);
            let cell = row.insertCell(0);
            cell.innerHTML = "Id";
            cell = row.insertCell(1);
            cell.innerHTML = "Name";
            cell = row.insertCell(2);
            cell.innerHTML = "Description";
            cell = row.insertCell(3);
            cell.innerHTML = "Actions";
            let body = table.createTBody();
            data.items.forEach(element => {
                row = body.insertRow(0);
                cell = row.insertCell(0);
                cell.innerHTML = element.id;
                cell = row.insertCell(1);
                cell.innerHTML = element.displayName;
                cell = row.insertCell(2);
                cell.innerHTML = element.description;
                cell = row.insertCell(3);
                cell.innerHTML = "<button onclick='getMerchantById(" + element.id + ")'>Edit</button>";
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error, status = " + textStatus + ", " + "error thrown: " + errorThrown);
        }
    });
}


function saveMerchants(){
    $.ajax({
        type: "POST",
        crossDomain: true,
        url: BASE_URL + "/Organization/Create",
        contentType: "application/json",
        headers: {
            Authorization: "Bearer " + localStorage.getItem("token")
        },
        data:  JSON.stringify( {
            "displayName": document.getElementById("displayName").value,
            "legalName": document.getElementById("legalName").value,
            "address": document.getElementById("address").value,
            "email": document.getElementById("email").value,
            "mainPhoneNumber":  document.getElementById("mainPhoneNumber").value,
            "secondaryPhoneNumber":     document.getElementById("secondaryPhoneNumber").value,
        }),
        success: function (data) {
            console.log(data);
            alert("Merchant saved");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error, status = " + textStatus + ", " + "error thrown: " + errorThrown);
        }
    });
}