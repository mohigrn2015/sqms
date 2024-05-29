$(document).ready(function () {
    modalGalleryCreate(GetSelectedItem);

});


function GetSelectedItem(value) {
    $('#file_name').val(value);
}