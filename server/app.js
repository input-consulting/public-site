const path = require('path');
//const appInsights = require('applicationinsights');
const Koa = require('koa');
const sb = require('./modules/site-builder');
const md = require('marked');
const moment = require('moment');
const nunjucks = require('koa-nunjucks-async');
const config = require('../config');

moment.locale('sv-SE');

const app = new Koa();

// application insight 
// appInsights.setup('99e84c77-b1e7-4069-8bdc-cc30c2b9fce6').start();
// appInsights.enableVerboseLogging(true);

// use for local debug 
// app.use(async function (ctx, next) {
//     const start = new Date();
//     await next();
//     const ms = new Date() - start;
//     console.log(`${ctx.status} - ${ctx.method} ${ctx.url} - ${ms}`);
// });

app.use(require('koa-helmet')());
app.use(require('koa-compress')());

app.use(require('./modules/handle-error')());
app.use(require('koa-conditional-get')());
app.use(require('koa-etag')());
app.use(require('koa-static')(path.join(__dirname, '/public')));

const nunjucksOptions = {
  opts: {
    autoescape: false,
    noCache: false,
    throwOnUndefined: false
  },
  filters: {
    date: d => moment(d).format('LL'),
    md: x => md(x),
    json: x => JSON.stringify(x, null, 2),
    ucfirst: e => typeof e === 'string' && e.toLowerCase() && e[0].toUpperCase() + e.slice(1)
  },
  globals: {
    title: config.title,
    sb: sb
  },
  ext: '.html'
};

app.use(nunjucks(path.join(path.resolve("."), `${config.root}/${config.site}`), nunjucksOptions));

app.use(require('./controllers/home').routes());
app.use(require('./controllers/default').routes());
app.use(require('./controllers/dates').routes());

module.exports = app;
