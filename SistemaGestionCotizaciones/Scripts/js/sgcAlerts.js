/* Modal Confirm INI*/
function confirm(heading, question, cancelButtonTxt, okButtonTxt, callback) {

    var confirmModal = '';
    //En caso de que se necesite un callback sin cancel
    if (cancelButtonTxt == '0') {
        var confirmModal =
          $('<div class="modal fade">' +
              '<div class="modal-dialog">' +
              '<div class="modal-content">' +
              '<div class="modal-header">' +
                '<h4 class="modal-title">' + heading + '</h4>' +
              '</div>' +

              '<div class="modal-body">' +
                '<p>' + question + '</p>' +
              '</div>' +

              '<div class="modal-footer">' +
                '<a href="#!" id="okButton" class="btn btn-primary">' +
                  okButtonTxt +
                '</a>' +
              '</div>' +
              '</div>' +
              '</div>' +
            '</div>');
    } else {
        var confirmModal =
          $('<div class="modal fade">' +
              '<div class="modal-dialog">' +
              '<div class="modal-content">' +
              '<div class="modal-header">' +
                '<h4 class="modal-title">' + heading + '</h4>' +
              '</div>' +

              '<div class="modal-body">' +
                '<p>' + question + '</p>' +
              '</div>' +

              '<div class="modal-footer">' +
                '<a href="#!" class="btn" data-dismiss="modal">' +
                  cancelButtonTxt +
                '</a>' +
                '<a href="#!" id="okButton" class="btn btn-primary">' +
                  okButtonTxt +
                '</a>' +
              '</div>' +
              '</div>' +
              '</div>' +
            '</div>');
    }

    confirmModal.find('#okButton').click(function (event) {
        callback();
        confirmModal.modal('hide');
    });

    confirmModal.modal('show');
};
/*Modal Confirm END*/
/* Modal alert INI*/
function c_alert(heading, text, ButtonTxt) {

    var confirmModal =
      $('<div class="modal fade">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content">' +
          '<div class="modal-header">' +
              '<h4 class="modal-title">' + heading + '</h4>' +
          '</div>' +

          '<div class="modal-body">' +
            '<p>' + text + '</p>' +
          '</div>' +

          '<div class="modal-footer">' +
            '<a href="#!" class="btn" data-dismiss="modal">' +
              ButtonTxt +
            '</a>' +
          '</div>' +
          '</div>' +
          '</div>' +
        '</div>');

    confirmModal.modal('show');
    /*
    confirmModal.find('.btn').click(function (event) {
        
        confirmModal.modal('hide');
    });*/
    
};
/*Modal alert END*/