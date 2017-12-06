const router = require('koa-router')();
const sb = require('../modules/site-builder')
const sd = require('../modules/site-defaults')

sb.pages.forEach(page => {
  router.get(page.route, async (ctx, next) => {
    const page = sb.getPagesByRoute(ctx.url)[0];
    if ( page.haveLayout ) {
      await ctx.render(page.layout, { page, host: ctx.host });
    } else {
      ctx.body = await ctx.renderString(sd.page, { page, host: ctx.host });
    }
    await next();
  });
});

module.exports = router;
