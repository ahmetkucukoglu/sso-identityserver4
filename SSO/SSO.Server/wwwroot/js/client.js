$(function () {
    $('#Command_LogoFile').change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.fileName = this.files[0].name;

            reader.onload = function (e) {
                $('#Command_LogoFile').next(".custom-file-label").text(e.target.fileName);
                $('#logoPreview').removeClass('d-none').attr('src', e.target.result);
            };

            reader.readAsDataURL(this.files[0]);
        }
    });

    new ClipboardJS('#btnCopyClientSecret');

    new ClipboardJS('#btnCopyClientId', {
        text: function (trigger) {
            var clientId = $('#Command_Id').val() + $('#Command_IdSuffix').val();

            return clientId;
        }
    });
});