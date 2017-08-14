const router = require('koa-router')();
const sb = require('../modules/site-builder')

const dateRoutes = [];
sb.pages.forEach(p => {
  let dateMatch = p.route.match(/[/](\d+)[/](\d+)[/](\d+)/);
  if (dateMatch) {
    dateRoutes.push(p.route.substring(0, dateMatch.index));
  }
});

const renderPages = async (route, ctx, next) => {
  const pages = sb.getPagesByRoute(ctx.url).sort((a, b) => -1 * ((a.date > b.date) - (a.date < b.date)));
  if (pages.length > 0) {
    await ctx.render(`_layout${route}` || '', { pages });
  }
  await next();
}

[...new Set(dateRoutes)].forEach(route => {
  router.get(`${route}`, async (ctx, next) => await renderPages(route, ctx, next));
  router.get(`${route}/:year`, async (ctx, next) => await renderPages(route, ctx, next));
  router.get(`${route}/:year/:month`, async (ctx, next) => await renderPages(route, ctx, next));
  router.get(`${route}/:year/:month/:day`, async (ctx, next) => await renderPages(route, ctx, next));
});

module.exports = router;
