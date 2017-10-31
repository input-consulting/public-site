/* which site to run */
const siteRoot = 'web';

const path = require('path');
module.exports = require(path.join(path.resolve('.'), `${siteRoot}/config`));
