/* which site to run */
const siteRoot = 'site';

const path = require('path');
module.exports = require(path.join(path.resolve('.'), `${siteRoot}/config`));
