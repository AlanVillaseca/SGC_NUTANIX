$(document).ready(function () {

    $('#cotVersion').selectmenu({
        change: function () {
            this.form.submit();
        }
    });

});