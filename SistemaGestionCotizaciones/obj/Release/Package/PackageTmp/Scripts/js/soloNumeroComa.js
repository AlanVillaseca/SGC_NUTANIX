$(document).ready(function () {

    $(document).on('keypress', '.solo-numero-coma', function (e) {

        if ($(this).val().indexOf(',') == '-1') {
            if (!(e.which <= 57 && e.which >= 48 || e.which == 0 || e.which == 8 || e.which == 44)) {

                return false;
            }
        } else {
            if (!(e.which <= 57 && e.which >= 48 || e.which == 0 || e.which == 8)) {

                return false;
            }
        }
        
    });
});