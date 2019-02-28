$(document).ready(function () {
    
    function fech_cr() {
        $('#fech_cr_min').datepicker({
            dateFormat: "dd/mm/yy",
            onClose: function (selectedDate) {
                $('#fech_cr_max').datepicker('option', 'minDate', selectedDate);
            }
        });

        $('#fech_cr_max').datepicker({
            dateFormat: "dd/mm/yy",
            onClose: function (selectedDate) {
                $('#fech_cr_min').datepicker('option', 'maxDate', selectedDate);
            }
        });
    };

    function fech_apr_ger() {
        $('#fech_apr_ger_min').datepicker({
            onClose: function (selectedDate) {
                $('#fech_apr_ger_max').datepicker('option', 'minDate', selectedDate);
            }
        });

        $('#fech_apr_ger_max').datepicker({
            onClose: function (selectedDate) {
                $('#fech_apr_ger_min').datepicker('option', 'maxDate', selectedDate);
            }
        });
    };

    function fech_apr_cli() {
        $('#fech_apr_cli_min').datepicker({
            onClose: function (selectedDate) {
                $('#fech_apr_cli_max').datepicker('option', 'minDate', selectedDate);
            }
        });

        $('#fech_apr_cli_max').datepicker({
            onClose: function (selectedDate) {
                $('#fech_apr_cli_min').datepicker('option', 'maxDate', selectedDate);
            }
        });
    };

    function fech_impl() {
        $('#fech_impl_min').datepicker({
            onClose: function (selectedDate) {
                $('#fech_impl_max').datepicker('option', 'minDate', selectedDate);
            }
        });

        $('#fech_impl_max').datepicker({
            onClose: function (selectedDate) {
                $('#fech_impl_min').datepicker('option', 'maxDate', selectedDate);
            }
        });
    };

    fech_cr();
    fech_apr_ger();
    fech_apr_cli();
    fech_impl(); 

    var pagina = '0';
    var cantidad = '10';
    var campoOrden = 'idCotizacion';
    var nombre = '';
    var jefe = '-1';
    var fechaI = '-1';
    var fechaF = '-1';
    var estado = '-1';

    var url = 'vpGrillaListar/' + pagina + '?cantidad=' + cantidad + '&campoOrden=' + campoOrden + '&nombre=' + nombre + '&jefe=' + jefe + '&fechaINI=' + fechaI + '&fechaFIN=' + fechaF + '&estado=' + estado;
    $('#tb_cttc').load(url);   

});