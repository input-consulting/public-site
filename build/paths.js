var path = require('path');

var project = 'Input.Site.Web/'
var appRoot = 'app/';
var siteRoot = 'site/';
var views = siteRoot + '/views';
var staticContent = siteRoot + '/static';

module.exports = {
  root: appRoot,
  scripts : {
  	source : [ 
  		'bower_components/jquery/dist/jquery.js',
  		'bower_components/bootstrap/dist/js/bootstrap.js',
      appRoot + project + 'src/js/**/*.js/'
  	],
  	output : appRoot + project + 'assets/js',
  },

  styles : {
  	source : [appRoot + '**/site.less'],
  	output : appRoot + project + 'assets/css',
  },

  fonts : {
      source : [
          'bower_components/bootstrap/dist/fonts/*.*',
          appRoot + project + 'src/fonts/*.*',
      ],
    output : appRoot + project + 'assets/fonts'
  },
  
  views : {
      source : views + '/_layout/**/*.html',
      output : appRoot + project + 'views/_layout', 
  },
  
  content : {
      source : [ views + '/**/*.markdown', views + '/**/*.md' ],
      output : appRoot + project + 'views/', 
  },
  static : {
      source : staticContent + '/**/*.*' ,
      output : appRoot + project + 'static', 
  },

  output: appRoot + project + 'assets',

  source: appRoot + '**/*.js',
  html: appRoot + '**/*.html',
  style: 'styles/**/*.css',
 
};
