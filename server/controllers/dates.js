const router = require('koa-router')();
const sb = require('../modules/site-builder');
const templates = require('../modules/site-defaults');
const fs = require('fs');
const config = require('../../config');
const sd = require('../modules/site-defaults');

const dateRoutes = [];
sb.pages.forEach(p => {
  const dateMatch = p.route.match(/[/](\d+)[/](\d+)[/](\d+)/);
  if (dateMatch) {
    dateRoutes.push(p.route.substring(0, dateMatch.index));
  }
});

const haveLayout = (route) => fs.existsSync(`${config.root}/${config.site}/_layout${route}.html`);

const renderPages = async (route, ctx, next) => {
  const pages = sb.getPagesByRoute(ctx.url).sort((a, b) => -1 * ((a.date > b.date) - (a.date < b.date)));
  if (pages.length > 0) {
      if ( haveLayout(route) ) {
        await ctx.render(`_layout${route}`, { pages });
      } else {
        ctx.body = await ctx.renderString( sd.pages, { pages }  );
      }
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
