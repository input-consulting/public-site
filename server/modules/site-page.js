const fs = require('fs');
const config = require('../../config');

module.exports = class SitePage {

  constructor(root, file) {
    this.root = root;
    this.file = file;

    this._meta = {};
    this._content = {};
    this._haveLayout = false;

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
  get haveLayout() { return this._haveLayout; }

  init() {
    try {
      let content = fs.readFileSync(this.file, 'utf-8');
      this.readPageMeta(content);
      this.readPageContent(content);
      this.makePageRoute();
      this.validatePageLayout();
      this.validatePageDate();
      this.validatePageTitle();
      this.validatePage();
    } catch (error) {
      console.error(`unable to read: ${this.file}`, error);
    }
  }

  validatePage() {
    const messages = [];
    if ( !this.title ) {
      messages.push('title');
    }

    if ( !this.date ) {
      messages.push('date');
    }

    if ( messages.length ) {
      console.warn(`${this.file} is missing: ${messages.join(',')}`);
    }

  }


  readPageMeta(fileContent) {
    const header = fileContent.match(/---([\s\S]*?)---/);
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

          if (key && value) {
            if (key === 'date') {
              prev[key] = new Date(value.trim());
            } else {
              prev[key] = value.trim();
            }
          }
          return prev;
        }, {});
    }
  }

  readPageContent(fileContent) {
    const data = fileContent.replace(/---([\s\S]*?)---/, '').trim();
    if (data) {
      this._content.body = data;
    } else {
      this._content.body = fileContent;
    }
  }

  makePageRoute() {
    if (!this._meta.route) {
      // remove absolute path
      this._meta.route = this.file.replace(this.root.toLowerCase(), '');
      // remove file ext
      this._meta.route = this._meta.route.substring(0, this._meta.route.lastIndexOf('.'));
      // win fix, change pesky \ to / 
      this._meta.route = this._meta.route.replace(/\\/g, '/');
      // make composite routes into 'real' ones
      this._meta.route = this._meta.route.replace(/[/](\d+)[-](\d+)[-](\d+)[-]/, (m) => m.replace(/\-/g, '/'));
    }
  }

  validatePageLayout() {
    this._haveLayout = this.layout && fs.existsSync(`${config.root}/${config.site}/${this.layout}.html`);
  }

  validatePageDate() {
    if ( this._meta.date ) return;

    // use date in filename if date is not provided
    const match = this._meta.route.match(/[/](\d+)[/](\d+)[/](\d+)/);
    if ( match ) {
      this._meta.date = new Date(`${match[1]}-${match[2]}-${match[3]}`);
    }

  }

  validatePageTitle() {
    if ( this._meta.title ) return;

    // use filename if title is not provided
    const index = this._meta.route.lastIndexOf('/');
    let title = this._meta.route.substring(index + 1 )
                .replace( /[-_]+/g, ' ')
                .replace( /[Öö]+/g, 'o')
                .replace( /[ÅåÄä]+/g, 'a')
    this._meta.title = title.substring( 0, 1 ).toUpperCase() + title.substr( 1 );
  }
  
}
