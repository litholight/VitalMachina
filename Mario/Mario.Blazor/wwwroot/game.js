function clearCanvas(canvas) {
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}

function setCanvasBackground(canvas, color) {
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = color;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
}

function drawRectangle(canvas, x, y, width, height, color) {
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = color;
    ctx.fillRect(x, y, width, height);
}

function drawTexture(canvas, imagePath, x, y, width, height) {
    const ctx = canvas.getContext('2d');
    const image = new Image();
    image.src = imagePath;
    image.onload = () => {
        ctx.drawImage(image, x, y, width, height);
    };
}

function drawText(canvas, text, fontPath, fontSize, color, x, y) {
    const ctx = canvas.getContext('2d');
    ctx.font = `${fontSize}px sans-serif`; // Adjust as needed
    ctx.fillStyle = color;
    ctx.fillText(text, x, y);
}

function drawSpritePart(canvas, imagePath, srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight) {
    const ctx = canvas.getContext('2d');
    const image = new Image();
    image.src = imagePath;

    if (image.complete && image.naturalHeight !== 0) {
        // Image is loaded, draw it
        ctx.drawImage(image, srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight);
    } else {
        // Image not loaded, set up onload to draw later
        image.onload = () => {
            ctx.drawImage(image, srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight);
        };
    }
}


function drawStaticTestSprite(canvas) {
    console.log('Attempting to draw static sprite part...');
    const ctx = canvas.getContext('2d');
    const image = new Image();
    image.src = '/Assets/mario-spritesheet.png'; // Ensure the path starts from the root if necessary
    image.onload = () => {
        console.log('Image loaded, drawing sprite part...');
        ctx.drawImage(image, 100, 50, 33, 48, 50, 50, 33, 48); // Adjust these values as needed
        console.log('Sprite part should be drawn.');
    };
    image.onerror = (e) => {
        console.error('Error loading image:', e);
    };
}


// Assuming DotNetReference is a reference to your Blazor component
window.startGameLoop = function(dotNetReference) {
    function gameLoop() {
        dotNetReference.invokeMethodAsync('UpdateAndRender')
            .then(() => {
                requestAnimationFrame(gameLoop); // Schedule the next frame
            });
    }
    requestAnimationFrame(gameLoop); // Start the loop
};

window.listenForKeyboardEvents = function(dotNetReference) {
    document.addEventListener('keydown', (event) => {
        dotNetReference.invokeMethodAsync('OnKeyDown', event.key);
    });

    document.addEventListener('keyup', (event) => {
        dotNetReference.invokeMethodAsync('OnKeyUp', event.key);
    });
};

window.renderingFunctions = {
    clearCanvas,
    setCanvasBackground,
    drawRectangle,
    drawTexture,
    drawText,
    drawSpritePart
};

window.initializeCanvas = function(canvas) {
    // Example initialization code, adjust as needed
    console.log('Canvas initialized', canvas);
    drawStaticTestSprite(canvas);
};