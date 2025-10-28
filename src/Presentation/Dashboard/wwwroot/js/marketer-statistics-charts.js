// Marketer Statistics Charts with Production-Ready On-Demand Highcharts Loading
window.initializeMarketerCharts = function (commissionData, referralData) {
    console.log('Initializing marketer charts with data:', { commissionData, referralData });
    
    // Store data globally for retry scenarios
    window.lastChartData = { commissionData, referralData };
    
    // Check if Highcharts is loaded, if not load it first
    if (!window.ScriptLoader.isHighchartsLoaded()) {
        console.log('Highcharts not loaded, loading on demand...');
        
        // Show loading indicators
        showChartLoadingIndicators();
        
        return window.ScriptLoader.loadHighcharts()
            .then(() => {
                console.log('Highcharts loaded successfully, initializing charts...');
                return initializeChartsWithData(commissionData, referralData);
            })
            .catch(error => {
                console.error('Failed to load Highcharts:', error);
                showChartErrorIndicators('Failed to load chart library: ' + error.message);
                return Promise.reject(error);
            });
    } else {
        // Highcharts already loaded, initialize charts directly
        return Promise.resolve(initializeChartsWithData(commissionData, referralData));
    }
};

function initializeChartsWithData(commissionData, referralData) {
    try {
        // Verify Highcharts is actually available
        if (typeof Highcharts === 'undefined') {
            throw new Error('Highcharts is not available after loading');
        }

        // Initialize Commission Chart
        const commissionResult = initializeCommissionChart(commissionData);
        
        // Initialize Referral Chart  
        const referralResult = initializeReferralChart(referralData);
        
        if (commissionResult && referralResult) {
            console.log('Both charts initialized successfully');
            return true;
        } else {
            throw new Error('One or more charts failed to initialize');
        }
    } catch (error) {
        console.error('Error initializing marketer charts:', error);
        showChartErrorIndicators('Error initializing charts: ' + error.message);
        //throw error;
    }
}

function showChartLoadingIndicators() {
    const loadingHtml = `
        <div class="text-center text-muted p-4">
            <div class="spinner-border spinner-border-sm mb-2" role="status" aria-hidden="true"></div>
            <p class="mb-0">Loading charts...</p>
            <small class="text-muted">This may take a moment</small>
        </div>
    `;
    
    const commissionChart = document.getElementById('commissionChart');
    const referralChart = document.getElementById('referralChart');
    
    if (commissionChart) commissionChart.innerHTML = loadingHtml;
    if (referralChart) referralChart.innerHTML = loadingHtml;
}

function showChartErrorIndicators(message) {
    const errorHtml = `
        <div class="text-center text-muted p-4">
            <i class="fas fa-exclamation-triangle fa-2x mb-2 text-warning" aria-hidden="true"></i>
            <p class="mb-2">${message}</p>
            <button class="btn btn-sm btn-outline-primary" onclick="retryChartLoading()" type="button">
                <i class="fas fa-redo" aria-hidden="true"></i> Retry
            </button>
            <br><small class="text-muted mt-2 d-block">Check your internet connection</small>
        </div>
    `;
    
    const commissionChart = document.getElementById('commissionChart');
    const referralChart = document.getElementById('referralChart');
    
    if (commissionChart) commissionChart.innerHTML = errorHtml;
    if (referralChart) referralChart.innerHTML = errorHtml;
}

// Enhanced retry function for failed chart loading
window.retryChartLoading = function() {
    console.log('Retrying chart loading...');
    showChartLoadingIndicators();
    
    // Clear any existing Highcharts state
    if (typeof Highcharts !== 'undefined') {
        try {
            ['commissionChart', 'referralChart', 'pvChart'].forEach(chartId => {
                const chart = Highcharts.charts.find(chart => chart && chart.container && chart.container.id === chartId);
                if (chart) {
                    chart.destroy();
                }
            });
        } catch (error) {
            console.warn('Error cleaning up existing charts:', error);
        }
    }
    
    // Force reload Highcharts with stored data
    const storedData = window.lastChartData || { commissionData: [], referralData: [] };
    
    window.ScriptLoader.loadHighcharts()
        .then(() => {
            console.log('Charts ready for retry, reinitializing...');
            return initializeChartsWithData(storedData.commissionData, storedData.referralData);
        })
        .catch(error => {
            console.error('Retry failed:', error);
            showChartErrorIndicators('Retry failed: ' + error.message);
        });
};

