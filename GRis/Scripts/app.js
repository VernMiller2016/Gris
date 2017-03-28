$(function () {
    // disable jquery date validation. see: http://stackoverflow.com/questions/5966244/jquery-datepicker-chrome
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, 'MM/YYYY').isValid();
    };

    $('.monthyear-datepicker').parent('.input-group.date').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY',
        showTodayButton: true,
        ignoreReadonly: true,
        //format: "dd-mm-yyyy",
        //startView: 1,
        //minViewMode: 1,
        //autoclose: true,
        //todayHighlight: true
    });
});