$(document).ready(function () {

    $("#ddlPayment").change(function () {

        if ($("#ddlPayment option:selected").val() != -1) {
            $("#submitPaymentForm").prop("disabled", false);
        }
        else {
            $("#submitPaymentForm").prop("disabled", true);
        }

    });

});