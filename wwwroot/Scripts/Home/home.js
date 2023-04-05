var currentRound;
var currentSeason;
var standingsTable;
var chart = null;

$(document).ready(function () {
    $('a[href="/Home"]').addClass('active');
    $.ajax({
        async: true,
        type: "GET",
        url: "Home/GetRecentResults",
        data: {},
        success: function (data) {
            $('#recentDetails').text(`- Race ${data.round} of ${data.season}`);
            currentRound = data.round;
            currentSeason = data.season;
            console.log(data)
            var tableData = [];
            for (var i = 0; i < data.standings.length; i++) {
                var standing = data.standings[i];
                tableData.push({
                    position: standing.position,
                    name: standing.driver.fullName,
                    constructor: standing.constructor.name,
                    points: standing.points,
                    id: standing.driver.driverId,
                    driverDetails: standing.driver
                });
            }
            buildRecentStandingsTable(tableData);
            setTimeout(function () {
                $('#recentStandingsTable tr:first-child td:first-child').click();
            },200)
        },
        error: function (error) {
            console.log(error)
        }
    });
})

function buildRecentStandingsTable(tabledata) {
    standingsTable = $('#recentStandingsTable').DataTable({
        data: tabledata,
        paging: false,
        dom: "t",
        scrollCollapse: true,
        scrollX: "100%",
        scrollY: "calc(50vh - 250px)",
        autoWidth: true,
        columns: [
            {
                data: "position",
                render: function (data) {
                    return data;
                }
            },
            {
                data: "name",
                render: function (data) {
                    return data;
                }
            },
            {
                data: "constructor",
                render: function (data) {
                    return data;
                }
            },
            {
                data: "points",
                render: function (data) {
                    return data;
                }
            },
            {
                data: "id",
                render: function (data) {
                    return data;
                }
            },
            {
                data: "driverDetails"
            }
        ],
        columnDefs: [
            { orderable: false, targets: [0, 1, 2, 3] },
            { visible: false, targets: [4, 5] }
        ],
        initComplete: function () {
            $('.sorting_disabled').removeClass('sorting_asc');
        }
    })
}

// work in progress
$(document).on("click", "#recentStandingsTable tr", function (e) {
    $('tr.active').removeClass('active');
    var row = $(e.target).closest('tr');
    $(row).addClass('active');
    getLapTimes(currentRound, standingsTable.row(row).data().id, standingsTable.row(row).data().name);
})

function getLapTimes(round, driverId, driverName) {
    $.ajax({
        async: false,
        type: "GET",
        url: "Home/GetLapTime",
        data: {
            round: round,
            year: "current",
            driverId: driverId
        },
        success: function (data) {
            console.log(data);
            $('#raceName').text(`- ${driverName}`);
            if (data.circuit.circuitName != null) {
                $('#recentDetails').text(`- ${data.circuit.circuitName}`);
            }
            var graphData = [];
            var categories = [];
            for (var i = 0; i < data.laps.length; i++) {
                if (data.laps[i] !== null) {
                    graphData.push(convertToSeconds(data.laps[i].timings[0].time));
                    categories.push(i + 1);
                }
            }
            buildChart(graphData, categories);
        },
        error: function (error) {
            console.log(error)
        }
    })
}

function buildChart(graphData, categories) {
    if (chart != null) {
        chart.destroy();
    }
    console.log(graphData)
    var options = {
        series: [{
            name: "Lap Time",
            data: graphData
        }],
        colors: ['var(--ct-main-accent-colour)', 'var(--ct-secondary-accent-colour)', 'var(--ct-tertiary-accent-colour)'],
        chart: {
            type: 'area',
            height: 320,
            zoom: {
                enabled: false
            },
            toolbar: {
                show: false,
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'straight'
        },
        xaxis: {
            type: 'categories',
            categories: categories,
            tickAmount: 6,
            title: {
                text: 'Laps',
                style: {
                    fontFamily: 'var(--ct-header-fonts)',
                    fontWeight: 700,
                    fontSize: '14px'
                }
            },
            axisBorder: {
                show: false,
            }
        },
        yaxis: {
            show: true,
            seriesName: 'Lap Times',
            labels: {
                show: true,
            },
            title: {
                text: 'Time (s)',
                style: {
                    fontFamily: 'var(--ct-header-fonts)',
                    fontWeight: 700,
                    fontSize: '14px'
                }
            }
        },
        legend: {
            horizontalAlign: 'left'
        },
        tooltip: {
            enabled: true,
            theme: 'dark',
            x: {
                formatter: function (value) {
                    return `Lap ${value}`;
                }
            },
            y: {
                formatter: function (value) {
                    var minutes = Math.floor(value / 60);
                    var remainingSeconds = value % 60;
                    return `${minutes.toString().padStart(2, '0')}:${remainingSeconds.toString().padStart(2, '0')}`;
                }
            },
        },
        stroke: {
            curve: 'smooth',
        },
    };

    chart = new ApexCharts(document.querySelector("#lapTimeChart"), options);
    chart.render();
}


function getDriverInfo(id) {
    $.ajax({
        async: false,
        type: "GET",
        url: "Home/GetDriverInfo",
        data: {
            id: id
        },
        success: function (data) {
            console.log(data)
        },
        error: function (error) {
            console.log(error)
        }
    })
}

function convertToSeconds(hhmmss) {
    var mm = hhmmss.slice(hhmmss.indexOf(":") + 1, hhmmss.indexOf(":") + 3);
    var ss = hhmmss.slice(hhmmss.indexOf(":") + 4, hhmmss.indexOf(":") + 6)
    var sec1 = parseInt(mm) * 60;
    return sec1 + parseInt(ss);
}