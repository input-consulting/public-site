var gulp = require('gulp');
var paths = require('../paths');
var browserSync = require('browser-sync');

function reportChange(event){
  console.log('File ' + event.path + ' was ' + event.type + ', running tasks...');
}

gulp.task('watch', function() {
  gulp.watch('site/source/less/*.less', ['build-styles', browserSync.reload]).on('change', reportChange);
  gulp.watch('site/source/js/*.js', ['build-system', browserSync.reload]).on('change', reportChange);
  gulp.watch('site/**/*.html', ['build-views', browserSync.reload]).on('change', reportChange);
  gulp.watch('site/**/*.md', ['copy-content', browserSync.reload]).on('change', reportChange);
  gulp.watch('site/**/*.markdown', ['copy-content', browserSync.reload]).on('change', reportChange);
});
