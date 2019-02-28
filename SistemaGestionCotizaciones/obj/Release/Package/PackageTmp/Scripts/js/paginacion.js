function filtrado(pagina, idHead, ascDesc) {

    var nombreProyecto = $('#txt_proyecto').val();
    if (nombreProyecto == '') {
        nombreProyecto = -1;
    }

    var codCot = $('#txt_cotizacion').val();
    if (codCot == '') {
        codCot = -1;
    }

    var negocio = $('#cmb_pais_negocio').val();
    var paisNegocio = $('#cmb_pais_negocio option:selected').attr('groupId');
    if (negocio == -1) {
        paisNegocio = -1;
    }
    var usrSolicitante = $('#cmb_solicitante').val();
    var usrAsignada = $('#cmb_jefe_pryct').val();
    var usrCreador = $('#cmb_creador').val();
    var srvNegocio = $('#cmb_negocio').val();
    var fchIni = $('#fech_sol_min').val();
    if (fchIni == '') {
        fchIni = -1;
    } else {
        fchIni = cambiaFecha(fchIni);
    }
    var fchFin = $('#fech_sol_max').val();
    if (fchFin == '') {
        fchFin = -1;
    } else {
        fchFin = cambiaFecha(fchFin);
    }

    var mostrar = $('#cmbMostrar').val();
        //'&negocio=' + negocio + '&usrSolicitante=' + usrSolicitante +
        //'&srvNegocio=' + srvNegocio + '&fchIni=' + fchIni + '&fchFin=' + fchFin + '&usrAsignada=' + usrAsignada
    $('#tb_pytc').load('vpGrillaListar/', {
                                            id: nombreProyecto,
                                            paisNegocio: paisNegocio,
                                            negocio: negocio,
                                            usrSolicitante: usrSolicitante,
                                            srvNegocio: srvNegocio,
                                            fchIni: fchIni,
                                            fchFin: fchFin,
                                            usrAsignada: usrAsignada,
                                            pagina: pagina,
                                            idHead: idHead,
                                            ascDesc: ascDesc,
                                            idMostrar: mostrar,
                                            codCot: codCot,
                                            usrCreador: usrCreador
                                          });
};