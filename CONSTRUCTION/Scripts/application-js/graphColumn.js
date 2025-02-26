/// <reference path="graph.js" /> dataPoints: result.barGraphData


$(document).ready(function () {


    function loadColumnGraph() {
        let columnChatHeader = '';

        let forGroup1 = $('select#ddlGroup1 option:selected').val();
        let forGroup2 = $('select#ddlGroup2 option:selected').val();
        let forGroup3 = $('select#ddlGroup3 option:selected').val();

        if (forGroup2 != "" && forGroup1 != "" && forGroup3 != "") { return; }

        if (forGroup1 != "") { columnChatHeader = forGroup1 }
        if (forGroup2 != "") { columnChatHeader = forGroup2 }
        if (forGroup3 != "") { columnChatHeader = forGroup3 }

        let data = {
            forMonth: '',//
            group1: forGroup1,//20 30 50
            group2: forGroup2,// 1.5k
            group3: forGroup3//sip
        }


        $.ajax({
            type: "POST",
            url: '/Orax/GetColumnChart',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            success: function (result) {
                if (result.barGraphData.length > 0) {
                    var chart = new CanvasJS.Chart("chartContainer",
                        {
                            title: { text: columnChatHeader },
                            axisY: { minimum: 0 },//title: "Reserves(MMbbl)",
                            legend: { verticalAlign: "bottom", horizontalAlign: "center" },
                            data: [{
                                color: "#B0D0B0",
                                indexLabelPlacement: "inside", // Ensure labels appear inside
                                indexLabelOrientation: "horizontal",
                                indexLabelFontColor: "black", // Set text color
                                indexLabelFontWeight: "bold", // Make text bold
                                indexLabelFontSize: 14, // Adjust label font size
                                type: "column",
                                showInLegend: true,
                                legendMarkerType: "none",
                                dataPoints: result.barGraphData.map(point => ({ ...point, indexLabel: point.label })),
                            }]
                        });
                    chart.render();
                }
                if (result.monthMstr.length > 0) {
                    let finalData = "";


                    let td1 = "";
                    let td2 = "";

                    

                    var tblRows = "";
                    $.each(result.monthMstr, function (a, b) {
                        td1 = td1 + '<td>' + b.monthName + '</td>';
                        td2 = td2 + '<td>' + b.need + '</td>';
                    });

                    let tr1 = '<tr>' + td1 + '</tr>'
                    let tr2 = '<tr>' + td2 + '</tr>'

                    finalData = tr1 + tr2;

                    $("#monthMasterTable").empty();
                    $("#monthMasterTable").append(finalData);
                }

            },
            error: function (xhr) {
                alert('Error: ' + xhr.statusText);
            }
        });
    }



    $('#ddlGroup1').change(function () {
        $("#ddlGroup2  option:first").prop("selected", "selected");
        $("#ddlGroup3  option:first").prop("selected", "selected");
        loadColumnGraph();
    });

    $('#ddlGroup2').change(function () {
        $("#ddlGroup1  option:first").prop("selected", "selected");
        $("#ddlGroup3  option:first").prop("selected", "selected");
        loadColumnGraph();
    });


    $('#ddlGroup3').change(function () {
        $("#ddlGroup1  option:first").prop("selected", "selected");
        $("#ddlGroup2  option:first").prop("selected", "selected");
        loadColumnGraph();
    });













    


    



});