function getSelectedDropDownItem() {
    const dropdown = document.getElementById('gameSystemDropDown');
    return dropdown.value;
}


function removeSelectedRoster() {
    $("#rostersList").find("[aria-selected='true']").remove()
}


function getSelectedRoster() {
   return $("#rostersList").find("[aria-selected='true']").val()
}


function setFilteredDropDownItems(filteredRosters) {
    var items = [];

    $.each(JSON.parse(filteredRosters), function (i, item) {
        items.push('<fast-option value="' + item.Id + '">' + item.Name + ' ' + item.Points + '</fast-option>');
        
    });
    $('#rostersList').empty();
    $('#rostersList').append(items.join(''));
}