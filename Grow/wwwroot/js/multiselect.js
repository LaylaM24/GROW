$(document).ready(function () {

    $("#btnAddR").click(function (event) {
        if ($("#availRestrictionOptions").val() != null) {
            var SelectData = $("#availRestrictionOptions :selected").toArray();
            $("#selectedRestrictionOptions").append(SelectData);
            SelectData.remove;
        }
    });

    $("#btnRemoveR").click(function (event) {
        if ($("#selectedRestrictionOptions").val() != null) {
            var SelectData = $("#selectedRestrictionOptions :selected").toArray();
            $("#availRestrictionOptions").append(SelectData);
            SelectData.remove;
        }
    });

    $("#btnAddC").click(function (event) {
        if ($("#availOptions").val() != null) {
            var SelectData = $("#availOptions :selected").toArray();
            $("#selectedHealthOptions").append(SelectData);
            SelectData.remove;
        }
    });

    $("#btnRemoveC").click(function (event) {
        if ($("#selectedHealthOptions").val() != null) {
            var SelectData = $("#selectedHealthOptions :selected").toArray();
            $("#availOptions").append(SelectData);
            SelectData.remove;
        }
    });

    //$("#submitMemberForm").click(function () {
    //    $("#selectedRestrictionOptions option").each(function (index) {
    //        $(this).prop('selected', true);
    //    });

    //    $("#selectedHealthOptions option").each(function (index) {
    //        $(this).prop('selected', true);
    //    });
    //});

});