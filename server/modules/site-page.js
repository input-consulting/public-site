const fs = require('fs');
const path = require('path');

module.exports = class SitePage {

    constructor(root, file) {
        this.root = root;
        this.file = file;

        this._meta = {};
        this._content = {};

        this.init();
    }

    get route() { return this._meta.route || ''; }
    get id() { return this._meta.id || '' }
    get title() { return this._meta.title || ''; }
    get author() { return this._meta.author || ''; }
    get date() { return this._meta.date || null; }
    get tags() { return this._meta.tags || ''; }
    get image() { return this._meta.image || ''; }
    get bgImage() { return this._meta.bgimage || ''; }
    get body() { return this._content.body || ''; }
    get layout() { return this._meta.layout || null; }

    init() {
        try {
            let content = fs.readFileSync(this.file, 'utf-8');
            this.readMeta(content);
            this.readContent(content);

            this._meta.route = this.file.replace(this.root, '');
            this._meta.route = this._meta.route.substring(0, this._meta.route.lastIndexOf('.'));
            this._meta.route = this._meta.route.replace(/[/](\d+)[-](\d+)[-](\d+)[-]/, (m) => m.replace(/\-/g, '/') );
        } catch (error) {
            console.error(`unable to read: ${this.file}`, error);
        }
    }

    readMeta(fileContent) {
        const header = fileContent.match(/@Meta([\s\S]*?)@EndMeta/);
        if (header) {
            this._meta = header[1]
                .split('\n')
                .filter(m => m.length > 0)
                .reduce((prev, item) => {
                    const kv = item.split(':');
                    const key = kv[0].toLowerCase().trim();
                    let value = kv[1];
                    if (kv.length > 2) {
                        kv.shift();
                        value = kv.join(':');
                    }
                    if ( key === 'date' ) {
                        prev[key] = new Date(value.trim());
                    } else {
                        prev[key] = value.trim();
                    }
                    return prev;
                }, {});
        }
        
        // backward compability
        const layout = fileContent.match(/@Master\[\'([\s\S]*?)\'\]/);
        if ( layout ) {
            this._meta.layout = layout[1];
        }
    }

    readContent(fileContent) {
        const data = fileContent.match(/@Section\[\'(.*)\'\]([\s\S]*?)@EndSection/);
        if (data) {
            this._content.section = data[1].trim();
            this._content.body = data[2].trim();
        } else { 
            this._content.body = fileContent; 
        }
    }
}
