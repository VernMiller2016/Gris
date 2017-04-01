
/*search for users that contains written string*/
function Search() {
    var searchVal = $("#searchText").val();
    var param = {};
    param.searchVal = searchVal;
    $.ajax({
        url: refreshUrl,
        type: "GET",
        data: param,
        success: function (html) {
            $("#userList").html(html);
        },
        error: function (e) {
            console.log(e);
        }
    });
}


/*press enter on search textBox*/
/*@event event object*/
function enterSearch(event) {
    if (event.keyCode == 13) {
        Search();
    }
}