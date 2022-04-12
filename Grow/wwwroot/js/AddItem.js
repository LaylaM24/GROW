$(document).ready(function () {

    $('#btnFilter').click(function () {

        var filter = $('#ddlCat option:selected').val();
        var search = $('#txtSearch').val();

        if (filter != -1 || search.trim()) {

            $.ajax({
                type: "POST",
                url: "/TransactionDetails/GetItems",
                data: {
                    filter: filter,
                    search: search
                },
                dataType: "json",
                success: function (data) {

                    var itemList = $('#itemList');
                    itemList.empty();

                    $.each(data, function (i) {

                        itemList.append($('<option value="' + data[i].id + '" >' + data[i].displayText + '</option>'));

                    });
                }
            });
        }

    });

    $('#btnClear').click(function () {

        $('#ddlCat').prop("selectedIndex", 0);
        $('#txtSearch').val('');

        $.ajax({
            type: "POST",
            url: "/TransactionDetails/GetItems",
            data: {
                filter: -1,
                search: ''
            },
            dataType: "json",
            success: function (data) {

                var itemList = $('#itemList');
                itemList.empty();

                $.each(data, function (i) {

                    itemList.append($('<option value="' + data[i].id + '" >' + data[i].displayText + '</option>'));

                });
            }
        });

    });

    $('#itemList').change(function () {

        var selected = $('#itemList :selected')[0];

        $('#txtItem').val(selected.text);
        $('#txtItemID').val(selected.value);

        $("#submitItemForm").prop("disabled", false);

    });

});