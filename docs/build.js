const fs = require('fs');
const path = require('path');
const marked = require('marked');


function* findReadmes(dir) {
    const entries = fs.readdirSync(dir, { withFileTypes: true });

    for (const entry of entries) {
        const fullPath = path.join(dir, entry.name);

        if (entry.isDirectory()) {
            const match = fullPath.match(/(\d{4})\/Day(\d{1,2})/i);
            if (match) {
                const year = parseInt(match[1], 10);
                const day = parseInt(match[2], 10);

                // Check the directory for a readme.md file
                const readmePath = path.join(fullPath, 'readme.md');
                const solutionPath = path.join(fullPath, 'Solution.cs');
                const illustrationPath = path.join(fullPath, 'illustration.jpeg');
                if (fs.existsSync(readmePath) && fs.existsSync(solutionPath)) {

                    const rawContent = fs.readFileSync(readmePath, 'utf8');

                    let name = '';
                    const match = rawContent.match(/^## --- Day \d+: (.+?) ---/);
                    if (match) {
                        name = match[1];
                    }

                    let lines = rawContent.split('\n');
                    while (lines.length > 0) {
                        let line = lines.shift();
                        if (line.match(/^## --- Day \d+: (.+?) ---/)) {
                            break;
                        }
                    }

                    yield {
                        year,
                        day,
                        path: readmePath,
                        name: name,
                        notes: marked.parse(lines.join('\n')),
                        code: fs.readFileSync(solutionPath, 'utf8'),
                        illustration: fs.existsSync(illustrationPath) ? illustrationPath : 'docs/elf.jpeg',
                    };
                }
            }

            // Recursively search within this directory
            yield* findReadmes(fullPath);
        }
    }
}


function copyDirectory(src, dest) {
    if (!fs.existsSync(src)) {
        throw new Error(`Source directory does not exist: ${src}`);
    }
  
    if (!fs.existsSync(dest)) {
      fs.mkdirSync(dest, { recursive: true });
    }
  
    const items = fs.readdirSync(src);
  
    items.forEach(item => {
      const srcPath = path.join(src, item);
      const destPath = path.join(dest, item);
  
      if (fs.statSync(srcPath).isDirectory()) {
        copyDirectory(srcPath, destPath);
      } else {
        fs.copyFileSync(srcPath, destPath);
      }
    });
  };


function loadTemplate(templatePath) {
    return fs.readFileSync(templatePath, 'utf8');
}


function fillTemplate(template, replacements) {
    return template.replace(/{{\s*([\w-]+)\s*}}/g, (_, key) => replacements[key] || '');
}

function generateYearPicker(year, day, yearToDays) {
    let res = '';
    for (let y of Object.keys(yearToDays).sort()){
        let lastDay =  yearToDays[y][yearToDays[y].length-1];
        let target = `/${y}/${lastDay}/`
        let selected = y == year ? 'selected="selected"' : '';
        res += `<option ${selected} value="${target}">${y}</option>`
    }
    return `<select>${res}</select>`;
}

function generateDayPicker(year, day, yearToDays) {
    let res = '';
    for (i = 1; i <= yearToDays[year].length; i++) {
        const link = `<a href="/${year}/${i}">${i.toString().padStart(2, '0')}</a>`;
        res +=  i == day ? `<span class="current">${link}</span>` : `<span>${link}</span>`;
    }
    return res;
}
const template = loadTemplate('docs/template.html');


const yearToDays = {}; 
for (const { year, day } of findReadmes('.')) {
    if (!yearToDays[year]) {
        yearToDays[year] = [];
    }
    yearToDays[year].push(day);
}

for(const year of Object.keys(yearToDays)){
    yearToDays[year] = yearToDays[year].sort((a,b) => a-b);
}



copyDirectory('docs/static', 'build');

// Iterate over readme.md files and print filled templates
for (const { year, day, name, notes, code, illustration } of findReadmes('.')) {
    const filledHtml = fillTemplate(template, {
        url: `https://aoc.csokavar.hu/${year}/${day}/`,
        'problem-id': `${year}/${day}`,
        'problem-name': `${name}`,
        'year-picker': generateYearPicker(year,day, yearToDays),
        'day-picker': generateDayPicker(year, day, yearToDays),
        notes,
        code,
    });
    const dst = `build/${year}/${day}`;
    fs.mkdirSync(dst, { recursive: true });
    fs.writeFileSync(path.join(dst, 'index.html'), filledHtml);
    fs.copyFileSync(illustration, path.join(dst, 'illustration.jpeg'));
}
