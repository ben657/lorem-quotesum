const fs = require('fs');
const { memoryUsage } = require('process');
const readline = require('readline');
const sqlite3 = require('sqlite3');

console.log(memoryUsage().heapUsed / 1024 / 1024);

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

let currentCharacter = '';
let currentScene = '';

let lastType = '';
let line = '';
rl.on('line', chunk => {
    if(chunk === 'END') {
        if(fs.existsSync('./data.db')) {
            fs.unlinkSync('./data.db');
        }

        const db = new sqlite3.Database('./data.db');
        db.serialize(() => {
            db.run(`
                CREATE TABLE movies (
                    title text PRIMARY KEY
                )
            `);

            db.run(`
                CREATE TABLE lines (
                    movie text PRIMARY KEY REFERENCES movies(title),
                    character text,
                    scene text,
                    line text
                )
            `);

            db.run(`
                INSERT INTO movies VALUES ('Shrek')
            `);

            quotes.forEach(quote => {
        });

        return;
    }

    const leadingWhitespace = chunk.length - chunk.trimLeft().length;
    const type = whitespaceMap[leadingWhitespace];
    switch (type) {
        case 'CHARACTER':
            const char = chunk.trim();
            currentCharacter = char;
            if(!characters.includes(char))
                characters.push(char);

            break;

        case 'LINE':
        case 'DESC':
            line += line.length ? (' ' + chunk.trim()) : chunk.trim();
            break;

        case 'SCENE':
            const scene = chunk.trim();
            currentScene = scene;
            if(!scenes.includes(scene))
                scenes.push(scene);

            break;

        default:
            if(line.length > 0) {
                const data = {
                    line,
                    character: currentCharacter,
                    scene: currentScene
                };
                if(lastType === 'LINE') {
                    quotes.push(data);
                } else if(lastType === 'DESC') {
                    descriptions.push(data);
                }
                line = '';
            }
            break;
    }

    lastType = type;
});