const Koa = require('koa');
const helmet = require('koa-helmet');
const compress = require('koa-compress');
const nunjucks = require('koa-nunjucks-async');
const serve = require('koa-static');
const sb = require('./modules/site-builder');
const md = require('marked');
const moment = require('moment');

moment.locale('sv-SE');

const app = new Koa();

// app.use(async function (ctx, next) {
//     const start = new Date();
//     await next();
//     const ms = new Date() - start;
//     console.log(`${ctx.status} - ${ctx.method} ${ctx.url} - ${ms}`);
// });

app.use(helmet());
app.use(compress());

app.use(serve(__dirname + '/public'));
//app.use(serve('./server/public'));

const nunjucksOptions = {
    opts: {
        autoescape: false,
        noCache: false,
        throwOnUndefined: false
    },
    filters: {
        date : d => moment(d).format('LL'),
        md: x => md(x),
        json: x => JSON.stringify(x, null, 2),
        ucfirst: e => typeof e === 'string' && e.toLowerCase() && e[0].toUpperCase() + e.slice(1)
    },
    globals: {
        title: 'Input Consulting Stockholm AB'
    },
    ext: '.html'
};
app.use(nunjucks('site/views', nunjucksOptions));

app.use(require('./controllers/home').routes());
app.use(require('./controllers/default').routes());
app.use(require('./controllers/dates').routes());

app.listen(process.env.SERVICE_PORT || 8000);