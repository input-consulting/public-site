const router = require('koa-router')();
const sb = require('../modules/site-builder')

router.get( '/', async (ctx, next) => {

    const model = {
       quote: sb.getPageById('quote'),
       news : sb.getPagesByRoute('nyheter/').sort( (a,b) => -1 * ((a.date > b.date) - (a.date < b.date)) ).slice(0,2),
       about: sb.getPageById('about'),
       culture: sb.getPageById('culture'),
       cultures: sb.getPagesByRoute('culture/'),
       service : sb.getPageById('services'),
       services: sb.getPagesByRoute('services/'),
       sale : sb.getPageById('sales'),
       sales : sb.getPagesByRoute('sales/'),
       crew : sb.getPageById('crew')
    };

    await ctx.render('_layout/home-page', model );
    await next();
} );

module.exports = router;