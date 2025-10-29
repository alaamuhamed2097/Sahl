// imageHandler.js

// Function to resize an image and return as base64
function resizeImage(base64, contentType) {
    return new Promise((resolve) => {
        const img = new Image();
        img.src = `data:${contentType};base64,${base64}`;

        img.onload = () => {
            const MAX_WIDTH = 800;
            const MAX_HEIGHT = 800;
            let width = img.width;
            let height = img.height;

            // Calculate new dimensions while maintaining aspect ratio
            if (width > height) {
                if (width > MAX_WIDTH) {
                    height *= MAX_WIDTH / width;
                    width = MAX_WIDTH;
                }
            } else {
                if (height > MAX_HEIGHT) {
                    width *= MAX_HEIGHT / height;
                    height = MAX_HEIGHT;
                }
            }

            // Create canvas and resize image
            const canvas = document.createElement('canvas');
            canvas.width = width;
            canvas.height = height;
            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0, width, height);

            // Return the resized image as base64
            resolve(canvas.toDataURL(contentType));
        };

        img.onerror = () => {
            // Return original if resizing fails
            resolve(`data:${contentType};base64,${base64}`);
        };
    });
}

