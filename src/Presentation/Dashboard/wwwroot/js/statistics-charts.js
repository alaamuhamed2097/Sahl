'use strict';

// Statistics Chart Functions for Admin Dashboard with On-Demand Highcharts Loading
window.statisticsCharts = {
    colors: {
        primary: '#19BCBF',
        secondary: '#463699', 
        success: '#13bd8a',
        warning: '#FF9764',
        danger: '#FF425C',
        info: '#2DCEE3'
    },
    
    initialized: false,
    
    // Version-safe color helper (supports Highcharts.color and Highcharts.Color)
    rgba: function(hex, alpha) {
        try {
            if (typeof Highcharts !== 'undefined') {
                if (typeof Highcharts.color === 'function') {
                    return Highcharts.color(hex).setOpacity(alpha).get('rgba');
                }
                if (typeof Highcharts.Color === 'function') {
                    return Highcharts.Color(hex).setOpacity(alpha).get('rgba');
                }
            }
        } catch (e) {
            console.warn('Highcharts color processing failed, falling back to hex:', e);
        }
        return hex; // Fallback
    },
    
    init: function() {
        if (this.initialized) return;
        
        // Ensure Highcharts is loaded
        if (typeof Highcharts === 'undefined') {
            console.warn('Highcharts library not found. Please include Highcharts library.');
            return false;
        }
        
        this.initialized = true;
        console.log('Statistics charts initialized');
        return true;
    },

    // Load Highcharts if needed
    ensureHighcharts: function() {
        if (typeof Highcharts !== 'undefined') {
            return Promise.resolve(this.init());
        }

        if (window.ScriptLoader && window.ScriptLoader.loadHighcharts) {
            return window.ScriptLoader.loadHighcharts()
                .then(() => this.init())
                .catch(error => {
                    console.error('Failed to load Highcharts for admin charts:', error);
                    throw error;
                });
        }

        return Promise.reject(new Error('Highcharts not available and ScriptLoader not found'));
    }
};

// User Registration Trend Chart with on-demand loading
function renderUserRegistrationChart(data) {
    return window.statisticsCharts.ensureHighcharts()
        .then(() => renderUserRegistrationChartInternal(data))
        .catch(error => {
            console.error('Failed to load Highcharts for user registration chart:', error);
            showChartError('user-registration-chart', 'Failed to load chart library');
        });
}

function renderUserRegistrationChartInternal(data) {
    try {
        if (!data || !Array.isArray(data) || data.length === 0) {
            console.warn('No data provided for user registration chart');
            showChartEmpty('user-registration-chart', 'No registration data available');
            return;
        }

        // Prepare chart data
        const chartData = data.map(item => [
            new Date(item.date).getTime(),
            parseInt(item.count) || 0
        ]);

        // Destroy existing chart if it exists
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'user-registration-chart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        // Create the chart
        Highcharts.chart('user-registration-chart', {
            chart: {
                type: 'areaspline',
                backgroundColor: 'transparent',
                height: 300
            },
            title: {
                text: null
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: {
                    day: '%e %b',
                    week: '%e %b',
                    month: '%b \'%y'
                },
                gridLineWidth: 1,
                gridLineColor: '#f0f0f0'
            },
            yAxis: {
                title: {
                    text: 'New Users'
                },
                min: 0,
                gridLineColor: '#f0f0f0'
            },
            tooltip: {
                shared: true,
                formatter: function() {
                    return '<b>' + Highcharts.dateFormat('%e %B %Y', this.x) + '</b><br/>' +
                           'New Users: <b>' + this.y + '</b>';
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                areaspline: {
                    fillColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0, window.statisticsCharts.rgba(window.statisticsCharts.colors.primary, 0.6)],
                            [1, window.statisticsCharts.rgba(window.statisticsCharts.colors.primary, 0.1)]
                        ]
                    },
                    lineWidth: 3,
                    lineColor: window.statisticsCharts.colors.primary,
                    marker: {
                        enabled: true,
                        radius: 4,
                        fillColor: '#fff',
                        lineColor: window.statisticsCharts.colors.primary,
                        lineWidth: 2,
                        states: {
                            hover: {
                                radius: 6
                            }
                        }
                    },
                    states: {
                        hover: {
                            lineWidth: 3
                        }
                    }
                }
            },
            series: [{
                name: 'User Registrations',
                data: chartData,
                color: window.statisticsCharts.colors.primary
            }],
            credits: {
                enabled: false
            },
            responsive: {
                rules: [{
                    condition: {
                        maxWidth: 500
                    },
                    chartOptions: {
                        chart: {
                            height: 250
                        }
                    }
                }]
            }
        });

        console.log('User registration chart rendered successfully');

    } catch (error) {
        console.error('Error rendering user registration chart:', error);
        showChartError('user-registration-chart', 'Error loading chart data');
    }
}

