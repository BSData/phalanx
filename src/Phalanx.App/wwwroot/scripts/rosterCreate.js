function getSelectedFactionDropDownItem() {
    const dropdown = document.getElementById('factionDropDown');
    return dropdown.value;
}

function setSubFactionFilteredDropDownItems(filteredSubfactions) {
    var items = [];
    items.push('<fast-option >Select subfaction</fast-option>');
    $.each(JSON.parse(filteredSubfactions), function (i, item) {
        items.push('<fast-option>' + item.Name + '</fast-option>');
    });
    $('#subFactionDropDown').empty();
    $('#subFactionDropDown').append(items.join(''));
    $('#subFactionDropDown').show();
}

function getSelectedDetachmentDropDownItem() {
    const dropdown = document.getElementById('detachmentDropDown');
    return dropdown.value;
}

function setDetachmentFilteredDropDownItems(detachments) {
    var items = [];
    items.push('<fast-option >Select detachment</fast-option>');
    $.each(JSON.parse(detachments), function (i, item) {
        items.push('<fast-option>' + item.Name + '</fast-option>');
    });
    $('#detachmentDropDown').empty();
    $('#detachmentDropDown').append(items.join(''));
    $('#detachmentDropDown').show();
}
