const gulp = require('gulp');
const del = require('del');
const vinylPaths = require('vinyl-paths');
const config = require('../../config');

// deletes all files in the output path
gulp.task('clean', function () {
  return gulp.src(config.public)
    .pipe(vinylPaths(del));
});
