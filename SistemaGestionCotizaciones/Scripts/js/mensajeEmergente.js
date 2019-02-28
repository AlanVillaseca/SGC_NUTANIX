//$(document).ready(function () {


    function exito(mensaje) {
        $('#msg_wrn').css({
            display: 'block'
        });
        $('#msg_wrn').attr({
            class: 'alert alert-success'
        });
        $('#msg_wrn span').attr({
            class: 'glyphicon glyphicon-ok'
        });
        $('#msg_wrn span').text(mensaje);
        $('#contenedorPagina').css({
            top: +70
        });
    };

    function precaucion(mensaje) {
        $('#msg_wrn').css({
            display: 'block'
        });
        $('#msg_wrn').attr({
            class: 'alert alert-warning'
        });
        $('#msg_wrn span').attr({
            class: 'glyphicon glyphicon-warning-sign'
        });
        $('#msg_wrn span').text(mensaje);
        $('#contenedorPagina').css({
            top: +70
        });
    };

    function error(mensaje) {        
        $('#msg_wrn').css({
            display: 'block'
        });
        $('#msg_wrn').attr({
            class: 'alert alert-danger'
        });
        $('#msg_wrn span').attr({
            class: 'glyphicon glyphicon-exclamation-sign'
        });
        $('#msg_wrn span').text(mensaje);
        $('#contenedorPagina').css({
            top: +70
        });
    };    
    function informacion(mensaje) {
        $('#msg_wrn').css({
            display: 'block'
        });
        $('#msg_wrn').attr({
            class: 'alert alert-info'
        });
        $('#msg_wrn span').attr({
            class: 'glyphicon glyphicon-info-sign'
        });
        $('#msg_wrn span').text(mensaje);
        $('#contenedorPagina').css({
            top: +70
        });
    };

    if ($('#msg_wrn').css('display') != 'none') {

        $('#fm_vpyct').css(
            'top', '60px'
        );
    }


//});