const path = require("path");
const appInsights = require("applicationinsights");
const Koa = require("koa");
const sb = require("./modules/site-builder");
const md = require("marked");
const moment = require("moment");
const nunjucks = require("koa-nunjucks-async");
const config = require("../config");

const startup = async () => {
  moment.locale("sv-SE");
  const app = new Koa();

  //
  // Overload protection, create proper responses to proxy
  // and Client-Retry header

  //
  // const protectionOptions = {
  //   // if production is false, detailed error messages are exposed to the client
  //   production: process.env.NODE_ENV === "production",
  //   // Client-Retry header, in seconds (0 to disable) [default 1]
  //   clientRetrySecs: 1,
  //   // sample rate, milliseconds [default 5]
  //   sampleInterval: 5,
  //   // maximum detected delay between event loop ticks [default 42]
  //   maxEventLoopDelay: 42,
  //   // maximum heap used threshold (0 to disable) [default 0]
  //   maxHeapUsedBytes: 0,
  //   // maximum rss size threshold (0 to disable) [default 0]
  //   maxRssBytes: 0,
  //   // dictate behavior: take over the response
  //   // or propagate an error to the framework [default false]
  //   errorPropagationMode: false
  // };
  // app.use(require("overload-protection")("koa", protectionOptions));

  //
  // Application insight
  //
  appInsights.setup("99e84c77-b1e7-4069-8bdc-cc30c2b9fce6").start();

  // use for local debug
  app.use(async function (ctx, next) {
      const start = new Date();
      await next();
      const ms = new Date() - start;
      console.log(`${ctx.status} - ${ctx.method} ${ctx.url} - ${ms}`);
  });

  //
  // Compress text/* responses
  //
  app.use(
    require("koa-compress")({
      filter: function(content_type) {
        return /text/i.test(content_type);
      },
      threshold: 2048,
      flush: require("zlib").Z_SYNC_FLUSH
    })
  );

  ///
  /// security messaures
  ///
  app.use(require("koa-helmet")());

  ///
  /// Handle 304's and generate proper etags
  ///
  app.use(require("koa-conditional-get")());
  app.use(require("koa-etag")());

  ///
  /// set client cache for static files
  ///
  app.use(
    require("koa-static")(path.join(__dirname, "/public"), {
      maxAge: process.env.NODE_ENV === "production" ? 365 * 24 * 60 * 60 : 0
    })
  );

  //
  // Generic error handler
  //
  app.use(require("./modules/handle-error")());

  ///
  /// Generate file hash for cache busting of css and js
  ///
  const crypto = require("crypto");
  const fs = require("fs");
  const sha1 = path =>
    new Promise((resolve, reject) => {
      const hash = crypto.createHash("sha1");
      const rs = fs.createReadStream(path);
      rs.on("error", reject);
      rs.on("data", chunk => hash.update(chunk));
      rs.on("end", () => resolve(hash.digest("hex")));
    });

  const css = await sha1(path.join(__dirname, "/public/css/site.css"));
  const js = await sha1(path.join(__dirname, "/public/js/site.js"));

  //
  // configure NunJucks
  //
  const nunjucksOptions = {
    opts: {
      autoescape: false,
      noCache: false,
      throwOnUndefined: false
    },
    filters: {
      date: d => moment(d).format("LL"),
      md: x => md(x),
      json: x => JSON.stringify(x, null, 2),
      sanitize: x => x.replace(/(<([^>]+)>)/gi, "")
    },
    globals: {
      title: config.title,
      sb: sb,
      cssHash: css,
      jsHash: js
    },
    ext: ".html"
  };

  app.use(
    nunjucks(
      path.join(path.resolve("."), `${config.root}/${config.site}`),
      nunjucksOptions
    )
  );

  ///
  /// Add app routes
  ///
  app.use(require("./controllers/home").routes());
  app.use(require("./controllers/default").routes());
  app.use(require("./controllers/dates").routes());

  return app;
};

module.exports = {
  startup
};
