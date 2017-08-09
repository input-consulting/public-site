'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass');

gulp.task('build:sass', function () {
  return gulp.src('site/src/scss/site.scss')
    .pipe(sass({outputStyle: 'compressed'}).on('error', sass.logError))
    .pipe(gulp.dest('server/public/css'));
});