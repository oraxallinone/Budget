$(document).ready(function () {
    const d = new Date();
    let month = d.getMonth();
    let date = d.getDate();

    let _nowMonth = d.getMonth() + 1;
    let _month = d.getMonth() + 2;

    let __nowMonth = "0" + _nowMonth.toString();

    var _todayDate = `${d.getFullYear()}-${__nowMonth}-${d.getDate()}`;

    $('#ddlMonth').prop('selectedIndex', _month);
    $('#createdDate').val(_todayDate)

    debugger
    let monthSelected = $('select#ddlMonth option:selected').val();
    if (monthSelected != "-- select --") {
        getList();
    }




    $(document).ajaxStart(function () {
        $(".content").addClass('body-blure');
    });
    $(document).ajaxStop(function () {
        $(".content").removeClass('body-blure');
    });

    $('#ddlMonth').change(function () {
        getList();
    });



    function getList() {
        var forYear = $('select#ddlYear option:selected').val();
        let forMonth = $('select#ddlMonth option:selected').val();


        if (forMonth == "-- select --") {
            alert('select month')
            return;
        }


        let GetExpensiveViewModel = {
            forYear: forYear,
            forMonth: forMonth,
            group1: '',
            group2: '',
            group3: ''
        }


        $.ajax({
            type: "POST",
            url: '/Orax/GetBymonthMobile',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(GetExpensiveViewModel),
            success: function (result) {

                $("#expMobList").empty();
                var tblRows = "";
                $.each(result.monthTransList, function (a, b) {
                    var tblRow = '<tr>'
                        + '<td style="width:15%" class="col21" >' + b.price + '</td>'
                        + '<td style="width:50%" class="col22" >' + b.details + '</td>'
                        + '<td style="width:33%" class="col23" >' + b.createdDate.split('-')['2']  + ' '+dateToDay(b.createdDate) + '</td>'
                        + '<td style="width:5%" > <a id="' + b.id + 'remove" class="btnRemove" href="#"> <i class="fa-solid fa-trash"></i></a> </td>'
                        + '</tr>';
                    tblRows = tblRows + tblRow;
                });

                $("#expMobList").append(tblRows);

            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }


    $('#inputDetails').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            saveExpensiveData();
        }
    });

    $('#btnSubmit').click(function () {
        saveExpensiveData();
    });

    function saveExpensiveData() {
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
                    
                    $('#inputPrice').val('');
                    $('#inputDetails').val('');
                    $('#inputPrice').focus();
                }
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }













   





    $(document).on('click', '#btnView', function () {
        var id = $(this).attr('class');
        alert(id);
    });


    function dateToDay(inputDate) {
        var days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
        var d = new Date(inputDate);
        return days[d.getDay()];
    }

   

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


});