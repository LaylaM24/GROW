$(document).ready(function () {

    $("#ddlItem").change(function () {

        if ($("#ddlItem option:selected").val() != -1) {
            $("#submitItemForm").prop("disabled", false);
        }
        else {
            $("#submitItemForm").prop("disabled", true);
        }

    });

});