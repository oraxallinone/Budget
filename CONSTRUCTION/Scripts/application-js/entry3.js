
$(document).ready(function () {
    $('#hidelIdHolder').val('0');

    $(document).ajaxStart(function () {
        $(".content").addClass('body-blure');
    });
    $(document).ajaxStop(function () {
        $(".content").removeClass('body-blure');
    });

    $('#btnSubmit').val("Save");

    let previousSelectTrID = 0;


    $('#tbldashboardCalc').hide();


    $('#ddlMonth').change(function () {
        getList();
        getTotal();
    });

    $('#ddlGroup1').change(function () {
        $("#ddlGroup2  option:first").prop("selected", "selected");
        $("#ddlGroup3  option:first").prop("selected", "selected");
        getList();
        getTotal();
    });

    $('#ddlGroup2').change(function () {
        $("#ddlGroup1  option:first").prop("selected", "selected");
        $("#ddlGroup3  option:first").prop("selected", "selected");
        getList();
        getTotal();
    });


    $('#ddlGroup3').change(function () {
        $("#ddlGroup1  option:first").prop("selected", "selected");
        $("#ddlGroup2  option:first").prop("selected", "selected");
        getList();
        getTotal();
    });



    $('#inputDetails').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            let check = $('#hidelIdHolder').val();
            if (check != '0') {
                updateExpensiveData()
            }
            else {
                saveExpensiveData();
            }
        }
    });


    function getList() {
        let forYear = $('select#ddlYear option:selected').val();
        let forMonth = $('select#ddlMonth option:selected').val();
        let forGroup1 = $('select#ddlGroup1 option:selected').val();
        let forGroup2 = $('select#ddlGroup2 option:selected').val();
        let forGroup3 = $('select#ddlGroup3 option:selected').val();

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

                $('#tbldashboardCalc').show();
                $('.total-fix').html(result.monthMaster.salary);


                var tblRows = "";
                $.each(result.monthTransList, function (a, b) {
                    var tblRow = '<tr id="' + b.id + ' "> '


                        + '<td class="col3">' + b.createdDate + ' <span class="spn-clr">' + dateToDay(b.createdDate) + '</span> </td>'
                        + '<td class="col1">' + b.price + ' | <a id="btnView" class="' + b.id + '" > <i class="fa-solid fa-eye"></i></a> | <input type="checkbox" id="' + b.price + '" class="checkRow" /> </td>'
                        + '<td class="col2">' + b.details + '</td>'
                        + '<td class="col4">' + b.group2 + '</td>'
                        + '<td class="">' + b.group1 + '</td>'
                        + '<td class="">' + b.group3 + '</td>'
                        + '<td> <a id="' + b.id + 'remove" class="btnRemove" href="#"> <i class="fa-solid fa-trash"></i></a> </td>'

                        + '</tr>';
                    tblRows = tblRows + tblRow;
                });

                $("#tblExpensive").empty();
                $("#tblExpensive").append(tblRows);

                getTotal();
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
                $('#btnSubmit').val("Update?");
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


    $('#btnSubmit').click(function () {
        let check = $('#hidelIdHolder').val();
        if (check != '0') {
            updateExpensiveData()
        }
        else {
            saveExpensiveData();
        }
    });


    function saveExpensiveData() {
        //var group2 = $('select#optiongroup2 option:selected').val();
        var details = $('#inputDetails').val();
        var price = $('#inputPrice').val();
        var createdDate = $('#createdDate').val();
        if (details == '' || price == '' || createdDate == '') {
            alert("empty");
            return false;
        }

        let CreateExpensiveViewModel = {
            id: parseInt(0),
            group1: '',
            group2: '',
            details: details,
            price: parseInt(price),
            createdDate: new Date(createdDate)
        }

        $.ajax({
            url: '/Orax/AddExpensive',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(CreateExpensiveViewModel),
            success: function (data) {
                if (data == 'success') {
                    getList();
                    getTotal();
                    $('#inputPrice').val('');
                    $('#inputDetails').val('');
                    $('#inputPrice').focus();
                    $('#hidelIdHolder').val('0');
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }

    function updateExpensiveData() {
        let id = $('#hidelIdHolder').val();
        let price = $('#inputPrice').val();
        let details = $('#inputDetails').val();

        let CreateExpensiveViewModel = {
            id: parseInt(id),
            details: details,
            price: parseInt(price),
        }

        $.ajax({
            url: '/Orax/UpdateExpensive',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(CreateExpensiveViewModel),
            success: function (data) {
                if (data == 'success') {
                    getList();
                    getTotal();
                    $('#inputPrice').val('');
                    $('#inputDetails').val('');
                    $('#inputPrice').focus();
                    $('#btnSubmit').val("Save");
                    $('#createdDate').val('');
                    $('#hidelIdHolder').val('0');
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }

    function getTotal() {

        var forMonth = $('select#ddlMonth option:selected').val();

        if (forMonth != "-- select --") {
            let thisMonthSalary = $('.total-fix').html();
            var total = 0.0;
            $('.col1').each(function (a, b) {
                total = total + parseFloat(b.innerText);
            });

            $('.total-fix').html(thisMonthSalary);
            $('.total-spend').html(total);
            $('.total_remaining').html(thisMonthSalary - total);
        }
    }


    $(document).on('change', '.group1', function () {
        var id = $(this).attr('id');

        let CreateExpensiveViewModel = {
            id: parseInt(id),
            group1: $(this).find(":selected").val(),
            group2: '',
            details: '',
            price: 0,
            createdDate: new Date(createdDate)
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
                    getTotal();


                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });

    });


    $(document).on('change', '.group2', function () {
        var id = $(this).attr('id');
        id3 = id.split('_');

        let CreateExpensiveViewModel = {
            id: parseInt(id3[0]),
            group1: '',
            group2: $(this).find(":selected").val(),
            details: '',
            price: 0,
            createdDate: new Date(createdDate)
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
                    getTotal();
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });

    });

    $(document).on('change', '.group3', function () {
        var id = $(this).attr('id');
        id3 = id.split('_');
        let CreateExpensiveViewModel = {
            id: parseInt(id3[0]),
            group1: '',
            group2: '',
            group3: $(this).find(":selected").val(),
            details: '',
            price: 0,
            createdDate: new Date(createdDate)
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
                    getTotal();
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
                    getTotal();
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



    $(document).on('click', '.checkRow', function () {
        let total = 0;
        let checkCount = 0;
        $('.checkRow').each(function () {

            if ($(this).prop('checked') == true) {
                let id = $(this).attr('id');
                total = total + parseInt(id);
                $("#totalSpan").html(total);
                checkCount = 1;
            }
            if (checkCount == 0) {
                $("#totalSpan").html(0);
            }
        });
        total = 0;
        checkCount = 0;
    });


    $(document).on('click', '#tblExpensive > tr', function () {
        $('#tblExpensive > tr').removeClass('dynamic-bg-tr');
        $(this).addClass('dynamic-bg-tr');
        previousSelectTrID = $(this).attr('id');
    });

});