// Revenue Trend Chart with on-demand loading
function renderRevenueTrendChart(data) {
    return window.statisticsCharts.ensureHighcharts()
        .then(() => renderRevenueTrendChartInternal(data))
        .catch(error => {
            console.error('Failed to load Highcharts for revenue trend chart:', error);
            showChartError('revenue-trend-chart', 'Failed to load chart library');
        });
}

function renderRevenueTrendChartInternal(data) {
    try {
        if (!data || !Array.isArray(data) || data.length === 0) {
            console.warn('No data provided for revenue trend chart');
            showChartEmpty('revenue-trend-chart', 'No revenue data available');
            return;
        }

        const chartData = data.map(item => [
            new Date(item.date).getTime(),
            parseFloat(item.amount) || 0
        ]);

        // Destroy existing chart if it exists
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'revenue-trend-chart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        Highcharts.chart('revenue-trend-chart', {
            chart: {
                type: 'spline',
                backgroundColor: 'transparent',
                height: 300
            },
            title: {
                text: null
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: {
                    day: '%e %b',
                    week: '%e %b', 
                    month: '%b \'%y'
                },
                gridLineWidth: 1,
                gridLineColor: '#f0f0f0'
            },
            yAxis: {
                title: {
                    text: 'Revenue ($)'
                },
                min: 0,
                gridLineColor: '#f0f0f0',
                labels: {
                    formatter: function() {
                        return '$' + Highcharts.numberFormat(this.value, 0);
                    }
                }
            },
            tooltip: {
                shared: true,
                formatter: function() {
                    return '<b>' + Highcharts.dateFormat('%e %B %Y', this.x) + '</b><br/>' +
                           'Revenue: <b>$' + Highcharts.numberFormat(this.y, 2) + '</b>';
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                spline: {
                    lineWidth: 3,
                    lineColor: window.statisticsCharts.colors.success,
                    marker: {
                        enabled: true,
                        radius: 4,
                        fillColor: '#fff',
                        lineColor: window.statisticsCharts.colors.success,
                        lineWidth: 2,
                        states: {
                            hover: {
                                radius: 6
                            }
                        }
                    }
                }
            },
            series: [{
                name: 'Revenue',
                data: chartData,
                color: window.statisticsCharts.colors.success
            }],
            credits: {
                enabled: false
            },
            responsive: {
                rules: [{
                    condition: {
                        maxWidth: 500
                    },
                    chartOptions: {
                        chart: {
                            height: 250
                        }
                    }
                }]
            }
        });

        console.log('Revenue trend chart rendered successfully');

    } catch (error) {
        console.error('Error rendering revenue trend chart:', error);
        showChartError('revenue-trend-chart', 'Error loading chart data');
    }
}

// User Status Distribution Pie Chart with on-demand loading
function renderUserStatusChart(statusData) {
    return window.statisticsCharts.ensureHighcharts()
        .then(() => renderUserStatusChartInternal(statusData))
        .catch(error => {
            console.error('Failed to load Highcharts for user status chart:', error);
            showChartError('user-status-chart', 'Failed to load chart library');
        });
}

