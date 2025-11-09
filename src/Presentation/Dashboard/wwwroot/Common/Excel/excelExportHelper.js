// Excel Export Helper using ExcelJS for full features including dropdowns
// This file provides Excel export/import functionality with Data Validation support

window.excelExportHelper = {
    /**
     * Load ExcelJS library dynamically from CDN
     */
    loadExcelJS: function () {
        return new Promise((resolve, reject) => {
            if (typeof ExcelJS !== 'undefined') {
                resolve();
                return;
            }

            const script = document.createElement('script');
            script.src = 'https://cdn.jsdelivr.net/npm/exceljs@4.4.0/dist/exceljs.min.js';
            script.onload = () => resolve();
            script.onerror = () => reject(new Error('Failed to load ExcelJS library'));
            document.head.appendChild(script);
        });
    },

    /**
     * Load SheetJS library dynamically from CDN (for backward compatibility)
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
     * Export data to Excel file (Simple - using SheetJS)
     * @param {string} filename - Name of the file to download
     * @param {string} sheetName - Name of the worksheet
     * @param {Array} headers - Array of column header names
     * @param {Array<Array>} data - 2D array of data rows
     * @param {boolean} rtl - Right-to-left text direction
     */
    exportToExcel: async function (filename, sheetName, headers, data, rtl = false) {
        try {
            if (typeof XLSX === 'undefined') {
                await this.loadSheetJS();
            }

            const wb = XLSX.utils.book_new();
            const wsData = [headers, ...data];
            const ws = XLSX.utils.aoa_to_sheet(wsData);

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

            const colWidths = headers.map((header, i) => {
                const headerLength = header.length;
                const dataLengths = data.map(row => (row[i] ? row[i].toString().length : 0));
                const maxDataLength = Math.max(...dataLengths, 0);
                return { wch: Math.max(headerLength, maxDataLength) + 2 };
            });
            ws['!cols'] = colWidths;

            if (rtl) {
                if (!ws['!views']) ws['!views'] = [{}];
                ws['!views'][0].rightToLeft = true;
            }

            XLSX.utils.book_append_sheet(wb, ws, sheetName);
            const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
            const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            
            this.downloadBlob(blob, filename);
            return true;
        } catch (error) {
            console.error('Excel export error:', error);
            return false;
        }
    },

    /**
     * Export data to Excel with advanced formatting (using SheetJS)
     * @param {string} filename - Name of the file to download
     * @param {string} sheetName - Name of the worksheet
     * @param {object} exportConfig - Configuration object
     */
    exportToExcelAdvanced: async function (filename, sheetName, exportConfig) {
        try {
            if (typeof XLSX === 'undefined') {
                await this.loadSheetJS();
            }

            const { headers, data, rtl = false, title = null } = exportConfig;
            const wb = XLSX.utils.book_new();
            const wsData = [];

            let startRow = 0;
            if (title) {
                wsData.push([title]);
                wsData.push([]);
                startRow = 2;
            }

            wsData.push(headers);
            wsData.push(...data);
            const ws = XLSX.utils.aoa_to_sheet(wsData);

            const colWidths = headers.map((header, i) => {
                const headerLength = header.length;
                const dataLengths = data.map(row => (row[i] ? row[i].toString().length : 0));
                const maxDataLength = Math.max(...dataLengths, 0);
                return { wch: Math.max(headerLength, maxDataLength) + 2 };
            });
            ws['!cols'] = colWidths;

            if (rtl) {
                if (!ws['!views']) ws['!views'] = [{}];
                ws['!views'][0].rightToLeft = true;
            }

            XLSX.utils.book_append_sheet(wb, ws, sheetName);
            const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
            const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            
            this.downloadBlob(blob, filename);
            return true;
        } catch (error) {
            console.error('Excel export error:', error);
            return false;
        }
    },

    /**
     * Generate Excel template WITH REAL DROPDOWNS using ExcelJS
     * @param {string} filename - Name of the file to download
     * @param {object} templateConfig - Configuration object with sheets, dropdowns, and data
     */
    generateProductImportTemplate: async function (filename, templateConfig) {
        try {
            console.log('?? Starting template generation with ExcelJS...');
            
            if (typeof ExcelJS === 'undefined') {
                await this.loadExcelJS();
            }

            const {
                headers = [],
                exampleRow = [],
                categories = [],
                brands = [],
                units = [],
                attributes = [],
                rtl = true
            } = templateConfig;

            // Create workbook
            const workbook = new ExcelJS.Workbook();
            workbook.creator = 'Basit System';
            workbook.created = new Date();

            // 1. Create Products Sheet
            const productsSheet = workbook.addWorksheet('Products', {
                views: [{ rightToLeft: rtl, state: 'frozen', ySplit: 1 }]
            });

            // Add headers with styling
            const headerRow = productsSheet.addRow(headers);
            headerRow.font = { bold: true, color: { argb: 'FFFFFFFF' } };
            headerRow.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FF4472C4' }
            };
            headerRow.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
            headerRow.height = 30;

            // Set column widths
            productsSheet.columns = headers.map((header, i) => ({
                key: `col${i}`,
                width: i < 6 ? 30 : (i < 9 ? 20 : 15)
            }));

            // Add example row
            if (exampleRow && exampleRow.length > 0) {
                const exRow = productsSheet.addRow(exampleRow);
                exRow.fill = {
                    type: 'pattern',
                    pattern: 'solid',
                    fgColor: { argb: 'FFFFEB3B' }
                };
                exRow.font = { italic: true };
                
                // Add comment to first cell
                exRow.getCell(1).note = {
                    texts: [{ text: '?? ???? ??? ???? ??? ?????????\nDelete this row before importing' }]
                };
            }

            // 2. ADD DROPDOWNS using Data Validation
            const startRow = 3; // Start from row 3 (after header and example)
            const endRow = 1000; // Support up to 1000 rows

            // Find column indices
            const categoryColIndex = headers.findIndex(h => h.includes('Category') || h.includes('?????')) + 1;
            const brandColIndex = headers.findIndex(h => h.includes('Brand') || h.includes('???????')) + 1;
            const unitColIndex = headers.findIndex(h => h.includes('Unit') || h.includes('??????')) + 1;
            const stockColIndex = headers.findIndex(h => h.includes('StockStatus') || h.includes('???????')) + 1;
            const newArrivalColIndex = headers.findIndex(h => h.includes('IsNewArrival')) + 1;
            const bestSellerColIndex = headers.findIndex(h => h.includes('IsBestSeller')) + 1;
            const recommendedColIndex = headers.findIndex(h => h.includes('IsRecommended')) + 1;

            // Category Dropdown
            if (categoryColIndex > 0 && categories.length > 0) {
                const categoryNames = categories.map(c => c.title || c.Title || c);
                for (let row = startRow; row <= endRow; row++) {
                    productsSheet.getCell(row, categoryColIndex).dataValidation = {
                        type: 'list',
                        allowBlank: false,
                        formulae: [`"${categoryNames.join(',')}"`],
                        showErrorMessage: true,
                        errorTitle: 'Invalid Category',
                        error: 'Please select a category from the list'
                    };
                }
            }

            // Brand Dropdown
            if (brandColIndex > 0 && brands.length > 0) {
                const brandNames = brands.map(b => b.name || b.Name || b);
                for (let row = startRow; row <= endRow; row++) {
                    productsSheet.getCell(row, brandColIndex).dataValidation = {
                        type: 'list',
                        allowBlank: false,
                        formulae: [`"${brandNames.join(',')}"`],
                        showErrorMessage: true,
                        errorTitle: 'Invalid Brand',
                        error: 'Please select a brand from the list'
                    };
                }
            }

            // Unit Dropdown
            if (unitColIndex > 0 && units.length > 0) {
                const unitNames = units.map(u => u.title || u.Title || u);
                for (let row = startRow; row <= endRow; row++) {
                    productsSheet.getCell(row, unitColIndex).dataValidation = {
                        type: 'list',
                        allowBlank: false,
                        formulae: [`"${unitNames.join(',')}"`],
                        showErrorMessage: true,
                        errorTitle: 'Invalid Unit',
                        error: 'Please select a unit from the list'
                    };
                }
            }

            // Boolean Dropdowns (True/False)
            const booleanColumns = [stockColIndex, newArrivalColIndex, bestSellerColIndex, recommendedColIndex].filter(c => c > 0);
            booleanColumns.forEach(colIndex => {
                for (let row = startRow; row <= endRow; row++) {
                    productsSheet.getCell(row, colIndex).dataValidation = {
                        type: 'list',
                        allowBlank: false,
                        formulae: ['"True,False"'],
                        showErrorMessage: true,
                        errorTitle: 'Invalid Value',
                        error: 'Please select True or False'
                    };
                }
            });

            // 3. Create Reference Sheets
            if (categories.length > 0) {
                const catSheet = workbook.addWorksheet('Categories');
                catSheet.addRow(['ID', 'Title (AR)', 'Title (EN)', 'Is Final']);
                categories.forEach(cat => {
                    catSheet.addRow([
                        cat.id || '',
                        cat.titleAr || cat.TitleAr || '',
                        cat.titleEn || cat.TitleEn || '',
                        cat.isFinal ? 'Yes' : 'No'
                    ]);
                });
                catSheet.getRow(1).font = { bold: true };
                catSheet.columns.forEach(col => col.width = 25);
            }

            if (brands.length > 0) {
                const brandSheet = workbook.addWorksheet('Brands');
                brandSheet.addRow(['ID', 'Brand Name']);
                brands.forEach(brand => {
                    brandSheet.addRow([
                        brand.id || '',
                        brand.name || brand.Name || ''
                    ]);
                });
                brandSheet.getRow(1).font = { bold: true };
                brandSheet.columns.forEach(col => col.width = 25);
            }

            if (units.length > 0) {
                const unitSheet = workbook.addWorksheet('Units');
                unitSheet.addRow(['ID', 'Unit Name']);
                units.forEach(unit => {
                    unitSheet.addRow([
                        unit.id || '',
                        unit.title || unit.Title || ''
                    ]);
                });
                unitSheet.getRow(1).font = { bold: true };
                unitSheet.columns.forEach(col => col.width = 25);
            }

            // 4. Create Instructions Sheet
            const instSheet = workbook.addWorksheet('Instructions');
            instSheet.getColumn(1).width = 100;
            
            const instructions = [
                'Excel Template Instructions',
                '',
                '1. Required Fields:',
                '   - All fields marked with * are required',
                '   - Provide titles in both Arabic and English',
                '   - Use dropdown lists for Category, Brand, Unit',
                '   - Price must be a numeric value',
                '   - Quantity must be an integer',
                '   - Stock Status: Select True or False',
                '',
                '2. Categories:',
                '   - Check the Categories sheet for available options',
                '   - Select from dropdown list in Category column',
                '',
                '3. Boolean Fields:',
                '   - StockStatus: True = In Stock, False = Out of Stock',
                '   - IsNewArrival: True/False',
                '   - IsBestSeller: True/False',
                '   - IsRecommended: True/False',
                '',
                '4. Tips:',
                '   - Delete the example row (row 2) before importing',
                '   - Save as .xlsx format',
                '   - Maximum 1000 rows per import',
                '   - Review data carefully before import'
            ];

            instructions.forEach((inst, i) => {
                const row = instSheet.addRow([inst]);
                if (inst.startsWith('1.') || inst.startsWith('2.') || 
                    inst.startsWith('3.') || inst.startsWith('4.') ||
                    inst === 'Excel Template Instructions') {
                    row.font = { bold: true, color: { argb: 'FF1E40AF' } };
                }
            });

            // 5. Generate and Download
            const buffer = await workbook.xlsx.writeBuffer();
            const blob = new Blob([buffer], { 
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
            });
            
            this.downloadBlob(blob, filename);

            console.log('? Template with REAL DROPDOWNS generated successfully!');
            return true;

        } catch (error) {
            console.error('? Template generation error:', error);
            return false;
        }
    },

    /**
     * Helper function to download blob
     */
    downloadBlob: function(blob, filename) {
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.style.visibility = 'hidden';

        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);

        URL.revokeObjectURL(url);
    },

    /**
     * Parse products from Excel file (using SheetJS for reading)
     * @param {string} base64File - Base64 encoded Excel file
     * @param {object} config - Configuration with categories, brands, units, attributes
     */
    parseProductsFromExcel: async function(base64File, config) {
        try {
            console.log('?? Starting Excel parsing...');
            
            // Load SheetJS for reading
            if (typeof XLSX === 'undefined') {
                await this.loadSheetJS();
            }

            // Convert base64 to array buffer
            const binaryString = window.atob(base64File);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }

            // Read workbook
            const workbook = XLSX.read(bytes, { type: 'array' });
            const sheetName = workbook.SheetNames[0]; // First sheet
            const worksheet = workbook.Sheets[sheetName];

            // Convert to JSON
            const rawData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });
            
            if (rawData.length < 2) {
                throw new Error('Excel file is empty or has no data rows');
            }

            const headers = rawData[0];
            const dataRows = rawData.slice(1); // Skip header row

            console.log(`?? Found ${dataRows.length} rows to parse`);

            // Parse each row
            const products = [];
            for (let i = 0; i < dataRows.length; i++) {
                const row = dataRows[i];
                if (!row || row.length === 0) continue; // Skip empty rows

                try {
                    const product = {
                        titleAr: row[0] || '',
                        titleEn: row[1] || '',
                        shortDescriptionAr: row[2] || '',
                        shortDescriptionEn: row[3] || '',
                        descriptionAr: row[4] || '',
                        descriptionEn: row[5] || '',
                        categoryId: this.parseId(row[6], config.categories, 'title'),
                        brandId: this.parseId(row[7], config.brands, 'name'),
                        unitId: this.parseId(row[8], config.units, 'title'),
                        price: parseFloat(row[9]) || 0,
                        quantity: parseInt(row[10]) || 0,
                        stockStatus: this.parseBoolean(row[11]),
                        isNewArrival: this.parseBoolean(row[12]),
                        isBestSeller: this.parseBoolean(row[13]),
                        isRecommended: this.parseBoolean(row[14]),
                        thumbnailImagePath: row[15] || '',
                        imagePaths: [],
                        attributeValues: {},
                        pricingAttributeValues: {},
                        rowNumber: i + 2 // +2 because: +1 for header, +1 for 1-based indexing
                    };

                    // Parse image paths
                    for (let j = 16; j <= 20; j++) {
                        if (row[j]) {
                            product.imagePaths.push(row[j]);
                        }
                    }

                    products.push(product);
                } catch (error) {
                    console.error(`Error parsing row ${i + 2}:`, error);
                }
            }

            console.log(`? Successfully parsed ${products.length} products`);
            return JSON.stringify(products);

        } catch (error) {
            console.error('? Excel parsing error:', error);
            throw error;
        }
    },

    /**
     * Helper to parse ID from name
     */
    parseId: function(value, list, nameField) {
        if (!value) return '00000000-0000-0000-0000-000000000000';
        
        // Try to parse as GUID first
        if (this.isValidGuid(value)) {
            return value;
        }

        // Try to find by name
        const item = list.find(x => x[nameField] && x[nameField].toLowerCase() === value.toLowerCase());
        return item ? item.id : '00000000-0000-0000-0000-000000000000';
    },

    /**
     * Helper to parse boolean
     */
    parseBoolean: function(value) {
        if (!value) return false;
        const str = value.toString().toLowerCase().trim();
        return str === 'true' || str === 'yes' || str === '1';
    },

    /**
     * Helper to validate GUID format
     */
    isValidGuid: function(str) {
        if (!str) return false;
        const guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
        return guidRegex.test(str);
    }
};
