var gulp = require('gulp');
var concat = require("gulp-concat");
var plumber = require('gulp-plumber');
var runSequence = require('run-sequence');
var sass = require('gulp-sass');
var uglify = require('gulp-uglify');
const autoprefixer = require('gulp-autoprefixer');

const config = require('../../config');

gulp.task('build:sass', function () {
    return gulp.src(config.build.styles.source)
        .pipe(sass({ 
          outputStyle: 'compressed' 
        }).on('error', sass.logError))
        .pipe(autoprefixer({
            browsers: ['last 2 versions']
        }))
        .pipe(gulp.dest(config.build.styles.target));
});

gulp.task('build:static', function () {
    return gulp.src(config.build.static.source)
        .pipe(gulp.dest(config.build.static.target));
});

gulp.task('build:js', function () {
    return gulp.src(config.build.js.source)
        .pipe(plumber())
        .pipe(concat('site.js'))
        .pipe(uglify())
        .pipe(gulp.dest(config.build.js.target));
});

gulp.task('build:fonts', function () {
    return gulp.src(config.build.fonts.source)
        .pipe(gulp.dest(config.build.fonts.target));
});

gulp.task('build', function (callback) {
    return runSequence(
        'clean',
        ['build:static', 'build:js', 'build:sass', 'build:fonts'],
        callback
    );
});
