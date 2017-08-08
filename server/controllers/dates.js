const router = require('koa-router')();
const sb = require('../modules/site-builder')

let found = [];
sb.pages.forEach(p => {
    let dateMatch = p.route.match(/[/](\d+)[/](\d+)[/](\d+)/);
    if (dateMatch) {
        found.push(p.route.substring(0, dateMatch.index));
    }
});

[...new Set(found)].forEach(route => {

    router.get(`${route}`, async (ctx, next) => {
        const pages = sb.getPagesByRoute(ctx.url).sort((a, b) => -1 * ((a.date > b.date) - (a.date < b.date)));
        if (pages.length > 0) {
            await ctx.render(`_layout${route}` || '', { pages });
        }
        await next();
    });

    router.get(`${route}/:year`, async (ctx, next) => {
        const pages = sb.getPagesByRoute(ctx.url).sort((a, b) => -1 * ((a.date > b.date) - (a.date < b.date)));
        if (pages.length > 0) {
            await ctx.render(`_layout${route}` || '', { pages });
        }
        await next();
    });

    router.get(`${route}/:year/:month`, async (ctx, next) => {
        const pages = sb.getPagesByRoute(ctx.url).sort((a, b) => -1 * ((a.date > b.date) - (a.date < b.date)));
        if (pages.length > 0) {
            await ctx.render(`_layout${route}` || '', { pages });
        }
        await next();
    });

    router.get(`${route}/:year/:month/:day`, async (ctx, next) => {
        const pages = sb.getPagesByRoute(ctx.url).sort((a, b) => -1 * ((a.date > b.date) - (a.date < b.date)));
        if (pages.length > 0) {
            await ctx.render(`_layout${route}` || '', { pages });
        }
        await next();
    });
});

module.exports = router;