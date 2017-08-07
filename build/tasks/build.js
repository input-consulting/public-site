var gulp = require('gulp');
var concat = require("gulp-concat");
var plumber = require('gulp-plumber');
var runSequence = require('run-sequence');

//var paths = require('../paths');


// gulp.task('copy:views', function () {
//   return gulp.src(paths.views.source)
//     .pipe(gulp.dest(paths.views.output));
// });

// gulp.task('copy:content', function () {
//   return gulp.src(paths.content.source)
//     .pipe(gulp.dest(paths.content.output));
// });

gulp.task('build:static', function () {
    return gulp.src('site/static/**/*.*')
        .pipe(gulp.dest('server/public'));
});


gulp.task('build:js', function () {
  return gulp.src([
      './node_modules/jquery/dist/jquery.min.js',
      './node_modules/tether/dist/js/tether.js',
      './node_modules/bootstrap-sass/assets/javascripts/bootstrap.js',
      'site/src/js/*.js'
  ])
    .pipe(plumber())
    .pipe(concat('site.js'))
    .pipe(gulp.dest('server/public/js'));
});

gulp.task('build:fonts', function () {
  return gulp.src('./node_modules/bootstrap-sass/assets/fonts/bootstrap/*.*')
    .pipe(gulp.dest('server/public/fonts'));
});

gulp.task('build', function (callback) {
    return runSequence(
        'clean',
        ['build:static', 'build:js', 'build:sass', 'build:fonts'],
        callback
    );
});