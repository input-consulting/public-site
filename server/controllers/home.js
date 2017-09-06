const router = require('koa-router')();

router.get('/', async (ctx, next) => {
  const model = {};
  await ctx.render('index', model);
  await next();
});

module.exports = router;
