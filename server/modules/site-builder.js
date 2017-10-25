const fs = require('fs');
const path = require('path');
const SitePage = require('./site-page');
const config = require('../../config');

class SiteBuilder {

  constructor() {
    this.pages = [];

    this.options = {
      root: path.join(path.resolve("."), `${config.root}/views`)
    };

    this.build(this.options);
    this.created = true;
  }

  static create() {
    return this.created ? this : new SiteBuilder();
  }

  build(config = {}) {
    this.options = Object.assign({}, this.options, config);

    let site = this.read(this.options.root);
    this.createSite(site);
  }

  getPageById(id) {
    return this.pages.find(p => p.id === id);
  }

  getPagesByRoute(route) {
    return this.pages
      .filter(p => p.route.includes(route))
      .sort((a, b) => -1 * ((a.date > b.date) - (a.date < b.date)));
  }

  createSite(site) {
    const p = site
      .filter( f => /md|markdown/.test(path.extname(f)) )
      .map(s => new SitePage(this.options.root, s));
    this.pages = [...new Set(p)];
  };

  read(dir) {
    return fs.readdirSync(dir)
      .filter(d => !/_layout/.test(d) && !/.DS_Store/.test(d) )
      .reduce((files, file) =>
        fs.statSync(path.join(dir, file)).isDirectory() ?
          files.concat(this.read(path.join(dir, file))) :
          files.concat(path.join(dir, file).toLowerCase()), []);
  }

}

module.exports = SiteBuilder.create();
