/************************************************************/
/*                 Validador Bootstrap Custom               */
/*                     JEP - Version 4.0                    */
/************************************************************/

$(document).ready(function () {   
    function cuerpo($selector) {
        var flag = 0;
        /* REQUERIDO */
        if ($selector.hasClass('es-requerido')) {
            if ($selector.val().length == 0) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Obligatorio</span>');
                return;
            }
            else {
                $selector.parent().removeClass('has-error');
                $selector.parent().children('span').remove('.validador');
            }
        }
        if ($selector.hasClass('solo-numero')) {
            if ($selector.val().length > 0) {
                if (!(Math.floor($selector.val()) == $selector.val() && $.isNumeric($selector.val()))) {
                    $selector.parent().addClass('has-error');
                    $selector.parent().children('span').remove('.validador');
                    $selector.parent().append('<span class="help-block validador">Numérico</span>');
                    flag = 1;
                }
            }
        }
        if ($selector.hasClass('solo-numero-dec')) {
            /*REEMPLAZAMOS LAS COMAS(,)*/
            var numeroR = $selector.val();
            numeroR = numeroR.replace('.', '*');
            numeroR = numeroR.replace(',', '.');

            if (numeroR.length > 0) {
                if (!($.isNumeric(numeroR))) {

                    $selector.parent().addClass('has-error');
                    $selector.parent().children('span').remove('.validador');
                    $selector.parent().append('<span class="help-block validador">Numérico Decimal</span>');

                    flag = 1;
                }
            }

        }

        if ($selector.hasClass('caracter-50')) {

            if ($selector.val().length > 51) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(50)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-100')) {

            if ($selector.val().length > 101) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(100)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-150')) {

            if ($selector.val().length > 151) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(150)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-200')) {

            if ($selector.val().length > 201) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(200)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-250')) {

            if ($selector.val().length > 251) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(250)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('sin-espacio')) {

            if ($selector.val().replace(/\ /g, '').length != $selector.val().length) {

                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">No debe tener espacios</span>');

                flag = 1;
            }
        }

        if (flag == 0) {
            $selector.parent().removeClass('has-error');
            $selector.parent().children('span').remove('.validador');
        }
    }

    $(document).on('focusout', '.form-control', function () {
        var context = $(this);
        cuerpo(context);
    });
});



(function ($) {
    function cuerpo($selector) {

        var flag = 0;
        /* REQUERIDO */
        if ($selector.hasClass('es-requerido')) {

            if ($selector.val().trim().length == 0) {

                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Obligatorio</span>');

                flag = 1;
                return;
            }
        }

        if ($selector.hasClass('solo-numero')) {
            if ($selector.val().length > 0) {
                if (!(Math.floor($selector.val()) == $selector.val() && $.isNumeric($selector.val()))) {

                    $selector.parent().addClass('has-error');
                    $selector.parent().children('span').remove('.validador');
                    $selector.parent().append('<span class="help-block validador">Numérico</span>');

                    flag = 1;
                }
            }

        }

        if ($selector.hasClass('solo-numero-dec')) {
            /*REEMPLAZAMOS LAS COMAS(,)*/
            var numeroR = $selector.val();
            numeroR = numeroR.replace('.', '*');
            numeroR = numeroR.replace(',', '.');

            if (numeroR.length > 0) {
                if (!($.isNumeric(numeroR))) {

                    $selector.parent().addClass('has-error');
                    $selector.parent().children('span').remove('.validador');
                    $selector.parent().append('<span class="help-block validador">Numérico Decimal</span>');

                    flag = 1;
                }
            }

        }

        if ($selector.hasClass('caracter-50')) {

            if ($selector.val().length > 51) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(50)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-100')) {

            if ($selector.val().length > 101) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(100)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-150')) {

            if ($selector.val().length > 151) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(150)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-200')) {

            if ($selector.val().length > 201) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(200)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('caracter-250')) {

            if ($selector.val().length > 251) {
                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">Rebasa máximo(250)</span>');

                flag = 1;
            }
        }

        if ($selector.hasClass('sin-espacio')) {

            if ($selector.val().replace(/\ /g, '').length != $selector.val().length) {

                $selector.parent().addClass('has-error');
                $selector.parent().children('span').remove('.validador');
                $selector.parent().append('<span class="help-block validador">No debe tener espacios</span>');

                flag = 1;
            }
        }

        if (flag == 0) {
            $selector.parent().removeClass('has-error');
            $selector.parent().children('span').remove('.validador');
        }
    }

    $.fn.validar = function () {
        var context = $(this);
        var flag = 0;
        $('.form-control', context).each(function () {

            cuerpo($(this));
        });

        $('.form-control', context).each(function () {
            if ($(this).parent().hasClass('has-error')) {
                flag = 1;
            }
        });

        if (flag == 1) {
            return true; //con error
        }
        else {
            return false; //sin error
        }

    };

})(jQuery);