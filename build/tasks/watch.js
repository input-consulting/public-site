'use strict';

var browserSync = require('browser-sync').create(),
    gulp = require('gulp'),
    nodemon = require('gulp-nodemon'),
    reload = browserSync.reload;

gulp.task('watch',
    [
        'browser-sync'
    ],
    function () {
        gulp.watch('site/src/js/**/*.js', ['build:js', reload] );
        gulp.watch('site/src/scss/*.scss', ['build:sass', reload]);

        gulp.watch('server/public/js/site.js').on('change', reload);
        gulp.watch('server/public/style.css').on('change', reload);

    }
);

gulp.task('browser-sync',
    [
        'nodemon'
    ],
    function () {
        browserSync.init(null, {
            proxy: {
                target: "http://localhost:8000",
                // middleware: function (req, res, next) {
                //     console.log(req.url);
                //     next();
                // }
            },
            browser: 'google chrome',
            port: 8001
        });
    }
);

gulp.task('nodemon',
    [
        //    'eslint',
        'build:sass',
        'build:js',
        'build:static'
        //    'ngtemplate',
        //    'karma'
    ],
    function (done) {
        var running = false;

        return nodemon({
            script: 'index.js',
            watch: [ 'index.js', '!server/public/js/*.js', 'server/**/*.js', 'site/views/**/*.*']
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
