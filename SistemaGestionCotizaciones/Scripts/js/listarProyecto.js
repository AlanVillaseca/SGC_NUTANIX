$(document).ready(function () {

    //var url = @Url.Action("grillaListarProyecto", "ListarProyecto");

    function fech_sol() {
        $('#fech_sol_min').datepicker({
            onClose: function (selectedDate) {
                $('#fech_sol_max').datepicker('option', 'minDate', selectedDate);
            }
        });

        $('#fech_sol_max').datepicker({
            onClose: function (selectedDate) {
                $('#fech_sol_min').datepicker('option', 'maxDate', selectedDate);
            }
        });
    };

    $('.glyphicon-th-list').click(function () {
        if ($('#selec_jefe').is(':hidden')) {
            $('#selec_jefe').slideDown();
        } else {
            $('#selec_jefe').hide();
        }
    });






    fech_sol();

    $('.glyphicon-file').click(function () {

    });

    $('#selec_jefe span').click(function () {
        $('#selec_jefe').hide();
    });

    $('#fech_sol_min').datepicker('option', 'dateFormat', 'dd-mm-yy');
    $('#fech_sol_max').datepicker('option', 'dateFormat', 'dd-mm-yy');

    //Filtro Nombre Proyecto
    $('#txt_proyecto, #txt_cotizacion, #fech_sol_min, #fech_sol_max, #cmb_jefe_pryct, #cmb_creador, #cmb_pais_negocio, #cmb_negocio, #cmb_solicitante, #cmb_neg_sol, #cmb_serv_neg, #cmbMostrar').change(function () {
        filtrado(0, 0, 0);
    });

    $('#txt_proyecto, #txt_cotizacion').keypress(function (e) {

        var key = e.which;

        if (key == 13) {
            filtrado(0, 0, 0);
        }
    });




});