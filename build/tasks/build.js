var gulp = require('gulp');
var less = require('gulp-less');
var rename = require('gulp-rename');
var concat = require("gulp-concat");
var runSequence = require('run-sequence');
var changed = require('gulp-changed');
var plumber = require('gulp-plumber');
var sourcemaps = require('gulp-sourcemaps');
var paths = require('../paths');
var assign = Object.assign || require('object.assign');


gulp.task('build-system', function () {
  return gulp.src(paths.scripts.source)
    .pipe(plumber())
    .pipe(concat('site.js'))
    .pipe(gulp.dest(paths.scripts.output));
});

gulp.task('build-styles', function () {
    return gulp.src(paths.styles.source)
        .pipe(less())
        .pipe(concat('site.css'))
        .pipe(gulp.dest(paths.styles.output));
});

gulp.task('build-fonts', function () {
  return gulp.src(paths.fonts.source)
    .pipe(gulp.dest(paths.fonts.output));
});

gulp.task('build-views', function () {
  return gulp.src(paths.views.source)
    .pipe(gulp.dest(paths.views.output));
});

gulp.task('copy-content', function () {
  return gulp.src(paths.content.source)
    .pipe(gulp.dest(paths.content.output));
});

gulp.task('copy-static', function () {
  return gulp.src(paths.static.source)
    .pipe(gulp.dest(paths.static.output));
});

gulp.task('build', function(callback) {
  return runSequence(
    'clean',
    ['build-system', 'build-styles', 'build-fonts', 'build-views', 'copy-content', 'copy-static'],
    callback
  );
});