function renderUserStatusChartInternal(statusData) {
    try {
        if (!statusData) {
            console.warn('No data provided for user status chart');
            showChartEmpty('user-status-chart', 'No user status data available');
            return;
        }

        const pieData = [
            {
                name: 'Active Users',
                y: statusData.activeUsers || 0,
                color: window.statisticsCharts.colors.success
            },
            {
                name: 'Pending Users', 
                y: statusData.pendingUsers || 0,
                color: window.statisticsCharts.colors.warning
            },
            {
                name: 'Banned Users',
                y: statusData.bannedUsers || 0,
                color: window.statisticsCharts.colors.danger
            },
            {
                name: 'Admins',
                y: statusData.totalAdmins || 0,
                color: window.statisticsCharts.colors.info
            }
        ];

        // Destroy existing chart if it exists
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'user-status-chart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        Highcharts.chart('user-status-chart', {
            chart: {
                type: 'pie',
                backgroundColor: 'transparent',
                height: 300
            },
            title: {
                text: null
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b><br/>Count: <b>{point.y}</b>'
            },
            accessibility: {
                point: {
                    valueSuffix: '%'
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                    },
                    showInLegend: true
                }
            },
            series: [{
                name: 'Users',
                colorByPoint: true,
                data: pieData
            }],
            credits: {
                enabled: false
            }
        });

        console.log('User status chart rendered successfully');

    } catch (error) {
        console.error('Error rendering user status chart:', error);
        showChartError('user-status-chart', 'Error loading chart data');
    }
}

// Order Status Distribution Chart with on-demand loading
function renderOrderStatusChart(orderData) {
    return window.statisticsCharts.ensureHighcharts()
        .then(() => renderOrderStatusChartInternal(orderData))
        .catch(error => {
            console.error('Failed to load Highcharts for order status chart:', error);
            showChartError('order-status-chart', 'Failed to load chart library');
        });
}

function renderOrderStatusChartInternal(orderData) {
    try {
        if (!orderData) {
            console.warn('No data provided for order status chart');
            showChartEmpty('order-status-chart', 'No order status data available');
            return;
        }

        const chartData = [
            {
                name: 'Completed Orders',
                y: orderData.completedOrders || 0,
                color: window.statisticsCharts.colors.success
            },
            {
                name: 'Pending Orders',
                y: orderData.pendingOrders || 0, 
                color: window.statisticsCharts.colors.warning
            }
        ];

        // Destroy existing chart if it exists
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'order-status-chart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        Highcharts.chart('order-status-chart', {
            chart: {
                type: 'pie',
                backgroundColor: 'transparent',
                height: 300
            },
            title: {
                text: null
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b><br/>Count: <b>{point.y}</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.y}'
                    },
                    showInLegend: true,
                    innerSize: '50%'
                }
            },
            series: [{
                name: 'Orders',
                colorByPoint: true,
                data: chartData
            }],
            credits: {
                enabled: false
            }
        });

        console.log('Order status chart rendered successfully');

    } catch (error) {
        console.error('Error rendering order status chart:', error);
        showChartError('order-status-chart', 'Error loading chart data');
    }
}

// Helper functions for chart states
function showChartEmpty(chartId, message) {
    const element = document.getElementById(chartId);
    if (element) {
        element.innerHTML = 
            `<div class="text-center text-muted p-4">
                <i class="fas fa-chart-line fa-3x mb-3"></i>
                <p>${message}</p>
            </div>`;
    }
}

function showChartError(chartId, message) {
    const element = document.getElementById(chartId);
    if (element) {
        element.innerHTML = 
            `<div class="text-center text-danger p-4">
                <i class="fas fa-exclamation-triangle fa-2x mb-3"></i>
                <p>${message}</p>
            </div>`;
    }
}

// Cleanup function for admin charts
window.disposeAdminCharts = function() {
    try {
        ['user-registration-chart', 'revenue-trend-chart', 'user-status-chart', 'order-status-chart'].forEach(chartId => {
            if (typeof Highcharts !== 'undefined' && Highcharts.charts) {
                const chart = Highcharts.charts.find(chart => chart && chart.container && chart.container.id === chartId);
                if (chart) {
                    chart.destroy();
                    console.log(`Admin chart ${chartId} destroyed`);
                }
            }
        });
        
        console.log('Admin charts disposed successfully');
        return true;
    } catch (error) {
        console.error('Error disposing admin charts:', error);
        return false;
    }
};

// Export functions for global access
window.renderUserRegistrationChart = renderUserRegistrationChart;
window.renderRevenueTrendChart = renderRevenueTrendChart;
window.renderUserStatusChart = renderUserStatusChart;
window.renderOrderStatusChart = renderOrderStatusChart;