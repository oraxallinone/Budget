$(document).ready(function () {

    let objectTransfer = null;

    $('#ddlMonth').change(function () {
        loadCharts();
    });


    function loadCharts() {
        var forYear = $('select#ddlYear option:selected').val();
        var forMonth = $('select#ddlMonth option:selected').val();

        let GetExpensiveViewModel = {
            forYear: forYear,
            forMonth: forMonth
        }

        $.ajax({
            type: "POST",
            url: '/Orax/GetAsBudgetRule',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(GetExpensiveViewModel),
            success: function (result) {

                objectTransfer = result.monthMaster;
                let forMonth = result.monthMaster;
                let total = 0;

                $.each(result.groupByBudgetRule, function (a, b) {

                    if (b.label == "need") {
                        let remainingNeed = (forMonth.need - b.y).toFixed(2);
                        let pMonthNeed = (Math.trunc((100 / forMonth.salary) * b.y)).toFixed(2);
                        let pSpendNeed = (Math.trunc((100 / forMonth.need) * b.y)).toFixed(2);
                        b.label = b.label.substring(0, 4) + " (" + pMonthNeed + "%_consumed) " + " ("+remainingNeed + "_remain) (" + pSpendNeed + "%_consumed)";
                        total = total + b.y;
                    }
                    if (b.label == "save") {
                        let remainingSave = (forMonth.saving - b.y).toFixed(2);
                        let pMonthSave = (Math.trunc((100 / forMonth.salary) * b.y)).toFixed(2);
                        let pSpendSave = (Math.trunc((100 / forMonth.saving) * b.y)).toFixed(2);
                        b.label = b.label.substring(0, 4) + " (" + pMonthSave + "%_consumed) (" + remainingSave + "_remain) (" + pSpendSave + "%_consumed)";
                        total = total + b.y;
                    }
                    if (b.label == "want") {
                        let remainingWant = (forMonth.want - b.y).toFixed(2);
                        let pMonthWant = (Math.trunc((100 / forMonth.salary) * b.y)).toFixed(2);
                        let pSpendWant = (Math.trunc((100 / forMonth.want) * b.y)).toFixed(2);
                        b.label = b.label.substring(0, 4) + " (" + pMonthWant + "%_consumed) (" + remainingWant + "_remain) (" + pSpendWant + "%_consumed)";
                        total = total + b.y;
                    }
                });

                $('.total-fix').html(result.monthMaster.salary);
                $('.total-spend').html(total);
                $('.total_remaining').html(result.monthMaster.salary - total);


                let options = {
                    data: [{
                        type: "pie",
                        startAngle: 45,
                        showInLegend: "true",
                        legendText: "{label}",
                        indexLabel: "{label} {y}",
                        yValueFormatString: "#,##0.#" % "",
                        dataPoints: result.groupByBudgetRule
                    }]
                };
                $("#chartContainerThree").CanvasJSChart(options);
                $('.canvasjs-chart-credit').remove();

                getChartByBudgets();
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }


    function dynamicTr(a, b) {
        debugger
        let _budget = 0;
        let _remaining = 0;
        let _icon = "";

        if (b.label == "beer-1.5k") { _budget = 1500; _icon = "fa-solid fa-champagne-glasses"; }
        else if (b.label == "essential-5k") { _budget = 5000; _icon = "fa-solid fa-users-line"; }
        else if (b.label == "extra-10k") { _budget = 10000; _icon = "fa-solid fa-e"; }
        else if (b.label == "food-5k") { _budget = 5000; _icon = "fa-solid fa-utensils"; }
        else if (b.label == "fromSaving-60k") { _budget = 60000; _icon = "fa-solid fa-comments-dollar"; }
        else if (b.label == "h-rent") { _budget = 6000; _icon = "fa-solid fa-house-flag"; }
        else if (b.label == "oil-4k") { _budget = 4000; _icon = "fa-solid fa-gas-pump"; }
        else if (b.label == "saving") { _budget = 10000; _icon = "fa-solid fa-piggy-bank"; }
        else {
            _budget = 0; _icon = "fa-solid fa-folder-open";
        }
        //fa-solid fa-credit-card

        _remaining = parseInt(_budget) -parseInt(b.y);


        var data = '<div class="col-md-3 col-sm-6 col-12">'
        + '<div class="info-box">'
        + '<span class="info-box-icon bg-info"><i class="'+_icon+'"></i></span>'
        + '<div class="info-box-content">'
        + '<span class="info-box-text to-upper">' + b.label + '</span>'
        + '<table>'
        + '<tr> <td>Budget</td><td>Spend</td><td>Remaining</td> </tr>'
        + '<tr>'
        + '<td><span class="info-box-number">' + _budget + '</span></td>'
        + '<td><span class="info-box-number">'+b.y+'</span></td>'
        + '<td><span class="info-box-number">' + _remaining + '</span></td>'
        + '</tr>'
        + '</table>'
        + '</div>'
        + '</div>'
        + '</div>';
        return data;
    }

    function getChartByBudgets() {
        var forYear = $('select#ddlYear option:selected').val();
        var forMonth = $('select#ddlMonth option:selected').val();

        let GetExpensiveViewModel = {
            forYear: forYear,
            forMonth: forMonth
        }

        $.ajax({
            type: "POST",
            url: '/Orax/GetAllDataBudget',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(GetExpensiveViewModel),
            success: function (result) {
                $('#divrenderCard').empty();

                let _cardHtmls = '';

                $.each(result, function (a, b) {
                    _cardHtmls = _cardHtmls + dynamicTr(a, b)

                    if (b.label == "beer-1.5k") { b.label = b.label.substring(0, 4) + " (" + (1500 - b.y).toFixed(2) + "r)"; }
                    if (b.label == "essential-5k") { b.label = b.label.substring(0, 4) + " (" + (5000 - b.y).toFixed(2) + "r)"; }
                    if (b.label == "extra-10k") { b.label = b.label.substring(0, 4) + " (" + (10000 - b.y).toFixed(2) + "r)"; }
                    if (b.label == "food-5k") { b.label = b.label.substring(0, 4) + " (" + (5000 - b.y).toFixed(2) + "r)"; }
                    if (b.label == "fromSaving-60k") { b.label.substring(0, 4) = b.label + " (" + (60000 - b.y).toFixed(2) + "r)"; }
                    if (b.label == "h-rent") { b.label = b.label.substring(0, 4) + " (" + (6000 - b.y).toFixed(2) + "r)"; }
                    if (b.label == "oil-4k") { b.label = b.label.substring(0, 4) + " (" + (4000 - b.y).toFixed(2) + "r)"; }
                    if (b.label == "saving") { b.label = b.label.substring(0, 4) + " (" + (objectTransfer.saving - b.y).toFixed(2) + "r)"; }

                    //if (b.label == "unplaned") { b.label = b.label + "_" + (15000 - b.y); }


                });

                //assign  
                
                $('.expectedNeed').html(objectTransfer.need);
                $('.expectedWant').html(objectTransfer.want);
                $('.expectedSave').html(objectTransfer.saving);

                $('#divrenderCard').append(_cardHtmls);

                

                var options = {
                    data: [{
                        type: "pie",
                        startAngle: 45,
                        showInLegend: "true",
                        legendText: "{label}",
                        indexLabel: "{label} ({y})",
                        yValueFormatString: "#,##0.#" % "",
                        dataPoints: result
                    }]
                };
                $("#chartContainer").CanvasJSChart(options);
                $('.canvasjs-chart-credit').remove();
            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }


    //function dateToDay(inputDate) {
    //    var days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    //    var d = new Date(inputDate);
    //    return days[d.getDay()];
    //}
});