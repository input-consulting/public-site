module.exports = () => {
  return async (ctx, next) => {
    try {
      await next()
      const status = ctx.status || 404;
      if (status === 404) {
        ctx.throw(404)
      }
    } catch (err) {
      ctx.status = err.status || 500;
      if (ctx.status === 404) {
        if (/\.jpg|\.css|\.js|\.png/.test(ctx.request.url)) {
          ctx.body = '';
        } else {
          switch (ctx.accepts('html', 'json')) {
            case 'html':
              await ctx.render('_layout/404');
              break;
            case 'json':
              ctx.body = {
                message: 'Not Found'
              };
              break;
            default:
              ctx.type = 'text';
              ctx.body = '';
          }
        }
      } else {
        ctx.body = '';
      }
    }
  }
}