function initializeCommissionChart(data) {
    try {
        console.log('Initializing commission chart with data:', data);
        
        // Verify Highcharts is available
        if (typeof Highcharts === 'undefined') {
            throw new Error('Highcharts is not available');
        }

        // Validate data
        if (!Array.isArray(data) || data.length === 0) {
            console.warn('No commission data provided, showing empty chart');
            data = [{ period: 'No Data', amount: 0 }];
        }

        // Prepare data for Highcharts
        const categories = data.map(item => item.period || 'Unknown');
        const values = data.map(item => parseFloat(item.amount) || 0);

        // Destroy existing chart if it exists
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'commissionChart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        // Create the chart
        const chart = Highcharts.chart('commissionChart', {
            chart: {
                type: 'column',
                backgroundColor: 'transparent',
                height: 300
            },
            title: {
                text: null
            },
            xAxis: {
                categories: categories,
                gridLineWidth: 0
            },
            yAxis: {
                title: {
                    text: 'Amount'
                },
                min: 0
            },
            tooltip: {
                pointFormat: '<b>{point.y:.2f}</b>'
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.0f}'
                    }
                }
            },
            series: [{
                name: 'Commissions',
                data: values,
                color: '#13bd8a'
            }],
            credits: {
                enabled: false
            }
        });
        
        if (chart) {
            console.log('Commission chart initialized successfully');
            return true;
        } else {
            throw new Error('Chart creation returned null');
        }
        
    } catch (error) {
        console.error('Error initializing commission chart:', error);
        const chartElement = document.getElementById('commissionChart');
        if (chartElement) {
            chartElement.innerHTML = 
                `<div class="text-center text-muted p-4">
                    <i class="fas fa-chart-bar fa-2x mb-2" aria-hidden="true"></i>
                    <p>Unable to load commission chart</p>
                    <small class="text-danger">${error.message}</small>
                </div>`;
        }
        return false;
    }
}

function initializeReferralChart(data) {
    try {
        console.log('Initializing referral chart with data:', data);
        
        // Verify Highcharts is available
        if (typeof Highcharts === 'undefined') {
            throw new Error('Highcharts is not available');
        }

        // Validate data
        if (!Array.isArray(data) || data.length === 0) {
            console.warn('No referral data provided, showing empty chart');
            data = [{ period: 'No Data', count: 0 }];
        }

        // Prepare data for Highcharts
        const categories = data.map(item => item.period || 'Unknown');
        const values = data.map(item => parseInt(item.count) || 0);

        // Destroy existing chart if it exists
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'referralChart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        // Create the chart
        const chart = Highcharts.chart('referralChart', {
            chart: {
                type: 'line',
                backgroundColor: 'transparent',
                height: 300
            },
            title: {
                text: null
            },
            xAxis: {
                categories: categories,
                gridLineWidth: 0
            },
            yAxis: {
                title: {
                    text: 'Count'
                },
                min: 0
            },
            tooltip: {
                pointFormat: '<b>{point.y}</b> referrals'
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    marker: {
                        enabled: true,
                        radius: 4
                    }
                }
            },
            series: [{
                name: 'Referrals',
                data: values,
                color: '#19bcbf',
                lineWidth: 3
            }],
            credits: {
                enabled: false
            }
        });
        
        if (chart) {
            console.log('Referral chart initialized successfully');
            return true;
        } else {
            throw new Error('Chart creation returned null');
        }
        
    } catch (error) {
        console.error('Error initializing referral chart:', error);
        const chartElement = document.getElementById('referralChart');
        if (chartElement) {
            chartElement.innerHTML = 
                `<div class="text-center text-muted p-4">
                    <i class="fas fa-chart-line fa-2x mb-2" aria-hidden="true"></i>
                    <p>Unable to load referral chart</p>
                    <small class="text-danger">${error.message}</small>
                </div>`;
        }
        return false;
    }
}

