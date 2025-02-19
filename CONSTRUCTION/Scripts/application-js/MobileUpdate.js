
$(document).ready(function () {
    $('#hidelIdHolder').val('0');

    $(document).ajaxStart(function () {
        $(".content").addClass('body-blure');
    });
    $(document).ajaxStop(function () {
        $(".content").removeClass('body-blure');
    });


    let previousSelectTrID = 0;




    $('#ddlMonth').change(function () {
        getList();
    });




    function getList() {
        let forYear = $('select#ddlYear option:selected').val();
        let forMonth = $('select#ddlMonth option:selected').val();
        let forGroup1 = '';
        let forGroup2 = '';
        let forGroup3 = '';

        if (forGroup2 != "" && forGroup1 != "" && forGroup3 != "") {
            $("#tblExpensive").empty();
            return;
        }

        if (forMonth == "-- select --") {
            $("#tblExpensive").empty();
            return;
        }

        let GetExpensiveViewModel = {
            forYear: forYear,//
            forMonth: forMonth,//
            group1: forGroup1,//20 30 50
            group2: forGroup2,// 1.5k
            group3: forGroup3//sip
        }


        $.ajax({
            type: "POST",
            url: '/Orax/GetAllExpensive',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(GetExpensiveViewModel),
            success: function (result) {



                var tblRows = "";
                $.each(result.monthTransList, function (a, b) {
                    var tblRow = '<tr style="white-space: nowrap;" id="' + b.id + ' "> '
                        + ' <td class="col3">' + b.createdDate + ' <span class="spn-clr">' + dateToDay(b.createdDate) + '</span> </td>'
                        + '<td class="col1">' + b.price + ' &nbsp </td>'
                        + '<td class="col2"> &nbsp ' + b.details + '</td>'
                        + '<td class="col4 box-default2">' + b.group2 + '</td>'
                        + '<td style="width:66px;"><select id="' + b.id + '_" class="form-control wd-select1 form-control-sm group2"> <option value="">---</option> <option value="beer-1.5k">beer-1.5k</option> <option value="emi">emi</option> <option value="essential-5k">essential-5k</option> <option value="extra-10k">extra-10k</option> <option value="food-5k" style="background-color:yellow;">food-5k</option> <option value="fromSaving-60k">fromSaving-60k</option> <option value="h-rent">h-rent</option> <option value="oil-4k">oil-4k</option> <option value="saving">saving</option> <option value="unplaned">unplaned</option><option value="creditCard">Credit Card</option> <option value="mrg">mrg</option>  <option value="goa">Goa</option> <option value="borrow">borrow</option> </select></td>'
                        + ' <td> ' +
                        '<table style="width:100%;margin: -3px;"><tr>'
                        + '<td class="box-default">' + b.group1 + '</td>'
                        + '<td style="width:66px;"><select id="' + b.id + '" class="form-control wd-select2 form-control-sm group1"> <option value="">---</option> <option value="need">Need</option> <option value="want">Want</option> <option value="save">Save</option> </select></td>'

                        + '<td class="box-default-3">' + b.group3 + '</td>'
                        + '<td style="width:66px;"><select id="' + b.id + '" class="form-control wd-select3 form-control-sm group3"> <option value="">---</option><option value="repeat">Repeat</option> <option value="carOil">Car Oil</option> <option value="bikeOil">Bike Oil</option> <option value="LIC">LIC</option><option value="stock">stock</option> </select></td>'

                        + ' </tr>'
                        + '</table>   </td>'
                        + '</tr>';
                    tblRows = tblRows + tblRow;
                });

                $("#tblExpensive").empty();
                $("#tblExpensive").append(tblRows);

                assignClass();

                $('#tblExpensive > #' + previousSelectTrID).addClass('dynamic-bg-tr');
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }


    $(document).on('click', '#btnView', function () {
        $('#hidelIdHolder').val('0');
        var id = $(this).attr('class');

        let CreateExpensiveViewModel = {
            id: parseInt(id),
            details: '',
            price: 0,
        }
        $.ajax({
            url: '/Orax/GetExpensive',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(CreateExpensiveViewModel),
            success: function (data) {
                $('#hidelIdHolder').val(id);
                $('#createdDate').val(data.createdDate);
                $('#inputPrice').val(data.price);
                $('#inputDetails').val(data.details);
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    });


    function dateToDay(inputDate) {
        var days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
        var d = new Date(inputDate);
        return days[d.getDay()];
    }


    




    


    $(document).on('change', '.group1', function () {
        debugger
        var id = $(this).attr('id');

        let CreateExpensiveViewModel = {
            id: parseInt(id),
            group1: $(this).find(":selected").val(),
            group2: '',
            details: '',
            price: 0,
            createdDate: null
        }
        $.ajax({
            url: '/Orax/UpdateGroup1',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(CreateExpensiveViewModel),
            success: function (data) {
                if (data == 'success') {
                    getList();
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });

    });


    $(document).on('change', '.group2', function () {
        debugger
        var id = $(this).attr('id');
        id3 = id.split('_');

        let CreateExpensiveViewModel = {
            id: parseInt(id3[0]),
            group1: '',
            group2: $(this).find(":selected").val(),
            details: '',
            price: 0,
            createdDate: null
        }
        $.ajax({
            url: '/Orax/UpdateGroup2',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(CreateExpensiveViewModel),
            success: function (data) {
                if (data == 'success') {
                    getList();
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });

    });

    $(document).on('change', '.group3', function () {
        debugger
        var id = $(this).attr('id');
        id3 = id.split('_');
        let CreateExpensiveViewModel = {
            id: parseInt(id3[0]),
            group1: '',
            group2: '',
            group3: $(this).find(":selected").val(),
            details: '',
            price: 0,
            createdDate: null
        }
        $.ajax({
            url: '/Orax/UpdateGroup3',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(CreateExpensiveViewModel),
            success: function (data) {
                if (data == 'success') {
                    getList();
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });

    });

    $(document).on('click', '.btnRemove', function () {
        confirm('want to delete?');
        var id = $(this).attr('id')
        let CreateExpensiveViewModel = {
            id: parseInt(id),
            group1: '',
            group2: '',
            details: '',
            price: 0,
            createdDate: new Date(createdDate)
        }
        $.ajax({
            url: '/Orax/removeRecord',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(CreateExpensiveViewModel),
            success: function (data) {
                if (data == 'success') {
                    alert('removed');
                    getList();
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });

    });

    function assignClass() {
        $(".box-default").each(function () {
            var calssData = $(this).html();
            if (calssData == "save") {
                $(this).addClass('box-save');
            }
            else if (calssData == "want") {
                $(this).addClass('box-want');
            }
            else if (calssData == "need") {
                $(this).addClass('box-need');
            }
            else {
                $(this).addClass('box-non');
            }
        });

        $(".box-default2").each(function () {
            var calssData = $(this).html();
            if (calssData != "null") {
                $(this).addClass('box-assigned');
            }
            else {
                $(this).addClass('box-non');
            }
        });

        $(".box-default-3").each(function () {
            var calssData = $(this).html();
            if (calssData != "null") {
                if (calssData == "repeat") {
                    $(this).addClass('box-assigned-repeat');
                }
                $(this).addClass('box-assigned-3');
            }
            else {
                $(this).addClass('box-non-3');
            }
        });
    }




    $(document).on('click', '#tblExpensive > tr', function () {
        $('#tblExpensive > tr').removeClass('dynamic-bg-tr');
        $(this).addClass('dynamic-bg-tr');
        previousSelectTrID = $(this).attr('id');
    });

});