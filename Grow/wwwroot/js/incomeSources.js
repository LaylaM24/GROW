$(document).ready(function () {
    
    $("#btnAddIncome").click(function () {
        if (!$("#IncomeSource1").is(":visible")) {
            $("#IncomeSource1").css("display", "inline");
            $("#ddl1").attr("name", "IncomeSource1");
            $("#txt1").attr("name", "IncomeAmount1");
        }
        else if (!$("#IncomeSource2").is(":visible")) {
            $("#IncomeSource2").css("display", "inline");
            $("#ddl2").attr("name", "IncomeSource2");
            $("#txt2").attr("name", "IncomeAmount2");
        }
        else if (!$("#IncomeSource3").is(":visible")) {
            $("#IncomeSource3").css("display", "inline");
            $("#ddl3").attr("name", "IncomeSource3");
            $("#txt3").attr("name", "IncomeAmount3");
        }

        if ($("#IncomeSource1").is(":visible") && $("#IncomeSource2").is(":visible") && $("#IncomeSource3").is(":visible")) {
            $("#btnAddIncome").css("display", "none");
        }
    });

    $(".rmIncome").click(function () {
        var num = $(this).attr("data-number");
        if (num == 1) {
            $("#IncomeSource1").css("display", "none");
            $("#ddl1").removeAttr("name");
            $("#txt1").removeAttr("name");
        }
        else if (num == 2) {
            $("#IncomeSource2").css("display", "none");
            $("#ddl2").removeAttr("name");
            $("#txt2").removeAttr("name");
        }
        else if (num == 3) {
            $("#IncomeSource3").css("display", "none");
            $("#ddl3").removeAttr("name");
            $("#txt3").removeAttr("name");
        }

        $("#btnAddIncome").css("display", "inline");
    });

});