// Enhanced chart rendering functions with production-ready on-demand loading
window.renderDetailedCommissionChart = function(data) {
    if (!window.ScriptLoader.isHighchartsLoaded()) {
        return window.ScriptLoader.loadHighcharts()
            .then(() => renderDetailedCommissionChartInternal(data))
            .catch(error => {
                console.error('Failed to load Highcharts for detailed commission chart:', error);
                throw error;
            });
    } else {
        return Promise.resolve(renderDetailedCommissionChartInternal(data));
    }
};

function renderDetailedCommissionChartInternal(data) {
    try {
        if (!Array.isArray(data) || data.length === 0) {
            throw new Error('No data provided for detailed commission chart');
        }

        const chartData = data.map(item => [
            new Date(item.date).getTime(),
            parseFloat(item.amount) || 0
        ]);

        // Destroy existing chart
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'commissionChart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        const chart = Highcharts.chart('commissionChart', {
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
                }
            },
            yAxis: {
                title: {
                    text: 'Commission Amount'
                },
                min: 0
            },
            tooltip: {
                shared: true,
                formatter: function() {
                    return '<b>' + Highcharts.dateFormat('%e %B %Y', this.x) + '</b><br/>' +
                           'Commission: <b>' + this.y.toFixed(2) + '</b>';
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                areaspline: {
                    fillColor: {
                        linearGradient: {
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        },
                        stops: [
                            [0, '#13bd8a'],
                            [1, Highcharts.color('#13bd8a').setOpacity(0).get('rgba')]
                        ]
                    }
                }
            },
            series: [{
                name: 'Commission',
                data: chartData,
                color: '#13bd8a'
            }],
            credits: {
                enabled: false
            }
        });

        return chart !== null;
    } catch (error) {
        console.error('Error rendering detailed commission chart:', error);
        return false;
    }
}

// Similar enhancement for other chart functions...
window.renderDetailedReferralChart = function(data) {
    if (!window.ScriptLoader.isHighchartsLoaded()) {
        return window.ScriptLoader.loadHighcharts()
            .then(() => renderDetailedReferralChartInternal(data))
            .catch(error => {
                console.error('Failed to load Highcharts for detailed referral chart:', error);
                throw error;
            });
    } else {
        return Promise.resolve(renderDetailedReferralChartInternal(data));
    }
};

function renderDetailedReferralChartInternal(data) {
    try {
        if (!Array.isArray(data) || data.length === 0) {
            throw new Error('No data provided for detailed referral chart');
        }

        const chartData = data.map(item => [
            new Date(item.date).getTime(),
            parseInt(item.count) || 0
        ]);

        // Destroy existing chart
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'referralChart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        const chart = Highcharts.chart('referralChart', {
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
                }
            },
            yAxis: {
                title: {
                    text: 'Referral Count'
                },
                min: 0
            },
            tooltip: {
                shared: true,
                formatter: function() {
                    return '<b>' + Highcharts.dateFormat('%e %B %Y', this.x) + '</b><br/>' +
                           'New Referrals: <b>' + this.y + '</b>';
                }
            },
            legend: {
                enabled: false
            },
            series: [{
                name: 'Referrals',
                data: chartData,
                color: '#19bcbf',
                lineWidth: 3,
                marker: {
                    enabled: true,
                    radius: 4
                }
            }],
            credits: {
                enabled: false
            }
        });

        return chart !== null;
    } catch (error) {
        console.error('Error rendering detailed referral chart:', error);
        return false;
    }
}

