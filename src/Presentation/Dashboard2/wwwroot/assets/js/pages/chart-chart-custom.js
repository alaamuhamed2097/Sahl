function getCanvasContext(id) {
    const element = document.getElementById(id);
    if (!element) {
        console.warn(`Canvas element with id '${id}' not found`);
        return null;
    }
    return element.getContext('2d');
}

// Function to initialize charts - call this from Blazor
// doughnutData should be a dictionary/object with label names as keys and data values as values
// barData should be a dictionary/object with label names as keys and data values as values
// Example doughnutData: { "Binary Commission": 300, "Direct Commission": 250, "Rank Commission": 200, "Level Commission": 30 }
// Example barData: { "Week 1": 20000, "Week 2": 15000, "Week 3": 10000, "Week 4": 5000 }
function initializeCharts(doughnutData = null, barData = null) {
    var ctx = getCanvasContext('doughnutChart');
    var ctx2 = getCanvasContext('barChart');

    if (!ctx || !ctx2) {
        console.error('Required canvas elements not found');
        return;
    }

    // Process doughnut data from dictionary parameter
    let doughnutLabels, doughnutValues;
    if (doughnutData && typeof doughnutData === 'object') {
        doughnutLabels = Object.keys(doughnutData);
        doughnutValues = Object.values(doughnutData);
    } else {
        // Default data if no parameter provided
        doughnutLabels = ['Binary Commission', 'Direct Commission', 'Rank Commission', 'Level Commission'];
        doughnutValues = [300, 250, 200, 30];
    }

    // Process bar data from dictionary parameter
    let barLabels, barValues;
    if (barData && typeof barData === 'object') {
        barLabels = Object.keys(barData);
        barValues = Object.values(barData);
    } else {
        // Default data if no parameter provided
        barLabels = ['Week 1', 'Week 2', 'Week 3', 'Week 4'];
        barValues = [20000, 15000, 10000, 5000];
    }

    // Generate colors dynamically based on data length
    const colors = ['#36a2eb', '#4bc0c0', '#ff6385', '#ff9f40', '#ffcd56', '#c9cbcf', '#4bc0c0', '#36a2eb'];
    const doughnutBackgroundColor = colors.slice(0, doughnutValues.length);
    const barBackgroundColor = colors.slice(0, barValues.length);

    var doughnutChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: doughnutLabels,
            datasets: [{
                label: 'Commission Data',
                data: doughnutValues,
                backgroundColor: doughnutBackgroundColor,
                borderColor: Array(doughnutValues.length).fill('#fff'),
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            return tooltipItem.label + ': ' + tooltipItem.raw;
                        }
                    }
                }
            }
        }
    });

    var barChart = new Chart(ctx2, {
        type: 'bar',
        data: {
            labels: barLabels,
            datasets: [{
                label: 'Weekly Data',
                data: barValues,
                backgroundColor: barBackgroundColor,
                borderColor: Array(barValues.length).fill('#fff'),
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            return tooltipItem.label + ': ' + tooltipItem.raw;
                        }
                    }
                }
            }
        }
    });

    // Store chart instances globally for updates
    window.doughnutChartInstance = doughnutChart;
    window.barChartInstance = barChart;

    return { doughnutChart, barChart }; // Return chart instances for further manipulation if needed
}

// Function to update doughnut chart with new data
function updateDoughnutChart(doughnutData) {
    if (window.doughnutChartInstance && doughnutData && typeof doughnutData === 'object') {
        const labels = Object.keys(doughnutData);
        const data = Object.values(doughnutData);

        // Generate colors dynamically based on data length
        const colors = ['#36a2eb', '#4bc0c0', '#ff6385', '#ff9f40', '#ffcd56', '#c9cbcf', '#4bc0c0', '#36a2eb'];
        const backgroundColor = colors.slice(0, data.length);

        // Update chart data
        window.doughnutChartInstance.data.labels = labels;
        window.doughnutChartInstance.data.datasets[0].data = data;
        window.doughnutChartInstance.data.datasets[0].backgroundColor = backgroundColor;
        window.doughnutChartInstance.data.datasets[0].borderColor = Array(data.length).fill('#fff');

        // Update the chart
        window.doughnutChartInstance.update();
    }
}

// Function to update bar chart with new data
function updateBarChart(barData) {
    if (window.barChartInstance && barData && typeof barData === 'object') {
        const labels = Object.keys(barData);
        const data = Object.values(barData);

        // Generate colors dynamically based on data length
        const colors = ['#36a2eb', '#4bc0c0', '#ff6385', '#ff9f40', '#ffcd56', '#c9cbcf', '#4bc0c0', '#36a2eb'];
        const backgroundColor = colors.slice(0, data.length);

        // Update chart data
        window.barChartInstance.data.labels = labels;
        window.barChartInstance.data.datasets[0].data = data;
        window.barChartInstance.data.datasets[0].backgroundColor = backgroundColor;
        window.barChartInstance.data.datasets[0].borderColor = Array(data.length).fill('#fff');

        // Update the chart
        window.barChartInstance.update();
    }
}

// Function to update both charts at once
function updateBothCharts(doughnutData, barData) {
    updateDoughnutChart(doughnutData);
    updateBarChart(barData);
}