
// Write your JavaScript code.

$(document).ready(function () {
    //alert("Hello World");
    $("#resetButton").click(function () {
       //fetch()
        
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: DOMAIN_URL + "Account/ResetPassword",
            contentType: "application/json",
            data:
                JSON.stringify({
                
                    "email": $("#email").val(),
                    "resetCode": $("#resetCode").val(),
                    "newPassword": $("#password").val()
                
            }),
            success: function (data) {
                console.log(data);
                document.getElementById("authorization-info").innerHTML = "Success";

            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById("authorization-info").innerHTML = "failed";
                console.log("Error, status = " + textStatus + ", " + "error thrown: " + errorThrown);
            }
        });
    });
});