// PV Trend Chart with production-ready on-demand loading
window.renderPVTrendChart = function(data) {
    if (!window.ScriptLoader.isHighchartsLoaded()) {
        const pvChartElement = document.getElementById('pvChart');
        if (pvChartElement) {
            pvChartElement.innerHTML = 
                `<div class="text-center text-muted p-4">
                    <div class="spinner-border spinner-border-sm mb-2" role="status" aria-hidden="true"></div>
                    <p>Loading chart library...</p>
                </div>`;
        }
            
        return window.ScriptLoader.loadHighcharts()
            .then(() => renderPVTrendChartInternal(data))
            .catch(error => {
                console.error('Failed to load Highcharts for PV trend chart:', error);
                if (pvChartElement) {
                    pvChartElement.innerHTML = 
                        `<div class="text-center text-muted p-4">
                            <i class="fas fa-chart-area fa-2x mb-2 text-warning" aria-hidden="true"></i>
                            <p>Failed to load chart library</p>
                            <small class="text-danger">${error.message}</small>
                        </div>`;
                }
                throw error;
            });
    } else {
        return Promise.resolve(renderPVTrendChartInternal(data));
    }
};

function renderPVTrendChartInternal(data) {
    try {
        if (!Array.isArray(data) || data.length === 0) {
            throw new Error('No data provided for PV trend chart');
        }

        const chartData = data.map(item => [
            new Date(item.date).getTime(),
            parseInt(item.pvs) || 0
        ]);

        // Destroy existing chart
        const existingChart = Highcharts.charts.find(chart => 
            chart && chart.container && chart.container.id === 'pvChart'
        );
        if (existingChart) {
            existingChart.destroy();
        }

        const chart = Highcharts.chart('pvChart', {
            chart: {
                type: 'area',
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
                }
            },
            yAxis: {
                title: {
                    text: 'PV Points'
                },
                min: 0
            },
            tooltip: {
                shared: true,
                formatter: function() {
                    return '<b>' + Highcharts.dateFormat('%e %B %Y', this.x) + '</b><br/>' +
                           'PV Points: <b>' + this.y + '</b>';
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                area: {
                    fillColor: {
                        linearGradient: {
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        },
                        stops: [
                            [0, '#FF9764'],
                            [1, Highcharts.color('#FF9764').setOpacity(0).get('rgba')]
                        ]
                    }
                }
            },
            series: [{
                name: 'PV Points',
                data: chartData,
                color: '#FF9764'
            }],
            credits: {
                enabled: false
            }
        });
        
        if (chart) {
            console.log('PV trend chart initialized successfully');
            return true;
        } else {
            throw new Error('Chart creation returned null');
        }
        
    } catch (error) {
        console.error('Error rendering PV trend chart:', error);
        const pvChartElement = document.getElementById('pvChart');
        if (pvChartElement) {
            pvChartElement.innerHTML = 
                `<div class="text-center text-muted p-4">
                    <i class="fas fa-chart-area fa-2x mb-2 text-warning" aria-hidden="true"></i>
                    <p>Unable to load PV trend chart</p>
                    <small class="text-danger">${error.message}</small>
                </div>`;
        }
        return false;
    }
}

// Enhanced cleanup function
window.disposeMarketerCharts = function() {
    try {
        // Destroy all Highcharts instances related to marketer statistics
        ['commissionChart', 'referralChart', 'pvChart'].forEach(chartId => {
            if (typeof Highcharts !== 'undefined' && Highcharts.charts) {
                const chart = Highcharts.charts.find(chart => chart && chart.container && chart.container.id === chartId);
                if (chart) {
                    chart.destroy();
                    console.log(`Chart ${chartId} destroyed`);
                }
            }
        });
        
        // Clear stored data
        delete window.lastChartData;
        
        console.log('Marketer charts disposed successfully');
        return true;
    } catch (error) {
        console.error('Error disposing marketer charts:', error);
        return false;
    }
};

// Backward compatibility functions
window.renderCommissionChart = function(data) {
    return window.initializeMarketerCharts(data, []);
};

window.renderReferralChart = function(data) {
    return window.initializeMarketerCharts([], data);
};

// Production debugging helper
window.debugChartState = function() {
    return {
        highchartsLoaded: typeof Highcharts !== 'undefined',
        scriptLoaderInfo: window.ScriptLoader.getEnvironmentInfo(),
        activeCharts: typeof Highcharts !== 'undefined' ? 
            Highcharts.charts.filter(chart => chart).map(chart => ({
                id: chart.container.id,
                type: chart.options.chart.type
            })) : [],
        lastData: window.lastChartData
    };
}