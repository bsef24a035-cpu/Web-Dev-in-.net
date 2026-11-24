// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(
    function temp() {
        $('#qfProductSearch').click(function () {
            const temp = $("#searchData").val();
            console.log(temp);
            try {
                $.get("/Products/Search", { search: temp }, function (x) {
                    $('#adminSide').html(x)
                    console.log("Request Generated and Response Received");
                })
            }
            catch (err) {
                console.error("Error occurred while making AJAX request:", err);
            }
        })
    }
)

