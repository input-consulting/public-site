const router = require('koa-router')();
const sb = require('../modules/site-builder')

sb.pages.forEach(page => {
    router.get(page.route, async (ctx, next) => {
        const page = sb.getPagesByRoute( ctx.url )[0];
        await ctx.render(page.layout || '', { page, host: ctx.host } );
        await next();
    });
});

module.exports = router;