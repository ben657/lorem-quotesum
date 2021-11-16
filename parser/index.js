const fs = require('fs');
const readline = require('readline');

const whitespaceMap = {
    40: 'CHARACTER',
    25: 'LINE',
    15: 'DESC',
    18: 'SCENE'
}

const rl = readline.createInterface({
    input: fs.createReadStream('./data.txt'),
    console: false
});

const quotes = [];
const scenes = [];
const characters = [];
const descriptions = [];

let lastChunk = null;
let line = '';
rl.on('line', chunk => {
    if(chunk === 'END') {
        console.log(characters);
    }

    if(lastChunk === null) {
        lastChunk = chunk;
        line = chunk;
    }

    const leadingWhitespace = chunk.length - chunk.trimLeft().length;
    switch (whitespaceMap[leadingWhitespace]) {
        case 'CHARACTER':
            const char = chunk.trim();
            if(!characters.includes(char))
                characters.push(char);
            break;

        case 'LINE':
            
            break;

        case 'DESC':
            break;

        case 'SCENE':
            const scene = chunk.trim();
            if(!scenes.includes(scene))
                scenes.push(scene);
            break;

        default:
            break;
    }
});