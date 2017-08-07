var gulp = require('gulp');
var del = require('del');
var vinylPaths = require('vinyl-paths');

// //var paths = require('../paths');

// deletes all files in the output path
gulp.task('clean', function() {
  return gulp.src([
      'server/public',
    ])
    .pipe(vinylPaths(del));
});
