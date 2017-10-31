'use strict';

const browserSync = require('browser-sync').create(),
    gulp = require('gulp'),
    nodemon = require('gulp-nodemon'),
    config = require('../../config'),
    reload = browserSync.reload;

gulp.task('watch',
    [
        'browser-sync'
    ],
    function () {
        gulp.watch(config.root + '/src/js/**/*.js', ['build:js', reload]);
        gulp.watch(config.root + '/src/scss/*.scss', ['build:sass', reload]);

        gulp.watch(config.public + '/js/site.js').on('change', reload);
        gulp.watch(config.public + '/style.css').on('change', reload);
    }
);

gulp.task('browser-sync',
    [
        'nodemon'
    ],
    function () {
        browserSync.init(null, {
            proxy: {
                target: "http://localhost:3000",
                // middleware: function (req, res, next) {
                //     console.log(req.url);
                //     next();
                // }
            },
            browser: 'google chrome',
            port: 3001
        });
    }
);

gulp.task('nodemon',
    [
        'build'
    ],
    function (done) {
        var running = false;

        return nodemon({
            script: 'index.js',
            watch: ['index.js', 'config.js', `!${config.public}/js/*.js`, 'server/**/*.js', `${config.root}/${config.site}/**/*.*`]
        })
        .on('start', function () {
            if (!running) {
                done();
            }
            running = true;
        })
        .on('restart', function () {
            setTimeout(function () {
                reload();
            }, 500);
        });
    }
);
