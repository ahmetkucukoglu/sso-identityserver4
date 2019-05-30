$(function () {
    $('[data-remove]').click(function () {
        var removeButton = $(this);
        var dataContent = removeButton.data('content');

        var href = removeButton.attr('href');

        if (dataContent) {
            $('#removeConfirmModal').find('.modal-body').text(dataContent);
        }

        $('#removeConfirmModal').data('href', href);
        $('#removeConfirmModal').modal('show');

        return false;
    });

    $('#removeConfirmModal #btnRemove').click(function () {

        $('#removeConfirmModal').modal('hide');

        var href = $('#removeConfirmModal').data('href');
        location.href = href;
    });
});