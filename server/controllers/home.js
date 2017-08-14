const router = require('koa-router')();

router.get('/', async (ctx, next) => {
  const model = {};
  await ctx.render('_layout/home-page', model);
  await next();
});

module.exports = router;
