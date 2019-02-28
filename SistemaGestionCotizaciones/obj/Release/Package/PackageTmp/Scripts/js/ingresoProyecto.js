$(document).ready(function () {

    var combobox_top;
    var combobox;
    var comboValues;

    $('#fech_cld').datepicker({
        minDate: new Date(),
        dateFormat: 'dd/mm/yy'
    });

    $("#fech_cld").datepicker("setDate", new Date());

   

    $('#ico_agr').click(function () {
        if ($('#neg ul li:last-child option:selected').text() != 'Ingresar Negocio') {
                        
            combobox = '<li><fieldset><select>' + $('#li_ini select').html() + '</select></fieldset></li>';          
        
            $('#ini').append(combobox);
        
            $('#ico_agr').css('top', parseFloat($('#ico_agr').css('top')) + 31 + 'px');

            $('#ico_qtr').css('top', parseFloat($('#ico_qtr').css('top')) + 31 + 'px');

            $('body').css('margin-top', parseFloat($('body').css('margin-top')) + 31 + 'px');

            $('select').selectmenu();
        }
    });

    $('#ico_qtr').click(function () {
        if ($('#neg ul li:last-child').attr('id') != 'li_ini') {

            $('#neg ul li:last-child').remove();

            $('#ico_agr').css('top', parseFloat($('#ico_agr').css('top')) - 31 + 'px');

            $('#ico_qtr').css('top', parseFloat($('#ico_qtr').css('top')) - 31 + 'px');

            $('body').css('margin-top', parseFloat($('body').css('margin-top')) - 31 + 'px');
        }
    });

    $('#btn_ing').click(function () {
        
        
        if ($('#nom_pyct input').val() != '') {
            
        }
        else
            alert('El campo Nombre del Proyecto no puede estar vacio');
    });

});