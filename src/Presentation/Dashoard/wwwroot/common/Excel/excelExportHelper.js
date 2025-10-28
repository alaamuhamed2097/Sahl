// Excel Export Helper using SheetJS
// This file provides Excel export functionality without server-side dependencies

window.excelExportHelper = {
    /**
     * Export data to Excel file
     * @param {string} filename - Name of the file to download
     * @param {string} sheetName - Name of the worksheet
     * @param {Array} headers - Array of column header names
     * @param {Array<Array>} data - 2D array of data rows
     * @param {boolean} rtl - Right-to-left text direction
     */
    exportToExcel: async function (filename, sheetName, headers, data, rtl = false) {
        try {
            // Dynamically load SheetJS library if not already loaded
            if (typeof XLSX === 'undefined') {
                await this.loadSheetJS();
            }

            // Create workbook
            const wb = XLSX.utils.book_new();

            // Prepare data with headers
            const wsData = [headers, ...data];

            // Create worksheet
            const ws = XLSX.utils.aoa_to_sheet(wsData);

            // Apply styling to headers
            const headerRange = XLSX.utils.decode_range(ws['!ref']);
            for (let col = headerRange.s.c; col <= headerRange.e.c; col++) {
                const cellAddress = XLSX.utils.encode_cell({ r: 0, c: col });
                if (!ws[cellAddress]) continue;

                ws[cellAddress].s = {
                    font: { bold: true, color: { rgb: "FFFFFF" } },
                    fill: { fgColor: { rgb: "4F81BD" } },
                    alignment: { horizontal: "center", vertical: "center" }
                };
            }

            // Set column widths (auto-fit)
            const colWidths = headers.map((header, i) => {
                const headerLength = header.length;
                const dataLengths = data.map(row => (row[i] ? row[i].toString().length : 0));
                const maxDataLength = Math.max(...dataLengths, 0);
                return { wch: Math.max(headerLength, maxDataLength) + 2 };
            });
            ws['!cols'] = colWidths;

            // Set RTL if needed
            if (rtl) {
                if (!ws['!views']) ws['!views'] = [{}];
                ws['!views'][0].rightToLeft = true;
            }

            // Add worksheet to workbook
            XLSX.utils.book_append_sheet(wb, ws, sheetName);

            // Generate Excel file
            const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });

            // Create blob and download
            const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.download = filename;
            link.style.visibility = 'hidden';

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            URL.revokeObjectURL(url);

            return true;
        } catch (error) {
            console.error('Excel export error:', error);
            return false;
        }
    },

    /**
     * Load SheetJS library dynamically from CDN
     */
    loadSheetJS: function () {
        return new Promise((resolve, reject) => {
            if (typeof XLSX !== 'undefined') {
                resolve();
                return;
            }

            const script = document.createElement('script');
            script.src = 'https://cdn.sheetjs.com/xlsx-0.20.3/package/dist/xlsx.full.min.js';
            script.onload = () => resolve();
            script.onerror = () => reject(new Error('Failed to load SheetJS library'));
            document.head.appendChild(script);
        });
    },

    /**
     * Export data to Excel with advanced formatting
     * @param {string} filename - Name of the file to download
     * @param {string} sheetName - Name of the worksheet
     * @param {object} exportConfig - Configuration object with headers, data, and formatting options
     */
    exportToExcelAdvanced: async function (filename, sheetName, exportConfig) {
        try {
            // Dynamically load SheetJS library if not already loaded
            if (typeof XLSX === 'undefined') {
                await this.loadSheetJS();
            }

            const { headers, data, rtl = false, title = null } = exportConfig;

            // Create workbook
            const wb = XLSX.utils.book_new();

            // Prepare data
            const wsData = [];

            // Add title if provided
            let startRow = 0;
            if (title) {
                wsData.push([title]);
                wsData.push([]); // Empty row
                startRow = 2;
            }

            // Add headers and data
            wsData.push(headers);
            wsData.push(...data);

            // Create worksheet
            const ws = XLSX.utils.aoa_to_sheet(wsData);

            // Set column widths
            const colWidths = headers.map((header, i) => {
                const headerLength = header.length;
                const dataLengths = data.map(row => (row[i] ? row[i].toString().length : 0));
                const maxDataLength = Math.max(...dataLengths, 0);
                return { wch: Math.max(headerLength, maxDataLength) + 2 };
            });
            ws['!cols'] = colWidths;

            // Set RTL if needed
            if (rtl) {
                if (!ws['!views']) ws['!views'] = [{}];
                ws['!views'][0].rightToLeft = true;
            }

            // Add worksheet to workbook
            XLSX.utils.book_append_sheet(wb, ws, sheetName);

            // Generate Excel file
            const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });

            // Create blob and download
            const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.download = filename;
            link.style.visibility = 'hidden';

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            URL.revokeObjectURL(url);

            return true;
        } catch (error) {
            console.error('Excel export error:', error);
            return false;
        }
    }
};
