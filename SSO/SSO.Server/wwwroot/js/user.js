$(function () {
    var url = document.location.toString();

    if (url.match('#')) {
        var tab = url.split('#')[1];
        $('#editTab a[href="#' + tab + '"]').tab('show');
    }
});