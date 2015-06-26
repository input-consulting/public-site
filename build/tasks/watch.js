var gulp = require('gulp');
var paths = require('../paths');
var browserSync = require('browser-sync');

function reportChange(event){
  console.log('File ' + event.path + ' was ' + event.type + ', running tasks...');
}

gulp.task('watch', function() {
  gulp.watch('app/**/src/less/*.less', ['build-styles', browserSync.reload]).on('change', reportChange);
  gulp.watch('app/**/src/js/*.js', ['build-system', browserSync.reload]).on('change', reportChange);
  gulp.watch('site/**/*.html', ['build-views', browserSync.reload]).on('change', reportChange);
  gulp.watch('site/**/*.md', ['build-content', browserSync.reload]).on('change', reportChange);
  gulp.watch('site/**/*.markdown', ['build-content', browserSync.reload]).on('change', reportChange);
});
